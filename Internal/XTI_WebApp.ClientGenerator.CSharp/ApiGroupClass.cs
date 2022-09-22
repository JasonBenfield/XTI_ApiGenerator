using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using XTI_App.Api;
using XTI_WebApp.CodeGeneration.CSharp;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace XTI_WebApp.ClientGenerator.CSharp;

public sealed class ApiGroupClass
{
    private readonly string ns;
    private readonly Func<string, Stream> createStream;
    private readonly AppApiGroupTemplate template;

    public ApiGroupClass(string ns, Func<string, Stream> createStream, AppApiGroupTemplate template)
    {
        this.ns = ns;
        this.createStream = createStream;
        this.template = template;
    }

    public async Task Output()
    {
        var groupClient = CreateGroupDocument();
        var className = GetGroupClassName();
        await outputClass(groupClient, className);
    }

    private CompilationUnitSyntax CreateGroupDocument()
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
                            List
                            (
                                CreateGroupClassDeclaration()
                            )
                        )
                )
            );
    }

    private MemberDeclarationSyntax[] CreateGroupClassDeclaration()
    {
        var groupClass = new List<MemberDeclarationSyntax>();
        groupClass.Add
        (
            ClassDeclaration(GetGroupClassName())
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
                            SingletonSeparatedList<BaseTypeSyntax>
                            (
                                SimpleBaseType(IdentifierName("AppClientGroup"))
                            )
                        )
                    )
                )
                .WithMembers
                (
                    List
                    (
                        GetGroupMembers()
                    )
                )
        );
        return groupClass.ToArray();
    }

    private MemberDeclarationSyntax[] GetGroupMembers()
    {
        var members = new List<MemberDeclarationSyntax>();
        members.Add(CreateCtorDeclaration());
        members.Add
        (
            PropertyDeclaration
            (
                IdentifierName($"{GetGroupClassName()}Actions"),
                Identifier("Actions")
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
        foreach (var action in template.ActionTemplates.Where(a => !a.IsRedirect() && !a.IsView() && !a.IsPartialView()))
        {
            members.AddRange
            (
                new[]
                {
                    CreateActionDeclaration(action)
                }
            );
        }
        members.Add(CreateActionsClassDeclaration());
        return members.ToArray();
    }

    private MemberDeclarationSyntax CreateCtorDeclaration()
    {
        return ConstructorDeclaration(Identifier(GetGroupClassName()))
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
                            Parameter(Identifier("xtiTokenAccessor"))
                                .WithType(IdentifierName("XtiTokenAccessor")),
                            Token(SyntaxKind.CommaToken),
                            Parameter(Identifier("clientUrl"))
                                .WithType(IdentifierName("AppClientUrl")),
                            Token(SyntaxKind.CommaToken),
                            Parameter(Identifier("options"))
                                .WithType(IdentifierName("AppClientOptions"))
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
                                Argument(IdentifierName("httpClientFactory")),
                                Token(SyntaxKind.CommaToken),
                                Argument(IdentifierName("xtiTokenAccessor")),
                                Token(SyntaxKind.CommaToken),
                                Argument(IdentifierName("clientUrl")),
                                Token(SyntaxKind.CommaToken),
                                Argument(IdentifierName("options")),
                                Token(SyntaxKind.CommaToken),
                                Argument
                                (
                                    LiteralExpression
                                    (
                                        SyntaxKind.StringLiteralExpression,
                                        Literal(template.Name)
                                    )
                                )
                            }
                        )
                    )
                )
            )
            .WithBody
            (
                Block(CreateCtorBody())
            );
    }

    private IEnumerable<StatementSyntax> CreateCtorBody()
    {
        var statements = new List<StatementSyntax>();
        statements.Add
        (
            ExpressionStatement
            (
                AssignmentExpression
                (
                    SyntaxKind.SimpleAssignmentExpression,
                    IdentifierName("Actions"),
                    ObjectCreationExpression(IdentifierName($"{GetGroupClassName()}Actions"))
                    .WithArgumentList
                    (
                        ArgumentList
                        (
                            SeparatedList<ArgumentSyntax>
                            (
                                GetObjectCreationArgsForActions()
                            )
                        )
                    )
                )
            )
        );
        return statements;
    }

    private SyntaxNodeOrToken[] GetObjectCreationArgsForActions()
    {
        var args = new List<SyntaxNodeOrToken>();
        var lastActionTemplate = template.ActionTemplates.Last();
        foreach (var actionTemplate in template.ActionTemplates)
        {
            args.Add
            (
                Argument
                (
                    InvocationExpression
                    (
                        actionTemplate.IsFile() ?
                            GenericName(Identifier("CreateFileAction"))
                            .WithTypeArgumentList
                            (
                                TypeArgumentList
                                (
                                    SeparatedList<TypeSyntax>
                                    (
                                        new SyntaxNodeOrToken[]
                                        {
                                            new TypeSyntaxFromValueTemplate(actionTemplate.ModelTemplate).Value()
                                        }
                                    )
                                )
                            )
                        : actionTemplate.IsContent() ?
                            GenericName(Identifier("CreateContentAction"))
                            .WithTypeArgumentList
                            (
                                TypeArgumentList
                                (
                                    SeparatedList<TypeSyntax>
                                    (
                                        new SyntaxNodeOrToken[]
                                        {
                                            new TypeSyntaxFromValueTemplate(actionTemplate.ModelTemplate).Value()
                                        }
                                    )
                                )
                            )
                        : actionTemplate.IsView() || actionTemplate.IsPartialView() || actionTemplate.IsRedirect() ?
                            GenericName(Identifier("CreateGetAction"))
                            .WithTypeArgumentList
                            (
                                TypeArgumentList
                                (
                                    SeparatedList<TypeSyntax>
                                    (
                                        new SyntaxNodeOrToken[]
                                        {
                                            new TypeSyntaxFromValueTemplate(actionTemplate.ModelTemplate).Value()
                                        }
                                    )
                                )
                            )
                        : GenericName(Identifier("CreatePostAction"))
                            .WithTypeArgumentList
                            (
                                TypeArgumentList
                                (
                                    SeparatedList<TypeSyntax>
                                    (
                                        new SyntaxNodeOrToken[]
                                        {
                                            new TypeSyntaxFromValueTemplate(actionTemplate.ModelTemplate).Value(),
                                            Token(SyntaxKind.CommaToken),
                                            new TypeSyntaxFromValueTemplate(actionTemplate.ResultTemplate).Value()
                                        }
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
                                        Literal(actionTemplate.Name)
                                    )
                                )
                            )
                        )
                    )
                )
                .WithNameColon
                (
                    NameColon(IdentifierName(actionTemplate.Name))
                )
            );
            if (actionTemplate.Name != lastActionTemplate.Name)
            {
                args.Add(Token(SyntaxKind.CommaToken));
            }
        }
        return args.ToArray();
    }

    private MethodDeclarationSyntax CreateActionDeclaration(AppApiActionTemplate action)
    {
        return MethodDeclaration
        (
            GenericName(Identifier("Task"))
                .WithTypeArgumentList
                (
                    TypeArgumentList
                    (
                        SingletonSeparatedList
                        (
                            action.IsContent() ? IdentifierName("AppClientContentResult")
                            : action.IsFile() ? IdentifierName("AppClientFileResult")
                            : new TypeSyntaxFromValueTemplate(action.ResultTemplate).Value()
                        )
                    )
                ),
                Identifier(action.Name)
        )
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
                    GetActionMethodDeclarationArgs(action, template.HasModifier)
                )
            )
        )
        .WithExpressionBody
        (
            ArrowExpressionClause
            (
                CreateActionMethodInvocation(action)
            )
        )
        .WithSemicolonToken(Token(SyntaxKind.SemicolonToken));
    }

    private static ParameterSyntax[] GetActionMethodDeclarationArgs(AppApiActionTemplate actionTemplate, bool includeModifier)
    {
        var parameters = new List<ParameterSyntax>();
        if (includeModifier)
        {
            parameters.Add
            (
                Parameter(Identifier("modifier"))
                    .WithType(PredefinedType(Token(SyntaxKind.StringKeyword)))
            );
        }
        if (!actionTemplate.HasEmptyModel())
        {
            parameters.Add
            (
                Parameter(Identifier("model"))
                    .WithType
                    (
                        new TypeSyntaxFromValueTemplate(actionTemplate.ModelTemplate).Value()
                    )
            );
        }
        parameters.Add
        (
            Parameter(Identifier("ct"))
                .WithType(IdentifierName("CancellationToken"))
                .WithDefault
                (
                    EqualsValueClause
                    (
                        LiteralExpression
                        (
                            SyntaxKind.DefaultLiteralExpression,
                            Token(SyntaxKind.DefaultKeyword)
                        )
                    )
                )
        );
        return parameters.ToArray();
    }

    private InvocationExpressionSyntax CreateActionMethodInvocation(AppApiActionTemplate action)
    {
        if (action.IsFile())
        {
            return InvocationExpression
            (
                MemberAccessExpression
                (
                    SyntaxKind.SimpleMemberAccessExpression,
                    MemberAccessExpression
                    (
                        SyntaxKind.SimpleMemberAccessExpression,
                        IdentifierName("Actions"),
                        IdentifierName(action.Name)
                    ),
                    IdentifierName("GetFile")
                )
            )
            .WithArgumentList
            (
                ArgumentList
                (
                    SeparatedList<ArgumentSyntax>
                    (
                        GetActionMethodInvocationArgs(action, template.HasModifier)
                    )
                )
            );
        }
        else if (action.IsContent())
        {
            return InvocationExpression
            (
                MemberAccessExpression
                (
                    SyntaxKind.SimpleMemberAccessExpression,
                    MemberAccessExpression
                    (
                        SyntaxKind.SimpleMemberAccessExpression,
                        IdentifierName("Actions"),
                        IdentifierName(action.Name)
                    ),
                    IdentifierName("Post")
                )
            )
            .WithArgumentList
            (
                ArgumentList
                (
                    SeparatedList<ArgumentSyntax>
                    (
                        GetActionMethodInvocationArgs(action, template.HasModifier)
                    )
                )
            );
        }
        return InvocationExpression
        (
            MemberAccessExpression
            (
                SyntaxKind.SimpleMemberAccessExpression,
                MemberAccessExpression
                (
                    SyntaxKind.SimpleMemberAccessExpression,
                    IdentifierName("Actions"),
                    IdentifierName(action.Name)
                ),
                IdentifierName("Post")
            )
        )
        .WithArgumentList
        (
            ArgumentList
            (
                SeparatedList<ArgumentSyntax>
                (
                    GetActionMethodInvocationArgs(action, template.HasModifier)
                )
            )
        );
    }

    private static SyntaxNodeOrToken[] GetActionMethodInvocationArgs(AppApiActionTemplate actionTemplate, bool includeModifier)
    {
        var args = new List<SyntaxNodeOrToken>();
        args.AddRange
        (
            new SyntaxNodeOrToken[]
            {
                Argument
                (
                    includeModifier
                        ? IdentifierName("modifier")
                        : LiteralExpression
                        (
                            SyntaxKind.StringLiteralExpression,
                            Literal("")
                        )
                ),
                Token(SyntaxKind.CommaToken)
            }
        );
        if (actionTemplate.HasEmptyModel())
        {
            args.Add
            (
                Argument
                (
                    ObjectCreationExpression(IdentifierName("EmptyRequest"))
                        .WithArgumentList(ArgumentList())
                )
            );
        }
        else
        {
            args.Add(Argument(IdentifierName("model")));
        }
        args.Add(Token(SyntaxKind.CommaToken));
        args.Add(Argument(IdentifierName("ct")));
        return args.ToArray();
    }

    private RecordDeclarationSyntax CreateActionsClassDeclaration()
    {
        return RecordDeclaration
        (
            Token(SyntaxKind.RecordKeyword),
            Identifier($"{GetGroupClassName()}Actions"))
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
            .WithParameterList
            (
                ParameterList
                (
                    SeparatedList<ParameterSyntax>
                    (
                        GetParametersForActions()
                    )
                )
            )
            .WithSemicolonToken(Token(SyntaxKind.SemicolonToken));
    }

    private SyntaxNodeOrToken[] GetParametersForActions()
    {
        var parameters = new List<SyntaxNodeOrToken>();
        var lastActionTemplate = template.ActionTemplates.Last();
        foreach (var actionTemplate in template.ActionTemplates)
        {
            if (actionTemplate.IsFile())
            {
                parameters.Add
                (
                    Parameter(Identifier(actionTemplate.Name))
                        .WithType
                        (
                            GenericName(Identifier("AppClientFileAction"))
                            .WithTypeArgumentList
                            (
                                TypeArgumentList
                                (
                                    SeparatedList
                                    (
                                        SingletonSeparatedList
                                        (
                                            new TypeSyntaxFromValueTemplate(actionTemplate.ModelTemplate).Value()
                                        )
                                    )
                                )
                            )
                        )
                );
            }
            else if (actionTemplate.IsContent())
            {
                parameters.Add
                (
                    Parameter(Identifier(actionTemplate.Name))
                        .WithType
                        (
                            GenericName(Identifier("AppClientContentAction"))
                            .WithTypeArgumentList
                            (
                                TypeArgumentList
                                (
                                    SeparatedList
                                    (
                                        SingletonSeparatedList
                                        (
                                            new TypeSyntaxFromValueTemplate(actionTemplate.ModelTemplate).Value()
                                        )
                                    )
                                )
                            )
                        )
                );
            }
            else if (actionTemplate.IsView() || actionTemplate.IsPartialView() || actionTemplate.IsRedirect())
            {
                parameters.Add
                (
                    Parameter(Identifier(actionTemplate.Name))
                        .WithType
                        (
                            GenericName(Identifier("AppClientGetAction"))
                            .WithTypeArgumentList
                            (
                                TypeArgumentList
                                (
                                    SeparatedList
                                    (
                                        SingletonSeparatedList
                                        (
                                            new TypeSyntaxFromValueTemplate(actionTemplate.ModelTemplate).Value()
                                        )
                                    )
                                )
                            )
                        )
                );
            }
            else
            {
                parameters.Add
                (
                    Parameter(Identifier(actionTemplate.Name))
                        .WithType
                        (
                            GenericName(Identifier("AppClientPostAction"))
                            .WithTypeArgumentList
                            (
                                TypeArgumentList
                                (
                                    SeparatedList<TypeSyntax>
                                    (
                                        new SyntaxNodeOrToken[]
                                        {
                                            new TypeSyntaxFromValueTemplate(actionTemplate.ModelTemplate).Value(),
                                            Token(SyntaxKind.CommaToken),
                                            new TypeSyntaxFromValueTemplate(actionTemplate.ResultTemplate).Value()
                                        }
                                    )
                                )
                            )
                        )
                );
            }
            if (actionTemplate.Name != lastActionTemplate.Name)
            {
                parameters.Add(Token(SyntaxKind.CommaToken));
            }
        }
        return parameters.ToArray();
    }

    private string GetGroupClassName() => $"{template.Name}Group";

    private Task outputClass(CompilationUnitSyntax compilationUnitSyntax, string className)
    {
        var cSharpFile = new CSharpFile(compilationUnitSyntax, createStream, className);
        return cSharpFile.Output();
    }
}