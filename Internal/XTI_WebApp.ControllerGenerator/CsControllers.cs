using Microsoft.CodeAnalysis;
using XTI_App.Api;
using XTI_WebApp.CodeGeneration;
using XTI_WebApp.CodeGeneration.CSharp;

namespace XTI_WebApp.ControllerGenerator;

public sealed class CsControllers : CodeGenerator
{
    private readonly string ns;
    private readonly Func<string, Stream> createStream;

    public CsControllers(string outputFolder, Func<string, Stream> createStream)
    {
        ns = new NamespaceFromFolder(outputFolder).Value();
        this.createStream = createStream;
    }

    public async Task Output(AppApiTemplate appTemplate)
    {
        foreach (var group in appTemplate.GroupTemplates)
        {
            if (group.IsODataGroup())
            {
                await new ODataControllerClassGenerator(ns, createStream, appTemplate, group).Output();
            }
            else
            {
                await new ControllerClassGenerator(ns, createStream, appTemplate, group).Output();
            }
        }
        var namespaces = appTemplate.ObjectTemplates(ApiCodeGenerators.Dotnet)
            .Select(o => o.DataType.Namespace ?? "")
            .Union
            (
                new[]
                {
                    "Microsoft.AspNetCore.Authorization",
                    "Microsoft.AspNetCore.Mvc",
                    "XTI_App.Abstractions",
                    "XTI_App.Api",
                    "XTI_WebApp.Api",
                    "XTI_WebApp.Abstractions"       
                }
            )
            .Distinct()
            .OrderBy(str => str)
            .ToArray();
        await new EdmModelBuilderClassGenerator(ns, createStream, appTemplate).Output();
        await new GlobalUsingsClass(createStream, namespaces).Output();
    }
}