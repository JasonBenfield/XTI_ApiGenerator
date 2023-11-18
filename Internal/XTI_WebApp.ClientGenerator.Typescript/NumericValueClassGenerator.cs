using System.Text.RegularExpressions;

namespace XTI_WebApp.ClientGenerator.Typescript;

internal sealed class NumericValueClassGenerator
{
    private readonly Func<string, Stream> createStream;
    private readonly NumericValueTemplate numericValueTemplate;

    private static readonly Regex whitespaceRegex = new Regex("\\s+");

    public NumericValueClassGenerator(Func<string, Stream> createStream, NumericValueTemplate numericValueTemplate)
    {
        this.createStream = createStream;
        this.numericValueTemplate = numericValueTemplate;
    }

    public Task Output()
    {
        var tsFile = new TypeScriptFile($"{numericValueTemplate.DataType.Name}.ts", createStream);
        var className = numericValueTemplate.DataType.Name;
        tsFile.AddLine("import { NumericValue } from '@jasonbenfield/sharedwebapp/NumericValue';");
        tsFile.AddLine("import { NumericValues } from '@jasonbenfield/sharedwebapp/NumericValues';");
        tsFile.AddLine();
        tsFile.AddLine($"export class {className}s extends NumericValues<{className}> {{");
        tsFile.Indent();
        tsFile.AddLine("constructor(");
        tsFile.Indent();
        var valueNames = numericValueTemplate.Values
            .Select(v => whitespaceRegex.Replace(v.DisplayText, ""));
        var lastValueName = valueNames.Last();
        foreach (var valueName in valueNames)
        {
            tsFile.AddLine($"public readonly {valueName}: {className}");
            if (valueName != lastValueName)
            {
                tsFile.Append(",");
            }
        }
        tsFile.Outdent();
        tsFile.AddLine(") {");
        tsFile.Indent();
        var joinedValueNames = string.Join(",", valueNames);
        tsFile.AddLine($"super([{joinedValueNames}]);");
        tsFile.Outdent();
        tsFile.AddLine("}");
        tsFile.Outdent();
        tsFile.AddLine("}");
        tsFile.AddLine();
        tsFile.AddLine($"export class {className} extends NumericValue implements I{className} {{");
        tsFile.Indent();
        tsFile.AddLine($"public static readonly values = new {className}s(");
        tsFile.Indent();
        var lastValue = numericValueTemplate.Values.Last();
        foreach (var numericValue in numericValueTemplate.Values)
        {
            tsFile.AddLine($"new {className}({numericValue.Value}, '{numericValue.DisplayText.Replace("'", "\'")}')");
            if (numericValue != lastValue)
            {
                tsFile.Append(",");
            }
        }
        tsFile.Outdent();
        tsFile.AddLine(");");
        tsFile.AddLine();
        tsFile.AddLine("private constructor(Value: number, DisplayText: string) {");
        tsFile.Indent();
        tsFile.AddLine("super(Value, DisplayText);");
        tsFile.Outdent();
        tsFile.AddLine("}");
        tsFile.AddLine();
        tsFile.AddLine($"equalsAny: (...other: this[] | I{className}[] | number[] | string[]) => boolean;");
        tsFile.AddLine();
        tsFile.AddLine($"equals: (other: this | I{className} | number | string) => boolean;");
        tsFile.Outdent();
        tsFile.AddLine("}");
        return tsFile.Output();
    }
}
