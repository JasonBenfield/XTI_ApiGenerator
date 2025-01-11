using Microsoft.Build.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
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

        public string SolutionDirectory { get; set; } = "";

        public string RepoOwner { get; set; } = "";

        public string RepoName { get; set; } = "";

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
            var appType = IsValueRequired(AppType) ?
                parsedNamespace.AppType :
                AppType;
            if (appType.Equals("WebApp"))
            {
                var appName = IsValueRequired(AppName) ?
                    parsedNamespace.AppName :
                    AppName;
                ExecutePostBuild(appName, appType, parsedNamespace);
            }
            return !Log.HasLoggedErrors;
        }

        private void ExecutePostBuild(string appName, string appType, ParsedApiNamespace parsedNamespace)
        {
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
                    throw new Exception("API Assembly Path is required.");
                }
                if (!File.Exists(ApiAssemblyPath))
                {
                    throw new Exception($"API Assembly Path '{ApiAssemblyPath}' was not found.");
                }
                var solutionDirectory = IsValueRequired(SolutionDirectory) ?
                    GetSolutionDirectory(ApiAssemblyPath) :
                    SolutionDirectory;
                if (string.IsNullOrWhiteSpace(solutionDirectory))
                {
                    throw new Exception("Solution Directory is required");
                }
                if (!Directory.Exists(solutionDirectory))
                {
                    throw new Exception($"Solution Directory '{solutionDirectory}' was not found.");
                }
                var repoOwner = IsValueRequired(RepoOwner) ? GetRepoOwner(solutionDirectory) : RepoOwner;
                if (string.IsNullOrWhiteSpace(repoOwner))
                {
                    throw new Exception("Repo Owner is required");
                }
                var repoName = IsValueRequired(RepoName) ? GetRepoName(solutionDirectory) : RepoName;
                if (string.IsNullOrWhiteSpace(repoName))
                {
                    throw new Exception("Repo Name is required");
                }
                var args = new List<string>
                {
                    $"--AssemblyPath \"{ApiAssemblyPath}\"",
                    $"--AssemblyNamespace {parsedNamespace.Namespace}",
                    $"--FactoryClassName {appName}AppApiFactory",
                    $"--SolutionDirectory {solutionDirectory}",
                    $"--RepoOwner {repoOwner}",
                    $"--RepoName {repoName}"
                };
                if (SkipControllers)
                {
                    args.Add("--SkipControllers");
                }
                else
                {
                    var controllersProjectPath = IsValueRequired(ControllersProjectPath) ?
                        Path.GetFullPath(Path.Combine(ApiDirectory, "..", $"{appName}WebApp.ApiControllers")) :
                        ControllersProjectPath;
                    args.Add($"--ControllersProjectPath \"{controllersProjectPath}\"");
                }
                if (SkipCsClient)
                {
                    args.Add("--SkipCsClient");
                }
                else
                {
                    var csClientProjectPath = IsValueRequired(CsClientProjectPath) ?
                        Path.GetFullPath(Path.Combine(ApiDirectory, "..", "..", "Lib", $"{parsedNamespace.Prefix}{appName}AppClient")) :
                        CsClientProjectPath;
                    args.Add($"--CsClientProjectPath \"{csClientProjectPath}\"");
                }
                if (SkipTsClient)
                {
                    args.Add("--SkipTsClient");
                }
                else
                {
                    var tsClientPath = IsValueRequired(TsClientPath) ?
                        Path.GetFullPath(Path.Combine(ApiDirectory, "..", "..", "Apps", $"{appName}WebApp", "Scripts", "Lib", "Http")) :
                        TsClientPath;
                    args.Add($"--TsClientPath \"{tsClientPath}\"");
                }
                var toolPath = Path.GetFullPath(ToolPath);
                var joinedArgs = string.Join(" ", args);
                LogCriticalMessage
                (
                    $"Running {toolPath} {joinedArgs}"
                );
                var startInfo = new ProcessStartInfo
                (
                    toolPath,
                    joinedArgs
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
                    throw new Exception($"API Client Generator Tool failed with exit code {process.ExitCode}\r\n{processOutput}");
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

        private bool IsValueRequired(string directory) =>
            string.IsNullOrWhiteSpace(directory) || directory == "[Default]";

        private string GetSolutionDirectory(string directory)
        {
            string solutionDirectory;
            if (string.IsNullOrWhiteSpace(directory))
            {
                solutionDirectory = "";
            }
            else if (Directory.GetFiles(directory, "*.sln").Any())
            {
                solutionDirectory = directory;
            }
            else
            {
                solutionDirectory = GetSolutionDirectory(Directory.GetParent(directory).FullName);
            }
            return solutionDirectory;
        }

        private string GetRepoOwner(string solutionDirectory) =>
            new DirectoryInfo(solutionDirectory).Parent.Name;

        private string GetRepoName(string solutionDirectory) =>
            new DirectoryInfo(solutionDirectory).Name;

        private void LogError(string message, string errorCode = "XTI0200", string file = null)
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
                code: "XTI0002",
                helpKeyword: null,
                file: "ApiClientGenerator",
                lineNumber: 0,
                columnNumber: 0,
                endLineNumber: 0,
                endColumnNumber: 0,
                message: message
            );
        }
    }

}
