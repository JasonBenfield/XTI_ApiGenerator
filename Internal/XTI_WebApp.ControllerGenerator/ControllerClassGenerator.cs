using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using XTI_App.Api;
using XTI_WebApp.CodeGeneration.CSharp;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace XTI_WebApp.ControllerGenerator;

internal class ControllerClassGenerator
{
    private readonly string ns;
    private readonly Func<string, Stream> createStream;
    private readonly AppApiTemplate appTemplate;
    private readonly AppApiGroupTemplate groupTemplate;

    public ControllerClassGenerator
    (
        string ns,
        Func<string, Stream> createStream,
        AppApiTemplate appTemplate,
        AppApiGroupTemplate groupTemplate
    )
    {
        this.ns = ns;
        this.createStream = createStream;
        this.appTemplate = appTemplate;
        this.groupTemplate = groupTemplate;
    }

    public Task Output()
    {
        var controller = createController();
        var controllerClassName = GetControllerClassName();
        var cSharpFile = new CSharpFile(controller, createStream, controllerClassName);
        return cSharpFile.Output();
    }

    private CompilationUnitSyntax createController()
    {
        var apiClassName = $"{appTemplate.Name}AppApi";
        var controllerClassName = GetControllerClassName();
        var actionDeclarations = groupTemplate.ActionTemplates.Select(a => ActionDeclaration(groupTemplate, a));
        var code = CompilationUnit()
        .WithMembers
        (
            SingletonList<MemberDeclarationSyntax>
            (
                namespaceDeclaration()
                .WithMembers
                (
                    SingletonList<MemberDeclarationSyntax>
                    (
                        ControllerDeclaration(groupTemplate)
                        .WithMembers
                        (
                            List
                            (
                                new MemberDeclarationSyntax[]
                                {
                                    apiFieldDeclaration(apiClassName),
                                    constructorDeclaration(apiClassName, controllerClassName)
                                }
                                .Union(actionDeclarations)
                            )
                        )
                    )
                )
            )
        );
        return code;
    }

    private string GetControllerClassName() => $"{groupTemplate.Name}Controller";

    private MethodDeclarationSyntax ActionDeclaration(AppApiGroupTemplate group, AppApiActionTemplate action)
    {
        MethodDeclarationSyntax actionDeclaration;
        if (action.IsRedirect())
        {
            actionDeclaration = ActionDeclarationForRedirect(group, action);
        }
        else if (action.IsView() || action.IsPartialView())
        {
            actionDeclaration = ActionDeclarationForView(group, action);
        }
        else if (action.IsFile())
        {
            actionDeclaration = ActionDeclarationForFile(group, action);
        }
        else if (action.IsContent())
        {
            actionDeclaration = ActionDeclarationForContent(group, action);
        }
        else
        {
            actionDeclaration = ActionDeclarationForPost(group, action);
        }
        return actionDeclaration;
    }

    private MethodDeclarationSyntax ActionDeclarationForRedirect(AppApiGroupTemplate group, AppApiActionTemplate action)
    {
        var actionDeclaration = CreateMethodReturningIActionResult(action)
        .WithBody
        (
            Block
            (
                LocalDeclarationStatement
                (
                    VariableDeclaration
                    (
                        IdentifierName("var")
                    )
                    .WithVariables
                    (
                        SingletonSeparatedList
                        (
                            VariableDeclarator(Identifier("result"))
                                .WithInitializer
                                (
                                    EqualsValueClause
                                    (
                                        AwaitExpression
                                        (
                                            InvokeExecuteAction(group, action)
                                        )
                                    )
                                )
                        )
                    )
                ),
                ReturnStatement
                (
                    InvocationExpression(IdentifierName("Redirect"))
                        .WithArgumentList
                        (
                            ArgumentList
                            (
                                SingletonSeparatedList
                                (
                                    Argument
                                    (
                                        MemberAccessExpression
                                        (
                                            SyntaxKind.SimpleMemberAccessExpression,
                                            MemberAccessExpression
                                            (
                                                SyntaxKind.SimpleMemberAccessExpression,
                                                IdentifierName("result"),
                                                IdentifierName("Data")
                                            ),
                                            IdentifierName("Url")
                                        )
                                    )
                                )
                            )
                        )
                )
            )
        );
        var attributes = GetAttributesForAction(group, action);
        if (attributes.Any())
        {
            actionDeclaration = actionDeclaration
                .WithAttributeLists
                (
                    List(attributes)
                );
        }
        actionDeclaration = actionDeclaration
            .WithParameterList
            (
                ParameterList
                (
                    SeparatedList(GetActionParameters(action))
                )
            );
        return actionDeclaration;
    }

