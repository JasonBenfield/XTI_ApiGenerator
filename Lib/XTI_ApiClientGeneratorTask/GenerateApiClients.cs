using Microsoft.Build.Framework;
using System;
using System.Diagnostics;
using System.IO;
using XTI_CoreApiGeneratorTask;

namespace XTI_ApiClientGeneratorTask
{
    public sealed class GenerateApiClients : Microsoft.Build.Utilities.Task
    {
        [Required]
        public string ApiDirectory { get; set; } = "";

        [Required]
        public string ApiAssemblyPath { get; set; } = "";

        [Required]
        public string ToolPath { get; set; } = "";

        public string AppName { get; set; } = "";

        public string AppType { get; set; } = "";

        public string VersionKey { get; set; } = "";

        public bool SkipControllers { get; set; }

        public string ControllersProjectPath { get; set; } = "";

        public bool SkipCsClient { get; set; }

        public string CsClientProjectPath { get; set; } = "";

        public bool SkipTsClient { get; set; }

        public string TsClientPath { get; set; } = "";

        public override bool Execute()
        {
            if (!Directory.Exists(ApiDirectory))
            {
                LogError($"API Directory '{ApiDirectory}' does not exist.");
                return false;
            }
            var ns = new DirectoryInfo(ApiDirectory).Name;
            var parsedNamespace = new ParsedApiNamespace(ns);
            var appType = string.IsNullOrWhiteSpace(AppType) || AppName == "[Default]" ?
                parsedNamespace.AppType :
                "";
            if (appType.Equals("WebApp"))
            {
                var appName = string.IsNullOrWhiteSpace(AppName) || AppName == "[Default]" ?
                    parsedNamespace.AppName :
                    "";
                ExecutePostBuild(appName, appType, parsedNamespace);
            }
            return !Log.HasLoggedErrors;
        }

        private void ExecutePostBuild(string appName, string appType, ParsedApiNamespace parsedNamespace)
        {
            LogCriticalMessage($"Generating API Clients AppName: '{appName}', AppType: '{appType}', Tool Path: '{ToolPath}'");
            try
            {
                if (string.IsNullOrWhiteSpace(appName))
                {
                    throw new Exception("App Name is required.");
                }
                if (string.IsNullOrWhiteSpace(ToolPath))
                {
                    throw new Exception("Tool Path is required.");
                }
                if (!File.Exists(ToolPath))
                {
                    throw new Exception($"Tool Path '{ToolPath}' was not found.");
                }
                if (string.IsNullOrWhiteSpace(ApiAssemblyPath))
                {
                    throw new Exception("API Output Path is required.");
                }
                if (!File.Exists(ApiAssemblyPath))
                {
                    throw new Exception($"API Output Path '{ApiAssemblyPath}' was not found.");
                }
                var versionKey = string.IsNullOrWhiteSpace(VersionKey) ?
                    "Current" :
                    VersionKey;
                var skipControllers = SkipControllers ? " --SkipControllers" : "";
                var controllersProjectPath = string.IsNullOrWhiteSpace(ControllersProjectPath) || ControllersProjectPath == "[Default]" ?
                    Path.GetFullPath(Path.Combine(ApiDirectory, "..", $"{appName}WebApp.ApiControllers")) :
                    "";
                var skipCsClient = SkipCsClient ? " --SkipCsClient" : "";
                var csClientProjectPath = string.IsNullOrWhiteSpace(CsClientProjectPath) || CsClientProjectPath == "[Default]" ?
                    Path.GetFullPath(Path.Combine(ApiDirectory, "..", "..", "Lib", $"{parsedNamespace.Prefix}{appName}AppClient")) :
                    "";
                var skipTsClient = SkipTsClient ? " --SkipTsClient" : "";
                var tsClientPath = string.IsNullOrWhiteSpace(TsClientPath) || TsClientPath == "[Default]" ?
                    Path.GetFullPath(Path.Combine(ApiDirectory, "..", "..", "Apps", $"{appName}WebApp", "Scripts", "Lib", "Http")) :
                    "";
                var startInfo = new ProcessStartInfo
                (
                    ToolPath,
                    $"--AssemblyPath \"{ApiAssemblyPath}\" --AssemblyNamespace {parsedNamespace.Namespace} --FactoryClassName {appName}AppApiFactory --VersionKey {versionKey}{skipControllers} --ControllersProjectPath \"{controllersProjectPath}\"{skipCsClient} --CsClientProjectPath \"{csClientProjectPath}\"{skipTsClient} --TsClientPath \"{tsClientPath}\""
                )
                {
                    WindowStyle = ProcessWindowStyle.Hidden,
                    RedirectStandardError = true,
                    UseShellExecute = false
                };
                var process = Process.Start(startInfo);
                process.WaitForExit(60000);
                if (process.HasExited && process.ExitCode != 0)
                {
                    var processOutput = process.StandardError.ReadToEnd();
                    throw new Exception($"API Generator Tool failed with exit code {process.ExitCode}\r\n{processOutput}");
                }
            }
            catch (ApiGeneratorTaskException taskEx)
            {
                foreach (var error in taskEx.Errors)
                {
                    LogError(message: error.Message, errorCode: error.ErrorCode, file: error.FilePath);
                }
            }
            catch (Exception ex)
            {
                Log.LogErrorFromException
                (
                    ex,
                    showStackTrace: true,
                    showDetail: true,
                    file: null
                );
            }
        }

        private void LogError(string message, string errorCode = "XTI0100", string file = null)
        {
            Log.LogError
            (
                subcategory: null,
                errorCode: errorCode,
                helpKeyword: null,
                file: file,
                lineNumber: 0,
                columnNumber: 0,
                endLineNumber: 0,
                endColumnNumber: 0,
                message: message
            );
        }

        private void LogCriticalMessage(string message)
        {
            Log.LogCriticalMessage
            (
                subcategory: "",
                code: "XTI0001",
                helpKeyword: null,
                file: "MyTask",
                lineNumber: 0,
                columnNumber: 0,
                endLineNumber: 0,
                endColumnNumber: 0,
                message: message
            );
        }
    }

}
