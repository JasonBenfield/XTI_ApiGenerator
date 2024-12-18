using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Generic;
using System.Linq;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace XTI_ApiGeneratorTask
{
    public sealed class GeneratedGroupExtensionsClass
    {
        private readonly GroupDefinition group;
        private readonly string ns;
        private readonly string className;

        public GeneratedGroupExtensionsClass(GroupDefinition group, string ns)
        {
            this.group = group;
            this.ns = ns;
            className = $"{group.Name}GroupExtensions";
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
                                Token(SyntaxKind.InternalKeyword),
                                Token(SyntaxKind.StaticKeyword),
                                Token(SyntaxKind.PartialKeyword)
                            }
                        )
                    )
                    .WithMembers
                    (
                        SingletonList<MemberDeclarationSyntax>
                        (
                            DeclarationForAddGroupServicesMethod(group)
                        )
                    )
            };
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
                        Token(SyntaxKind.InternalKeyword),
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
                        group.Actions.SelectMany(a => InvocationsForAddActionServiceMethod(a))
                    )
                )
            );
        }

        private ExpressionStatementSyntax[] InvocationsForAddActionServiceMethod(ActionDefinition action)
        {
            var invocations = new List<ExpressionStatementSyntax>();
            var addAction = ExpressionStatement
            (
                InvocationExpression
                (
                    MemberAccessExpression
                    (
                        SyntaxKind.SimpleMemberAccessExpression,
                        IdentifierName("services"),
                        GenericName("AddScoped")
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
            invocations.Add(addAction);
            if (!string.IsNullOrWhiteSpace(action.ValidationClassName))
            {
                var addValidation = ExpressionStatement
                (
                    InvocationExpression
                    (
                        MemberAccessExpression
                        (
                            SyntaxKind.SimpleMemberAccessExpression,
                            IdentifierName("services"),
                            GenericName("AddScoped")
                                .WithTypeArgumentList
                                (
                                    TypeArgumentList
                                    (
                                        SingletonSeparatedList<TypeSyntax>
                                        (
                                            IdentifierName(Identifier(action.ValidationClassName))
                                        )
                                    )
                                )
                        )
                    )
                );
                invocations.Add(addValidation);
            }
            return invocations.ToArray();
        }

    }
}
