using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace XTI_ApiGenerator
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
            var groups = new List<GroupDefinition>();
            var childDirs = Directory.GetDirectories(projectDir);
            childDirs = childDirs
                .Where
                (
                    d =>
                        !d.EndsWith("\\bin", StringComparison.OrdinalIgnoreCase) &&
                        !d.EndsWith("\\obj", StringComparison.OrdinalIgnoreCase)
                )
                .ToArray();
            foreach (var childDir in childDirs)
            {
                var group = new GeneratedGroupDefinition(childDir).Value();
                if (group.Actions.Any())
                {
                    groups.Add(group);
                }
            }
            return new AppDefinition(appName, appType, groups.ToArray());
        }
    }
}
