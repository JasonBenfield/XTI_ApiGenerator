namespace XTI_WebApp.ClientGenerator.Typescript;

internal sealed class GroupClassName
{
    public GroupClassName(AppApiGroupTemplate group)
    {
        Value =  $"{group.Name}Group";
    }

    public string Value { get; }
}
