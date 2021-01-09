using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using XTI_App.Api;
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

        private static readonly Regex generatedCodeRegex = new Regex("\\s*//\\s*Generated code\\s*", RegexOptions.IgnoreCase);

        public Task Output(AppApiTemplate appTemplate)
        {
            if (!Directory.Exists(folderPath))
            {
                throw new ArgumentException($"Output folder '{folderPath}' does not exist");
            }
            var filePaths = Directory.GetFiles(folderPath)
                .Where(f => f.EndsWith(".ts", StringComparison.OrdinalIgnoreCase) || f.EndsWith(".cs", StringComparison.OrdinalIgnoreCase));
            foreach (var filePath in filePaths)
            {
                string firstLine;
                using (var reader = new StreamReader(filePath))
                {
                    firstLine = reader.ReadLine();
                }
                if (generatedCodeRegex.IsMatch(firstLine))
                {
                    try
                    {
                        File.Delete(filePath);
                        if (filePath.EndsWith(".ts", StringComparison.OrdinalIgnoreCase))
                        {
                            var jsFilePath = filePath.Remove(filePath.Length - 3) + ".js";
                            if (File.Exists(jsFilePath))
                            {
                                File.Delete(jsFilePath);
                            }
                            var jsMapFilePath = filePath.Remove(filePath.Length - 3) + ".js.map";
                            if (File.Exists(jsMapFilePath))
                            {
                                File.Delete(jsMapFilePath);
                            }
                        }
                    }
                    catch
                    {
                    }
                }
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
