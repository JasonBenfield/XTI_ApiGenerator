using System.Collections.Generic;
using System.IO;

namespace XTI_ApiGenerator
{
    internal sealed class GeneratedGroupDefinition
    {
        private readonly string groupDir;

        public GeneratedGroupDefinition(string groupDir)
        {
            this.groupDir = groupDir;
        }

        public GroupDefinition Value()
        {
            var actions = new List<ActionDefinition>();
            var filePaths = Directory.GetFiles(groupDir, "*.cs");
            foreach (var filePath in filePaths)
            {
                var fileActions = new GeneratedActionDefinition(filePath).Value();
                actions.AddRange(fileActions);
            }
            var groupName = new DirectoryInfo(groupDir).Name;
            var group = new GroupDefinition(groupName, actions.ToArray());
            return group;
        }
    }
}
