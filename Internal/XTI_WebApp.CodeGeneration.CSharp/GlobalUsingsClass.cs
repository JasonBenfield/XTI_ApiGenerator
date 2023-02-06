using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace XTI_WebApp.CodeGeneration.CSharp;

public sealed class GlobalUsingsClass
{
    private readonly Func<string, Stream> createStream;
    private readonly string[] namespaces;

    public GlobalUsingsClass(Func<string, Stream> createStream, string[] namespaces)
    {
        this.createStream = createStream;
        this.namespaces = namespaces;
    }

    public async Task Output()
    {
        var usings = new List<UsingDirectiveSyntax>();
        var firstNS = namespaces[0];
        usings.Add
        (
            UsingDirective(IdentifierName(firstNS))
                .WithGlobalKeyword
                (
                    Token
                    (
                        TriviaList(Comment("// Generated Code")),
                        SyntaxKind.GlobalKeyword,
                        TriviaList()
                    )
                )
        );
        foreach (var ns in namespaces.Skip(1))
        {
            usings.Add
            (
                UsingDirective(IdentifierName(ns))
                    .WithGlobalKeyword(Token(SyntaxKind.GlobalKeyword))
            );
        }
        var code = CompilationUnit()
            .WithUsings
            (
                List(usings)
            );
        var cSharpFile = new CSharpFile(code, createStream, "DefaultUsings");
        await cSharpFile.Output();
    }

}