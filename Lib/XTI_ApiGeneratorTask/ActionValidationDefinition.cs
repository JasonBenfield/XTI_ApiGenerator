using System.Text.RegularExpressions;

namespace XTI_ApiGeneratorTask
{
    public sealed class ActionValidationDefinition
    {
        public ActionValidationDefinition()
            : this("", "")
        {
        }

        public ActionValidationDefinition
        (
            string className,
            string requestDataName
        )
        {
            Name = GetName(className);
            ClassName = className;
            RequestDataName = requestDataName;
        }

        private static readonly Regex nameRegex = new Regex("^(?<Name>([a-z]|\\d)+)Validation$", RegexOptions.IgnoreCase);

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

        public bool IsEmpty() => string.IsNullOrWhiteSpace(Name);
    }
}
