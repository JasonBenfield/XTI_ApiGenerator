namespace XTI_ApiGeneratorTask
{
    public sealed class GroupDefinition
    {
        public GroupDefinition()
            : this("", new ActionDefinition[0])
        {
        }

        public GroupDefinition(string name, ActionDefinition[] actions)
        {
            Name = name;
            BuilderClassName = $"{name}GroupBuilder";
            ClassName = $"{name}Group";
            Actions = actions;
        }

        public string Name { get; }
        public string BuilderClassName { get; }
        public string ClassName { get; }
        public ActionDefinition[] Actions { get; }
    }
}
