using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Generic;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace XTI_ApiGeneratorTask
{
    public sealed class GeneratedAppFactoryClass
    {
        private readonly AppDefinition app;
        private readonly string ns;
        private readonly string className;

        public GeneratedAppFactoryClass(AppDefinition app, string ns)
        {
            this.app = app;
            this.ns = ns;
            className = $"{app.Name}AppApiFactory";
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
                                Token(SyntaxKind.SealedKeyword)
                            }
                        )
                    )
                    .WithBaseList
                    (
                        BaseList
                        (
                            SingletonSeparatedList<BaseTypeSyntax>
                            (
                                SimpleBaseType(IdentifierName("AppApiFactory"))
                            )
                        )
                    )
                    .WithMembers
                    (
                        List
                        (
                            MembersForClass()
                        )
                    )
            };
        }

        private MemberDeclarationSyntax[] MembersForClass()
        {
            var members = new List<MemberDeclarationSyntax>
            {
                DeclarationForServiceProviderField(),
                DeclarationForCtor(),
                DeclarationForCreateMethod(),
                DeclarationForCreateForSuperUserMethod(),
                DeclarationForProtectedCreateMethod()
            };
            return members.ToArray();
        }

        private FieldDeclarationSyntax DeclarationForServiceProviderField()
        {
            return FieldDeclaration
            (
                VariableDeclaration(IdentifierName("IServiceProvider"))
                    .WithVariables
                    (
                        SingletonSeparatedList
                        (
                            VariableDeclarator(Identifier("sp"))
                        )
                    )
            )
            .WithModifiers
            (
                TokenList
                (
                    new[]
                    {
                        Token(SyntaxKind.PrivateKeyword),
                        Token(SyntaxKind.ReadOnlyKeyword)
                    }
                )
            );
        }

        private ConstructorDeclarationSyntax DeclarationForCtor()
        {
            return
                ConstructorDeclaration(Identifier(className))
                .WithModifiers
                (
                    TokenList(Token(SyntaxKind.PublicKeyword))
                )
                .WithParameterList
                (
                    ParameterList
                    (
                        SeparatedList
                        (
                            new[]
                            {
                                Parameter(Identifier("sp"))
                                    .WithType(IdentifierName("IServiceProvider"))
                            }
                        )
                    )
                )
                .WithBody
                (
                    Block
                    (
                        SeparatedList
                        (
                            new[]
                            {
                                ExpressionStatement
                                (
                                    AssignmentExpression
                                    (
                                        SyntaxKind.SimpleAssignmentExpression,
                                        MemberAccessExpression
                                        (
                                            SyntaxKind.SimpleMemberAccessExpression,
                                            ThisExpression(),
                                            IdentifierName("sp")
                                        ),
                                        IdentifierName("sp")
                                    )
                                )
                            }
                        )
                    )
                );
        }

        private MemberDeclarationSyntax DeclarationForCreateMethod()
        {
            return MethodDeclaration
            (
                IdentifierName(app.ClassName),
                Identifier("Create")
            )
            .WithModifiers
            (
                TokenList
                (
                    new[]
                    {
                        Token(SyntaxKind.PublicKeyword),
                        Token(SyntaxKind.NewKeyword)
                    }
                )
            )
            .WithParameterList
            (
                ParameterList
                (
                    SingletonSeparatedList
                    (
                        Parameter(Identifier("user"))
                            .WithType(IdentifierName("IAppApiUser"))
                    )
                )
            )
            .WithExpressionBody
            (
                ArrowExpressionClause
                (
                    CastExpression
                    (
                        IdentifierName(app.ClassName),
                        InvocationExpression
                        (
                            MemberAccessExpression
                            (
                                SyntaxKind.SimpleMemberAccessExpression,
                                BaseExpression(),
                                IdentifierName("Create")
                            )
                        )
                        .WithArgumentList
                        (
                            ArgumentList
                            (
                                SingletonSeparatedList
                                (
                                    Argument(IdentifierName("user"))
                                )
                            )
                        )
                    )
                )
            )
            .WithSemicolonToken(Token(SyntaxKind.SemicolonToken));
        }

        private MemberDeclarationSyntax DeclarationForCreateForSuperUserMethod()
        {
            return MethodDeclaration
            (
                IdentifierName(app.ClassName),
                Identifier("CreateForSuperUser")
            )
            .WithModifiers
            (
                TokenList
                (
                    new[]
                    {
                        Token(SyntaxKind.PublicKeyword),
                        Token(SyntaxKind.NewKeyword)
                    }
                )
            )
            .WithExpressionBody
            (
                ArrowExpressionClause
                (
                    CastExpression
                    (
                        IdentifierName(app.ClassName),
                        InvocationExpression
                        (
                            MemberAccessExpression
                            (
                                SyntaxKind.SimpleMemberAccessExpression,
                                BaseExpression(),
                                IdentifierName("CreateForSuperUser")
                            )
                        )
                    )
                )
            )
            .WithSemicolonToken(Token(SyntaxKind.SemicolonToken));
        }

        private MethodDeclarationSyntax DeclarationForProtectedCreateMethod()
        {
            return MethodDeclaration
            (
                IdentifierName("IAppApi"),
                Identifier("_Create")
            )
            .WithModifiers
            (
                TokenList
                (
                    new[]
                    {
                        Token(SyntaxKind.ProtectedKeyword),
                        Token(SyntaxKind.OverrideKeyword)
                    }
                )
            )
            .WithParameterList
            (
                ParameterList
                (
                    SingletonSeparatedList
                    (
                        Parameter(Identifier("user"))
                            .WithType(IdentifierName("IAppApiUser"))
                    )
                )
            )
            .WithExpressionBody
            (
                ArrowExpressionClause
                (
                    InvocationExpression
                    (
                        MemberAccessExpression
                        (
                            SyntaxKind.SimpleMemberAccessExpression,
                            ObjectCreationExpression(IdentifierName(app.BuilderClassName))
                                .WithArgumentList
                                (
                                    ArgumentList
                                    (
                                        SeparatedList
                                        (
                                            new[]
                                            {
                                                Argument(IdentifierName("sp")),
                                                Argument(IdentifierName("user"))
                                            }
                                        )
                                    )
                                ),
                            IdentifierName("Build")
                        )
                    )
                )
            )
            .WithSemicolonToken(Token(SyntaxKind.SemicolonToken));
        }

    }
}
