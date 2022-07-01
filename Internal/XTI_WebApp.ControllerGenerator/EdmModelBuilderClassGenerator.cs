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
                        Token(SyntaxKind.SealedKeyword)
                    }
                )
            )
            .WithMembers
            (
                SingletonList<MemberDeclarationSyntax>
                (
                    MethodDeclaration
                    (
                        IdentifierName("IEdmModel"),
                        Identifier("GetEdmModel")
                    )
                    .WithModifiers
                    (
                        TokenList(Token(SyntaxKind.PublicKeyword))
                    )
                    .WithBody
                    (
                        GetEdmModelMethodBody()
                    )
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

    private BlockSyntax GetEdmModelMethodBody()
    {
        var statements = new List<StatementSyntax>();
        statements.Add(CreateODataConventionModelBuilderStatement());
        foreach (var groupTemplate in GetOdataGroupTemplates())
        {
            statements.Add(GetEntitySetExpression(groupTemplate));
        }
        statements.Add(GetEdmModelReturnStatement());
        return Block(statements);
    }

    private IEnumerable<AppApiGroupTemplate> GetOdataGroupTemplates() =>
        appTemplate.GroupTemplates.Where(g => g.IsODataGroup());

    private LocalDeclarationStatementSyntax CreateODataConventionModelBuilderStatement() =>
        LocalDeclarationStatement
        (
            VariableDeclaration
            (
                IdentifierName
                (
                    Identifier
                    (
                        TriviaList(),
                        SyntaxKind.VarKeyword,
                        "var",
                        "var",
                        TriviaList()
                    )
                )
            )
            .WithVariables
            (
                SingletonSeparatedList
                (
                    VariableDeclarator(Identifier("odataBuilder"))
                    .WithInitializer
                    (
                        EqualsValueClause
                        (
                            ObjectCreationExpression
                            (
                                IdentifierName("ODataConventionModelBuilder")
                            )
                            .WithArgumentList(ArgumentList())
                        )
                    )
                )
            )
        );


    private ExpressionStatementSyntax GetEntitySetExpression(AppApiGroupTemplate groupTemplate) =>
        ExpressionStatement
        (
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
            )
        );

    private ReturnStatementSyntax GetEdmModelReturnStatement() =>
        ReturnStatement
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
        );
}
