using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using XTI_App.Abstractions;
using XTI_App.Api;
using XTI_WebApp.CodeGeneration.CSharp;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace XTI_WebApp.ClientGenerator.CSharp;

internal sealed class RolesClass
{
    private readonly string ns;
    private readonly Func<string, Stream> createStream;
    private readonly AppApiTemplate template;

    public RolesClass(string ns, Func<string, Stream> createStream, AppApiTemplate template)
    {
        this.ns = ns;
        this.createStream = createStream;
        this.template = template;
    }

    public async Task Output()
    {
        var groupClient = classFile();
        var className = getClassName();
        await outputClass(groupClient, className);
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
                            instanceDeclaration(),
                            ctorDeclaration()
                        }
                        .Union(roleNameDeclarations())
                    )
                )
        );
        return groupClass.ToArray();
    }

    private MemberDeclarationSyntax instanceDeclaration() =>
        FieldDeclaration
        (
            VariableDeclaration(IdentifierName(getClassName()))
                .WithVariables
                (
                    SingletonSeparatedList
                    (
                        VariableDeclarator(Identifier("Instance"))
                            .WithInitializer
                            (
                                EqualsValueClause
                                (
                                    ObjectCreationExpression(IdentifierName(getClassName()))
                                        .WithArgumentList(ArgumentList())
                                )
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
                        Token(SyntaxKind.PublicKeyword),
                        Token(SyntaxKind.StaticKeyword),
                        Token(SyntaxKind.ReadOnlyKeyword)
                    }
                )
            );

    private MemberDeclarationSyntax ctorDeclaration() =>
        ConstructorDeclaration(Identifier(getClassName()))
            .WithModifiers
            (
                TokenList(Token(SyntaxKind.PrivateKeyword))
            )
            .WithBody(Block());

    private IEnumerable<MemberDeclarationSyntax> roleNameDeclarations()
    {
        var statements = new List<MemberDeclarationSyntax>();
        foreach (var roleName in template.RoleNames)
        {
            statements.Add(roleNameDeclaration(roleName));
        }
        return statements;
    }

    private MemberDeclarationSyntax roleNameDeclaration(AppRoleName roleName) =>
        PropertyDeclaration
        (
            PredefinedType(Token(SyntaxKind.StringKeyword)),
            Identifier(roleName.DisplayText.Replace(" ", ""))
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
                LiteralExpression
                (
                    SyntaxKind.StringLiteralExpression,
                    Literal(roleName.DisplayText)
                )
            )
        )
        .WithSemicolonToken(Token(SyntaxKind.SemicolonToken));

    private string getClassName() => $"{template.Name}RoleNames";

    private Task outputClass(CompilationUnitSyntax compilationUnitSyntax, string className)
    {
        var cSharpFile = new CSharpFile(compilationUnitSyntax, createStream, className);
        return cSharpFile.Output();
    }
}
