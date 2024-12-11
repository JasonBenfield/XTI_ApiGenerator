using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.IO;
using System.Linq;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace XTI_ApiGeneratorTask
{
    internal sealed class ParsedCsFile
    {
        private readonly string filePath;

        public ParsedCsFile(string filePath)
        {
            this.filePath = filePath;
        }

        public CsClass[] Classes()
        {
            var code = File.ReadAllText(filePath);
            var tree = ParseCompilationUnit(code);
            return tree.DescendantNodes()
                .OfType<ClassDeclarationSyntax>()
                .Select(c => GetCsClass(c))
                .ToArray();
        }

        private CsClass GetCsClass(ClassDeclarationSyntax classDeclaration)
        {
            var baseClass = GetGenericBaseClass(classDeclaration);
            return new CsClass
            (
                filePath: filePath,
                className: classDeclaration.Identifier.Text,
                baseClassName: baseClass?.Identifier.Text ?? "",
                baseClassTypeArgs: baseClass?.TypeArgumentList.Arguments
                    .OfType<IdentifierNameSyntax>()
                    .Select(a => a.Identifier.Text)
                    .ToArray() ??
                    new string[0]
            );
        }

        private static GenericNameSyntax GetGenericBaseClass(ClassDeclarationSyntax classDeclaration)
        {
            var baseType = classDeclaration.BaseList?.Types.FirstOrDefault() as SimpleBaseTypeSyntax;
            return baseType?.Type as GenericNameSyntax;
        }
    }
}
