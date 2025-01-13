using System.Linq;
using System.Text.RegularExpressions;

namespace XTI_CoreApiGeneratorTask
{
    public sealed class ActionDefinition
    {
        private readonly CsClass csClass;

        private static readonly Regex nameRegex = new Regex("^(?<Name>([a-z]|\\d)+)(Action|Page)$", RegexOptions.IgnoreCase);

        public ActionDefinition()
            : this(new CsClass(), "")
        {
        }

        internal ActionDefinition
        (
            CsClass csClass,
            string validationClassName = ""
        )
        {
            this.csClass = csClass;
            Namespace = csClass.Namespace;
            Name = GetName(csClass.ClassName);
            ClassName = csClass.ClassName;
            IsPublic = csClass.IsPublic;
            RequestDataName = csClass.BaseClassTypeArgs.ElementAtOrDefault(0) ?? "";
            ResultDataName = csClass.BaseClassTypeArgs.ElementAtOrDefault(1) ?? "";
            ValidationClassName = validationClassName;
        }

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
        public string ResultDataName { get; }
        public string ValidationClassName { get; }

        public ActionDefinition WithValidationClassName(string validationClassName) =>
            new ActionDefinition
            (
                csClass: csClass,
                validationClassName: validationClassName
            );

        public bool IsEmpty() => string.IsNullOrWhiteSpace(Name);
    }
}