    private ParameterSyntax[] GetActionParameters(AppApiActionTemplate action)
    {
        var parameters = new List<ParameterSyntax>();
        if (!action.HasEmptyModel())
        {
            var modelParameter = Parameter(Identifier("model"))
                .WithType(typeSyntax(action.ModelTemplate));
            if (IsPost(action))
            {
                modelParameter = modelParameter.WithAttributeLists
                (
                    SingletonList
                    (
                        AttributeList
                        (
                            SingletonSeparatedList
                            (
                                Attribute(IdentifierName("FromBody"))
                            )
                        )
                    )
                );
            }
            parameters.Add(modelParameter);
        }
        parameters.Add
        (
            Parameter(Identifier("ct"))
                .WithType(IdentifierName("CancellationToken"))
        );
        return parameters.ToArray();
    }

    private MethodDeclarationSyntax ActionDeclarationForView(AppApiGroupTemplate group, AppApiActionTemplate action)
    {
        var actionDeclaration = CreateMethodReturningIActionResult(action)
        .WithBody
        (
            Block
            (
                LocalDeclarationStatement
                (
                    VariableDeclaration(IdentifierName("var"))
                        .WithVariables
                        (
                            SingletonSeparatedList
                            (
                                VariableDeclarator(Identifier("result"))
                                    .WithInitializer
                                    (
                                        EqualsValueClause
                                        (
                                            AwaitExpression
                                            (
                                                InvokeExecuteAction(group, action)
                                            )
                                        )
                                    )
                            )
                        )
                ),
                ReturnStatement
                (
                    InvocationExpression(IdentifierName(action.IsView() ? "View" : "PartialView"))
                        .WithArgumentList
                        (
                            ArgumentList
                            (
                                SingletonSeparatedList
                                (
                                    Argument
                                    (
                                        MemberAccessExpression
                                        (
                                            SyntaxKind.SimpleMemberAccessExpression,
                                            MemberAccessExpression
                                            (
                                                SyntaxKind.SimpleMemberAccessExpression,
                                                IdentifierName("result"),
                                                IdentifierName("Data")
                                            ),
                                            IdentifierName("ViewName")
                                        )
                                    )
                                )
                            )
                        )
                )
            )
        );
        actionDeclaration = actionDeclaration
            .WithParameterList
            (
                ParameterList
                (
                    SeparatedList(GetActionParameters(action))
                )
            );
        var attributes = GetAttributesForAction(group, action);
        if (attributes.Any())
        {
            actionDeclaration = actionDeclaration
                .WithAttributeLists
                (
                    List(attributes)
                );
        }
        return actionDeclaration;
    }

