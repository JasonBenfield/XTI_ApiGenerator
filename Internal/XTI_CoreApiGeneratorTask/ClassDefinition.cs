namespace XTI_CoreApiGeneratorTask
{
    public sealed class ClassDefinition
    {
        public ClassDefinition(string className, string contents)
        {
            ClassName = className;
            Contents = contents;
        }

        public string ClassName { get; }
        public string Contents { get; }
    }
}
