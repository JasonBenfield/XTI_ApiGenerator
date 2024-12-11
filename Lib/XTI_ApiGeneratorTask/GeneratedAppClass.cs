using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Generic;
using System.Linq;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace XTI_ApiGeneratorTask
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
                            MembersForClass()
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

        private MemberDeclarationSyntax[] MembersForClass()
        {
            var members = new List<MemberDeclarationSyntax>
            {
                DeclarationForCtor(),
                GeneratedConfigureMethod.Declaration()
            };
            foreach (var group in app.Groups)
            {
                members.Add(DeclarationForGroup(group));
            }
            foreach (var query in app.Queries)
            {
                members.Add(DeclarationForQuery(query));
            }
            return members.ToArray();
        }

        private ConstructorDeclarationSyntax DeclarationForCtor()
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
                            AssignmentsForGroups()
                            .Union(AssignmentsForQueries())
                            .Union(new[] { GeneratedConfigureMethod.Invocation() })
                        )
                    )
                );
        }

        private StatementSyntax[] AssignmentsForGroups()
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

        private StatementSyntax[] AssignmentsForQueries()
        {
            var statements = new List<StatementSyntax>();
            foreach (var query in app.Queries)
            {
                statements.Add
                (
                    ExpressionStatement
                    (
                        AssignmentExpression
                        (
                            SyntaxKind.SimpleAssignmentExpression,
                            IdentifierName(query.Name),
                            InvocationExpression
                            (
                                MemberAccessExpression
                                (
                                    SyntaxKind.SimpleMemberAccessExpression,
                                    MemberAccessExpression
                                    (
                                        SyntaxKind.SimpleMemberAccessExpression,
                                        IdentifierName("builder"),
                                        IdentifierName(Identifier(query.Name))
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

        private PropertyDeclarationSyntax DeclarationForGroup(GroupDefinition group)
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

        private PropertyDeclarationSyntax DeclarationForQuery(QueryDefinition query)
        {
            return PropertyDeclaration
            (
                GenericName(Identifier("ODataGroup"))
                    .WithTypeArgumentList
                    (
                        TypeArgumentList
                        (
                            SeparatedList<TypeSyntax>
                            (
                                new[]
                                {
                                    IdentifierName(query.RequestDataName),
                                    IdentifierName(query.EntityName)
                                }
                            )
                        )
                    ),
                Identifier(query.Name)
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
