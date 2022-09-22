using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using XTI_App.Api;
using XTI_WebApp.CodeGeneration.CSharp;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace XTI_WebApp.ClientGenerator.CSharp;

public sealed class ApiObjectClass
{
    private readonly string ns;
    private readonly Func<string, Stream> createStream;
    private readonly ObjectValueTemplate template;

    public ApiObjectClass(string ns, Func<string, Stream> createStream, ObjectValueTemplate template)
    {
        this.ns = ns;
        this.createStream = createStream;
        this.template = template;
    }

    public async Task Output()
    {
        var apiObject = createApiObject();
        var className = template.DataType.Name;
        await outputClass(apiObject, className);
    }

    private CompilationUnitSyntax createApiObject()
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
                            SingletonList<MemberDeclarationSyntax>
                            (
                                ClassDeclaration(template.DataType.Name)
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
                                            getProperties(template.PropertyTemplates)
                                        )
                                    )
                                )
                            )
                        )
                );
    }

    private static IEnumerable<MemberDeclarationSyntax> getProperties(IEnumerable<ObjectPropertyTemplate> properties)
    {
        var propDecls = new List<MemberDeclarationSyntax>();
        foreach (var property in properties)
        {
            var propDecl = PropertyDeclaration
                (
                    GetTypeSyntaxFromValueTemplate(property.ValueTemplate),
                    Identifier(property.Name)
                )
                .WithModifiers
                (
                    TokenList(Token(SyntaxKind.PublicKeyword))
                )
                .WithAccessorList
                (
                    AccessorList
                    (
                        List
                        (
                            new AccessorDeclarationSyntax[]
                            {
                                AccessorDeclaration(SyntaxKind.GetAccessorDeclaration)
                                    .WithSemicolonToken(Token(SyntaxKind.SemicolonToken)),
                                AccessorDeclaration(SyntaxKind.SetAccessorDeclaration)
                                    .WithSemicolonToken(Token(SyntaxKind.SemicolonToken))
                            }
                        )
                    )
                );
            if (property.ValueTemplate.DataType == typeof(string))
            {
                propDecl = propDecl
                    .WithInitializer
                    (
                        EqualsValueClause
                        (
                            LiteralExpression
                            (
                                SyntaxKind.StringLiteralExpression,
                                Literal("")
                            )
                        )
                    )
                    .WithSemicolonToken(Token(SyntaxKind.SemicolonToken));
            }
            else if (property.ValueTemplate is NumericValueTemplate)
            {
                propDecl = propDecl
                    .WithInitializer
                    (
                        EqualsValueClause
                        (
                            InvocationExpression
                            (
                                MemberAccessExpression
                                (
                                    SyntaxKind.SimpleMemberAccessExpression,
                                    MemberAccessExpression
                                    (
                                        SyntaxKind.SimpleMemberAccessExpression,
                                        IdentifierName(property.ValueTemplate.DataType.Name),
                                        IdentifierName("Values")
                                    ),
                                    IdentifierName("GetDefault")
                                )
                            )
                        )
                    )
                    .WithSemicolonToken(Token(SyntaxKind.SemicolonToken));
            }
            else if (property.ValueTemplate is ArrayValueTemplate arr)
            {
                propDecl = propDecl
                    .WithInitializer
                    (
                        EqualsValueClause
                        (
                            ArrayCreationExpression
                            (
                                ArrayType
                                (
                                    GetTypeSyntaxFromValueTemplate(arr.ElementTemplate)
                                )
                                .WithRankSpecifiers
                                (
                                    SingletonList
                                    (
                                        ArrayRankSpecifier
                                        (
                                            SingletonSeparatedList<ExpressionSyntax>
                                            (
                                                LiteralExpression
                                                (
                                                    SyntaxKind.NumericLiteralExpression,
                                                    Literal(0)
                                                )
                                            )
                                        )
                                    )
                                )
                            )
                        )
                    )
                    .WithSemicolonToken(Token(SyntaxKind.SemicolonToken));
            }
            else if (property.ValueTemplate is DictionaryValueTemplate dict)
            {
                propDecl = propDecl
                    .WithInitializer
                    (
                        EqualsValueClause
                        (
                            ObjectCreationExpression
                            (
                                    GenericName(Identifier("Dictionary"))
                                    .WithTypeArgumentList
                                    (
                                        TypeArgumentList
                                        (
                                            SeparatedList<TypeSyntax>
                                            (
                                                new SyntaxNodeOrToken[]
                                                {
                                                    GetTypeSyntaxFromValueTemplate(dict.KeyTemplate),
                                                    Token(SyntaxKind.CommaToken),
                                                    GetTypeSyntaxFromValueTemplate(dict.ValueTemplate)
                                                }
                                            )
                                        )
                                    )
                                )
                                .WithArgumentList(ArgumentList())
                        )
                    )
                    .WithSemicolonToken(Token(SyntaxKind.SemicolonToken));
            }
            else if(property.ValueTemplate is not SimpleValueTemplate)
            {
                propDecl = propDecl
                    .WithInitializer
                    (
                        EqualsValueClause
                        (
                            ObjectCreationExpression(IdentifierName(property.ValueTemplate.DataType.Name == "IFormFile" ? "FileUpload" : property.ValueTemplate.DataType.Name))
                                .WithArgumentList(ArgumentList())
                        )
                    )
                    .WithSemicolonToken(Token(SyntaxKind.SemicolonToken));
            }
            propDecls.Add(propDecl);
        }
        return propDecls;
    }

    private static TypeSyntax GetTypeSyntaxFromValueTemplate(ValueTemplate valueTemplate)
    {
        if(valueTemplate is FileUploadValueTemplate)
        {
            return IdentifierName("FileUpload");
        }
        if(valueTemplate is ArrayValueTemplate arrTempl && arrTempl.ElementTemplate is FileUploadValueTemplate)
        {
            return ArrayType(IdentifierName("FileUpload"))
            .WithRankSpecifiers
            (
                SingletonList
                (
                    ArrayRankSpecifier
                    (
                        SingletonSeparatedList<ExpressionSyntax>
                        (
                            OmittedArraySizeExpression()
                        )
                    )
                )
            );
        }
        return new TypeSyntaxFromValueTemplate(valueTemplate).Value();
    }

    private Task outputClass(CompilationUnitSyntax compilationUnitSyntax, string className)
    {
        var cSharpFile = new CSharpFile(compilationUnitSyntax, createStream, className);
        return cSharpFile.Output();
    }
}