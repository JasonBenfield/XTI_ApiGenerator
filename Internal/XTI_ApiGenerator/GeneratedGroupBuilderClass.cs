using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Generic;
using System.Linq;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace XTI_ApiGenerator
{
    public sealed class GeneratedGroupBuilderClass
    {
        private readonly GroupDefinition group;
        private readonly string ns;
        private readonly string className;

        public GeneratedGroupBuilderClass(GroupDefinition group, string ns)
        {
            this.group = group;
            this.ns = ns;
            className = group.BuilderClassName;
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
            foreach (var action in group.Actions)
            {
                members.Add(GetActionDeclaration(action));
            }
            members.Add(GetBuildMethodDeclaration());
            return members.ToArray();
        }

        private FieldDeclarationSyntax GetSourceDeclaration()
        {
            return FieldDeclaration
            (
                VariableDeclaration(IdentifierName("AppApiGroup"))
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
                                .WithType(IdentifierName("AppApiGroup"))
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
                                .Union(GetActionInitializations())
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

        private StatementSyntax[] GetActionInitializations()
        {
            var statements = new List<StatementSyntax>();
            foreach (var action in group.Actions)
            {
                statements.Add
                (
                    ExpressionStatement
                    (
                        AssignmentExpression
                        (
                            SyntaxKind.SimpleAssignmentExpression,
                            IdentifierName(action.Name),
                            InvocationExpression
                            (
                                MemberAccessExpression
                                (
                                    SyntaxKind.SimpleMemberAccessExpression,
                                    InvocationExpression
                                    (
                                        MemberAccessExpression
                                        (
                                            SyntaxKind.SimpleMemberAccessExpression,
                                            IdentifierName("source"),
                                            GenericName(Identifier("AddAction"))
                                                .WithTypeArgumentList
                                                (
                                                    TypeArgumentList
                                                    (
                                                        SeparatedList<TypeSyntax>
                                                        (
                                                            new SyntaxNodeOrToken[]
                                                            {
                                                                IdentifierName(action.RequestDataName),
                                                                Token(SyntaxKind.CommaToken),
                                                                IdentifierName(action.ResultDataName)
                                                            }
                                                        )
                                                    )
                                                )
                                        )
                                    ),
                                    GenericName(Identifier("WithExecution"))
                                        .WithTypeArgumentList
                                        (
                                            TypeArgumentList
                                            (
                                                SingletonSeparatedList<TypeSyntax>
                                                (
                                                    IdentifierName(action.ClassName)
                                                )
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
                                            LiteralExpression
                                            (
                                                SyntaxKind.StringLiteralExpression,
                                                Literal(action.Name)
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

        private PropertyDeclarationSyntax GetActionDeclaration(ActionDefinition action)
        {
            return PropertyDeclaration
            (
                GenericName(Identifier("AppApiActionBuilder"))
                    .WithTypeArgumentList
                    (
                        TypeArgumentList
                        (
                            SeparatedList<TypeSyntax>
                            (
                                new SyntaxNodeOrToken[]
                                {
                                    IdentifierName(action.RequestDataName),
                                    Token(SyntaxKind.CommaToken),
                                    IdentifierName(action.ResultDataName)
                                }
                            )
                        )
                    ),
                    Identifier(action.Name)
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
                IdentifierName(group.ClassName),
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
                    ObjectCreationExpression(IdentifierName(group.ClassName))
                        .WithArgumentList
                        (
                            ArgumentList
                            (
                                SingletonSeparatedList
                                (
                                    Argument
                                    (
                                        IdentifierName("source")
                                    )
                                )
                            )
                        )
                )
            )
            .WithSemicolonToken(Token(SyntaxKind.SemicolonToken));
        }
    }
}
