using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace XTI_ApiGenerator
{
    internal sealed class GeneratedActionDefinition
    {
        private readonly string filePath;

        public GeneratedActionDefinition(string filePath)
        {
            this.filePath = filePath;
        }

        public ActionDefinition[] Value()
        {
            var code = File.ReadAllText(filePath);
            var tree = ParseCompilationUnit(code);
            var classDeclarations = tree.DescendantNodes()
                .OfType<ClassDeclarationSyntax>()
                .Where(c => IsAppAction(c))
                .ToArray();
            var actions = new List<ActionDefinition>();
            foreach (var classDeclaration in classDeclarations)
            {
                var className = classDeclaration.Identifier.Text;
                var name = GetName(className);
                var genericName = GetGenericName(classDeclaration);
                var requestNameSyntax = genericName.TypeArgumentList.Arguments.FirstOrDefault() as IdentifierNameSyntax;
                var requestDataName = requestNameSyntax?.Identifier.Text ?? "";
                var resultNameSyntax = genericName.TypeArgumentList.Arguments.LastOrDefault() as IdentifierNameSyntax;
                var resultDataName = resultNameSyntax?.Identifier.Text ?? "";
                var action = new ActionDefinition(name, className, requestDataName, resultDataName);
                actions.Add(action);
            }
            return actions
                .Where(a => !a.IsEmpty())
                .ToArray();
        }

        private static readonly Regex classNameRegex = new Regex("^(?<Name>([a-z]|\\d)+)(Action|Page)$", RegexOptions.IgnoreCase);

        private static string GetName(string className)
        {
            string name;
            var match = classNameRegex.Match(className);
            if (match.Success)
            {
                name = match.Groups["Name"].Value;
            }
            else
            {
                name = className;
            }
            return name;
        }

        private bool IsAppAction(ClassDeclarationSyntax classDeclaration)
        {
            var genericName = GetGenericName(classDeclaration);
            return genericName?.Identifier.Text == "AppAction";
        }

        private static GenericNameSyntax GetGenericName(ClassDeclarationSyntax classDeclaration)
        {
            var baseType = classDeclaration.BaseList.Types.FirstOrDefault() as SimpleBaseTypeSyntax;
            var genericName = baseType?.Type as GenericNameSyntax;
            return genericName;
        }
    }
}
