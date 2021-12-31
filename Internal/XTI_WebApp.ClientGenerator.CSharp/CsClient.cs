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
        var complexFieldTemplates = formTemplates.SelectMany(ft => ft.Form.ComplexFieldTemplates).Distinct();
        foreach (var complexFieldTemplate in complexFieldTemplates)
        {
            await new ComplexFieldClass(ns, createStream, complexFieldTemplate, "ComplexField").Output();
        }
        foreach (var formTemplate in formTemplates)
        {
            await new ComplexFieldClass(ns, createStream, formTemplate.Form, "Form").Output();
        }
        foreach (var groupTemplate in appTemplate.GroupTemplates)
        {
            await new ApiGroupClass(ns, createStream, groupTemplate).Output();
        }
        foreach (var numericValueTemplate in appTemplate.NumericValueTemplates(ApiCodeGenerators.Dotnet))
        {
            await new NumericValueClass(ns, createStream, numericValueTemplate).Output();
        }
        await new ApiAppClass(ns, createStream, appTemplate, versionKey).Output();
        var namespaces = new[]
        {
            "XTI_WebAppClient",
            "XTI_WebAppClient.Forms"
        };
        await new GlobalUsingsClass(createStream, namespaces).Output();
    }
}