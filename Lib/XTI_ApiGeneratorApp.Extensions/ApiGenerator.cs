using Microsoft.Extensions.Options;
using System;
using System.IO;
using System.Threading.Tasks;
using XTI_App;
using XTI_App.Api;
using XTI_WebApp.ClientGenerator.CSharp;
using XTI_WebApp.ClientGenerator.Typescript;
using XTI_WebApp.ControllerGenerator;

namespace XTI_ApiGeneratorApp.Extensions
{
    public sealed class ApiGenerator
    {
        public ApiGenerator(AppFactory appFactory, IOptions<OutputOptions> options)
        {
            this.appFactory = appFactory;
            this.options = options.Value;
        }

        private readonly AppFactory appFactory;
        private readonly OutputOptions options;

        public async Task Execute(AppApiTemplate api)
        {
            if (string.IsNullOrWhiteSpace(options.TsOutputFolder)) { throw new ArgumentException("TsOutputFolder is required"); }
            if (string.IsNullOrWhiteSpace(options.CsClient?.OutputFolder)) { throw new ArgumentException("CsClient OutputFolder is required"); }
            if (string.IsNullOrWhiteSpace(options.CsController?.OutputFolder)) { throw new ArgumentException("CsController OutputFolder is required"); }
            if (!Directory.Exists(options.TsOutputFolder))
            {
                throw new ArgumentException($"TS Output Folder {options.TsOutputFolder} does not exist");
            }
            if (!Directory.Exists(options.CsClient.OutputFolder))
            {
                throw new ArgumentException($"CS Client Output Folder {options.CsClient.OutputFolder} does not exist");
            }
            if (!Directory.Exists(options.CsController.OutputFolder))
            {
                throw new ArgumentException($"CS Controller Output Folder {options.CsController.OutputFolder} does not exist");
            }
            var tsClientToDisk = new CodeToDisk(createStream => new TsClient(appFactory, createStream), options.TsOutputFolder);
            await tsClientToDisk.Output(api);
            var controllerGenerator = new CodeToDisk
            (
                createStream =>
                    new CsControllers
                    (
                        options.CsController.Namespace,
                        options.CsController.AdditionalNamespaces,
                        createStream
                    ),
                options.CsController.OutputFolder
            );
            await controllerGenerator.Output(api);
            var csClientToDisk = new CodeToDisk
            (
                createStream => new CsClient(appFactory, options.CsClient.Namespace, createStream),
                options.CsClient.OutputFolder
            );
            await csClientToDisk.Output(api);
        }
    }
}
