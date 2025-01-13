using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using XTI_App.Api;
using XTI_WebApp.CodeGeneration;
using XTI_WebApp.CodeGeneration.CSharp;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace XTI_WebApp.ClientGenerator.CSharp;

public sealed class AppClientClass
{
    private readonly string ns;
    private readonly Func<string, Stream> createStream;
    private readonly AppApiTemplate template;

    public AppClientClass(string ns, Func<string, Stream> createStream, AppApiTemplate template)
    {
        this.ns = ns;
        this.createStream = createStream;
        this.template = template;
    }

    public async Task Output()
    {
        var appClient = CodeForClass();
        var className = GetAppClassName();
        await OutputClass(appClient, className);
    }

    private CompilationUnitSyntax CodeForClass()
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
                            SingletonList<MemberDeclarationSyntax>
                            (
                                ClassDeclaration(GetAppClassName())
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
                                    .WithBaseList
                                    (
                                        BaseList
                                        (
                                            SeparatedList
                                            (
                                                BaseTypeForCtor()
                                            )
                                        )
                                    )
                                    .WithMembers
                                    (
                                        List
                                        (
                                            DeclarationForMembers()
                                        )
                                    )
                                )
                            )
                        )
                );
    }

    private static string GetGroupClassName(AppApiGroupTemplate groupTemplate) => $"{groupTemplate.Name}Group";

    private string GetAppClassName() => $"{template.Name}AppClient";

    private MemberDeclarationSyntax[] DeclarationForMembers()
    {
        var members = new List<MemberDeclarationSyntax>();
        members.Add(DeclarationForCtor());
        members.Add(GeneratedConfigureMethod.Declaration());
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
        foreach (var group in template.GroupTemplates.Where(gt => !gt.IsUser() && !gt.IsUserCache()))
        {
            members.Add
            (
                PropertyDeclaration
                (
                    group.IsODataGroup()
                        ? GenericName(Identifier("AppClientODataGroup"))
                            .WithTypeArgumentList
                            (
                                TypeArgumentList
                                (
                                    SeparatedList
                                    (
                                        new[]
                                        {
                                            new TypeSyntaxFromValueTemplate(group.ActionTemplates.First().ModelTemplate).Value(),
                                            new TypeSyntaxFromValueTemplate(group.QueryableTemplates().First().ElementTemplate).Value()
                                        }
                                    )
                                )
                            )
                        : IdentifierName($"{group.Name}Group"),
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

    private ConstructorDeclarationSyntax DeclarationForCtor()
    {
        return ConstructorDeclaration(Identifier(GetAppClassName()))
            .WithModifiers
            (
                TokenList(Token(SyntaxKind.PublicKeyword))
            )
            .WithParameterList
            (
                ParameterList
                (
                    SeparatedList
                    (
                        ArgumentsForCtor()
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
                        SeparatedList
                        (
                            BaseArgumentsForCtor()
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
                        StatementsForCtorBody()
                    )
                )
            );
    }

    private StatementSyntax[] StatementsForCtorBody()
    {
        var statements = new List<StatementSyntax>();
        foreach (var group in template.GroupTemplates.Where(gt => !gt.IsUser() && !gt.IsUserCache()))
        {
            statements.Add
            (
                group.IsODataGroup()
                ? ExpressionStatement
                (
                    AssignmentExpression
                    (
                        SyntaxKind.SimpleAssignmentExpression,
                        IdentifierName(group.Name),
                        InvocationExpression
                        (
                            GenericName(Identifier("CreateODataGroup"))
                                .WithTypeArgumentList
                                (
                                    TypeArgumentList
                                    (
                                        SeparatedList
                                        (
                                            [
                                                new TypeSyntaxFromValueTemplate(group.ActionTemplates.First().ModelTemplate).Value(),
                                                new TypeSyntaxFromValueTemplate(group.QueryableTemplates().First().ElementTemplate).Value()
                                            ]
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
                                            Literal(group.Name)
                                        )
                                    )
                                )
                            )
                        )
                    )
                )
                : ExpressionStatement
                (
                    AssignmentExpression
                    (
                        SyntaxKind.SimpleAssignmentExpression,
                        IdentifierName(group.Name),
                        InvocationExpression
                        (
                            IdentifierName("CreateGroup")
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
                                                    SeparatedList
                                                    (
                                                        [
                                                            Parameter(Identifier("_clientFactory")),
                                                            Parameter(Identifier("_tokenAccessor")),
                                                            Parameter(Identifier("_url")),
                                                            Parameter(Identifier("_options"))
                                                        ]
                                                    )
                                                )
                                            )
                                            .WithExpressionBody
                                            (
                                                ObjectCreationExpression(IdentifierName(GetGroupClassName(group)))
                                                    .WithArgumentList
                                                    (
                                                        ArgumentList
                                                        (
                                                            SeparatedList
                                                            (
                                                                [
                                                                    Argument(IdentifierName("_clientFactory")),
                                                                    Argument(IdentifierName("_tokenAccessor")),
                                                                    Argument(IdentifierName("_url")),
                                                                    Argument(IdentifierName("_options"))
                                                                ]
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
        statements.Add(GeneratedConfigureMethod.Invocation());
        return statements.ToArray();
    }

    private static BaseTypeSyntax[] BaseTypeForCtor()
    {
        var baseTypes = new List<BaseTypeSyntax>();
        baseTypes.Add(SimpleBaseType(IdentifierName("AppClient")));
        return baseTypes.ToArray();
    }

    private ParameterSyntax[] ArgumentsForCtor() =>
        [
            Parameter(Identifier("httpClientFactory"))
                .WithType(IdentifierName("IHttpClientFactory")),
            Parameter(Identifier("xtiTokenAccessorFactory"))
                .WithType(IdentifierName("XtiTokenAccessorFactory")),
            Parameter(Identifier("clientUrl"))
                .WithType(IdentifierName("AppClientUrl")),
            Parameter(Identifier("sessionKey"))
                .WithType(IdentifierName("IAppClientSessionKey")),
            Parameter(Identifier("requestKey"))
                .WithType(IdentifierName("IAppClientRequestKey")),
            Parameter(Identifier("version"))
                .WithType(IdentifierName($"{template.Name}AppClientVersion"))
        ];

    private ArgumentSyntax[] BaseArgumentsForCtor() =>
        [
            Argument(IdentifierName("httpClientFactory")),
            Argument(IdentifierName("xtiTokenAccessorFactory")),
            Argument(IdentifierName("clientUrl")),
            Argument(IdentifierName("sessionKey")),
            Argument(IdentifierName("requestKey")),
            Argument
            (
                LiteralExpression
                (
                    SyntaxKind.StringLiteralExpression,
                    Literal(template.Name)
                )
            ),
            Argument
            (
                MemberAccessExpression
                (
                    SyntaxKind.SimpleMemberAccessExpression,
                    IdentifierName("version"),
                    IdentifierName("Value")
                )
            )
        ];

    private Task OutputClass(CompilationUnitSyntax compilationUnitSyntax, string className)
    {
        var cSharpFile = new CSharpFile(compilationUnitSyntax, createStream, className);
        return cSharpFile.Output();
    }

}