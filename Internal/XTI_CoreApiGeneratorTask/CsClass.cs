using System.IO;

namespace XTI_CoreApiGeneratorTask
{
    internal sealed class CsClass
    {
        public CsClass()
            : this("", "", "", "", "", new string[0])
        {
        }

        public CsClass
        (
            string ns,
            string filePath,
            string accessModifier,
            string className,
            string baseClassName,
            string[] baseClassTypeArgs
        )
        {
            Namespace = ns;
            FilePath = filePath;
            IsPublic = accessModifier.Equals("public");
            ClassName = className;
            BaseClassName = baseClassName;
            BaseClassTypeArgs = baseClassTypeArgs;
            DirectoryPath = Path.GetDirectoryName(filePath);
            DirectoryName = new DirectoryInfo(DirectoryPath).Name;
            IsAction = BaseClassName.Equals("AppAction");
            IsActionValidation = BaseClassName.Equals("AppActionValidation");
            IsQuery = BaseClassName.Equals("QueryAction");
        }

        public string Namespace { get; }
        public string FilePath { get; }
        public bool IsPublic { get; }
        public string DirectoryPath { get; }
        public string DirectoryName { get; }
        public string ClassName { get; }
        public string BaseClassName { get; }
        public string[] BaseClassTypeArgs { get; }
        public bool IsAction { get; }
        public bool IsActionValidation { get; }
        public bool IsQuery { get; }



        public ActionDefinition ToActionDefinition()
        {
            return IsAction ?
                new ActionDefinition(this) :
                new ActionDefinition();
        }

        public ActionValidationDefinition ToActionValidationDefinition()
        {
            return IsActionValidation ?
                new ActionValidationDefinition(this) :
                new ActionValidationDefinition();
        }

        public QueryDefinition ToQueryDefinition()
        {
            return IsQuery ?
                new QueryDefinition(this) :
                new QueryDefinition();
        }

    }
}
