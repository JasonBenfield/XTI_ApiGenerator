﻿using Microsoft.CodeAnalysis;
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
                DeclarationForAddServicesMethod(),
                DeclarationForAddMoreServicesMethod()
            };
            return members.ToArray();
        }

        private MethodDeclarationSyntax DeclarationForAddServicesMethod()
        {
            return MethodDeclaration
            (
                PredefinedType(Token(SyntaxKind.VoidKeyword)),
                Identifier($"Add{app.Name}AppApiServices")
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
                        .Union(new[] { InvocationForAddAppFactory() })
                        .Union
                        (
                            app.Queries.Select(q => InvocationForAddQuery(q))
                        )
                        .Union(new[] { InvocationForAddMoreServicesMethod() })
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

        private ExpressionStatementSyntax InvocationForAddQuery(QueryDefinition query)
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
                                    SingletonSeparatedList<TypeSyntax>
                                    (
                                        IdentifierName
                                        (
                                            new QualifiedClassName
                                            (
                                                query.Namespace, 
                                                query.ClassName
                                            )
                                            .Value
                                        )
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
