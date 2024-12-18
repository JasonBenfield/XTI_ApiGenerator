namespace XTI_ApiGeneratorTask
{
    internal sealed class QualifiedClassName
    {
        internal QualifiedClassName(string ns, string className)
        {
            Value = string.IsNullOrWhiteSpace(ns) ?
                    className :
                    $"{ns}.{className}";
        }

        public string Value { get; }
    }
}
