using System.Text.RegularExpressions;
using XTI_Core;

namespace XTI_WebApp.ClientGenerator.Typescript;

internal sealed class ODataColumnsBuilderGenerator
{
    private readonly Func<string, Stream> createStream;
    private readonly QueryableValueTemplate queryableTemplate;
    private static readonly Regex delimiterRegex = new Regex("[\\s_]+");

    public ODataColumnsBuilderGenerator(Func<string, Stream> createStream, QueryableValueTemplate queryableTemplate)
    {
        this.createStream = createStream;
        this.queryableTemplate = queryableTemplate;
    }

    public Task Output()
    {
        var entityType = queryableTemplate.ElementTemplate.DataType;
        var entityName = entityType.Name;
        var properties = entityType.GetProperties();
        var className = $"OData{entityName}ColumnsBuilder";
        var tsFile = new TypeScriptFile(className, createStream);
        tsFile.AddLine("import { ODataColumnBuilder } from \"@jasonbenfield/sharedwebapp/OData/ODataColumnBuilder\";");
        tsFile.AddLine("import { ODataColumnViewBuilder } from \"@jasonbenfield/sharedwebapp/OData/ODataColumnViewBuilder\";");
        tsFile.AddLine("import { SourceType } from \"@jasonbenfield/sharedwebapp/OData/SourceType\";");
        tsFile.AddLine("import { ODataColumns } from \"@jasonbenfield/sharedwebapp/OData/Types\";");
        tsFile.AddLine();
        tsFile.AddLine($"export class OData{entityName}ColumnViewsBuilder {{");
        tsFile.Indent();
        foreach (var property in properties)
        {
            tsFile.AddLine($"readonly {property.Name} = new ODataColumnViewBuilder();");
        }
        tsFile.Outdent();
        tsFile.AddLine("}");
        tsFile.AddLine();
        tsFile.AddLine($"export class OData{entityName}ColumnsBuilder {{");
        tsFile.Indent();
        tsFile.AddLine($"constructor(views: OData{entityName}ColumnViewsBuilder) {{");
        tsFile.Indent();
        foreach (var property in properties)
        {
            tsFile.AddLine($"this.{property.Name} = new ODataColumnBuilder('{property.Name}', new SourceType('{property.PropertyType.Name}'), views.{property.Name});");
            var propertyName = delimiterRegex.Replace(property.Name, " ");
            var propertyNameWithSpaces = string.Join(" ", new CamelCasedWord(propertyName).Words());
            if (property.Name != propertyNameWithSpaces)
            {
                tsFile.AddLine($"this.{property.Name}.setDisplayText('{propertyNameWithSpaces}');");
            }
        }
        tsFile.Outdent();
        tsFile.AddLine("}");
        foreach (var property in properties)
        {
            tsFile.AddLine($"readonly {property.Name}: ODataColumnBuilder;");
        }
        tsFile.AddLine();
        tsFile.AddLine("build() {");
        tsFile.Indent();
        tsFile.AddLine("return {");
        tsFile.Indent();
        var lastProperty = properties.Last();
        foreach (var property in properties)
        {
            tsFile.AddLine($"{property.Name}: this.{property.Name}.build()");
            if (property != lastProperty)
            {
                tsFile.Append(",");
            }
        }
        tsFile.Outdent();
        tsFile.AddLine($"}} as ODataColumns<IQueryable{entityName}>;");
        tsFile.Outdent();
        tsFile.AddLine("}");
        tsFile.Outdent();
        tsFile.AddLine("}");
        return tsFile.Output();
    }
}
