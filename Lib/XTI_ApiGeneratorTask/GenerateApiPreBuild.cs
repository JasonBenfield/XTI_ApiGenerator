using Microsoft.Build.Framework;
using System;
using System.IO;
using System.Linq;

namespace XTI_ApiGeneratorTask
{
    public sealed class GenerateApiPreBuild : Microsoft.Build.Utilities.Task
    {
        [Required]
        public string ApiDirectory { get; set; } = "";

        [Required]
        public string ActionsDirectory { get; set; } = "";

        public string AppName { get; set; } = "";

        public string AppType { get; set; } = "";

        public override bool Execute()
        {
            var ns = new DirectoryInfo(ApiDirectory).Name;
            var parsedNamespace = new ParsedApiNamespace(ns);
            var appName = string.IsNullOrWhiteSpace(AppName) || AppName == "[Default]" ?
                parsedNamespace.AppName :
                "";
            var appType = string.IsNullOrWhiteSpace(AppType) || AppName == "[Default]" ?
                parsedNamespace.AppType :
                "";
            LogCriticalMessage($"Generating API AppName: '{appName}', AppType: '{appType}'");
            if (string.IsNullOrWhiteSpace(appName))
            {
                LogError("App Name is required.");
            }
            if (string.IsNullOrWhiteSpace(appType))
            {
                LogError("App Type is required.");
            }
            try
            {
                var generatedAppDefinition = new GeneratedAppDefinition(appName, appType, ActionsDirectory);
                var appDefinition = generatedAppDefinition.Value();
                var childDirs = Directory.GetDirectories(ApiDirectory)
                    .Where(d => !"bin".Equals(Path.GetFileName(d)) && !"obj".Equals(Path.GetFileName(d)))
                    .ToArray();
                foreach (var childDir in childDirs)
                {
                    DeleteGeneratedFiles(childDir);
                    if (!Directory.GetFiles(childDir).Any() && !Directory.GetDirectories(childDir).Any())
                    {
                        Directory.Delete(childDir);
                    }
                }
                DeleteGeneratedFiles(ApiDirectory);
                foreach (var group in appDefinition.Groups)
                {
                    var groupUsing = $"{ActionsDirectory}.{group.Name}";
                    var groupBuilderClass = new GeneratedGroupBuilderClass(group, ns).Value();
                    OutputClass(groupBuilderClass, group.Name);
                    var groupClass = new GeneratedGroupClass(group, ns).Value();
                    OutputClass(groupClass, group.Name);
                    var groupExtensionsClass = new GeneratedGroupExtensionsClass(group, ns).Value();
                    OutputClass(groupExtensionsClass, group.Name);
                }
                var appBuilderClass = new GeneratedAppBuilderClass(appDefinition, ns).Value();
                OutputClass(appBuilderClass);
                var appKeyClass = new GeneratedAppKeyClass(appDefinition, ns).Value();
                OutputClass(appKeyClass);
                var appClass = new GeneratedAppClass(appDefinition, ns).Value();
                OutputClass(appClass);
                var appFactoryClass = new GeneratedAppFactoryClass(appDefinition, ns).Value();
                OutputClass(appFactoryClass);
                var extensionsClass = new GeneratedApiExtensionsClass(appDefinition, ns).Value();
                OutputClass(extensionsClass);
            }
            catch (Exception ex)
            {
                Log.LogErrorFromException(ex, showStackTrace: true);
            }
            return !Log.HasLoggedErrors;
        }

        private static void DeleteGeneratedFiles(string dir)
        {
            var existingFiles = Directory.GetFiles(dir, "*.Generated.cs");
            foreach (var existingFile in existingFiles)
            {
                File.Delete(existingFile);
            }
        }

        private void LogError(string message)
        {
            Log.LogError
            (
                subcategory: null,
                errorCode: "XTI0100",
                helpKeyword: null,
                file: "",
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

        private void OutputClass(ClassDefinition classDefinition) =>
            OutputClass(classDefinition, "");

        private void OutputClass(ClassDefinition classDefinition, string folderName)
        {
            var fileName = $"{classDefinition.ClassName}.Generated.cs";
            if (!string.IsNullOrWhiteSpace(folderName))
            {
                fileName = Path.Combine(folderName, fileName);
                if (!Directory.Exists(folderName))
                {
                    Directory.CreateDirectory(folderName);
                }
            }
            if (File.Exists(fileName))
            {
                File.Delete(fileName);
            }
            File.WriteAllText(fileName, classDefinition.Contents);
        }

    }

}