    private MethodDeclarationSyntax ActionDeclarationForFile(AppApiGroupTemplate group, AppApiActionTemplate action)
    {
        var actionDeclaration = CreateMethodReturningIActionResult(action)
        .WithBody
        (
            Block
            (
                LocalDeclarationStatement
                (
                    VariableDeclaration(IdentifierName("var"))
                        .WithVariables
                        (
                            SingletonSeparatedList
                            (
                                VariableDeclarator(Identifier("result"))
                                    .WithInitializer
                                    (
                                        EqualsValueClause
                                        (
                                            AwaitExpression
                                            (
                                                InvokeExecuteAction(group, action)
                                            )
                                        )
                                    )
                            )
                        )
                ),
                ReturnStatement
                (
                    InvocationExpression(IdentifierName("File"))
                        .WithArgumentList
                        (
                            ArgumentList
                            (
                                SeparatedList
                                (
                                    new ArgumentSyntax[]
                                    {
                                        Argument
                                        (
                                            MemberAccessExpression
                                            (
                                                SyntaxKind.SimpleMemberAccessExpression,
                                                MemberAccessExpression
                                                (
                                                    SyntaxKind.SimpleMemberAccessExpression,
                                                    IdentifierName("result"),
                                                    IdentifierName("Data")
                                                ),
                                                IdentifierName("FileStream")
                                            )
                                        ),
                                        Argument
                                        (
                                            MemberAccessExpression
                                            (
                                                SyntaxKind.SimpleMemberAccessExpression,
                                                MemberAccessExpression
                                                (
                                                    SyntaxKind.SimpleMemberAccessExpression,
                                                    IdentifierName("result"),
                                                    IdentifierName("Data")
                                                ),
                                                IdentifierName("ContentType")
                                            )
                                        ),
                                        Argument
                                        (
                                            MemberAccessExpression
                                            (
                                                SyntaxKind.SimpleMemberAccessExpression,
                                                MemberAccessExpression
                                                (
                                                    SyntaxKind.SimpleMemberAccessExpression,
                                                    IdentifierName("result"),
                                                    IdentifierName("Data")
                                                ),
                                                IdentifierName("DownloadName")
                                            )
                                        )
                                    }
                                )
                            )
                        )
                )
            )
        );
        actionDeclaration = actionDeclaration
            .WithParameterList
            (
                ParameterList
                (
                    SeparatedList(GetActionParameters(action))
                )
            );
        var attributes = GetAttributesForAction(group, action);
        if (attributes.Any())
        {
            actionDeclaration = actionDeclaration
                .WithAttributeLists
                (
                    List(attributes)
                );
        }
        return actionDeclaration;
    }

    private MethodDeclarationSyntax ActionDeclarationForContent(AppApiGroupTemplate group, AppApiActionTemplate action)
    {
        var actionDeclaration = CreateMethodReturningIActionResult(action)
        .WithBody
        (
            Block
            (
                LocalDeclarationStatement
                (
                    VariableDeclaration(IdentifierName("var"))
                        .WithVariables
                        (
                            SingletonSeparatedList
                            (
                                VariableDeclarator(Identifier("result"))
                                    .WithInitializer
                                    (
                                        EqualsValueClause
                                        (
                                            AwaitExpression
                                            (
                                                InvokeExecuteAction(group, action)
                                            )
                                        )
                                    )
                            )
                        )
                ),
                ReturnStatement
                (
                    InvocationExpression(IdentifierName("Content"))
                        .WithArgumentList
                        (
                            ArgumentList
                            (
                                SeparatedList
                                (
                                    new ArgumentSyntax[]
                                    {
                                        Argument
                                        (
                                            MemberAccessExpression
                                            (
                                                SyntaxKind.SimpleMemberAccessExpression,
                                                MemberAccessExpression
                                                (
                                                    SyntaxKind.SimpleMemberAccessExpression,
                                                    IdentifierName("result"),
                                                    IdentifierName("Data")
                                                ),
                                                IdentifierName("Content")
                                            )
                                        ),
                                        Argument
                                        (
                                            MemberAccessExpression
                                            (
                                                SyntaxKind.SimpleMemberAccessExpression,
                                                MemberAccessExpression
                                                (
                                                    SyntaxKind.SimpleMemberAccessExpression,
                                                    IdentifierName("result"),
                                                    IdentifierName("Data")
                                                ),
                                                IdentifierName("ContentType")
                                            )
                                        )
                                    }
                                )
                            )
                        )
                )
            )
        );
        actionDeclaration = actionDeclaration
            .WithParameterList
            (
                ParameterList
                (
                    SeparatedList(GetActionParameters(action))
                )
            );
        var attributes = GetAttributesForAction(group, action);
        if (attributes.Any())
        {
            actionDeclaration = actionDeclaration
                .WithAttributeLists
                (
                    List(attributes)
                );
        }
        return actionDeclaration;
    }

    private MethodDeclarationSyntax CreateMethodReturningIActionResult(AppApiActionTemplate action) =>
        MethodDeclaration
        (
            GenericName(Identifier("Task"))
                .WithTypeArgumentList
                (
                    TypeArgumentList
                    (
                        SingletonSeparatedList<TypeSyntax>
                        (
                            IdentifierName("IActionResult")
                        )
                    )
                ),
                Identifier(action.Name)
        )
        .WithModifiers
        (
            TokenList
            (
                new[]
                {
                    Token(SyntaxKind.PublicKeyword),
                    Token(SyntaxKind.AsyncKeyword)
                }
            )
        );

