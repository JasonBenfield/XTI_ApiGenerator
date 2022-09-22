namespace XTI_WebApp.ClientGenerator.Typescript;

internal sealed class ApiGroupClassGenerator
{
    private readonly Func<string, Stream> createStream;
    private readonly AppApiGroupTemplate group;

    public ApiGroupClassGenerator(Func<string, Stream> createStream, AppApiGroupTemplate group)
    {
        this.createStream = createStream;
        this.group = group;
    }

    public Task Output()
    {
        var groupClassName = new GroupClassName(group).Value;
        var tsFile = new TypeScriptFile(groupClassName, createStream);
        tsFile.AddLine();
        tsFile.AddLine("import { AppApiGroup } from \"@jasonbenfield/sharedwebapp/Api/AppApiGroup\";");
        tsFile.AddLine("import { AppApiAction } from \"@jasonbenfield/sharedwebapp/Api/AppApiAction\";");
        tsFile.AddLine("import { AppApiView } from \"@jasonbenfield/sharedwebapp/Api/AppApiView\";");
        tsFile.AddLine("import { AppApiEvents } from \"@jasonbenfield/sharedwebapp/Api/AppApiEvents\";");
        tsFile.AddLine("import { AppResourceUrl } from \"@jasonbenfield/sharedwebapp/Api/AppResourceUrl\";");
        foreach (var form in group.FormTemplates())
        {
            tsFile.AddLine($"import {{ {form.Form.TypeName} }} from \"./{form.Form.TypeName}\";");
        }
        tsFile.AddLine();
        tsFile.AddLine($"export class {groupClassName} extends AppApiGroup {{");
        tsFile.Indent();
        tsFile.AddLine("constructor(events: AppApiEvents, resourceUrl: AppResourceUrl) {");
        tsFile.Indent();
        tsFile.AddLine($"super(events, resourceUrl, '{group.Name}');");
        foreach (var action in group.ActionTemplates)
        {
            if (IsWebPage(action))
            {
                var modelType = new TsType(action.ModelTemplate).Value;
                tsFile.AddLine($"this.{action.Name} = this.createView<{modelType}>('{action.Name}');");
            }
            else
            {
                var genericArgs = GetGenericArguments(action);
                tsFile.AddLine($"this.{action.Name}Action = this.createAction{genericArgs}('{action.Name}', '{action.FriendlyName}');");
            }
        }
        tsFile.Outdent();
        tsFile.AddLine("}");
        tsFile.AddLine();
        foreach (var action in group.ActionTemplates)
        {
            if (IsWebPage(action))
            {
                var modelType = new TsType(action.ModelTemplate).Value;
                tsFile.AddLine($"readonly {action.Name}: AppApiView<{modelType}>;");
            }
            else
            {
                var genericArgs = GetGenericArguments(action);
                tsFile.AddLine($"readonly {action.Name}Action: AppApiAction{genericArgs};");
            }
        }
        tsFile.AddLine();
        foreach (var action in group.ActionTemplates)
        {
            if (!IsWebPage(action))
            {
                var modelType = new TsType(action.ModelTemplate.DataType).Value;
                var modelDecl = action.HasEmptyModel() ? "" : $"model: {modelType}, ";
                var modelParm = action.HasEmptyModel() ? "{}" : "model";
                tsFile.AddLine($"{action.Name}({modelDecl}errorOptions?: IActionErrorOptions) {{");
                tsFile.Indent();
                tsFile.AddLine($"return this.{action.Name}Action.execute({modelParm}, errorOptions || {{}});");
                tsFile.Outdent();
                tsFile.AddLine("}");
            }
        }
        tsFile.Outdent();
        tsFile.AddLine("}");
        return tsFile.Output();
    }

    private static bool IsWebPage(AppApiActionTemplate action) => 
        action.IsView() || action.IsPartialView() || action.IsRedirect() || 
        action.IsFile() || action.IsQueryToExcel();

    private string GetGenericArguments(AppApiActionTemplate action)
    {
        var modelType = new TsType(action.ModelTemplate).Value;
        var resultType = new TsType(action.ResultTemplate).Value;
        return $"<{modelType},{resultType}>";
    }

}
