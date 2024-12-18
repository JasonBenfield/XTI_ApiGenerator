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
            var actionClasses = csClasses
                .Where(c => c.IsAction)
                .ToArray();
            var groups = csClasses
                .ToLookup(c => new { c.DirectoryName, c.DirectoryPath })
                .Select
                (
                    g =>
                    {
                        var validations = g
                            .Where(c => c.IsActionValidation)
                            .Select(c => c.ToActionValidationDefinition())
                            .ToArray();
                        var actions = g
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
                        return new GroupDefinition(g.Key.DirectoryName, actions);
                    }
                )
                .Where(g => g.Actions.Any())
                .ToArray();
            var queries = csClasses
                .Where(c => c.IsQuery)
                .Select(c => c.ToQueryDefinition())
                .ToArray();
            return new AppDefinition(appName, appType, groups, queries);
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
