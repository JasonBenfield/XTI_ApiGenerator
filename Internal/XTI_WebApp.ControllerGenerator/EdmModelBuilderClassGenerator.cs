using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using XTI_App.Api;
using XTI_WebApp.CodeGeneration;
using XTI_WebApp.CodeGeneration.CSharp;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace XTI_WebApp.ControllerGenerator;

internal sealed class EdmModelBuilderClassGenerator
{
    private readonly string ns;
    private readonly Func<string, Stream> createStream;
    private readonly AppApiTemplate appTemplate;

    public EdmModelBuilderClassGenerator
    (
        string ns,
        Func<string, Stream> createStream,
        AppApiTemplate appTemplate
    )
    {
        this.ns = ns;
        this.createStream = createStream;
        this.appTemplate = appTemplate;
    }

    public async Task Output()
    {
        var odataGroupTemplates = GetOdataGroupTemplates();
        if (odataGroupTemplates.Any())
        {
            var cu = CompilationUnit()
                .WithUsings(List(GetUsingDirectives()))
                .WithMembers
                (
                    SingletonList<MemberDeclarationSyntax>
                    (
                        FileScopedNamespaceDeclaration
                        (
                            IdentifierName(ns)
                        )
                        .WithMembers
                        (
                            SingletonList<MemberDeclarationSyntax>
                            (
                                GetClassDeclaration()
                            )
                        )
                    )
                );
            var cSharpFile = new CSharpFile(cu, createStream, "EdmModelBuilder");
            await cSharpFile.Output();
        }
    }

    private ClassDeclarationSyntax GetClassDeclaration() =>
        ClassDeclaration("EdmModelBuilder")
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
            .WithMembers
            (
                List
                (
                    GetClassMembers()
                )
            );

    private UsingDirectiveSyntax[] GetUsingDirectives() =>
        new[]
        {
            UsingDirective
            (
                QualifiedName
                (
                    QualifiedName
                    (
                        IdentifierName("Microsoft"),
                        IdentifierName("OData")
                    ),
                    IdentifierName("Edm")
                )
            )
            .WithUsingKeyword
            (
                Token
                (
                    TriviaList(Comment("// Generated Code")),
                    SyntaxKind.UsingKeyword,
                    TriviaList()
                )
            ),
            UsingDirective
            (
                QualifiedName
                (
                    QualifiedName
                    (
                        IdentifierName("Microsoft"),
                        IdentifierName("OData")
                    ),
                    IdentifierName("ModelBuilder")
                )
            )
        };

    private MemberDeclarationSyntax[] GetClassMembers()
    {
        var members = new List<MemberDeclarationSyntax>();
        members.Add(CreateODataConventionModelBuilderStatement());
        members.Add(GetConstructor());
        members.Add(GetInitMethodDeclaration());
        members.AddRange(GetEntitySetPropertyDeclarations());
        members.Add(GetEdmModelMethodDeclaration());
        return members.ToArray();
    }

    private IEnumerable<AppApiGroupTemplate> GetOdataGroupTemplates() =>
        appTemplate.GroupTemplates.Where(g => g.IsODataGroup());

    private FieldDeclarationSyntax CreateODataConventionModelBuilderStatement() =>
        FieldDeclaration
        (
            VariableDeclaration(IdentifierName("ODataConventionModelBuilder"))
            .WithVariables
            (
                SingletonSeparatedList
                (
                    VariableDeclarator(Identifier("odataBuilder"))
                        .WithInitializer
                        (
                            EqualsValueClause(ImplicitObjectCreationExpression())
                        )
                )
            )
        )
        .WithModifiers
        (
            TokenList
            (
                new[]
                {
                    Token(SyntaxKind.PrivateKeyword),
                    Token(SyntaxKind.ReadOnlyKeyword)
                }
            )
        );

