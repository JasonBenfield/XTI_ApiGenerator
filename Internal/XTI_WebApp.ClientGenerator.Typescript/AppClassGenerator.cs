using XTI_WebApp.CodeGeneration;

namespace XTI_WebApp.ClientGenerator.Typescript;

internal sealed class AppClassGenerator
{
    private readonly Func<string, Stream> createStream;
    private readonly AppApiTemplate appTemplate;

    public AppClassGenerator(Func<string, Stream> createStream, AppApiTemplate appTemplate)
    {
        this.createStream = createStream;
        this.appTemplate = appTemplate;
    }

    public Task Output()
    {
        var appClassName = $"{appTemplate.Name}AppApi";
        var tsFile = new TypeScriptFile(appClassName, createStream);
        tsFile.AddLine();
        tsFile.AddLine("import { AppApi } from \"@jasonbenfield/sharedwebapp/Api/AppApi\";");
        tsFile.AddLine("import { AppApiEvents } from \"@jasonbenfield/sharedwebapp/Api/AppApiEvents\";");
        tsFile.AddLine("import { AppApiQuery } from \"@jasonbenfield/sharedwebapp/Api/AppApiQuery\";");
        foreach (var groupTemplate in appTemplate.GroupTemplates.Where(g => !g.IsODataGroup() && !g.IsUser() && !g.IsUserCache()))
        {
            var groupClassName = new GroupClassName(groupTemplate).Value;
            tsFile.AddLine($"import {{ {groupClassName} }} from \"./{groupClassName}\";");
        }
        tsFile.AddLine();
        tsFile.AddLine($"\r\nexport class {appClassName} extends AppApi {{");
        tsFile.Indent();
        tsFile.AddLine($"constructor(events: AppApiEvents) {{");
        tsFile.Indent();
        tsFile.AddLine($"super(events, '{appTemplate.Name}');");
        foreach (var groupTemplate in appTemplate.GroupTemplates)
        {
            if (groupTemplate.IsODataGroup())
            {
                var modelTemplate = new TsType(groupTemplate.ActionTemplates.First().ModelTemplate).Value;
                var entityTemplate = new TsType(groupTemplate.QueryableTemplates().First().ElementTemplate).Value;
                tsFile.AddLine($"this.{groupTemplate.Name} = this.addODataGroup((evts, resourceUrl) => new AppApiQuery<{modelTemplate}, {entityTemplate}>(evts, resourceUrl.odata('{groupTemplate.Name}'), '{groupTemplate.Name}'));");
            }
            else if(!groupTemplate.IsUser() && !groupTemplate.IsUserCache())
            {
                tsFile.AddLine($"this.{groupTemplate.Name} = this.addGroup((evts, resourceUrl) => new {groupTemplate.Name}Group(evts, resourceUrl));");
            }
        }
        tsFile.Outdent();
        tsFile.AddLine("}");
        tsFile.AddLine();
        foreach (var groupTemplate in appTemplate.GroupTemplates.Where(g => !g.IsUser() && !g.IsUserCache()))
        {
            if (groupTemplate.IsODataGroup())
            {
                var modelTemplate = new TsType(groupTemplate.ActionTemplates.First().ModelTemplate).Value;
                var entityTemplate = new TsType(groupTemplate.QueryableTemplates().First().ElementTemplate).Value;
                tsFile.AddLine($"readonly {groupTemplate.Name}: AppApiQuery<{modelTemplate}, {entityTemplate}>;");
            }
            else
            {
                tsFile.AddLine($"readonly {groupTemplate.Name}: {groupTemplate.Name}Group;");
            }
        }
        tsFile.Outdent();
        tsFile.AddLine("}");
        return tsFile.Output();
    }
}
