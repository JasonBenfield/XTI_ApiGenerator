using System.Linq;
using System.Text.RegularExpressions;

namespace XTI_CoreApiGeneratorTask
{
    public sealed class ActionValidationDefinition
    {
        public ActionValidationDefinition()
            : this(new CsClass())
        {
        }

        internal ActionValidationDefinition(CsClass csClass)
        {
            Name = GetName(csClass.ClassName);
            IsPublic = csClass.IsPublic;
            ClassName = csClass.ClassName;
            RequestDataName = csClass.BaseClassTypeArgs.ElementAtOrDefault(0) ?? "";
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
        public bool IsPublic { get; }
        public string RequestDataName { get; }

        public bool IsEmpty() => string.IsNullOrWhiteSpace(Name);
    }
}
