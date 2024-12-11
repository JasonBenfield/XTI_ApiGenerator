using System.Text.RegularExpressions;

namespace XTI_ApiGeneratorTask
{
    public sealed class ActionDefinition
    {
        public ActionDefinition()
            : this("", "", "", "")
        {
        }

        public ActionDefinition
        (
            string className, 
            string requestDataName, 
            string resultDataName, 
            string validationClassName = ""
        )
        {
            Name = GetName(className);
            ClassName = className;
            RequestDataName = requestDataName;
            ResultDataName = resultDataName;
            ValidationClassName = validationClassName;
        }

        private static readonly Regex nameRegex = new Regex("^(?<Name>([a-z]|\\d)+)(Action|Page)$", RegexOptions.IgnoreCase);

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
        public string ResultDataName { get; }
        public string ValidationClassName { get; }

        public ActionDefinition WithValidationClassName(string validationClassName) =>
            new ActionDefinition
            (
                ClassName, 
                RequestDataName, 
                ResultDataName, 
                validationClassName
            );

        public bool IsEmpty() => string.IsNullOrWhiteSpace(Name);
    }
}
