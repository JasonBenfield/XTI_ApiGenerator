using Microsoft.Build.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using XTI_CoreApiGeneratorTask;

namespace XTI_ApiGeneratorTask
{
    public sealed class GenerateApi : Microsoft.Build.Utilities.Task
    {
        [Required]
        public string ApiDirectory { get; set; } = "";

        [Required]
        public string ActionsDirectory { get; set; } = "";

        public string AppName { get; set; } = "";

        public string AppType { get; set; } = "";

        public override bool Execute()
        {
            try
            {
                if (!Directory.Exists(ActionsDirectory))
                {
                    throw new Exception($"Actions Directory '{ActionsDirectory}' does not exist.");
                }
                if (string.IsNullOrWhiteSpace(ApiDirectory) || ApiDirectory == "[Default]")
                {
                    var actionsDirectoryName = new DirectoryInfo(ActionsDirectory).Name;
                    if (!actionsDirectoryName.EndsWith("Actions"))
                    {
                        throw new Exception($"API Directory not found from Actions Directory '{ActionsDirectory}'");
                    }
                    ApiDirectory = Path.GetFullPath
                    (
                        Path.Combine
                        (
                            ActionsDirectory,
                            "..",
                            actionsDirectoryName.Remove(actionsDirectoryName.Length - 7)
                        )
                    );
                }
                if (!Directory.Exists(ApiDirectory))
                {
                    throw new Exception($"API Directory '{ApiDirectory}' does not exist.");
                }
                var ns = new DirectoryInfo(ApiDirectory).Name;
                var parsedNamespace = new ParsedApiNamespace(ns);
                var appName = string.IsNullOrWhiteSpace(AppName) || AppName == "[Default]" ?
                    parsedNamespace.AppName :
                    "";
                var appType = string.IsNullOrWhiteSpace(AppType) || AppName == "[Default]" ?
                    parsedNamespace.AppType :
                    "";
                LogCriticalMessage($"Generating API AppName: '{appName}', AppType: '{appType}', ApiDirectory: '{ApiDirectory}', ActionsDirectory: '{ActionsDirectory}'");
                if (string.IsNullOrWhiteSpace(appName))
                {
                    throw new Exception("App Name is required.");
                }
                if (string.IsNullOrWhiteSpace(appType))
                {
                    throw new Exception("App Type is required.");
                }
                var generatedAppDefinition = new GeneratedAppDefinition(appName, appType, ActionsDirectory);
                var appDefinition = generatedAppDefinition.Value();
                var generatedFiles = new List<string>();
                foreach (var group in appDefinition.Groups)
                {
                    var groupUsing = $"{ActionsDirectory}.{group.Name}";
                    var groupBuilderClass = new GeneratedGroupBuilderClass(group, ns).Value();
                    generatedFiles.Add(OutputClass(groupBuilderClass, group.Name));
                    var groupClass = new GeneratedGroupClass(group, ns).Value();
                    generatedFiles.Add(OutputClass(groupClass, group.Name));
                    var groupExtensionsClass = new GeneratedGroupExtensionsClass(group, ns).Value();
                    generatedFiles.Add(OutputClass(groupExtensionsClass, group.Name));
                }
                var appBuilderClass = new GeneratedAppBuilderClass(appDefinition, ns).Value();
                generatedFiles.Add(OutputClass(appBuilderClass));
                var appKeyClass = new GeneratedAppKeyClass(appDefinition, ns).Value();
                generatedFiles.Add(OutputClass(appKeyClass));
                var appClass = new GeneratedAppClass(appDefinition, ns).Value();
                generatedFiles.Add(OutputClass(appClass));
                var appFactoryClass = new GeneratedAppFactoryClass(appDefinition, ns).Value();
                generatedFiles.Add(OutputClass(appFactoryClass));
                var extensionsClass = new GeneratedApiExtensionsClass(appDefinition, ns).Value();
                generatedFiles.Add(OutputClass(extensionsClass));
                var childDirs = Directory.GetDirectories(ApiDirectory)
                    .Where(d => !"bin".Equals(Path.GetFileName(d)) && !"obj".Equals(Path.GetFileName(d)))
                    .ToArray();
                foreach (var childDir in childDirs)
                {
                    DeleteGeneratedFiles(childDir, generatedFiles);
                    if (!Directory.GetFiles(childDir).Any() && !Directory.GetDirectories(childDir).Any())
                    {
                        Directory.Delete(childDir);
                    }
                }
                DeleteGeneratedFiles(ApiDirectory, generatedFiles);
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
            return !Log.HasLoggedErrors;
        }

        private void DeleteGeneratedFiles(string dir, IEnumerable<string> generatedFiles)
        {
            var extraFiles = Directory.GetFiles(dir, "*.Generated.cs")
                .Where(f => !generatedFiles.Any(gf => f.EndsWith(gf, StringComparison.OrdinalIgnoreCase)));
            foreach (var extraFile in extraFiles)
            {
                File.Delete(extraFile);
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

        private string OutputClass(ClassDefinition classDefinition) =>
            OutputClass(classDefinition, "");

        private string OutputClass(ClassDefinition classDefinition, string folderName)
        {
            var targetDirectory = ApiDirectory;
            if (!string.IsNullOrWhiteSpace(folderName))
            {
                targetDirectory = Path.Combine(targetDirectory, folderName);
                if (!Directory.Exists(targetDirectory))
                {
                    Directory.CreateDirectory(targetDirectory);
                }
            }
            var filePath = Path.Combine(targetDirectory, $"{classDefinition.ClassName}.Generated.cs");
            var existingContents = File.Exists(filePath) ?
                File.ReadAllText(filePath) :
                "";
            if (!classDefinition.Contents.Equals(existingContents))
            {
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                }
                File.WriteAllText(filePath, classDefinition.Contents);
            }
            return filePath;
        }

    }

}
