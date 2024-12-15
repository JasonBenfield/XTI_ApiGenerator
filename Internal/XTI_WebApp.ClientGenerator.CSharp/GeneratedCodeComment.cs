using Microsoft.CodeAnalysis;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace XTI_WebApp.ClientGenerator.CSharp;

internal sealed class GeneratedCodeComment
{
    public SyntaxTrivia Value() => Comment("// Generated Code");
}
