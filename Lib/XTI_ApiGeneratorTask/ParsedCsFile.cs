using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Generic;
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
            var ns = GetNamespace(tree);
            return tree.DescendantNodes()
                .OfType<ClassDeclarationSyntax>()
                .Select(c => GetCsClass(ns, c))
                .ToArray();
        }

        private string GetNamespace(CompilationUnitSyntax tree) =>
            tree.DescendantNodes()
                .OfType<BaseNamespaceDeclarationSyntax>()
                .FirstOrDefault()?
                .Name
                .ToString() ??
                "";


        private CsClass GetCsClass(string ns, ClassDeclarationSyntax classDeclaration)
        {
            var baseClass = GetGenericBaseClass(classDeclaration);
            var baseTypeArguments = baseClass?.TypeArgumentList.Arguments ?? new SeparatedSyntaxList<TypeSyntax>();
            var baseTypeArgs = new List<string>();
            foreach (var baseTypeArgument in baseTypeArguments)
            {
                baseTypeArgs.Add($"{baseTypeArgument}");
            }
            return new CsClass
            (
                ns: ns,
                filePath: filePath,
                className: classDeclaration.Identifier.Text,
                baseClassName: baseClass?.Identifier.Text ?? "",
                baseClassTypeArgs: baseTypeArgs.ToArray()
            );
        }

        private static GenericNameSyntax GetGenericBaseClass(ClassDeclarationSyntax classDeclaration)
        {
            var baseType = classDeclaration.BaseList?.Types.FirstOrDefault() as SimpleBaseTypeSyntax;
            return baseType?.Type as GenericNameSyntax;
        }
    }
}
