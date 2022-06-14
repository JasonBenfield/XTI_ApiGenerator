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
        tsFile.AddLine("import { AppApiODataGroup } from \"@jasonbenfield/sharedwebapp/Api/AppApiODataGroup\";");
        foreach (var groupTemplate in appTemplate.GroupTemplates.Where(g => !g.IsODataGroup()))
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
                var entityTemplate = new TsType(groupTemplate.QueryableTemplates().First().ElementTemplate).Value;
                tsFile.AddLine($"this.{groupTemplate.Name} = this.addODataGroup<IQueryable{entityTemplate.Substring(1)}>((evts, resourceUrl) => new AppApiODataGroup<IQueryable{entityTemplate.Substring(1)}>(evts, resourceUrl, '{groupTemplate.Name}'));");
            }
            else
            {
                tsFile.AddLine($"this.{groupTemplate.Name} = this.addGroup((evts, resourceUrl) => new {groupTemplate.Name}Group(evts, resourceUrl));");
            }
        }
        tsFile.Outdent();
        tsFile.AddLine("}");
        tsFile.AddLine();
        foreach (var group in appTemplate.GroupTemplates)
        {
            if (group.IsODataGroup())
            {
                var entityTemplate = new TsType(group.QueryableTemplates().First().ElementTemplate).Value;
                tsFile.AddLine($"readonly {group.Name}: AppApiODataGroup<IQueryable{entityTemplate.Substring(1)}>;");
            }
            else
            {
                tsFile.AddLine($"readonly {group.Name}: {group.Name}Group;");
            }
        }
        tsFile.Outdent();
        tsFile.AddLine("}");
        return tsFile.Output();
    }
}
