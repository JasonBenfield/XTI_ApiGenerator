using Microsoft.CodeAnalysis;
using XTI_App.Abstractions;
using XTI_App.Api;
using XTI_WebApp.CodeGeneration;
using XTI_WebApp.CodeGeneration.CSharp;

namespace XTI_WebApp.ClientGenerator.CSharp;

public sealed class CsClient : CodeGenerator
{
    private readonly AppVersionKey versionKey;
    private readonly string ns;
    private readonly Func<string, Stream> createStream;

    public CsClient
    (
        AppVersionKey versionKey,
        string outputFolder,
        Func<string, Stream> createStream
    )
    {
        this.versionKey = versionKey;
        ns = new NamespaceFromFolder(outputFolder).Value();
        this.createStream = createStream;
    }

    public async Task Output(AppApiTemplate appTemplate)
    {
        var objTemplates = appTemplate.ObjectTemplates(ApiCodeGenerators.Dotnet);
        foreach (var objTemplate in objTemplates)
        {
            await new ApiObjectClass(ns, createStream, objTemplate).Output();
        }
        var formTemplates = appTemplate.FormTemplates(ApiCodeGenerators.Dotnet);
        var complexFieldTemplates = formTemplates
            .SelectMany(ft => ft.Form.ComplexFieldTemplates)
            .Distinct();
        foreach (var complexFieldTemplate in complexFieldTemplates)
        {
            await new ComplexFieldClass(ns, createStream, complexFieldTemplate, "ComplexField").Output();
        }
        foreach (var formTemplate in formTemplates)
        {
            await new ComplexFieldClass(ns, createStream, formTemplate.Form, "Form").Output();
        }
        foreach (var groupTemplate in appTemplate.GroupTemplates.Where(gt => !gt.IsODataGroup() && !gt.IsUser() && !gt.IsUserCache()))
        {
                await new ApiGroupClass(ns, createStream, groupTemplate).Output();
        }
        foreach (var numericValueTemplate in appTemplate.NumericValueTemplates(ApiCodeGenerators.Dotnet))
        {
            await new NumericValueClass(ns, createStream, numericValueTemplate).Output();
        }
        foreach (var enumValueTemplate in appTemplate.EnumValueTemplates(ApiCodeGenerators.Dotnet))
        {
            await new EnumValueClass(ns, createStream, enumValueTemplate).Output();
        }
        await new AppVersionClass(ns, createStream, appTemplate, versionKey).Output();
        await new RolesClass(ns, createStream, appTemplate).Output();
        await new ApiAppClass(ns, createStream, appTemplate).Output();
        var namespaces = new[]
        {
            "XTI_WebAppClient",
            "XTI_WebAppClient.Forms",
            "Microsoft.Extensions.Hosting"
        };
        await new GlobalUsingsClass(createStream, namespaces).Output();
    }
}