using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Generic;
using System.Linq;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace XTI_ApiGeneratorTask
{
    public sealed class GeneratedApiExtensionsClass
    {
        private readonly AppDefinition app;
        private readonly string ns;
        private readonly string className;

        public GeneratedApiExtensionsClass(AppDefinition app, string ns)
        {
            this.app = app;
            this.ns = ns;
            className = $"{app.Name}ApiExtensions";
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
                                Token(SyntaxKind.StaticKeyword),
                                Token(SyntaxKind.PartialKeyword)
                            }
                        )
                    )
                    .WithMembers
                    (
                        List
                        (
                            DeclarationForClassMembers()
                        )
                    )
            };
        }

        private MemberDeclarationSyntax[] DeclarationForClassMembers()
        {
            var members = new List<MemberDeclarationSyntax>
            {
                DeclarationForAddServicesMethod()
            };
            foreach (var group in app.Groups)
            {
                members.Add(DeclarationForAddGroupServicesMethod(group));
            }
            members.Add(DeclarationForAddMoreServicesMethod());
            return members.ToArray();
        }

        private MethodDeclarationSyntax DeclarationForAddServicesMethod()
        {
            return MethodDeclaration
            (
                PredefinedType(Token(SyntaxKind.VoidKeyword)),
                Identifier($"Add{app.Name}ApiServices")
            )
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
            .WithParameterList
            (
                ParameterList
                (
                    SingletonSeparatedList
                    (
                        Parameter(Identifier("services"))
                            .WithModifiers(TokenList(Token(SyntaxKind.ThisKeyword)))
                            .WithType(IdentifierName("IServiceCollection"))
                    )
                )
            )
            .WithBody
            (
                Block
                (
                    SeparatedList
                    (
                        app.Groups.Select(g => InvocationForAddGroupServicesMethod(g))
                        .Union
                        (
                            new[] 
                            { 
                                InvocationForAddAppFactory(),
                                InvocationForAddMoreServicesMethod() 
                            }
                        )
                    )
                )
            );
        }

        private MethodDeclarationSyntax DeclarationForAddGroupServicesMethod(GroupDefinition group)
        {
            return MethodDeclaration
            (
                PredefinedType(Token(SyntaxKind.VoidKeyword)),
                Identifier($"Add{group.Name}Services")
            )
            .WithModifiers
            (
                TokenList
                (
                    new[]
                    {
                        Token(SyntaxKind.PrivateKeyword),
                        Token(SyntaxKind.StaticKeyword)
                    }
                )
            )
            .WithParameterList
            (
                ParameterList
                (
                    SingletonSeparatedList
                    (
                        Parameter(Identifier("services"))
                            .WithModifiers(TokenList(Token(SyntaxKind.ThisKeyword)))
                            .WithType(IdentifierName("IServiceCollection"))
                    )
                )
            )
            .WithBody
            (
                Block
                (
                    SeparatedList
                    (
                        group.Actions.Select(a => InvocationForAddActionServiceMethod(a))
                    )
                )
            );
        }

        private MethodDeclarationSyntax DeclarationForAddMoreServicesMethod()
        {
            return MethodDeclaration
            (
                PredefinedType(Token(SyntaxKind.VoidKeyword)),
                Identifier("AddMoreServices")
            )
            .WithModifiers
            (
                TokenList
                (
                    new[]
                    {
                        Token(SyntaxKind.StaticKeyword),
                        Token(SyntaxKind.PartialKeyword)
                    }
                )
            )
            .WithParameterList
            (
                ParameterList
                (
                    SingletonSeparatedList
                    (
                        Parameter(Identifier("services"))
                            .WithModifiers(TokenList(Token(SyntaxKind.ThisKeyword)))
                            .WithType(IdentifierName("IServiceCollection"))
                    )
                )
            )
            .WithSemicolonToken(Token(SyntaxKind.SemicolonToken));
        }

        private ExpressionStatementSyntax InvocationForAddGroupServicesMethod(GroupDefinition group)
        {
            return ExpressionStatement
            (
                InvocationExpression
                (
                    MemberAccessExpression
                    (
                        SyntaxKind.SimpleMemberAccessExpression,
                        IdentifierName("services"),
                        IdentifierName($"Add{group.Name}Services")
                    )
                )
            );
        }

        private ExpressionStatementSyntax InvocationForAddActionServiceMethod(ActionDefinition action)
        {
            return ExpressionStatement
            (
                InvocationExpression
                (
                    MemberAccessExpression
                    (
                        SyntaxKind.SimpleMemberAccessExpression,
                        IdentifierName("services"),
                        GenericName("AddService")
                            .WithTypeArgumentList
                            (
                                TypeArgumentList
                                (
                                    SingletonSeparatedList<TypeSyntax>
                                    (
                                        IdentifierName(Identifier(action.ClassName))
                                    )
                                )
                            )
                    )
                )
            );
        }

        private ExpressionStatementSyntax InvocationForAddAppFactory()
        {
            return ExpressionStatement
            (
                InvocationExpression
                (
                    MemberAccessExpression
                    (
                        SyntaxKind.SimpleMemberAccessExpression,
                        IdentifierName("services"),
                        GenericName(Identifier("AddScoped"))
                            .WithTypeArgumentList
                            (
                                TypeArgumentList
                                (
                                    SeparatedList<TypeSyntax>
                                    (
                                        new SyntaxNodeOrToken[]
                                        {
                                            IdentifierName("AppApiFactory"),
                                            Token(SyntaxKind.CommaToken),
                                            IdentifierName($"{app.Name}AppApiFactory")
                                        }
                                    )
                                )
                            )
                        )
                )
            );
        }

        private ExpressionStatementSyntax InvocationForAddMoreServicesMethod()
        {
            return ExpressionStatement
            (
                InvocationExpression
                (
                    MemberAccessExpression
                    (
                        SyntaxKind.SimpleMemberAccessExpression,
                        IdentifierName("services"),
                        IdentifierName("AddMoreServices")
                    )
                )
            );
        }
    }
}
