using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using XTI_App.Api;
using XTI_WebApp.CodeGeneration.CSharp;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace XTI_WebApp.ClientGenerator.CSharp
{
    public sealed class ApiObjectClass
    {
        private readonly string ns;
        private readonly Func<string, Stream> createStream;
        private readonly ObjectValueTemplate template;

        public ApiObjectClass(string ns, Func<string, Stream> createStream, ObjectValueTemplate template)
        {
            this.ns = ns;
            this.createStream = createStream;
            this.template = template;
        }

        public async Task Output()
        {
            var apiObject = createApiObject();
            var className = template.DataType.Name;
            await outputClass(apiObject, className);
        }

        private CompilationUnitSyntax createApiObject()
        {
            return CompilationUnit()
                .WithUsings
                (
                    List
                    (
                        new[]
                        {
                            UsingDirective(IdentifierName("System"))
                                .WithUsingKeyword
                                (
                                    Token
                                    (
                                        TriviaList(Comment("// Generated Code")),
                                        SyntaxKind.UsingKeyword,
                                        TriviaList()
                                    )
                                ),
                            UsingDirective(IdentifierName("System.Collections.Generic"))
                        }
                    )
                )
                .WithMembers
                (
                    SingletonList<MemberDeclarationSyntax>
                    (
                        NamespaceDeclaration(IdentifierName(ns))
                            .WithMembers
                            (
                                SingletonList<MemberDeclarationSyntax>
                                (
                                    ClassDeclaration(template.DataType.Name)
                                        .WithModifiers
                                        (
                                            TokenList
                                            (
                                                new[]
                                                {
                                                    Token(SyntaxKind.PublicKeyword),
                                                    Token(SyntaxKind.SealedKeyword),
                                                    Token(SyntaxKind.PartialKeyword)
                                                }
                                            )
                                        )
                                        .WithMembers
                                        (
                                            List
                                            (
                                                getProperties(template.PropertyTemplates)
                                            )
                                        )
                                    )
                                )
                            )
                    );
        }

        private static IEnumerable<MemberDeclarationSyntax> getProperties(IEnumerable<ObjectPropertyTemplate> properties)
        {
            return properties.Select
            (
                property =>
                    PropertyDeclaration
                    (
                        new TypeSyntaxFromValueTemplate(property.ValueTemplate).Value(),
                        Identifier(property.Name)
                    )
                    .WithModifiers
                    (
                        TokenList(Token(SyntaxKind.PublicKeyword))
                    )
                    .WithAccessorList
                    (
                        AccessorList
                        (
                            List
                            (
                                new AccessorDeclarationSyntax[]
                                {
                                    AccessorDeclaration(SyntaxKind.GetAccessorDeclaration)
                                        .WithSemicolonToken(Token(SyntaxKind.SemicolonToken)),
                                    AccessorDeclaration(SyntaxKind.SetAccessorDeclaration)
                                        .WithSemicolonToken(Token(SyntaxKind.SemicolonToken))
                                }
                            )
                        )
                    )
            );
        }

        private Task outputClass(CompilationUnitSyntax compilationUnitSyntax, string className)
        {
            var cSharpFile = new CSharpFile(compilationUnitSyntax, createStream, className);
            return cSharpFile.Output();
        }
    }
}
