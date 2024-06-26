﻿using XTI_App.Abstractions;
using XTI_WebApp.CodeGeneration;

namespace XTI_WebApp.ClientGenerator.Typescript;

public sealed class TsClient : CodeGenerator
{
    private readonly Func<string, Stream> createStream;
    private readonly AppVersionKey versionKey;

    public TsClient(Func<string, Stream> createStream, AppVersionKey versionKey)
    {
        this.createStream = createStream;
        this.versionKey = versionKey;
    }

    public async Task Output(AppApiTemplate appTemplate)
    {
        await new AppClassGenerator(createStream, appTemplate, versionKey).Output();
        var formTemplates = appTemplate.FormTemplates(ApiCodeGenerators.TypeScript);
        var complexFieldTemplates = formTemplates.SelectMany(ft => ft.Form.ComplexFieldTemplates).Distinct();
        foreach (var complexFieldTemplate in complexFieldTemplates)
        {
            await new ComplexFieldClassGenerator(createStream, complexFieldTemplate, false).Output();
            await new ComplexFieldViewClassGenerator(createStream, complexFieldTemplate, false).Output();
        }
        foreach (var formTemplate in formTemplates)
        {
            await new ComplexFieldClassGenerator(createStream, formTemplate.Form, true).Output();
            await new ComplexFieldViewClassGenerator(createStream, formTemplate.Form, true).Output();
        }
        foreach (var group in appTemplate.GroupTemplates.Where(g => !g.IsODataGroup() && !g.IsUser() && !g.IsUserCache()))
        {
            await new ApiGroupClassGenerator(createStream, group).Output();
        }
        foreach (var group in appTemplate.GroupTemplates.Where(g => g.IsODataGroup()))
        {
            await new ODataColumnsBuilderGenerator(createStream, group.QueryableTemplates().First()).Output();
        }
        await new EntityGenerator(createStream, appTemplate).Output();
        foreach (var numericValueTemplate in appTemplate.NumericValueTemplates(ApiCodeGenerators.TypeScript))
        {
            await new NumericValueClassGenerator(createStream, numericValueTemplate).Output();
        }
        foreach (var enumValueTemplate in appTemplate.EnumValueTemplates(ApiCodeGenerators.TypeScript))
        {
            await new EnumGenerator(createStream, enumValueTemplate).Output();
        }
    }
}