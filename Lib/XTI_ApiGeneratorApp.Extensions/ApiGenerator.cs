using Microsoft.Extensions.Options;
using XTI_App.Abstractions;
using XTI_App.Api;
using XTI_WebApp.ClientGenerator.CSharp;
using XTI_WebApp.ClientGenerator.Typescript;
using XTI_WebApp.ControllerGenerator;

namespace XTI_ApiGeneratorApp.Extensions;

public sealed class ApiGenerator
{
    private readonly AppVersionKey versionKey;
    private readonly OutputOptions options;

    public ApiGenerator(IOptions<OutputOptions> options)
    {
        this.options = options.Value;
        versionKey = AppVersionKey.Parse(this.options.DefaultVersion);
    }

    public async Task Execute(AppApiTemplate api)
    {
        if (options.TsClient?.Disable == false)
        {
            if (string.IsNullOrWhiteSpace(options.TsClient?.OutputFolder))
            {
                throw new ArgumentException("TsOutputFolder is required");
            }
            if (!Directory.Exists(options.TsClient.OutputFolder))
            {
                throw new ArgumentException($"TS Output Folder {options.TsClient.OutputFolder} does not exist");
            }
            var tsClientToDisk = new CodeToDisk(createStream => new TsClient(versionKey, createStream), options.TsClient.OutputFolder);
            await tsClientToDisk.Output(api);
        }
        if (options.CsController?.Disable == false)
        {
            if (string.IsNullOrWhiteSpace(options.CsController?.OutputFolder))
            {
                throw new ArgumentException("CsController OutputFolder is required");
            }
            if (!Directory.Exists(options.CsController.OutputFolder))
            {
                throw new ArgumentException($"CS Controller Output Folder {options.CsController.OutputFolder} does not exist");
            }
            var controllerGenerator = new CodeToDisk
            (
                createStream =>
                    new CsControllers
                    (
                        options.CsController.OutputFolder,
                        createStream
                    ),
                options.CsController.OutputFolder
            );
            await controllerGenerator.Output(api);
        }
        if (options.CsClient?.Disable == false)
        {
            if (string.IsNullOrWhiteSpace(options.CsClient?.OutputFolder))
            {
                throw new ArgumentException("CsClient OutputFolder is required");
            }
            if (!Directory.Exists(options.CsClient.OutputFolder))
            {
                throw new ArgumentException($"CS Client Output Folder {options.CsClient.OutputFolder} does not exist");
            }
            var csClientToDisk = new CodeToDisk
            (
                createStream => new CsClient(versionKey, options.CsClient.OutputFolder, createStream),
                options.CsClient.OutputFolder
            );
            await csClientToDisk.Output(api);
        }
    }
}