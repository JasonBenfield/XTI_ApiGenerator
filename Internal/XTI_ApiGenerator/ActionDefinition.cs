namespace XTI_ApiGenerator
{
    public sealed class ActionDefinition
    {
        public ActionDefinition()
            : this("", "", "", "")
        {
        }

        public ActionDefinition(string name, string className, string requestDataName, string resultDataName)
        {
            Name = name;
            ClassName = className;
            RequestDataName = requestDataName;
            ResultDataName = resultDataName;
        }

        public string Name { get; }
        public string ClassName { get; }
        public string RequestDataName { get; }
        public string ResultDataName { get; }

        public bool IsEmpty() => string.IsNullOrWhiteSpace(Name);
    }
}
