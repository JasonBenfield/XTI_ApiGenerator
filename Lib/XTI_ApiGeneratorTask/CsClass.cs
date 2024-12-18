using System.IO;
using System.Linq;

namespace XTI_ApiGeneratorTask
{
    internal sealed class CsClass
    {
        public CsClass(string ns, string filePath, string className, string baseClassName, string[] baseClassTypeArgs)
        {
            Namespace = ns;
            FilePath = filePath;
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
                new ActionDefinition
                (
                    ns: Namespace,
                    className: ClassName,
                    requestDataName: BaseClassTypeArgs.ElementAtOrDefault(0) ?? "",
                    resultDataName: BaseClassTypeArgs.ElementAtOrDefault(1) ?? ""
                ) : 
                new ActionDefinition();
        }

        public ActionValidationDefinition ToActionValidationDefinition()
        {
            return IsActionValidation ?
                new ActionValidationDefinition
                (
                    className: ClassName,
                    requestDataName: BaseClassTypeArgs.ElementAtOrDefault(0) ?? ""
                ) :
                new ActionValidationDefinition();
        }

        public QueryDefinition ToQueryDefinition()
        {
            return IsQuery ?
                new QueryDefinition
                (
                    ns: Namespace,
                    className: ClassName,
                    requestDataName: BaseClassTypeArgs.ElementAtOrDefault(0) ?? "",
                    entityName: BaseClassTypeArgs.ElementAtOrDefault(1) ?? ""
                ) :
                new QueryDefinition();
        }

    }
}
