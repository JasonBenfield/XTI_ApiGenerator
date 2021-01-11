using Microsoft.CodeAnalysis;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using XTI_App.Api;
using XTI_WebApp.CodeGeneration;

namespace XTI_WebApp.ClientGenerator.CSharp
{
    public sealed class CsClient : CodeGenerator
    {
        private readonly DefaultVersion defaultVersion;
        private readonly string ns;
        private readonly Func<string, Stream> createStream;

        public CsClient
        (
            DefaultVersion defaultVersion,
            string ns,
            Func<string, Stream> createStream
        )
        {
            this.defaultVersion = defaultVersion;
            this.ns = ns;
            this.createStream = createStream;
        }

        private static readonly string[] ModelsToOmit = new[]
        {
            "LoginCredentials",
            "LoginModel",
            "LoginResult",
            "EmptyRequest"
        };

        public async Task Output(AppApiTemplate appTemplate)
        {
            var objTemplates = appTemplate.ObjectTemplates()
                .Where
                (
                    obj =>
                        !ModelsToOmit.Any
                        (
                            m => obj.DataType.Name.Equals(m, StringComparison.OrdinalIgnoreCase)
                        )
                );
            foreach (var objTemplate in objTemplates)
            {
                await new ApiObjectClass(ns, createStream, objTemplate).Output();
            }
            var formTemplates = appTemplate.FormTemplates();
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
            foreach (var numericValueTemplate in appTemplate.NumericValueTemplates())
            {
                await new NumericValueClass(ns, createStream, numericValueTemplate).Output();
            }
            await new ApiAppClass(ns, createStream, appTemplate, defaultVersion).Output();
        }

    }
}
