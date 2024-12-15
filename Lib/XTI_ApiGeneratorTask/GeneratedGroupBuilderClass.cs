using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Generic;
using System.Linq;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace XTI_ApiGeneratorTask
{
    public sealed class GeneratedGroupBuilderClass
    {
        private readonly GroupDefinition group;
        private readonly string ns;
        private readonly string className;

        public GeneratedGroupBuilderClass(GroupDefinition group, string baseNS)
        {
            this.group = group;
            ns = $"{baseNS}.{group.Name}";
            className = group.BuilderClassName;
        }

        public ClassDefinition Value()
        {
            var contents = GenerateCode()
                .NormalizeWhitespace(elasticTrivia: true)
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
                    group.Namespaces().Select(u => UsingDirective(IdentifierName(u))).ToArray()
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
                            MembersForClass()
                        )
                    )
            };
        }

        private MemberDeclarationSyntax[] MembersForClass()
        {
            var members = new List<MemberDeclarationSyntax>
            {
                DeclarationForSourceField(),
                DeclarationForCtor(),
                GeneratedConfigureMethod.Declaration()
            };
            foreach (var action in group.Actions)
            {
                members.Add(DeclarationForAction(action));
            }
            members.Add(DeclarationForBuildMethod());
            return members.ToArray();
        }

        private FieldDeclarationSyntax DeclarationForSourceField()
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
                            new[] { AssignmentForSource() }
                            .Union(AssignmentsForAction())
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

        private StatementSyntax[] AssignmentsForAction()
        {
            var statements = new List<StatementSyntax>();
            foreach (var action in group.Actions)
            {
                var invocation = string.IsNullOrWhiteSpace(action.ValidationClassName) ?
                    InvocationForAddActionWithExecution(action) :
                    InvocationForAddActionWithExecutionAndValidation(action);
                statements.Add
                (
                    ExpressionStatement
                    (
                        AssignmentExpression
                        (
                            SyntaxKind.SimpleAssignmentExpression,
                            IdentifierName(action.Name),
                            invocation
                        )
                    )
                );
            }
            return statements.ToArray();
        }

        private static InvocationExpressionSyntax InvocationForAddActionWithExecutionAndValidation(ActionDefinition action)
        {
            return InvocationExpression
            (
                MemberAccessExpression
                (
                    SyntaxKind.SimpleMemberAccessExpression,
                    InvocationForAddActionWithExecution(action),
                    GenericName(Identifier("WithValidation"))
                        .WithTypeArgumentList
                        (
                            TypeArgumentList
                            (
                                SingletonSeparatedList
                                (
                                    (TypeSyntax)IdentifierName(action.ValidationClassName)
                                )
                            )
                        )
                )
            );
        }

        private static InvocationExpressionSyntax InvocationForAddActionWithExecution(ActionDefinition action)
        {
            return InvocationExpression
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
            );
        }

        private PropertyDeclarationSyntax DeclarationForAction(ActionDefinition action)
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

        private MethodDeclarationSyntax DeclarationForBuildMethod()
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
