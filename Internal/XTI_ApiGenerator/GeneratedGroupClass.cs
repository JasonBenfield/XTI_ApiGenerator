using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Generic;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace XTI_ApiGenerator
{
    public sealed class GeneratedGroupClass
    {
        private readonly GroupDefinition group;
        private readonly string ns;
        private readonly string className;

        public GeneratedGroupClass(GroupDefinition group, string ns)
        {
            this.group = group;
            this.ns = ns;
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
                                SimpleBaseType(IdentifierName("AppApiGroupWrapper"))
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

        private MemberDeclarationSyntax[] GetClassMembers()
        {
            var members = new List<MemberDeclarationSyntax>();
            members.Add(GetCtorDeclaration());
            foreach (var action in group.Actions)
            {
                members.Add(GetActionDeclaration(action));
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
                        SingletonSeparatedList
                        (
                            Parameter(Identifier("source"))
                                .WithType(IdentifierName("AppApiGroup"))
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
                            GetActionInitializations()
                        )
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
                                    IdentifierName("source"),
                                    IdentifierName("Action")
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
