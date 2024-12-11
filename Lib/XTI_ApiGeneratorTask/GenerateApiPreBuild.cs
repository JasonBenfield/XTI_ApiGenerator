using Microsoft.Build.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace XTI_ApiGeneratorTask
{
    public sealed class GenerateApiPreBuild : Microsoft.Build.Utilities.Task
    {
        [Required]
        public string ApiDirectory { get; set; } = "";

        public override bool Execute()
        {
            var message = $"Generating API for '{ApiDirectory}'";
            LogCriticalMessage(message);
            return !Log.HasLoggedErrors;
        }

        private void LogCriticalMessage(string message)
        {
            Log.LogCriticalMessage
            (
                subcategory: "",
                code: "MSG0001",
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
