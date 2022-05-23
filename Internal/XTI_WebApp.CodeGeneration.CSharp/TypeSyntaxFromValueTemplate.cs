using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using XTI_App.Api;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace XTI_WebApp.CodeGeneration.CSharp;

public sealed class TypeSyntaxFromValueTemplate
{
    private readonly ValueTemplate valueTemplate;

    public TypeSyntaxFromValueTemplate(ValueTemplate valueTemplate)
    {
        this.valueTemplate = valueTemplate;
    }

    public TypeSyntax Value()
    {
        if (valueTemplate is DictionaryValueTemplate dict)
        {
            return GenericName(Identifier("IDictionary"))
                .WithTypeArgumentList
                (
                    TypeArgumentList
                    (
                        SeparatedList
                        (
                            new[]
                            {
                                new TypeSyntaxFromValueTemplate(dict.KeyTemplate).Value(),
                                new TypeSyntaxFromValueTemplate(dict.ValueTemplate).Value()
                            }
                        )
                    )
                );
        }
        if (valueTemplate.DataType == typeof(string))
        {
            return PredefinedType(Token(SyntaxKind.StringKeyword));
        }
        if (valueTemplate is SimpleValueTemplate simple)
        {
            TypeSyntax typeSyntax;
            if 
            (
                valueTemplate.DataType == typeof(DateTime) || 
                valueTemplate.DataType == typeof(DateTimeOffset) ||
                valueTemplate.DataType == typeof(TimeSpan)
            )
            {
                typeSyntax = IdentifierName(valueTemplate.DataType.Name);
            }
            else
            {
                SyntaxKind syntaxKind = SyntaxKind.None;
                if (valueTemplate.DataType == typeof(int))
                {
                    syntaxKind = SyntaxKind.IntKeyword;
                }
                else if (valueTemplate.DataType == typeof(long))
                {
                    syntaxKind = SyntaxKind.LongKeyword;
                }
                else if (valueTemplate.DataType == typeof(short))
                {
                    syntaxKind = SyntaxKind.ShortKeyword;
                }
                else if (valueTemplate.DataType == typeof(decimal))
                {
                    syntaxKind = SyntaxKind.DecimalKeyword;
                }
                else if (valueTemplate.DataType == typeof(double))
                {
                    syntaxKind = SyntaxKind.DoubleKeyword;
                }
                else if (valueTemplate.DataType == typeof(bool))
                {
                    syntaxKind = SyntaxKind.BoolKeyword;
                }
                else if (valueTemplate.DataType == typeof(string))
                {
                    syntaxKind = SyntaxKind.StringKeyword;
                }
                else if (valueTemplate.DataType == typeof(object))
                {
                    return PredefinedType(Token(SyntaxKind.ObjectKeyword));
                }
                typeSyntax = PredefinedType(Token(syntaxKind));
            }
            if (typeSyntax != null)
            {
                if (simple.IsNullable)
                {
                    return NullableType(typeSyntax);
                }
                return typeSyntax;
            }
        }
        if (valueTemplate is ArrayValueTemplate arr)
        {
            return ArrayType
            (
                new TypeSyntaxFromValueTemplate(arr.ElementTemplate).Value()
            )
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
        return IdentifierName(valueTemplate.DataType.Name);
    }
}