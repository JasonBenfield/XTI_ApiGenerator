using XTI_App.Abstractions;
using XTI_WebApp.CodeGeneration;

namespace XTI_WebApp.ClientGenerator.Typescript;

internal sealed class AppClassGenerator
{
    private readonly Func<string, Stream> createStream;
    private readonly AppApiTemplate appTemplate;
    private readonly AppVersionKey versionKey;

    public AppClassGenerator(Func<string, Stream> createStream, AppApiTemplate appTemplate, AppVersionKey versionKey)
    {
        this.createStream = createStream;
        this.appTemplate = appTemplate;
        this.versionKey = versionKey;
    }

    public Task Output()
    {
        var appClassName = $"{appTemplate.Name}AppClient";
        var tsFile = new TypeScriptFile(appClassName, createStream);
        tsFile.AddLine();
        tsFile.AddLine("import { AppClient } from \"@jasonbenfield/sharedwebapp/Http/AppClient\";");
        tsFile.AddLine("import { AppClientEvents } from \"@jasonbenfield/sharedwebapp/Http/AppClientEvents\";");
        tsFile.AddLine("import { AppClientQuery } from \"@jasonbenfield/sharedwebapp/Http/AppClientQuery\";");
        foreach (var groupTemplate in appTemplate.GroupTemplates.Where(g => !g.IsODataGroup() && !g.IsUser() && !g.IsUserCache()))
        {
            var groupClassName = new GroupClassName(groupTemplate).Value;
            tsFile.AddLine($"import {{ {groupClassName} }} from \"./{groupClassName}\";");
        }
        tsFile.AddLine();
        tsFile.AddLine($"\r\nexport class {appClassName} extends AppClient {{");
        tsFile.Indent();
        tsFile.AddLine($"constructor(events: AppClientEvents) {{");
        tsFile.Indent();
        tsFile.AddLine($"super(");
        tsFile.Indent();
        tsFile.AddLine($"events, ");
        tsFile.AddLine($"'{appTemplate.Name}', ");
        tsFile.AddLine($"pageContext.EnvironmentName === 'Production' || pageContext.EnvironmentName === 'Staging' ? '{versionKey.DisplayText}' : 'Current'");
        tsFile.Outdent();
        tsFile.AddLine(");");
        foreach (var groupTemplate in appTemplate.GroupTemplates)
        {
            if (groupTemplate.IsODataGroup())
            {
                var modelTemplate = new TsType(groupTemplate.ActionTemplates.First().ModelTemplate).Value;
                var entityTemplate = new TsType(groupTemplate.QueryableTemplates().First().ElementTemplate).Value;
                tsFile.AddLine($"this.{groupTemplate.Name} = this.addODataGroup((evts, resourceUrl) => new AppClientQuery<{modelTemplate}, {entityTemplate}>(evts, resourceUrl.odata('{groupTemplate.Name}'), '{groupTemplate.Name}'));");
            }
            else if (!groupTemplate.IsUser() && !groupTemplate.IsUserCache())
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
                tsFile.AddLine($"readonly {groupTemplate.Name}: AppClientQuery<{modelTemplate}, {entityTemplate}>;");
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
