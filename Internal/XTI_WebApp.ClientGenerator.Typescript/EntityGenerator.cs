namespace XTI_WebApp.ClientGenerator.Typescript;

internal sealed class EntityGenerator
{
    private readonly Func<string, Stream> createStream;
    private readonly AppApiTemplate appTemplate;

    public EntityGenerator(Func<string, Stream> createStream, AppApiTemplate appTemplate)
    {
        this.createStream = createStream;
        this.appTemplate = appTemplate;
    }

    public Task Output()
    {
        var tsFile = new TypeScriptFile($"{appTemplate.Name}Entities.d.ts", createStream);
        tsFile.AddLine();
        foreach (var objectTemplate in appTemplate.ObjectTemplates(ApiCodeGenerators.TypeScript))
        {
            AddObjectInterface(tsFile, objectTemplate);
        }
        foreach (var numericValueTemplate in appTemplate.NumericValueTemplates(ApiCodeGenerators.TypeScript))
        {
            AddNumericValueInterface(tsFile, numericValueTemplate);
        }
        return tsFile.Output();
    }

    private static void AddObjectInterface(TypeScriptFile tsFile, ObjectValueTemplate objectTemplate)
    {
        tsFile.AddLine($"interface I{objectTemplate.DataType.Name} {{");
        tsFile.Indent();
        foreach (var property in objectTemplate.PropertyTemplates)
        {
            var tsType = new TsType(property.ValueTemplate).Value;
            tsFile.AddLine($"{property.Name}: {tsType};");
        }
        tsFile.Outdent();
        tsFile.AddLine("}");
    }

    private static void AddNumericValueInterface(TypeScriptFile tsFile, NumericValueTemplate numericValueTemplate)
    {
        tsFile.AddLine($"interface I{numericValueTemplate.DataType.Name} {{");
        tsFile.Indent();
        tsFile.AddLine($"Value: number;");
        tsFile.AddLine($"DisplayText: string;");
        tsFile.Outdent();
        tsFile.AddLine("}");
    }

}
