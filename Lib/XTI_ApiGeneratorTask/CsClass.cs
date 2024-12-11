using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace XTI_ApiGeneratorTask
{
    internal sealed class CsClass
    {
        public CsClass(string filePath, string className, string baseClassName, string[] baseClassTypeArgs)
        {
            FilePath = filePath;
            ClassName = className;
            BaseClassName = baseClassName;
            BaseClassTypeArgs = baseClassTypeArgs;
            ParentDirectoryPath = Path.GetDirectoryName(filePath);
            ParentDirectoryName = new DirectoryInfo(ParentDirectoryPath).Name;
            IsAction = BaseClassName.Equals("AppAction");
            IsActionValidation = BaseClassName.Equals("AppActionValidation");
            IsQuery = BaseClassName.Equals("QueryAction");
        }

        public string FilePath { get; }
        public string ParentDirectoryPath { get; }
        public string ParentDirectoryName { get; }
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
                    className: ClassName,
                    requestDataName: BaseClassTypeArgs.ElementAtOrDefault(0) ?? "",
                    entityName: BaseClassTypeArgs.ElementAtOrDefault(1) ?? ""
                ) :
                new QueryDefinition();
        }

    }
}
