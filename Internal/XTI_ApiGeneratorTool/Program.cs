using CommandLine;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using XTI_ApiGeneratorTool;
using XTI_App.Abstractions;
using XTI_App.Api;
using XTI_WebApp.ClientGenerator.CSharp;
using XTI_WebApp.ClientGenerator.Typescript;
using XTI_WebApp.ControllerGenerator;

var options = new ToolOptions();
Parser.Default.ParseArguments<ToolOptions>(args)
    .WithParsed
    (
        o =>
        {
            options = o;
        }
    );
try
{
    if (string.IsNullOrWhiteSpace(options.AssemblyPath))
    {
        throw new Exception("Assembly Path is required");
    }
    if (!File.Exists(options.AssemblyPath))
    {
        throw new Exception($"Assembly Path '{options.AssemblyPath}' was not found.");
    }
    if (string.IsNullOrWhiteSpace(options.FactoryClassName))
    {
        throw new Exception("Factory Class Name is required");
    }
    if (string.IsNullOrWhiteSpace(options.AssemblyNamespace))
    {
        throw new Exception("Assembly Namespace is required");
    }
    var asm = Assembly.LoadFrom(options.AssemblyPath);
    var factoryTypeName = $"{options.AssemblyNamespace}.{options.FactoryClassName}";
    var factoryType = asm.GetType(factoryTypeName);
    if (factoryType == null)
    {
        throw new Exception($"Factory type '{factoryTypeName}' was not found.");
    }
    var sp = new ServiceCollection().BuildServiceProvider();
    var factory = (AppApiFactory?)Activator.CreateInstance
    (
        factoryType,
        [sp]
    );
    if (factory == null)
    {
        throw new NullReferenceException("factory cannot be null");
    }
    var template = factory.CreateTemplate();
    var versionKeyText = Environment.GetEnvironmentVariable("XtiVersion");
    if (string.IsNullOrWhiteSpace(versionKeyText))
    {
        versionKeyText = "Current";
    }
    var versionKey = AppVersionKey.Parse(versionKeyText);
    if (!options.SkipControllers)
    {
        if (string.IsNullOrWhiteSpace(options.ControllersProjectPath))
        {
            throw new Exception("Controllers Project Path is required.");
        }
        if (!Directory.Exists(options.ControllersProjectPath))
        {
            throw new Exception($"Controllers Project Path '{options.ControllersProjectPath}' was not found.");
        }
        var controllerGenerator = new CodeToDisk
        (
            createStream =>
                new CsControllers
                (
                    options.ControllersProjectPath,
                    createStream
                ),
            options.ControllersProjectPath
        );
        await controllerGenerator.Output(template);
    }
    if (!options.SkipCsClient)
    {
        if (string.IsNullOrWhiteSpace(options.CsClientProjectPath))
        {
            throw new Exception("C# Client Project Path is required.");
        }
        if (!Directory.Exists(options.CsClientProjectPath))
        {
            throw new Exception($"C# Client Project Path '{options.CsClientProjectPath}' was not found.");
        }
        var csClientToDisk = new CodeToDisk
        (
            createStream => new CsClient
            (
                versionKey,
                options.CsClientProjectPath,
                true,
                createStream
            ),
            options.CsClientProjectPath
        );
        await csClientToDisk.Output(template);
    }
    if (!options.SkipTsClient)
    {
        if (string.IsNullOrWhiteSpace(options.TsClientPath))
        {
            throw new Exception("Typescript Client Path is required.");
        }
        if (!Directory.Exists(options.TsClientPath))
        {
            throw new Exception($"Typescript Client Path '{options.TsClientPath}' was not found.");
        }
        var tsClientToDisk = new CodeToDisk
        (
            createStream => new TsClient(createStream, versionKey),
            options.TsClientPath
        );
        await tsClientToDisk.Output(template);
    }
}
catch (Exception ex)
{
    Console.Error.WriteLine(ex.ToString());
    Environment.ExitCode = -999;
}
