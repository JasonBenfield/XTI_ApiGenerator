using System.Text.RegularExpressions;

namespace XTI_ApiGeneratorTask
{
    public sealed class QueryDefinition
    {
        public QueryDefinition()
            : this("", "", "")
        {
        }

        public QueryDefinition(string className, string requestDataName, string entityName)
        {
            Name = GetName(className);
            ClassName = className;
            RequestDataName = requestDataName;
            EntityName = entityName;
        }

        private static readonly Regex nameRegex = new Regex("^(?<Name>([a-z]|\\d)+)Action$", RegexOptions.IgnoreCase);

        private static string GetName(string className)
        {
            string name;
            var match = nameRegex.Match(className);
            if (match.Success)
            {
                name = match.Groups["Name"].Value;
            }
            else
            {
                name = className;
            }
            return name;
        }

        public string Name { get; }
        public string ClassName { get; }
        public string RequestDataName { get; }
        public string EntityName { get; }

        public bool IsEmpty() => string.IsNullOrWhiteSpace(Name);
    }
}
