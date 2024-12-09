using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Generic;
using System.Linq;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace XTI_ApiGenerator
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
                    .WithMembers
                    (
                        List
                        (
                            GetClassMembers()
                        )
                    )
            };
        }

        private MemberDeclarationSyntax[] GetClassMembers()
        {
            var members = new List<MemberDeclarationSyntax>
            {
                GetSourceDeclaration(),
                GetCtorDeclaration()
            };
            foreach (var group in app.Groups)
            {
                members.Add(GetGroupDeclaration(group));
            }
            members.Add(GetBuildMethodDeclaration());
            return members.ToArray();
        }

        private FieldDeclarationSyntax GetSourceDeclaration()
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

        private ConstructorDeclarationSyntax GetCtorDeclaration()
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
                        SingletonSeparatedList
                        (
                            Parameter(Identifier("source"))
                                .WithType(IdentifierName("AppApi"))
                        )
                    )
                )
                .WithBody
                (
                    Block
                    (
                        SeparatedList
                        (
                            new[] { GetSourceAssignment() }
                            .Union(GetActionAssignments())
                        )
                    )
                );
        }

        private StatementSyntax GetSourceAssignment()
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
                        IdentifierName("source")
                    ),
                    IdentifierName("source")
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

        private MethodDeclarationSyntax GetBuildMethodDeclaration()
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
