using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Generic;
using System.Linq;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace XTI_ApiGenerator
{
    public sealed class GeneratedAppClass
    {
        private readonly AppDefinition app;
        private readonly string ns;
        private readonly string className;

        public GeneratedAppClass(AppDefinition app, string ns)
        {
            this.app = app;
            this.ns = ns;
            className = app.ClassName;
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
                                GetClassDeclaration()
                            )
                        )
                )
            );
        }

        private MemberDeclarationSyntax[] GetClassDeclaration()
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
                                Token(SyntaxKind.SealedKeyword),
                                Token(SyntaxKind.PartialKeyword)
                            }
                        )
                    )
                    .WithBaseList
                    (
                        BaseList
                        (
                            SingletonSeparatedList<BaseTypeSyntax>
                            (
                                SimpleBaseType(IdentifierName(GetBaseTypeName()))
                            )
                        )
                    )
                    .WithMembers
                    (
                        List
                        (
                            GetClassMembers()
                        )
                    )
            };
        }

        private string GetBaseTypeName()
        {
            string baseTypeName;
            if (app.IsWebApp)
            {
                baseTypeName = "WebAppApiWrapper";
            }
            else if (app.IsConsoleApp)
            {
                baseTypeName = "ConsoleAppApiWrapper";
            }
            else
            {
                baseTypeName = "AppApiWrapper";
            }
            return baseTypeName;
        }

        private MemberDeclarationSyntax[] GetClassMembers()
        {
            var members = new List<MemberDeclarationSyntax>
            {
                GetCtorDeclaration()
            };
            foreach (var group in app.Groups)
            {
                members.Add(GetGroupDeclaration(group));
            }
            return members.ToArray();
        }

        private ConstructorDeclarationSyntax GetCtorDeclaration()
        {
            return
                ConstructorDeclaration(Identifier(className))
                .WithModifiers
                (
                    TokenList(Token(SyntaxKind.InternalKeyword))
                )
                .WithParameterList
                (
                    ParameterList
                    (
                        SeparatedList
                        (
                            new[]
                            {
                                Parameter(Identifier("source"))
                                    .WithType(IdentifierName("AppApi")),
                                Parameter(Identifier("builder"))
                                    .WithType(IdentifierName(app.BuilderClassName))
                            }
                        )
                    )
                )
                .WithInitializer
                (
                    ConstructorInitializer
                    (
                        SyntaxKind.BaseConstructorInitializer,
                        ArgumentList
                        (
                            SingletonSeparatedList
                            (
                                Argument(IdentifierName("source"))
                            )
                        )
                    )
                )
                .WithBody
                (
                    Block
                    (
                        SeparatedList
                        (
                            GetActionAssignments()
                        )
                    )
                );
        }

        private StatementSyntax[] GetActionAssignments()
        {
            var statements = new List<StatementSyntax>();
            foreach (var group in app.Groups)
            {
                statements.Add
                (
                    ExpressionStatement
                    (
                        AssignmentExpression
                        (
                            SyntaxKind.SimpleAssignmentExpression,
                            IdentifierName(group.Name),
                            InvocationExpression
                            (
                                MemberAccessExpression
                                (
                                    SyntaxKind.SimpleMemberAccessExpression,
                                    MemberAccessExpression
                                    (
                                        SyntaxKind.SimpleMemberAccessExpression,
                                        IdentifierName("builder"),
                                        IdentifierName(Identifier(group.Name))
                                    ),
                                    IdentifierName("Build")
                                )
                            )
                        )
                    )
                );
            }
            return statements.ToArray();
        }

        private PropertyDeclarationSyntax GetGroupDeclaration(GroupDefinition group)
        {
            return PropertyDeclaration
            (
                IdentifierName(Identifier(group.ClassName)),
                Identifier(group.Name)
            )
            .WithModifiers
            (
                TokenList(Token(SyntaxKind.PublicKeyword))
            )
            .WithAccessorList
            (
                AccessorList
                (
                    SingletonList
                    (
                        AccessorDeclaration(SyntaxKind.GetAccessorDeclaration)
                            .WithSemicolonToken(Token(SyntaxKind.SemicolonToken))
                    )
                )
            );
        }

    }
}
