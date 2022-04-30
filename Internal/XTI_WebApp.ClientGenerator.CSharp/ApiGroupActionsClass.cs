using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using XTI_App.Api;
using XTI_WebApp.CodeGeneration.CSharp;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace XTI_WebApp.ClientGenerator.CSharp;

internal sealed class ApiGroupActionsClass
{
    private readonly string ns;
    private readonly Func<string, Stream> createStream;
    private readonly AppApiGroupTemplate template;
    private readonly AppApiActionTemplate[] actions;

    public ApiGroupActionsClass(string ns, Func<string, Stream> createStream, AppApiGroupTemplate template)
    {
        this.ns = ns;
        this.createStream = createStream;
        this.template = template;
        actions = template.ActionsForGetMethod();
    }

    public async Task Output()
    {
        if (actions.Any())
        {
            var groupClient = classFile();
            var className = getClassName();
            await outputClass(groupClient, className);
        }
    }

    private CompilationUnitSyntax classFile()
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
                                classDeclaration()
                            )
                        )
                )
            );
    }

    private MemberDeclarationSyntax[] classDeclaration()
    {
        var groupClass = new List<MemberDeclarationSyntax>();
        groupClass.Add
        (
            ClassDeclaration(getClassName())
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
                        new[]
                        {
                            ctorDeclaration()
                        }
                        .Union(actionDeclarations())
                    )
                )
        );
        return groupClass.ToArray();
    }

    private MemberDeclarationSyntax ctorDeclaration()
    {
        return ConstructorDeclaration(Identifier(getClassName()))
            .WithModifiers
            (
                TokenList(Token(SyntaxKind.InternalKeyword))
            )
            .WithParameterList
            (
                ParameterList
                (
                    SeparatedList<ParameterSyntax>
                    (
                        new SyntaxNodeOrToken[]
                        {
                            Parameter(Identifier("appClientUrl"))
                                .WithType(IdentifierName("AppClientUrl"))
                        }
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
        foreach (var action in actions)
        {
            statements.Add(actionAssignment(action));
        }
        return statements;
    }

    private StatementSyntax actionAssignment(AppApiActionTemplate action) =>
        ExpressionStatement
        (
            AssignmentExpression
            (
                SyntaxKind.SimpleAssignmentExpression,
                IdentifierName(action.Name),
                ObjectCreationExpression
                (
                    GenericName(Identifier("AppClientGetAction"))
                        .WithTypeArgumentList
                        (
                            TypeArgumentList
                            (
                                SingletonSeparatedList
                                (
                                    new TypeSyntaxFromValueTemplate(action.ModelTemplate).Value()
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
                            new SyntaxNodeOrToken[]
                            {
                                Argument(IdentifierName("appClientUrl")),
                                Token(SyntaxKind.CommaToken),
                                Argument(LiteralExpression(SyntaxKind.StringLiteralExpression, Literal(action.Name)))
                            }
                        )
                    )
                )
            )
        );

    private IEnumerable<MemberDeclarationSyntax> actionDeclarations()
    {
        var statements = new List<MemberDeclarationSyntax>();
        foreach (var action in actions)
        {
            statements.Add(actionDeclaration(action));
        }
        return statements;
    }

    private MemberDeclarationSyntax actionDeclaration(AppApiActionTemplate action) =>
        PropertyDeclaration
        (
            GenericName(Identifier("AppClientGetAction"))
                .WithTypeArgumentList
                (
                    TypeArgumentList
                    (
                        SingletonSeparatedList
                        (
                            new TypeSyntaxFromValueTemplate(action.ModelTemplate).Value()
                        )
                    )
                ),
            Identifier(action.Name)
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
        );

    private string getClassName() => $"{template.Name}Actions";

    private Task outputClass(CompilationUnitSyntax compilationUnitSyntax, string className)
    {
        var cSharpFile = new CSharpFile(compilationUnitSyntax, createStream, className);
        return cSharpFile.Output();
    }
}
