using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using XTI_App.Api;
using XTI_WebApp.CodeGeneration.CSharp;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace XTI_WebApp.ClientGenerator.CSharp;

internal sealed class AppClientFactoryClass
{
    private readonly string ns;
    private readonly Func<string, Stream> createStream;
    private readonly AppApiTemplate template;

    public AppClientFactoryClass(string ns, Func<string, Stream> createStream, AppApiTemplate template)
    {
        this.ns = ns;
        this.createStream = createStream;
        this.template = template;
    }

    public async Task Output()
    {
        var appClient = GetRoslyn();
        var className = GetClassName();
        await OutputClass(appClient, className);
    }

    private CompilationUnitSyntax GetRoslyn() =>
        CompilationUnit()
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
                    SingletonList<MemberDeclarationSyntax>
                    (
                        ClassDeclaration(GetClassName())
                        .WithModifiers
                        (
                            TokenList
                            (
                                [
                                    Token(SyntaxKind.PublicKeyword),
                                    Token(SyntaxKind.SealedKeyword),
                                    Token(SyntaxKind.PartialKeyword)
                                ]
                            )
                        )
                        .WithMembers
                        (
                            List
                            (
                                new MemberDeclarationSyntax[]
                                {
                                    FieldDeclaration
                                    (
                                        VariableDeclaration(IdentifierName("IHttpClientFactory"))
                                        .WithVariables
                                        (
                                            SingletonSeparatedList
                                            (
                                                VariableDeclarator(Identifier("httpClientFactory"))
                                            )
                                        )
                                    )
                                    .WithModifiers
                                    (
                                        TokenList
                                        (
                                            [
                                                Token(SyntaxKind.PrivateKeyword),
                                                Token(SyntaxKind.ReadOnlyKeyword)
                                            ]
                                        )
                                    ),
                                    FieldDeclaration
                                    (
                                        VariableDeclaration(IdentifierName("XtiTokenAccessorFactory"))
                                        .WithVariables
                                        (
                                            SingletonSeparatedList
                                            (
                                                VariableDeclarator(Identifier("xtiTokenAccessorFactory"))
                                            )
                                        )
                                    )
                                    .WithModifiers
                                    (
                                        TokenList
                                        (
                                            [
                                                Token(SyntaxKind.PrivateKeyword),
                                                Token(SyntaxKind.ReadOnlyKeyword)
                                            ]
                                        )
                                    ),
                                    FieldDeclaration
                                    (
                                        VariableDeclaration(IdentifierName("AppClientUrl"))
                                        .WithVariables
                                        (
                                            SingletonSeparatedList
                                            (
                                                VariableDeclarator(Identifier("clientUrl"))
                                            )
                                        )
                                    )
                                    .WithModifiers
                                    (
                                        TokenList
                                        (
                                            [
                                                Token(SyntaxKind.PrivateKeyword),
                                                Token(SyntaxKind.ReadOnlyKeyword)
                                            ]
                                        )
                                    ),
                                    FieldDeclaration
                                    (
                                        VariableDeclaration(IdentifierName("IAppClientRequestKey"))
                                        .WithVariables
                                        (
                                            SingletonSeparatedList
                                            (
                                                VariableDeclarator(Identifier("requestKey"))
                                            )
                                        )
                                    )
                                    .WithModifiers
                                    (
                                        TokenList
                                        (
                                            [
                                                Token(SyntaxKind.PrivateKeyword),
                                                Token(SyntaxKind.ReadOnlyKeyword)
                                            ]
                                        )
                                    ),
                                    FieldDeclaration
                                    (
                                        VariableDeclaration(IdentifierName($"{template.Name}AppClientVersion"))
                                        .WithVariables
                                        (
                                            SingletonSeparatedList
                                            (
                                                VariableDeclarator(Identifier("version"))
                                            )
                                        )
                                    )
                                    .WithModifiers
                                    (
                                        TokenList
                                        (
                                            [
                                                Token(SyntaxKind.PrivateKeyword),
                                                Token(SyntaxKind.ReadOnlyKeyword)
                                            ]
                                        )
                                    ),
                                    ConstructorDeclaration(Identifier($"{template.Name}AppClientFactory"))
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
                                                    Parameter(Identifier("httpClientFactory"))
                                                        .WithType(IdentifierName("IHttpClientFactory")),
                                                    Token(SyntaxKind.CommaToken),
                                                    Parameter(Identifier("xtiTokenAccessorFactory"))
                                                        .WithType(IdentifierName("XtiTokenAccessorFactory")),
                                                    Token(SyntaxKind.CommaToken),
                                                    Parameter(Identifier("clientUrl"))
                                                        .WithType(IdentifierName("AppClientUrl")),
                                                    Token(SyntaxKind.CommaToken),
                                                    Parameter(Identifier("requestKey"))
                                                        .WithType(IdentifierName("IAppClientRequestKey")),
                                                    Token(SyntaxKind.CommaToken),
                                                    Parameter(Identifier("version"))
                                                        .WithType(IdentifierName($"{template.Name}AppClientVersion"))
                                                }
                                            )
                                        )
                                    )
                                    .WithBody
                                    (
                                        Block
                                        (
                                            ExpressionStatement
                                            (
                                                AssignmentExpression
                                                (
                                                    SyntaxKind.SimpleAssignmentExpression,
                                                    MemberAccessExpression
                                                    (
                                                        SyntaxKind.SimpleMemberAccessExpression,
                                                        ThisExpression(),
                                                        IdentifierName("httpClientFactory")
                                                    ),
                                                    IdentifierName("httpClientFactory")
                                                )
                                            ),
                                            ExpressionStatement
                                            (
                                                AssignmentExpression
                                                (
                                                    SyntaxKind.SimpleAssignmentExpression,
                                                    MemberAccessExpression
                                                    (
                                                        SyntaxKind.SimpleMemberAccessExpression,
                                                        ThisExpression(),
                                                        IdentifierName("xtiTokenAccessorFactory")
                                                    ),
                                                    IdentifierName("xtiTokenAccessorFactory")
                                                )
                                            ),
                                            ExpressionStatement
                                            (
                                                AssignmentExpression
                                                (
                                                    SyntaxKind.SimpleAssignmentExpression,
                                                    MemberAccessExpression
                                                    (
                                                        SyntaxKind.SimpleMemberAccessExpression,
                                                        ThisExpression(),
                                                        IdentifierName("clientUrl")
                                                    ),
                                                    IdentifierName("clientUrl")
                                                )
                                            ),
                                            ExpressionStatement
                                            (
                                                AssignmentExpression
                                                (
                                                    SyntaxKind.SimpleAssignmentExpression,
                                                    MemberAccessExpression
                                                    (
                                                        SyntaxKind.SimpleMemberAccessExpression,
                                                        ThisExpression(),
                                                        IdentifierName("requestKey")
                                                    ),
                                                    IdentifierName("requestKey")
                                                )
                                            ),
                                            ExpressionStatement
                                            (
                                                AssignmentExpression
                                                (
                                                    SyntaxKind.SimpleAssignmentExpression,
                                                    MemberAccessExpression
                                                    (
                                                        SyntaxKind.SimpleMemberAccessExpression,
                                                        ThisExpression(),
                                                        IdentifierName("version")
                                                    ),
                                                    IdentifierName("version")
                                                )
                                            )
                                        )
                                    ),
                                    MethodDeclaration
                                    (
                                        IdentifierName($"{template.Name}AppClient"),
                                        Identifier("Create")
                                    )
                                    .WithModifiers
                                    (
                                        TokenList(Token(SyntaxKind.PublicKeyword))
                                    )
                                    .WithExpressionBody
                                    (
                                        ArrowExpressionClause
                                        (
                                            ObjectCreationExpression(IdentifierName($"{template.Name}AppClient"))
                                            .WithArgumentList
                                            (
                                                ArgumentList
                                                (
                                                    SeparatedList<ArgumentSyntax>
                                                    (
                                                        new SyntaxNodeOrToken[]
                                                        {
                                                            Argument(IdentifierName("httpClientFactory")),
                                                            Token(SyntaxKind.CommaToken),
                                                            Argument(IdentifierName("xtiTokenAccessorFactory")),
                                                            Token(SyntaxKind.CommaToken),
                                                            Argument(IdentifierName("clientUrl")),
                                                            Token(SyntaxKind.CommaToken),
                                                            Argument(IdentifierName("requestKey")),
                                                            Token(SyntaxKind.CommaToken),
                                                            Argument(IdentifierName("version"))
                                                        }
                                                    )
                                                )
                                            )
                                        )
                                    )
                                    .WithSemicolonToken(Token(SyntaxKind.SemicolonToken))
                                }
                            )
                        )
                    )
                )
            )
        )
        .NormalizeWhitespace();

    private string GetClassName() => $"{template.Name}AppClientFactory";

    private Task OutputClass(CompilationUnitSyntax compilationUnitSyntax, string className)
    {
        var cSharpFile = new CSharpFile(compilationUnitSyntax, createStream, className);
        return cSharpFile.Output();
    }
}
