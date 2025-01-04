using System.Linq;
using System.Text.RegularExpressions;

namespace XTI_ApiGeneratorTask
{
    public sealed class QueryDefinition
    {
        public QueryDefinition()
            : this(new CsClass())
        {
        }

        internal QueryDefinition(CsClass csClass)
        {
            Namespace = csClass.Namespace;
            Name = GetName(csClass.ClassName);
            ClassName = csClass.ClassName;
            IsPublic = csClass.IsPublic;
            RequestDataName = csClass.BaseClassTypeArgs.ElementAtOrDefault(0) ?? "";
            EntityName = csClass.BaseClassTypeArgs.ElementAtOrDefault(1) ?? "";
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

        public string Namespace { get; }
        public string Name { get; }
        public string ClassName { get; }
        public bool IsPublic { get; }
        public string RequestDataName { get; }
        public string EntityName { get; }

        public bool IsEmpty() => string.IsNullOrWhiteSpace(Name);
    }
}