    private MethodDeclarationSyntax ActionDeclarationForPost(AppApiGroupTemplate group, AppApiActionTemplate action)
    {
        var actionDeclaration =
            MethodDeclaration
            (
                GenericName(Identifier("Task")
            )
            .WithTypeArgumentList
            (
                TypeArgumentList
                (
                    SingletonSeparatedList<TypeSyntax>
                    (
                        GenericName(Identifier("ResultContainer"))
                            .WithTypeArgumentList
                            (
                                TypeArgumentList
                                (
                                    SingletonSeparatedList
                                    (
                                        typeSyntax(action.ResultTemplate)
                                    )
                                )
                            )
                    )
                )
            ),
            Identifier(action.Name)
        )
        .WithAttributeLists
        (
            List
            (
                GetAttributesForAction(group, action)
            )
        )
        .WithModifiers
        (
            TokenList(Token(SyntaxKind.PublicKeyword))
        );
        actionDeclaration = actionDeclaration
            .WithParameterList
            (
                ParameterList
                (
                    SeparatedList(GetActionParameters(action))
                )
            );
        return actionDeclaration
            .WithBody
            (
                Block
                (
                    SingletonList<StatementSyntax>
                    (
                        ReturnStatement
                        (
                            InvokeExecuteAction(group, action)
                        )
                    )
                )
            );
    }

    private AttributeListSyntax[] GetAttributesForAction(AppApiGroupTemplate group, AppApiActionTemplate action)
    {
        var attributes = new List<AttributeSyntax>();
        if (IsPost(action))
        {
            attributes.Add
            (
                Attribute(IdentifierName("HttpPost"))
            );
        }
        if (action.IsQuery())
        {
            attributes.Add(Attribute(IdentifierName("EnableQuery")));
        }
        if (action.IsPartialView())
        {
            attributes.Add
            (
                Attribute(IdentifierName("ResponseCache"))
                    .WithArgumentList
                    (
                        AttributeArgumentList
                        (
                            SingletonSeparatedList
                            (
                                AttributeArgument
                                (
                                    LiteralExpression
                                    (
                                        SyntaxKind.StringLiteralExpression,
                                        Literal("Default")
                                    )
                                )
                                .WithNameEquals
                                (
                                    NameEquals(IdentifierName("CacheProfileName"))
                                )
                            )
                        )
                    )
            );
        }
        if (group.Access.IsAnonymousAllowed != action.Access.IsAnonymousAllowed)
        {
            attributes.Add
            (
                Attribute
                (
                    IdentifierName(action.Access.IsAnonymousAllowed ? "AllowAnonymous" : "Authorize")
                )
            );
        }
        return attributes.Select(attr => AttributeList(SingletonSeparatedList(attr))).ToArray();
    }

    private static bool IsPost(AppApiActionTemplate action) =>
        !action.IsRedirect() && !action.IsView() && !action.IsPartialView() &&
        !action.IsFile() && !action.IsQueryToExcel();

