using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace XTI_WebApp.CodeGeneration.CSharp
{
    public sealed class CSharpFile
    {
        private readonly CompilationUnitSyntax compilationUnitSyntax;
        private readonly Func<string, Stream> createStream;
        private readonly string className;

        public CSharpFile(CompilationUnitSyntax compilationUnitSyntax, Func<string, Stream> createStream, string className)
        {
            this.compilationUnitSyntax = compilationUnitSyntax;
            this.createStream = createStream;
            this.className = className;
        }

        public async Task Output()
        {
            var code = compilationUnitSyntax
                .NormalizeWhitespace()
                .ToFullString();
            using (var writer = createStream($"{className}.cs"))
            {
                var bytes = UTF8Encoding.UTF8.GetBytes(code);
                await writer.WriteAsync(bytes, 0, bytes.Length);
            }
        }
    }
}
