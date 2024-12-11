using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace XTI_ApiGeneratorTask
{
    public sealed class GeneratedAppKeyClass
    {
        private readonly AppDefinition app;
        private readonly string ns;
        private readonly string className;

        public GeneratedAppKeyClass(AppDefinition app, string ns)
        {
            this.app = app;
            this.ns = ns;
            className = $"{app.Name}AppKey";
        }

        public ClassDefinition Value()
        {
            var contents = GenerateCode()
                .NormalizeWhitespace()
                .ToFullString();
            return new ClassDefinition(className, contents);
        }

        private CompilationUnitSyntax GenerateCode()
        {
            return CompilationUnit()
            .WithMembers
            (
                SingletonList<MemberDeclarationSyntax>
                (
                    FileScopedNamespaceDeclaration(IdentifierName(ns))
                        .WithNamespaceKeyword
                        (
                            Token
                            (
                                TriviaList(new GeneratedCodeComment().Value()),
                                SyntaxKind.NamespaceKeyword,
                                TriviaList()
                            )
                        )
                        .WithMembers
                        (
                            List
                            (
                                DeclarationForClass()
                            )
                        )
                )
            );
        }

        private MemberDeclarationSyntax[] DeclarationForClass()
        {
            return new[]
            {
                ClassDeclaration(className)
                    .WithModifiers
                    (
                        TokenList
                        (
                            new[]
                            {
                                Token(SyntaxKind.PublicKeyword),
                                Token(SyntaxKind.StaticKeyword)
                            }
                        )
                    )
                    .WithMembers
                    (
                        List
                        (
                            new MemberDeclarationSyntax[]
                            {
                                DeclarationForValueField()
                            }
                        )
                    )
            };
        }

        private FieldDeclarationSyntax DeclarationForValueField()
        {
            return FieldDeclaration
            (
                VariableDeclaration(IdentifierName("AppKey"))
                    .WithVariables
                    (
                        SingletonSeparatedList
                        (
                            VariableDeclarator(Identifier("Value"))
                                .WithInitializer
                                (
                                    EqualsValueClause
                                    (
                                        InvocationExpression
                                        (
                                            MemberAccessExpression
                                            (
                                                SyntaxKind.SimpleMemberAccessExpression,
                                                IdentifierName("AppKey"),
                                                IdentifierName(GetAppKeyType(app))
                                            )
                                        )
                                        .WithArgumentList
                                        (
                                            ArgumentList
                                            (
                                                SingletonSeparatedList
                                                (
                                                    Argument
                                                    (
                                                        LiteralExpression
                                                        (
                                                            SyntaxKind.StringLiteralExpression,
                                                            Literal(app.Name)
                                                        )
                                                    )
                                                )
                                            )
                                        )
                                    )
                                )
                        )
                    )
            )
            .WithModifiers
            (
                TokenList
                (
                    new[]
                    {
                        Token(SyntaxKind.PublicKeyword),
                        Token(SyntaxKind.StaticKeyword),
                        Token(SyntaxKind.ReadOnlyKeyword)
                    }
                )
            );
        }

        private string GetAppKeyType(AppDefinition app)
        {
            string appType;
            if (app.IsConsoleApp)
            {
                appType = "ConsoleApp";
            }
            else if (app.IsServiceApp)
            {
                appType = "ServiceApp";
            }
            else if (app.IsWebApp)
            {
                appType = "WebApp";
            }
            else
            {
                appType = app.Type;
            }
            return appType;
        }
    }
}
