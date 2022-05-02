using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using XTI_App.Api;
using XTI_WebApp.CodeGeneration.CSharp;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace XTI_WebApp.ClientGenerator.CSharp;

public sealed class ApiAppClass
{
    private readonly string ns;
    private readonly Func<string, Stream> createStream;
    private readonly AppApiTemplate template;

    public ApiAppClass(string ns, Func<string, Stream> createStream, AppApiTemplate template)
    {
        this.ns = ns;
        this.createStream = createStream;
        this.template = template;
    }

    public async Task Output()
    {
        var appClient = createAppClient();
        var className = getAppClassName();
        await outputClass(appClient, className);
    }

    private CompilationUnitSyntax createAppClient()
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
                                TriviaList(Comment("// Generated Code")),
                                SyntaxKind.NamespaceKeyword,
                                TriviaList()
                            )
                        )
                        .WithMembers
                        (
                            SingletonList<MemberDeclarationSyntax>
                            (
                                ClassDeclaration(getAppClassName())
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
                                            SeparatedList
                                            (
                                                appBaseList()
                                            )
                                        )
                                    )
                                    .WithMembers
                                    (
                                        List
                                        (
                                            appMembers()
                                        )
                                    )
                                )
                            )
                        )
                );
    }

    private static string getGroupClassName(AppApiGroupTemplate groupTemplate) => $"{groupTemplate.Name}Group";

    private string getAppClassName() => $"{template.Name}AppClient";

    private MemberDeclarationSyntax[] appMembers()
    {
        var members = new List<MemberDeclarationSyntax>();
        members.Add(appCtor());
        members.Add
        (
            PropertyDeclaration
            (
                IdentifierName
                (
                    $"{template.Name}RoleNames"
                ),
                Identifier("RoleNames")
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
            .WithInitializer
            (
                EqualsValueClause
                (
                    MemberAccessExpression
                    (
                        SyntaxKind.SimpleMemberAccessExpression,
                        IdentifierName($"{template.Name}RoleNames"),
                        IdentifierName("Instance")
                    )
                )
            )
            .WithSemicolonToken(Token(SyntaxKind.SemicolonToken))
        );
        members.Add
        (
            PropertyDeclaration
            (
                PredefinedType(Token(SyntaxKind.StringKeyword)),
                Identifier("AppName")
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
            .WithInitializer
            (
                EqualsValueClause
                (
                    LiteralExpression(SyntaxKind.StringLiteralExpression, Literal(template.Name))
                )
            )
            .WithSemicolonToken(Token(SyntaxKind.SemicolonToken))
        );
        foreach (var group in template.GroupTemplates)
        {
            members.Add
            (
                PropertyDeclaration
                (
                    IdentifierName
                    (
                        $"{group.Name}Group"
                    ),
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
                )
            );
        }
        return members.ToArray();
    }

    private ConstructorDeclarationSyntax appCtor()
    {
        return ConstructorDeclaration(Identifier(getAppClassName()))
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
                        appCtorArgs()
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
                            appBaseCtorArgs()
                        )
                    )
                )
            )
            .WithBody
            (
                Block
                (
                    List
                    (
                        appCtorBodyStatements()
                    )
                )
            );
    }

    private StatementSyntax[] appCtorBodyStatements()
    {
        var statements = new List<StatementSyntax>();
        foreach (var group in template.GroupTemplates)
        {
            statements.Add
            (
                ExpressionStatement
                (
                    AssignmentExpression
                    (
                        SyntaxKind.SimpleAssignmentExpression,
                        IdentifierName(group.Name),
                        InvocationExpression
                        (
                            IdentifierName("GetGroup")
                        )
                        .WithArgumentList
                        (
                            ArgumentList
                            (
                                SingletonSeparatedList
                                (
                                    Argument
                                    (
                                        ParenthesizedLambdaExpression()
                                            .WithParameterList
                                            (
                                                ParameterList
                                                (
                                                    SeparatedList<ParameterSyntax>
                                                    (
                                                        new SyntaxNodeOrToken[]
                                                        {
                                                            Parameter(Identifier("_clientFactory")),
                                                            Token(SyntaxKind.CommaToken),
                                                            Parameter(Identifier("_tokenAccessor")),
                                                            Token(SyntaxKind.CommaToken),
                                                            Parameter(Identifier("_url"))
                                                        }
                                                    )
                                                )
                                            )
                                            .WithExpressionBody
                                            (
                                                ObjectCreationExpression(IdentifierName(getGroupClassName(group)))
                                                    .WithArgumentList
                                                    (
                                                        ArgumentList
                                                        (
                                                            SeparatedList<ArgumentSyntax>
                                                            (
                                                                new SyntaxNodeOrToken[]
                                                                {
                                                                    Argument(IdentifierName("_clientFactory")),
                                                                    Token(SyntaxKind.CommaToken),
                                                                    Argument(IdentifierName("_tokenAccessor")),
                                                                    Token(SyntaxKind.CommaToken),
                                                                    Argument(IdentifierName("_url"))
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
                )
            );
        }
        return statements.ToArray();
    }

    private static BaseTypeSyntax[] appBaseList()
    {
        var baseTypes = new List<BaseTypeSyntax>();
        baseTypes.Add(SimpleBaseType(IdentifierName("AppClient")));
        return baseTypes.ToArray();
    }

    private SyntaxNodeOrToken[] appCtorArgs()
    {
        var args = new List<SyntaxNodeOrToken>();
        args.AddRange
        (
            new SyntaxNodeOrToken[]
            {
                Parameter(Identifier("httpClientFactory"))
                    .WithType(IdentifierName("IHttpClientFactory")),
                Token(SyntaxKind.CommaToken),
                Parameter(Identifier("xtiTokenAccessor"))
                    .WithType(IdentifierName("XtiTokenAccessor")),
                Token(SyntaxKind.CommaToken),
                Parameter(Identifier("clientUrl"))
                    .WithType(IdentifierName("AppClientUrl")),
                Token(SyntaxKind.CommaToken),
                Parameter(Identifier("version"))
                    .WithType(IdentifierName($"{template.Name}AppClientVersion"))
            }
        );
        return args.ToArray();
    }

    private SyntaxNodeOrToken[] appBaseCtorArgs()
    {
        var args = new List<SyntaxNodeOrToken>();
        args.AddRange
        (
            new SyntaxNodeOrToken[]
            {
                Argument(IdentifierName("httpClientFactory")),
                Token(SyntaxKind.CommaToken),
                Argument(IdentifierName("xtiTokenAccessor")),
                Token(SyntaxKind.CommaToken),
                Argument(IdentifierName("clientUrl")),
                Token(SyntaxKind.CommaToken),
                Argument
                (
                    LiteralExpression
                    (
                        SyntaxKind.StringLiteralExpression,
                        Literal(template.Name)
                    )
                ),
                Token(SyntaxKind.CommaToken),
                Argument
                (
                    MemberAccessExpression
                    (
                        SyntaxKind.SimpleMemberAccessExpression,
                        IdentifierName("version"),
                        IdentifierName("Value")
                    )
                )
            }
        );
        return args.ToArray();
    }

    private Task outputClass(CompilationUnitSyntax compilationUnitSyntax, string className)
    {
        var cSharpFile = new CSharpFile(compilationUnitSyntax, createStream, className);
        return cSharpFile.Output();
    }

}