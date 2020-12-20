using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using XTI_App.Api;
using XTI_WebApp.CodeGeneration.CSharp;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace XTI_WebApp.ClientGenerator.CSharp
{
    public sealed class NumericValueClass
    {
        private readonly string ns;
        private readonly Func<string, Stream> createStream;
        private readonly NumericValueTemplate template;

        public NumericValueClass(string ns, Func<string, Stream> createStream, NumericValueTemplate template)
        {
            this.ns = ns;
            this.createStream = createStream;
            this.template = template;
        }

        public async Task Output()
        {
            var apiObject = declareClass();
            var className = template.DataType.Name;
            await outputClass(apiObject, className);
        }

        private CompilationUnitSyntax declareClass()
        {
            return CompilationUnit()
                .WithUsings
                (
                    SingletonList
                    (
                        UsingDirective(IdentifierName("XTI_WebAppClient"))
                            .WithUsingKeyword
                            (
                                Token
                                (
                                    TriviaList(Comment("// Generated Code")),
                                    SyntaxKind.UsingKeyword,
                                    TriviaList()
                                )
                            )
                    )
                )
                .WithMembers
                (
                    SingletonList<MemberDeclarationSyntax>
                    (
                        NamespaceDeclaration(IdentifierName(ns))
                            .WithMembers
                            (
                                SingletonList<MemberDeclarationSyntax>
                                (
                                    ClassDeclaration(template.DataType.Name)
                                        .WithModifiers
                                        (
                                            TokenList
                                            (
                                                new[]
                                                {
                                                    Token(SyntaxKind.PublicKeyword),
                                                    Token(SyntaxKind.SealedKeyword)
                                                }
                                            )
                                        )
                                        .WithBaseList
                                        (
                                            BaseList
                                            (
                                                SingletonSeparatedList<BaseTypeSyntax>
                                                (
                                                    SimpleBaseType
                                                    (
                                                        IdentifierName("NumericValue")
                                                    )
                                                )
                                            )
                                        )
                                        .WithMembers
                                        (
                                            List
                                            (
                                                new MemberDeclarationSyntax[]
                                                {
                                                    ClassDeclaration($"{template.DataType.Name}s")
                                                        .WithModifiers
                                                        (
                                                            TokenList
                                                            (
                                                                new []
                                                                {
                                                                    Token(SyntaxKind.PublicKeyword),
                                                                    Token(SyntaxKind.SealedKeyword)
                                                                }
                                                            )
                                                        )
                                                        .WithBaseList
                                                        (
                                                            BaseList
                                                            (
                                                                SingletonSeparatedList<BaseTypeSyntax>
                                                                (
                                                                    SimpleBaseType
                                                                    (
                                                                        GenericName
                                                                        (
                                                                            Identifier("NumericValues")
                                                                        )
                                                                        .WithTypeArgumentList
                                                                        (
                                                                            TypeArgumentList
                                                                            (
                                                                                SingletonSeparatedList<TypeSyntax>
                                                                                (
                                                                                    IdentifierName(template.DataType.Name)
                                                                                )
                                                                            )
                                                                        )
                                                                    )
                                                                )
                                                            )
                                                        )
                                                        .WithMembers
                                                        (
                                                            List
                                                            (
                                                                new MemberDeclarationSyntax[]
                                                                {
                                                                    declareNumericValuesConstructor()
                                                                }
                                                                .Union
                                                                (
                                                                    declareProperties()
                                                                )
                                                            )
                                                        ),
                                                        FieldDeclaration
                                                        (
                                                            VariableDeclaration
                                                            (
                                                                IdentifierName($"{template.DataType.Name}s")
                                                            )
                                                            .WithVariables
                                                            (
                                                                SingletonSeparatedList
                                                                (
                                                                    VariableDeclarator(Identifier("Values"))
                                                                        .WithInitializer
                                                                        (
                                                                            EqualsValueClause
                                                                            (
                                                                                ObjectCreationExpression
                                                                                (
                                                                                    IdentifierName($"{template.DataType.Name}s")
                                                                                )
                                                                                .WithArgumentList
                                                                                (
                                                                                    ArgumentList()
                                                                                )
                                                                            )
                                                                        )
                                                                )
                                                            )
                                                        )
                                                        .WithModifiers
                                                        (
                                                            TokenList
                                                            (
                                                                new []
                                                                {
                                                                    Token(SyntaxKind.PublicKeyword),
                                                                    Token(SyntaxKind.StaticKeyword),
                                                                    Token(SyntaxKind.ReadOnlyKeyword)
                                                                }
                                                            )
                                                        ),
                                                        ConstructorDeclaration
                                                        (
                                                            Identifier(template.DataType.Name)
                                                        )
                                                        .WithModifiers
                                                        (
                                                            TokenList(Token(SyntaxKind.PublicKeyword))
                                                        )
                                                        .WithParameterList
                                                        (
                                                            ParameterList
                                                            (
                                                                SeparatedList<ParameterSyntax>
                                                                (
                                                                    new SyntaxNodeOrToken[]
                                                                    {
                                                                        Parameter(Identifier("value"))
                                                                            .WithType
                                                                            (
                                                                                PredefinedType(Token(SyntaxKind.IntKeyword))
                                                                            ),
                                                                            Token(SyntaxKind.CommaToken),
                                                                            Parameter(Identifier("displayText"))
                                                                                .WithType
                                                                                (
                                                                                    PredefinedType(Token(SyntaxKind.StringKeyword))
                                                                                )
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
                                                                    SeparatedList<ArgumentSyntax>
                                                                    (
                                                                        new SyntaxNodeOrToken[]
                                                                        {
                                                                            Argument(IdentifierName("value")),
                                                                            Token(SyntaxKind.CommaToken),
                                                                            Argument(IdentifierName("displayText"))
                                                                        }
                                                                    )
                                                                )
                                                            )
                                                        )
                                                        .WithBody
                                                        (
                                                            Block()
                                                        )
                                                    }
                                                )
                                            )
                                        )
                                )
                            )
                    );
        }

        private MemberDeclarationSyntax declareNumericValuesConstructor()
        {
            return ConstructorDeclaration(Identifier($"{template.DataType.Name}s"))
                .WithModifiers
                (
                    TokenList
                    (
                        Token(SyntaxKind.InternalKeyword)
                    )
                )
                .WithBody
                (
                    Block
                    (
                        assignNumericValues()
                    )
                );
        }

        private IEnumerable<MemberDeclarationSyntax> declareProperties()
        {
            var properties = new List<MemberDeclarationSyntax>();
            foreach (var numericValue in template.Values)
            {
                properties.Add
                (
                    PropertyDeclaration
                    (
                        IdentifierName(template.DataType.Name),
                        Identifier(nameFromDisplayText(numericValue))
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
                                    .WithSemicolonToken
                                    (
                                        Token(SyntaxKind.SemicolonToken)
                                    )
                            )
                        )
                    )
                );
            }
            return properties;
        }

        private static readonly Regex whitespaceRegex = new Regex("\\s+");

        private IEnumerable<StatementSyntax> assignNumericValues()
        {
            var statements = new List<StatementSyntax>();
            foreach (var numericValue in template.Values)
            {
                statements.Add
                (
                    ExpressionStatement
                    (
                        AssignmentExpression
                        (
                            SyntaxKind.SimpleAssignmentExpression,
                            IdentifierName(nameFromDisplayText(numericValue)),
                            InvocationExpression
                            (
                                IdentifierName("Add")
                            )
                            .WithArgumentList
                            (
                                ArgumentList
                                (
                                    SingletonSeparatedList
                                    (
                                        Argument
                                        (
                                            ObjectCreationExpression
                                            (
                                                IdentifierName(template.DataType.Name)
                                            )
                                            .WithArgumentList
                                            (
                                                ArgumentList
                                                (
                                                    SeparatedList<ArgumentSyntax>
                                                    (
                                                        new SyntaxNodeOrToken[]
                                                        {
                                                            Argument
                                                            (
                                                                LiteralExpression
                                                                (
                                                                    SyntaxKind.NumericLiteralExpression,
                                                                    Literal(numericValue.Value)
                                                                )
                                                            ),
                                                            Token(SyntaxKind.CommaToken),
                                                            Argument
                                                            (
                                                                LiteralExpression
                                                                (
                                                                    SyntaxKind.StringLiteralExpression,
                                                                    Literal(numericValue.DisplayText)
                                                                )
                                                            )
                                                        }
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
            return statements;
        }

        private static string nameFromDisplayText(XTI_Core.NumericValue numericValue)
        {
            return whitespaceRegex.Replace(numericValue.DisplayText, "");
        }

        private async Task outputClass(CompilationUnitSyntax compilationUnitSyntax, string className)
        {
            var cSharpFile = new CSharpFile(compilationUnitSyntax, createStream, className);
            await cSharpFile.Output();
        }

    }
}
