using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Generic;
using System.Linq;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace XTI_ApiGeneratorTask
{
    public sealed class GeneratedGroupClass
    {
        private readonly GroupDefinition group;
        private readonly string ns;
        private readonly string className;

        public GeneratedGroupClass(GroupDefinition group, string baseNS)
        {
            this.group = group;
            ns = $"{baseNS}.{group.Name}";
            className = group.ClassName;
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
                    .WithBaseList
                    (
                        BaseList
                        (
                            SingletonSeparatedList<BaseTypeSyntax>
                            (
                                SimpleBaseType(IdentifierName("AppApiGroupWrapper"))
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
                DeclarationForCtor(),
                GeneratedConfigureMethod.Declaration()
            };
            foreach (var action in group.Actions)
            {
                members.Add(DeclarationForAction(action));
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
                                    .WithType(IdentifierName("AppApiGroup")),
                                Parameter(Identifier("builder"))
                                    .WithType(IdentifierName(group.BuilderClassName))
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
                            AssignmentsForAction()
                            .Union(new[] { GeneratedConfigureMethod.Invocation() })
                        )
                    )
                );
        }

        private StatementSyntax[] AssignmentsForAction()
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
                                    MemberAccessExpression
                                    (
                                        SyntaxKind.SimpleMemberAccessExpression,
                                        IdentifierName("builder"),
                                        IdentifierName(action.Name)
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

        private PropertyDeclarationSyntax DeclarationForAction(ActionDefinition action)
        {
            return PropertyDeclaration
            (
                GenericName(Identifier("AppApiAction"))
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

    }
}
