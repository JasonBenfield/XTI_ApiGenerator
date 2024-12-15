using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace XTI_ApiGeneratorTask
{
    public sealed class GeneratedAppBuilderClass
    {
        private readonly AppDefinition app;
        private readonly string ns;
        private readonly string className;

        public GeneratedAppBuilderClass(AppDefinition app, string ns)
        {
            this.app = app;
            this.ns = ns;
            className = app.BuilderClassName;
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
            .WithUsings
            (
                List
                (
                    app.GetTargetNamespaces(ns)
                        .Select(u => UsingDirective(IdentifierName(u)))
                        .ToArray()
                )
            )
            .WithMembers
            (
                SingletonList<MemberDeclarationSyntax>
                (
                    FileScopedNamespaceDeclaration(IdentifierName(ns))
                        .WithNamespaceKeyword
                        (
                            Token
                            (
                                TriviaList
                                (
                                    new GeneratedCodeComment().Value(),
                                    Trivia
                                    (
                                        NullableDirectiveTrivia
                                        (
                                            Token(SyntaxKind.EnableKeyword),
                                            true
                                        )
                                    )
                                ),
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
                    .WithMembers
                    (
                        List
                        (
                            MemberDeclarationsForClass()
                        )
                    )
            };
        }

        private MemberDeclarationSyntax[] MemberDeclarationsForClass()
        {
            var members = new List<MemberDeclarationSyntax>
            {
                DeclarationForSourceField(),
                DeclarationForServiceProviderField(),
                DeclarationForCtor(),
                GeneratedConfigureMethod.Declaration()
            };
            foreach (var group in app.Groups)
            {
                members.Add(DeclarationForGroupProperty(group));
            }
            foreach (var query in app.Queries)
            {
                members.Add(DeclarationForQueryProperty(query));
            }
            members.Add(DeclarationForBuildMethod());
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

        private FieldDeclarationSyntax DeclarationForSourceField()
        {
            return FieldDeclaration
            (
                VariableDeclaration(IdentifierName("AppApi"))
                    .WithVariables
                    (
                        SingletonSeparatedList
                        (
                            VariableDeclarator(Identifier("source"))
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
                                    .WithType(IdentifierName("IServiceProvider")),
                                Parameter(Identifier("user"))
                                    .WithType(IdentifierName("IAppApiUser"))
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
                            new[] { AssignmentForSource(), AssignmentForServiceProvider() }
                            .Union(AssignmentsForGroupBuilders())
                            .Union(AssignmentsForQueryBuilders())
                            .Union(new[] { GeneratedConfigureMethod.Invocation() })
                        )
                    )
                );
        }

        private StatementSyntax AssignmentForSource()
        {
            return ExpressionStatement
            (
                AssignmentExpression
                (
                    SyntaxKind.SimpleAssignmentExpression,
                    IdentifierName("source"),
                    ObjectCreationExpression
                    (
                        IdentifierName("AppApi")
                    )
                    .WithArgumentList
                    (
                        ArgumentList
                        (
                            SeparatedList
                            (
                                new[]
                                {
                                    Argument(IdentifierName("sp")),
                                    Argument
                                    (
                                        MemberAccessExpression
                                        (
                                            SyntaxKind.SimpleMemberAccessExpression,
                                            IdentifierName($"{app.Name}AppKey"),
                                            IdentifierName("Value")
                                        )
                                    ),
                                    Argument(IdentifierName("user"))
                                }
                            )
                        )
                    )
                )
            );
        }

        private StatementSyntax AssignmentForServiceProvider()
        {
            return ExpressionStatement
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
            );
        }

        private StatementSyntax[] AssignmentsForGroupBuilders()
        {
            var statements = new List<StatementSyntax>();
            foreach (var group in app.Groups)
            {
                statements.Add
                (
                    ExpressionStatement
                    (
                        AssignmentForGroupBuilder(group)
                    )
                );
            }
            return statements.ToArray();
        }

        private static AssignmentExpressionSyntax AssignmentForGroupBuilder(GroupDefinition group)
        {
            return AssignmentExpression
            (
                SyntaxKind.SimpleAssignmentExpression,
                IdentifierName(group.Name),
                ObjectCreationExpression(IdentifierName(group.BuilderClassName))
                    .WithArgumentList
                    (
                        ArgumentList
                        (
                            SingletonSeparatedList
                            (
                                Argument
                                (
                                    InvocationExpression
                                    (
                                        MemberAccessExpression
                                        (
                                            SyntaxKind.SimpleMemberAccessExpression,
                                            IdentifierName("source"),
                                            IdentifierName(Identifier("AddGroup"))
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
                                                        Literal(group.Name)
                                                    )
                                                )
                                            )
                                        )
                                    )
                                )
                            )
                        )
                    )
            );
        }

        private StatementSyntax[] AssignmentsForQueryBuilders()
        {
            var statements = new List<StatementSyntax>();
            foreach (var query in app.Queries)
            {
                statements.Add
                (
                    ExpressionStatement
                    (
                        AssignmentForQueryBuilder(query)
                    )
                );
            }
            return statements.ToArray();
        }

        private static AssignmentExpressionSyntax AssignmentForQueryBuilder(QueryDefinition query)
        {
            return AssignmentExpression
            (
                SyntaxKind.SimpleAssignmentExpression,
                IdentifierName(query.Name),
                InvocationExpression
                (
                    MemberAccessExpression
                    (
                        SyntaxKind.SimpleMemberAccessExpression,
                        ObjectCreationExpression
                        (
                            GenericName(Identifier("ODataGroupBuilder"))
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
                                        InvocationExpression
                                        (
                                            MemberAccessExpression
                                            (
                                                SyntaxKind.SimpleMemberAccessExpression,
                                                IdentifierName("source"),
                                                IdentifierName(Identifier("AddGroup"))
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
                                                            Literal(query.Name)
                                                        )
                                                    )
                                                )
                                            )
                                        )
                                    )
                                )
                            )
                        ),
                        GenericName(Identifier("WithQuery"))
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

        private PropertyDeclarationSyntax DeclarationForGroupProperty(GroupDefinition group)
        {
            return PropertyDeclaration
            (
                IdentifierName(Identifier(group.BuilderClassName)),
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

        private PropertyDeclarationSyntax DeclarationForQueryProperty(QueryDefinition query)
        {
            return PropertyDeclaration
            (
                GenericName(Identifier("ODataGroupBuilder"))
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

        private MethodDeclarationSyntax DeclarationForBuildMethod()
        {
            return MethodDeclaration
            (
                IdentifierName(app.ClassName),
                Identifier("Build")
            )
            .WithModifiers
            (
                TokenList
                (
                    new[]
                    {
                        Token(SyntaxKind.PublicKeyword)
                    }
                )
            )
            .WithExpressionBody
            (
                ArrowExpressionClause
                (
                    ObjectCreationExpression(IdentifierName(app.ClassName))
                        .WithArgumentList
                        (
                            ArgumentList
                            (
                                SeparatedList
                                (
                                    new[]
                                    {
                                        Argument(IdentifierName("source")),
                                        Argument(ThisExpression())
                                    }
                                )
                            )
                        )
                )
            )
            .WithSemicolonToken(Token(SyntaxKind.SemicolonToken));
        }
    }
}