    private ConstructorDeclarationSyntax GetConstructor() =>
        ConstructorDeclaration
        (
            Identifier("EdmModelBuilder")
        )
        .WithModifiers(TokenList(Token(SyntaxKind.PublicKeyword)))
        .WithBody
        (
            Block
            (
               GetEntitySetAssignments()
                .Union
                (
                    new[]
                    {
                        ExpressionStatement
                        (
                            InvocationExpression
                            (
                                IdentifierName
                                (
                                    Identifier
                                    (
                                        TriviaList(),
                                        SyntaxKind.InitKeyword,
                                        "init",
                                        "init",
                                        TriviaList()
                                    )
                                )
                            )
                        )
                    }
                )
            )
        );

    private MethodDeclarationSyntax GetInitMethodDeclaration() =>
        MethodDeclaration
        (
            PredefinedType(Token(SyntaxKind.VoidKeyword)),
            Identifier
            (
                TriviaList(),
                SyntaxKind.InitKeyword,
                "init",
                "init",
                TriviaList()
            )
        )
        .WithModifiers(TokenList(Token(SyntaxKind.PartialKeyword)))
        .WithSemicolonToken(Token(SyntaxKind.SemicolonToken));

    private ExpressionStatementSyntax[] GetEntitySetAssignments()
    {
        var expressions = new List<ExpressionStatementSyntax>();
        foreach(var groupTemplate in GetOdataGroupTemplates())
        {
            expressions.Add
            (
                ExpressionStatement
                (
                    AssignmentExpression
                    (
                        SyntaxKind.SimpleAssignmentExpression,
                        IdentifierName(groupTemplate.Name),
                        GetEntitySetExpression(groupTemplate)
                    )
                )
            );
        }
        return expressions.ToArray();
    }

    private InvocationExpressionSyntax GetEntitySetExpression(AppApiGroupTemplate groupTemplate) =>
        InvocationExpression
        (
            MemberAccessExpression
            (
                SyntaxKind.SimpleMemberAccessExpression,
                IdentifierName("odataBuilder"),
                GenericName(Identifier("EntitySet"))
                .WithTypeArgumentList
                (
                    TypeArgumentList
                    (
                        SingletonSeparatedList
                        (
                            new TypeSyntaxFromValueTemplate
                            (
                                groupTemplate.QueryableTemplates().First().ElementTemplate
                            )
                            .Value()
                        )
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
                            Literal(groupTemplate.Name)
                        )
                    )
                )
            )
        );

    private PropertyDeclarationSyntax[] GetEntitySetPropertyDeclarations()
    {
        var properties = new List<PropertyDeclarationSyntax>();
        foreach (var groupTemplate in GetOdataGroupTemplates())
        {
            properties.Add
            (
                PropertyDeclaration
                (
                    GenericName(Identifier("EntitySetConfiguration")
                )
                .WithTypeArgumentList
                (
                    TypeArgumentList
                    (
                        SingletonSeparatedList
                        (
                            new TypeSyntaxFromValueTemplate
                            (
                                groupTemplate.QueryableTemplates().First().ElementTemplate
                            )
                            .Value()
                        )
                    )
                ),
                Identifier(groupTemplate.Name))
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
        return properties.ToArray();
    }

    private MethodDeclarationSyntax GetEdmModelMethodDeclaration() =>
        MethodDeclaration
        (
            IdentifierName("IEdmModel"),
            Identifier("GetEdmModel")
        )
        .WithModifiers
        (
            TokenList(Token(SyntaxKind.PublicKeyword))
        )
        .WithExpressionBody
        (
            ArrowExpressionClause
            (
                InvocationExpression
                (
                    MemberAccessExpression
                    (
                        SyntaxKind.SimpleMemberAccessExpression,
                        IdentifierName("odataBuilder"),
                        IdentifierName("GetEdmModel")
                    )
                )
            )
        )
        .WithSemicolonToken(Token(SyntaxKind.SemicolonToken));

}