    private InvocationExpressionSyntax InvokeExecuteAction(AppApiGroupTemplate group, AppApiActionTemplate action)
    {
        GenericNameSyntax actionAccessor;
        if (action.IsQuery())
        {
            actionAccessor = GenericName(Identifier("Query"))
                .WithTypeArgumentList
                (
                    TypeArgumentList
                    (
                        SingletonSeparatedList
                        (
                            typeSyntax(((QueryableValueTemplate)action.ResultTemplate).ElementTemplate)
                        )
                    )
                );
        }
        else if (action.IsQueryToExcel())
        {
            actionAccessor = GenericName(Identifier("QueryToExcel"))
                .WithTypeArgumentList
                (
                    TypeArgumentList
                    (
                        SingletonSeparatedList
                        (
                            typeSyntax(((QueryOptionsTemplate)action.ModelTemplate).EntityTemplate)
                        )
                    )
                );
        }
        else
        {
            actionAccessor = GenericName(Identifier("Action"))
                .WithTypeArgumentList
                (
                    TypeArgumentList
                    (
                        SeparatedList<TypeSyntax>
                        (
                            new SyntaxNodeOrToken[]
                            {
                                typeSyntax(action.ModelTemplate),
                                Token(SyntaxKind.CommaToken),
                                typeSyntax(action.ResultTemplate)
                            }
                        )
                    )
                );
        }
        return InvocationExpression
        (
            MemberAccessExpression
            (
                SyntaxKind.SimpleMemberAccessExpression,
                InvocationExpression
                (
                    MemberAccessExpression
                    (
                        SyntaxKind.SimpleMemberAccessExpression,
                        InvocationExpression
                        (
                            MemberAccessExpression
                            (
                                SyntaxKind.SimpleMemberAccessExpression,
                                IdentifierName("api"),
                                IdentifierName("Group")
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
                        ),
                        actionAccessor
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
                ),
                IdentifierName("Execute")
            )
        )
        .WithArgumentList
        (
            ArgumentList
            (
                SeparatedList
                (
                    new[]
                    {
                        Argument
                        (
                            action.HasEmptyModel()
                                ? newEmptyRequest()
                                : IdentifierName("model")
                        ),
                        Argument(IdentifierName("ct"))
                    }
                )
            )
        );
    }

    private static ObjectCreationExpressionSyntax newEmptyRequest()
    {
        return ObjectCreationExpression(IdentifierName("EmptyRequest"))
            .WithArgumentList(ArgumentList());
    }

    private static FieldDeclarationSyntax apiFieldDeclaration(string apiClassName)
    {
        return
            FieldDeclaration
            (
                VariableDeclaration(IdentifierName(apiClassName))
                    .WithVariables
                    (
                        SingletonSeparatedList
                        (
                            VariableDeclarator(Identifier("api"))
                        )
                    )
            )
            .WithModifiers
            (
                TokenList
                (
                    Token(SyntaxKind.PrivateKeyword),
                    Token(SyntaxKind.ReadOnlyKeyword)
                )
            );
    }

    private static ConstructorDeclarationSyntax constructorDeclaration(string apiClassName, string groupClassName)
    {
        return
            ConstructorDeclaration(Identifier(groupClassName))
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
                        new ParameterSyntax[]
                        {
                            Parameter(Identifier("api"))
                                .WithType(IdentifierName(apiClassName))
                        }
                    )
                )
            )
            .WithBody
            (
                Block
                (
                    SeparatedList
                    (
                        new StatementSyntax[]
                        {
                                ExpressionStatement
                                (
                                    AssignmentExpression
                                    (
                                        SyntaxKind.SimpleAssignmentExpression,
                                        MemberAccessExpression
                                        (
                                            SyntaxKind.SimpleMemberAccessExpression,
                                            ThisExpression(),
                                            IdentifierName("api")
                                        ),
                                        IdentifierName("api")
                                    )
                                )
                        }
                    )
                )
            );
    }

    private ClassDeclarationSyntax ControllerDeclaration(AppApiGroupTemplate group) =>
        ClassDeclaration(GetControllerClassName())
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
            .WithAttributeLists
            (
                List
                (
                    new[]
                    {
                        AttributeList
                        (
                            SingletonSeparatedList
                            (
                                Attribute
                                (
                                    IdentifierName
                                    (
                                        group.Access.IsAnonymousAllowed ? "AllowAnonymous" : "Authorize"
                                    )
                                )
                            )
                        )
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
                            IdentifierName
                            (
                                group.ActionTemplates.Any(a => a.IsQuery())
                                    ? "ODataController"
                                    : "Controller"
                            )
                        )
                    )
                )
            );

    private FileScopedNamespaceDeclarationSyntax namespaceDeclaration()
    {
        return FileScopedNamespaceDeclaration(IdentifierName(ns))
            .WithNamespaceKeyword
            (
                Token
                (
                    TriviaList(Comment("// Generated Code")),
                    SyntaxKind.NamespaceKeyword,
                    TriviaList()
                )
            );
    }

    private TypeSyntax typeSyntax(ValueTemplate valueTemplate)
    {
        if (valueTemplate is QueryableValueTemplate queryTemplate)
        {
            return GenericName(Identifier("IQueryable"))
                .WithTypeArgumentList
                (
                    TypeArgumentList
                    (
                        SingletonSeparatedList
                        (
                            new TypeSyntaxFromValueTemplate(queryTemplate.ElementTemplate).Value()
                        )
                    )
                );
        }
        return new TypeSyntaxFromValueTemplate(valueTemplate).Value();
    }
}
