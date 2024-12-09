using System;

namespace XTI_ApiGenerator
{
    public sealed class AppDefinition
    {
        public AppDefinition(string name, string type, GroupDefinition[] groups)
        {
            Name = name;
            Type = type;
            BuilderClassName = $"{name}AppApiBuilder";
            ClassName = $"{name}AppApi";
            Groups = groups;
            IsWebApp = type.Replace(" ", "").Equals("WebApp", StringComparison.OrdinalIgnoreCase);
            IsConsoleApp = type.Replace(" ", "").Equals("ConsoleApp", StringComparison.OrdinalIgnoreCase);
        }

        public string Name { get; }
        public string Type { get; }
        public string BuilderClassName { get; }
        public string ClassName { get; }
        public GroupDefinition[] Groups { get; }
        public bool IsWebApp { get; }
        public bool IsConsoleApp { get; }
    }
}
