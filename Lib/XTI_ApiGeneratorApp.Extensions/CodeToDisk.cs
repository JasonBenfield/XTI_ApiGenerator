using System;
using System.IO;
using System.Threading.Tasks;
using XTI_WebApp.Api;
using XTI_WebApp.CodeGeneration;

namespace XTI_ApiGeneratorApp.Extensions
{
    public sealed class CodeToDisk
    {
        private readonly Func<Func<string, Stream>, CodeGenerator> createCodeGenerator;
        private readonly string folderPath;

        public CodeToDisk(Func<Func<string, Stream>, CodeGenerator> createCodeGenerator, string folderPath)
        {
            this.createCodeGenerator = createCodeGenerator;
            this.folderPath = folderPath;
        }

        public Task Output(AppApiTemplate appTemplate)
        {
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }
            var codeGenerator = createCodeGenerator(createStream);
            return codeGenerator.Output(appTemplate);
        }

        private Stream createStream(string name)
        {
            var filePath = Path.Combine(folderPath, name);
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
            return new FileStream(filePath, FileMode.OpenOrCreate);
        }
    }
}
