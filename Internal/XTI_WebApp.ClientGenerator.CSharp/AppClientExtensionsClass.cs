using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using XTI_App.Api;
using XTI_WebApp.CodeGeneration.CSharp;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace XTI_WebApp.ClientGenerator.CSharp;

internal sealed class AppClientExtensionsClass
{
    private readonly string ns;
    private readonly Func<string, Stream> createStream;
    private readonly AppApiTemplate template;

    public AppClientExtensionsClass(string ns, Func<string, Stream> createStream, AppApiTemplate template)
    {
        this.ns = ns;
        this.createStream = createStream;
        this.template = template;
    }

    public async Task Output()
    {
        var groupClient = GenerateCode();
        var className = getClassName();
        await outputClass(groupClient, className);
    }

    private CompilationUnitSyntax GenerateCode()
    {
        return CompilationUnit()
            .WithUsings
            (
                SingletonList
                (
                    UsingDirective
                    (
                        QualifiedName
                        (
                            QualifiedName
                            (
                                IdentifierName("Microsoft"),
                                IdentifierName("Extensions")
                            ),
                            IdentifierName("DependencyInjection")
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
                    )
                )
            )
            .WithMembers
            (
                SingletonList<MemberDeclarationSyntax>
                (
                    FileScopedNamespaceDeclaration(IdentifierName(ns))
                        .WithNamespaceKeyword
                        (
                            Token
                            (
                                TriviaList(),
                                SyntaxKind.NamespaceKeyword,
                                TriviaList()
                            )
                        )
                        .WithMembers
                        (
                            List
                            (
                                DeclarationForClass()
                            )
                        )
                )
            );
    }

    private MemberDeclarationSyntax[] DeclarationForClass()
    {
        var extensionsClass = new List<MemberDeclarationSyntax>();
        extensionsClass.Add
        (
            ClassDeclaration(getClassName())
                .WithModifiers
                (
                    TokenList
                    (
                        [
                            Token(SyntaxKind.PublicKeyword),
                            Token(SyntaxKind.StaticKeyword)
                        ]
                    )
                )
                .WithMembers
                (
                    SingletonList<MemberDeclarationSyntax>
                    (
                        MethodDeclaration
                        (
                            PredefinedType(Token(SyntaxKind.VoidKeyword)),
                            Identifier($"Add{template.Name}AppClient")
                        )
                        .WithModifiers
                        (
                            TokenList
                            (
                                [
                                    Token(SyntaxKind.PublicKeyword),
                                    Token(SyntaxKind.StaticKeyword)
                                ]
                            )
                        )
                        .WithParameterList
                        (
                            ParameterList
                            (
                                SingletonSeparatedList
                                (
                                    Parameter(Identifier("services"))
                                        .WithModifiers
                                        (
                                            TokenList(Token(SyntaxKind.ThisKeyword))
                                        )
                                        .WithType(IdentifierName("IServiceCollection"))
                                )
                            )
                        )
                        .WithBody
                        (
                            Block
                            (
                                ExpressionStatement
                                (
                                    InvocationExpression
                                    (
                                        MemberAccessExpression
                                        (
                                            SyntaxKind.SimpleMemberAccessExpression,
                                            IdentifierName("services"),
                                            GenericName(Identifier("TryAddScoped"))
                                                .WithTypeArgumentList
                                                (
                                                    TypeArgumentList
                                                    (
                                                        SeparatedList<TypeSyntax>
                                                        (
                                                            [
                                                                IdentifierName("IAppClientSessionKey"),
                                                                IdentifierName("EmptyAppClientSessionKey")
                                                            ]
                                                        )
                                                    )
                                                )
                                            )
                                    )
                                ),
                                ExpressionStatement
                                (
                                    InvocationExpression
                                    (
                                        MemberAccessExpression
                                        (
                                            SyntaxKind.SimpleMemberAccessExpression,
                                            IdentifierName("services"),
                                            GenericName(Identifier("TryAddScoped"))
                                                .WithTypeArgumentList
                                                (
                                                    TypeArgumentList
                                                    (
                                                        SeparatedList<TypeSyntax>
                                                        (
                                                            [
                                                                IdentifierName("IAppClientRequestKey"),
                                                                IdentifierName("EmptyAppClientRequestKey")
                                                            ]
                                                        )
                                                    )
                                                )
                                            )
                                    )
                                ),
                                ExpressionStatement
                                (
                                    InvocationExpression
                                    (
                                        MemberAccessExpression
                                        (
                                            SyntaxKind.SimpleMemberAccessExpression,
                                            IdentifierName("services"),
                                            GenericName(Identifier("AddScoped"))
                                                .WithTypeArgumentList
                                                (
                                                    TypeArgumentList
                                                    (
                                                        SingletonSeparatedList<TypeSyntax>
                                                        (
                                                            IdentifierName($"{template.Name}AppClientFactory")
                                                        )
                                                    )
                                                )
                                            )
                                    )
                                ),
                                ExpressionStatement
                                (
                                    InvocationExpression
                                    (
                                        MemberAccessExpression
                                        (
                                            SyntaxKind.SimpleMemberAccessExpression,
                                            IdentifierName("services"),
                                            IdentifierName("AddScoped")
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
                                                    SimpleLambdaExpression
                                                    (
                                                        Parameter(Identifier("sp"))
                                                    )
                                                    .WithExpressionBody
                                                    (
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
                                                                        IdentifierName("sp"),
                                                                        GenericName(Identifier("GetRequiredService"))
                                                                        .WithTypeArgumentList
                                                                        (
                                                                            TypeArgumentList
                                                                            (
                                                                                SingletonSeparatedList<TypeSyntax>
                                                                                (
                                                                                    IdentifierName($"{template.Name}AppClientFactory")
                                                                                )
                                                                            )
                                                                        )
                                                                    )
                                                                ),
                                                                IdentifierName("Create")
                                                            )
                                                        )
                                                    )
                                                )
                                            )
                                        )
                                    )
                                ),
                                ExpressionStatement
                                (
                                    InvocationExpression
                                    (
                                        MemberAccessExpression
                                        (
                                            SyntaxKind.SimpleMemberAccessExpression,
                                            IdentifierName("services"),
                                            GenericName(Identifier("AddScoped"))
                                                .WithTypeArgumentList
                                                (
                                                    TypeArgumentList
                                                    (
                                                        SingletonSeparatedList<TypeSyntax>
                                                        (
                                                            IdentifierName($"{template.Name}AppClientVersion")
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
        return extensionsClass.ToArray();
    }

    private string getClassName() => $"{template.Name}AppClientExtensions";

    private Task outputClass(CompilationUnitSyntax compilationUnitSyntax, string className)
    {
        var cSharpFile = new CSharpFile(compilationUnitSyntax, createStream, className);
        return cSharpFile.Output();
    }
}
