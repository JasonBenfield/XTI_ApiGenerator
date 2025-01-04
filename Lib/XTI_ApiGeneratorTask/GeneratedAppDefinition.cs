using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace XTI_ApiGeneratorTask
{
    public sealed class GeneratedAppDefinition
    {
        private readonly string appName;
        private readonly string appType;
        private readonly string projectDir;

        public GeneratedAppDefinition(string appName, string appType, string projectDir)
        {
            this.appName = appName;
            this.appType = appType;
            this.projectDir = projectDir;
        }

        public AppDefinition Value()
        {
            if (string.IsNullOrWhiteSpace(projectDir))
            {
                throw new Exception($"Project Directory is required.");
            }
            if (!Directory.Exists(projectDir))
            {
                throw new Exception($"Project Directory '{projectDir}' does not exist.");
            }
            var csClasses = GetCsClasses();
            var groupLookup = csClasses
                .ToLookup(c => new { c.DirectoryName, c.DirectoryPath });
            var errors = new List<ApiGeneratorTaskError>();
            var groups = new List<GroupDefinition>();
            foreach (var groupedClasses in groupLookup)
            {
                var validations = groupedClasses
                    .Where(c => c.IsActionValidation)
                    .Select(c => c.ToActionValidationDefinition())
                    .ToArray();
                var actions = groupedClasses
                    .Where(c => c.IsAction)
                    .Select
                    (
                        c =>
                        {
                            var action = c.ToActionDefinition();
                            var validationClassName = validations
                                .Where(v => v.Name == action.Name)
                                .FirstOrDefault()?.ClassName ??
                                "";
                            if (!string.IsNullOrWhiteSpace(validationClassName))
                            {
                                action = action.WithValidationClassName(validationClassName);
                            }
                            return action;
                        }
                    )
                    .ToArray();
                var unmatchedValidations = validations
                    .Where(v => !actions.Any(a => a.ValidationClassName == v.ClassName));
                var unmatchedValidationErrors = unmatchedValidations
                    .Select
                    (
                        v => new ApiGeneratorTaskError
                        (
                            message: $"Validation '{v.Name}' does not have an action in group {groupedClasses.Key.DirectoryName}",
                            errorCode: "XTI0101",
                            filePath: Path.Combine(groupedClasses.Key.DirectoryPath, $"{v.ClassName}.cs")
                        )
                    );
                errors.AddRange(unmatchedValidationErrors);
                var nonPublicValidationErrors = validations
                    .Except(unmatchedValidations)
                    .Where(v => !v.IsPublic)
                    .Select
                    (
                        v => new ApiGeneratorTaskError
                        (
                            message: $"Validation '{v.Name}' in group {groupedClasses.Key.DirectoryName} is not public",
                            errorCode: "XTI0102",
                            filePath: Path.Combine(groupedClasses.Key.DirectoryPath, $"{v.ClassName}.cs")
                        )
                    );
                errors.AddRange(nonPublicValidationErrors);
                var nonPublicActionErrors = actions
                    .Where(v => !v.IsPublic)
                    .Select
                    (
                        a => new ApiGeneratorTaskError
                        (
                            message: $"Action '{a.Name}' in group {groupedClasses.Key.DirectoryName} is not public",
                            errorCode: "XTI0102",
                            filePath: Path.Combine(groupedClasses.Key.DirectoryPath, $"{a.ClassName}.cs")
                        )
                    );
                errors.AddRange(nonPublicActionErrors);
                if (actions.Any())
                {
                    var group = new GroupDefinition(groupedClasses.Key.DirectoryName, actions);
                    groups.Add(group);
                }
            }
            var queries = csClasses
                .Where(c => c.IsQuery)
                .Select(c => c.ToQueryDefinition())
                .ToArray();
            var nonPublicQueryErrors = queries
                .Where(v => !v.IsPublic)
                .Select
                (
                    a => new ApiGeneratorTaskError
                    (
                        message: $"Query '{a.Name}' is not public",
                        errorCode: "XTI0102",
                        filePath: Path.Combine(projectDir, $"{a.ClassName}.cs")
                    )
                );
            errors.AddRange(nonPublicQueryErrors);
            if (errors.Any())
            {
                throw new ApiGeneratorTaskException(errors.ToArray());
            }
            return new AppDefinition(appName, appType, groups.ToArray(), queries);
        }

        private CsClass[] GetCsClasses() =>
            GetCsFiles()
                .Select(f => new ParsedCsFile(f))
                .SelectMany(f => f.Classes())
                .ToArray();

        private string[] GetCsFiles()
        {
            var files = new List<string>(Directory.GetFiles(projectDir, "*.cs"));
            files.AddRange
            (
                Directory.GetDirectories(projectDir).SelectMany(d => Directory.GetFiles(d, "*.cs"))
            );
            return files.ToArray();
        }

    }
}
