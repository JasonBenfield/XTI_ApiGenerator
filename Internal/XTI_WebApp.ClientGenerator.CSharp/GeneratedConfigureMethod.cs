using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace XTI_WebApp.ClientGenerator.CSharp;

internal static class GeneratedConfigureMethod
{
    internal static ExpressionStatementSyntax Invocation()
    {
        return ExpressionStatement
        (
            InvocationExpression
            (
                IdentifierName("Configure")
            )
        );
    }

    internal static MethodDeclarationSyntax Declaration()
    {
        return MethodDeclaration
        (
            PredefinedType(Token(SyntaxKind.VoidKeyword)),
            Identifier("Configure")
        )
        .WithModifiers
        (
            TokenList
            (
                [
                    Token(SyntaxKind.PartialKeyword)
                ]
            )
        )
        .WithSemicolonToken(Token(SyntaxKind.SemicolonToken));
    }

}
