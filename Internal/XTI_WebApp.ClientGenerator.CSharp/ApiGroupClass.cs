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
    private readonly AppApiActionTemplate[] actionsForGetMethod;

    public ApiGroupClass(string ns, Func<string, Stream> createStream, AppApiGroupTemplate template)
    {
        this.ns = ns;
        this.createStream = createStream;
        this.template = template;
        actionsForGetMethod = template.ActionsForGetMethod();
    }

    public async Task Output()
    {
        var groupClient = createGroupClient();
        var className = getGroupClassName();
        await outputClass(groupClient, className);
    }

    private CompilationUnitSyntax createGroupClient()
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
                                groupClass()
                            )
                        )
                )
            );
    }

    private MemberDeclarationSyntax[] groupClass()
    {
        var groupClass = new List<MemberDeclarationSyntax>();
        groupClass.Add
        (
            ClassDeclaration(getGroupClassName())
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
                            groupBaseList()
                        )
                    )
                )
                .WithMembers
                (
                    List
                    (
                        groupMembers()
                    )
                )
        );
        return groupClass.ToArray();
    }

    private MemberDeclarationSyntax[] groupMembers()
    {
        var members = new List<MemberDeclarationSyntax>();
        members.Add(groupCtor());
        if (actionsForGetMethod.Any())
        {
            members.Add
            (
                PropertyDeclaration
                (
                    IdentifierName($"{template.Name}Actions"),
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
        }
        foreach (var action in template.ActionTemplates.Where(a => !a.IsRedirect() && !a.IsView() && !a.IsPartialView()))
        {
            members.AddRange
            (
                new[]
                {
                    actionDeclaration(action)
                }
            );
        }
        return members.ToArray();
    }

    private MethodDeclarationSyntax actionDeclaration(AppApiActionTemplate action)
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
                            new TypeSyntaxFromValueTemplate(action.ResultTemplate).Value()
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
                    actionArgs(action, template.HasModifier)
                )
            )
        )
        .WithExpressionBody
        (
            ArrowExpressionClause
            (
                InvocationExpression
                (
                    GenericName(Identifier("Post"))
                    .WithTypeArgumentList
                    (
                        TypeArgumentList
                        (
                            SeparatedList<TypeSyntax>
                            (
                                new SyntaxNodeOrToken[]
                                {
                                    new TypeSyntaxFromValueTemplate(action.ResultTemplate).Value(),
                                    Token(SyntaxKind.CommaToken),
                                    new TypeSyntaxFromValueTemplate(action.ModelTemplate).Value()
                                }
                            )
                        )
                    )
                )
                .WithArgumentList
                (
                    ArgumentList
                    (
                        SeparatedList<ArgumentSyntax>
                        (
                            postArgs(action, template.HasModifier)
                        )
                    )
                )
            )
        )
        .WithSemicolonToken(Token(SyntaxKind.SemicolonToken));
    }

    private static SyntaxNodeOrToken[] postArgs(AppApiActionTemplate actionTemplate, bool includeModifier)
    {
        var args = new List<SyntaxNodeOrToken>();
        args.AddRange
        (
            new SyntaxNodeOrToken[]
            {
                Argument
                (
                    LiteralExpression
                    (
                        SyntaxKind.StringLiteralExpression,
                        Literal(actionTemplate.Name)
                    )
                ),
                Token(SyntaxKind.CommaToken)
            }
        );
        if (includeModifier)
        {
            args.AddRange
            (
                new SyntaxNodeOrToken[]
                {
                    Argument(IdentifierName("modifier")),
                    Token(SyntaxKind.CommaToken)
                }
            );
        }
        else
        {
            args.AddRange
            (
                new SyntaxNodeOrToken[]
                {
                    Argument
                    (
                        LiteralExpression
                        (
                            SyntaxKind.StringLiteralExpression,
                            Literal("")
                        )
                    ),
                    Token(SyntaxKind.CommaToken)
                }
            );
        }
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
        return args.ToArray();
    }

    private static ParameterSyntax[] actionArgs(AppApiActionTemplate actionTemplate, bool includeModifier)
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
        return parameters.ToArray();
    }

    private MemberDeclarationSyntax groupCtor()
    {
        return ConstructorDeclaration(Identifier(getGroupClassName()))
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
                                .WithType(IdentifierName("AppClientUrl"))
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
                Block(ctorBody())
            );
    }

    private IEnumerable<StatementSyntax> ctorBody()
    {
        var statements = new List<StatementSyntax>();
        if (actionsForGetMethod.Any())
        {
            statements.Add
            (
                ExpressionStatement
                (
                    AssignmentExpression
                    (
                        SyntaxKind.SimpleAssignmentExpression,
                        IdentifierName("Actions"),
                        ObjectCreationExpression(IdentifierName($"{template.Name}Actions"))
                        .WithArgumentList
                        (
                            ArgumentList
                            (
                                SeparatedList<ArgumentSyntax>
                                (
                                    new SyntaxNodeOrToken[]
                                    {
                                        Argument(IdentifierName("clientUrl"))
                                    }
                                )
                            )
                        )
                    )
                )
            );
        }
        return statements;
    }

    private static BaseTypeSyntax[] groupBaseList()
    {
        var baseTypes = new List<BaseTypeSyntax>();
        baseTypes.Add(SimpleBaseType(IdentifierName("AppClientGroup")));
        return baseTypes.ToArray();
    }

    private string getGroupClassName() => $"{template.Name}Group";

    private Task outputClass(CompilationUnitSyntax compilationUnitSyntax, string className)
    {
        var cSharpFile = new CSharpFile(compilationUnitSyntax, createStream, className);
        return cSharpFile.Output();
    }
}