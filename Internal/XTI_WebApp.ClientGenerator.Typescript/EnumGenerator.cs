namespace XTI_WebApp.ClientGenerator.Typescript;

internal sealed class EnumGenerator
{
    private readonly Func<string, Stream> createStream;
    private readonly EnumValueTemplate enumValueTemplate;

    public EnumGenerator(Func<string, Stream> createStream, EnumValueTemplate enumValueTemplate)
    {
        this.createStream = createStream;
        this.enumValueTemplate = enumValueTemplate;
    }

    public Task Output()
    {
        var className = enumValueTemplate.DataType.Name;
        var tsFile = new TypeScriptFile($"{className}.ts", createStream);
        tsFile.AddLine($"enum {className} {{");
        tsFile.Indent();
        var lastValue = enumValueTemplate.EnumValues.Last();
        foreach (var value in enumValueTemplate.EnumValues)
        {
            tsFile.AddLine($"{value.Name} = {(int)value.Value}");
            if (value != lastValue)
            {
                tsFile.Append(",");
            }
        }
        tsFile.Outdent();
        tsFile.AddLine("}");
        return tsFile.Output();
    }
}
