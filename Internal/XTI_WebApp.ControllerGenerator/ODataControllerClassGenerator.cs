using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using XTI_App.Api;
using XTI_WebApp.CodeGeneration.CSharp;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace XTI_WebApp.ControllerGenerator;

internal class ODataControllerClassGenerator
{
    private readonly string ns;
    private readonly Func<string, Stream> createStream;
    private readonly AppApiTemplate appTemplate;
    private readonly AppApiGroupTemplate groupTemplate;

    public ODataControllerClassGenerator
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
                        ControllerClassDeclaration()
                        .WithMembers
                        (
                            List
                            (
                                new MemberDeclarationSyntax[]
                                {
                                    constructorDeclaration(apiClassName)
                                }
                            )
                        )
                    )
                )
            )
        );
        return code;
    }

    private string GetControllerClassName() => $"{groupTemplate.Name}Controller";

    private ConstructorDeclarationSyntax constructorDeclaration(string apiClassName)
    {
        return
            ConstructorDeclaration(Identifier(GetControllerClassName()))
            .WithModifiers
            (
                TokenList
                (
                    new[]
                    {
                        Token(SyntaxKind.PublicKeyword)
                    }
                )
            )
            .WithParameterList
            (
                ParameterList
                (
                    SingletonSeparatedList
                    (
                        Parameter(Identifier("api"))
                            .WithType(IdentifierName(apiClassName))
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
                        SingletonSeparatedList
                        (
                            Argument
                            (
                                MemberAccessExpression
                                (
                                    SyntaxKind.SimpleMemberAccessExpression,
                                    IdentifierName("api"),
                                    IdentifierName(groupTemplate.Name)
                                )
                            )
                        )
                    )
                )
            )
            .WithBody
            (
                Block
                (
                )
            );
    }

    private ClassDeclarationSyntax ControllerClassDeclaration() =>
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
                                        groupTemplate.Access.IsAnonymousAllowed 
                                            ? "AllowAnonymous" 
                                            : "Authorize"
                                    )
                                )
                            )
                        ),
                        AttributeList
                        (
                            SingletonSeparatedList
                            (
                                Attribute(IdentifierName("Route"))
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
                                                        Literal($"odata/{groupTemplate.Name}")
                                                    )
                                                )
                                            )
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
                            GenericName(Identifier("XtiODataController"))
                                .WithTypeArgumentList
                                (
                                    TypeArgumentList
                                    (
                                        SeparatedList
                                        (
                                            new[]
                                            {
                                                typeSyntax(groupTemplate.ActionTemplates.First().ModelTemplate),
                                                typeSyntax(groupTemplate.QueryableTemplates().First().ElementTemplate)
                                            }
                                        )
                                    )
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
