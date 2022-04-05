using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XTI_App.Abstractions;
using XTI_App.Api;
using XTI_WebApp.CodeGeneration.CSharp;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace XTI_WebApp.ClientGenerator.CSharp;

internal sealed class AppVersionClass
{
    private readonly string ns;
    private readonly Func<string, Stream> createStream;
    private readonly AppApiTemplate template;
    private readonly AppVersionKey versionKey;

    public AppVersionClass(string ns, Func<string, Stream> createStream, AppApiTemplate template, AppVersionKey versionKey)
    {
        this.ns = ns;
        this.createStream = createStream;
        this.template = template;
        this.versionKey = versionKey;
    }

    public async Task Output()
    {
        var apiObject = declareClass();
        var className = $"{template.Name}AppClientVersion";
        await outputClass(apiObject, className);
    }
    private CompilationUnitSyntax declareClass()
    {
        return CompilationUnit().WithMembers
        (
            SingletonList<MemberDeclarationSyntax>
            (
                FileScopedNamespaceDeclaration(IdentifierName(ns))
                    .WithNamespaceKeyword
                    (
                        Token
                        (
                            TriviaList(Comment("// Generated Code")),
                            SyntaxKind.NamespaceKeyword,
                            TriviaList()
                        )
                    )
                    .WithMembers
                    (
                        SingletonList<MemberDeclarationSyntax>
                        (
                            ClassDeclaration($"{template.Name}AppClientVersion")
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
                            .WithMembers
                            (
                                List
                                (
                                    new MemberDeclarationSyntax[]
                                    {
                                        MethodDeclaration
                                        (
                                            IdentifierName($"{template.Name}AppClientVersion"),
                                            Identifier("Version")
                                        )
                                        .WithModifiers
                                        (
                                            TokenList
                                            (
                                                new []
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
                                                    Parameter(Identifier("value"))
                                                        .WithType
                                                        (
                                                            PredefinedType(Token(SyntaxKind.StringKeyword))
                                                        )
                                                )
                                            )
                                        )
                                        .WithExpressionBody
                                        (
                                            ArrowExpressionClause
                                            (
                                                ObjectCreationExpression(IdentifierName($"{template.Name}AppClientVersion"))
                                                .WithArgumentList
                                                (
                                                    ArgumentList
                                                    (
                                                        SingletonSeparatedList
                                                        (
                                                            Argument(IdentifierName("value"))
                                                        )
                                                    )
                                                )
                                            )
                                        )
                                        .WithSemicolonToken(Token(SyntaxKind.SemicolonToken)),
                                        ConstructorDeclaration
                                        (
                                            Identifier($"{template.Name}AppClientVersion")
                                        )
                                        .WithModifiers
                                        (
                                            TokenList
                                            (
                                                Token(SyntaxKind.PublicKeyword)
                                            )
                                        )
                                        .WithParameterList
                                        (
                                            ParameterList
                                            (
                                                SingletonSeparatedList
                                                (
                                                    Parameter(Identifier("hostEnv"))
                                                        .WithType
                                                        (
                                                            IdentifierName("IHostEnvironment")
                                                        )
                                                    )
                                            )
                                        )
                                        .WithInitializer
                                        (
                                            ConstructorInitializer
                                            (
                                                SyntaxKind.ThisConstructorInitializer,
                                                ArgumentList
                                                (
                                                    SingletonSeparatedList
                                                    (
                                                        Argument
                                                        (
                                                            InvocationExpression(IdentifierName("getValue"))
                                                                .WithArgumentList
                                                                (
                                                                    ArgumentList
                                                                    (
                                                                        SingletonSeparatedList
                                                                        (
                                                                            Argument(IdentifierName("hostEnv")
                                                                        )
                                                                    )
                                                                )
                                                        )
                                                    )
                                                )
                                            )
                                        )
                                    )
                                    .WithBody(Block()),
                                    getValueDeclaration(),
                            ConstructorDeclaration(Identifier($"{template.Name}AppClientVersion"))
                            .WithModifiers
                            (
                                TokenList(Token(SyntaxKind.PrivateKeyword))
                            )
                            .WithParameterList
                            (
                                ParameterList
                                (
                                    SingletonSeparatedList
                                    (
                                        Parameter(Identifier("value"))
                                        .WithType
                                        (
                                            PredefinedType
                                            (
                                                Token(SyntaxKind.StringKeyword)
                                            )
                                        )
                                    )
                                )
                            )
                            .WithBody
                            (
                                Block
                                (
                                    SingletonList<StatementSyntax>
                                    (
                                        ExpressionStatement
                                        (
                                            AssignmentExpression
                                            (
                                                SyntaxKind.SimpleAssignmentExpression,
                                                IdentifierName("Value"),
                                                IdentifierName("value")
                                            )
                                        )
                                    )
                                )
                            ),
                            PropertyDeclaration
                            (
                                PredefinedType(Token(SyntaxKind.StringKeyword)),
                                Identifier("Value")
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
                            )
                        }
                    )
                )
            )
        )
        )
    );
    }

    private MethodDeclarationSyntax getValueDeclaration() => MethodDeclaration
        (
            PredefinedType(Token(SyntaxKind.StringKeyword)),
            Identifier("getValue")
        )
        .WithModifiers
        (
            TokenList
            (
                new[]
                {
                Token(SyntaxKind.PrivateKeyword),
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
                    Parameter(Identifier("hostEnv"))
                        .WithType(IdentifierName("IHostEnvironment"))
                )
            )
        )
        .WithBody
        (
            Block
            (
                LocalDeclarationStatement
                (
                    VariableDeclaration
                    (
                        PredefinedType(Token(SyntaxKind.StringKeyword)))
                            .WithVariables
                            (
                                SingletonSeparatedList
                                (
                                    VariableDeclarator(Identifier("value")))
                            )
                ),
                IfStatement
                (
                    InvocationExpression
                    (
                        MemberAccessExpression
                        (
                            SyntaxKind.SimpleMemberAccessExpression,
                            IdentifierName("hostEnv"),
                            IdentifierName("IsProduction")
                        )
                    ),
                    Block
                    (
                        SingletonList<StatementSyntax>
                        (
                            ExpressionStatement
                            (
                                AssignmentExpression
                                (
                                    SyntaxKind.SimpleAssignmentExpression,
                                    IdentifierName("value"),
                                    LiteralExpression
                                    (
                                        SyntaxKind.StringLiteralExpression,
                                        Literal(versionKey.DisplayText)
                                    )
                                )
                            )
                        )
                    )
                )
                .WithElse
                (
                    ElseClause
                    (
                        Block
                        (
                            SingletonList<StatementSyntax>
                            (
                                ExpressionStatement
                                (
                                    AssignmentExpression
                                    (
                                        SyntaxKind.SimpleAssignmentExpression,
                                        IdentifierName("value"),
                                        LiteralExpression
                                        (
                                            SyntaxKind.StringLiteralExpression,
                                            Literal("Current")
                                        )
                                    )
                                )
                            )
                        )
                    )
                ),
                ReturnStatement(IdentifierName("value"))
        )
    );

    private async Task outputClass(CompilationUnitSyntax compilationUnitSyntax, string className)
    {
        var cSharpFile = new CSharpFile(compilationUnitSyntax, createStream, className);
        await cSharpFile.Output();
    }
}