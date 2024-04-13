using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using XTI_App.Api;
using XTI_WebApp.CodeGeneration.CSharp;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace XTI_WebApp.ClientGenerator.CSharp;

internal class EnumValueClass
{
    private readonly string ns;
    private readonly Func<string, Stream> createStream;
    private readonly EnumValueTemplate template;

    public EnumValueClass(string ns, Func<string, Stream> createStream, EnumValueTemplate template)
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
                            TriviaList(new GeneratedCodeComment().Value()),
                            SyntaxKind.NamespaceKeyword,
                            TriviaList()
                        )
                    )
                    .WithMembers
                    (
                        SingletonList<MemberDeclarationSyntax>
                        (
                            EnumDeclaration(template.DataType.Name)
                                .WithModifiers
                                (
                                    TokenList(Token(SyntaxKind.PublicKeyword))
                                )
                                .WithMembers
                                (
                                    SeparatedList<EnumMemberDeclarationSyntax>
                                    (
                                        valueDeclarations()
                                    )
                                )
                            )
                    )
                )
            );
    }

    private IEnumerable<SyntaxNodeOrToken> valueDeclarations()
    {
        var declarations = new List<SyntaxNodeOrToken>();
        var lastValue = template.EnumValues.Last();
        foreach(var value in template.EnumValues)
        {
            declarations.Add
            (
                EnumMemberDeclaration(Identifier(value.Name))
                    .WithEqualsValue
                    (
                        EqualsValueClause
                        (
                            LiteralExpression
                            (
                                SyntaxKind.NumericLiteralExpression,
                                Literal((int)value.Value)
                            )
                        )
                    )
            );
            if(value != lastValue)
            {
                declarations.Add(Token(SyntaxKind.CommaToken));
            }
        }
        return declarations;
    }

    private string getClassName() => template.DataType.Name;

    private Task outputClass(CompilationUnitSyntax compilationUnitSyntax, string className)
    {
        var cSharpFile = new CSharpFile(compilationUnitSyntax, createStream, className);
        return cSharpFile.Output();
    }
}
