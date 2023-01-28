using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using XTI_App.Api;
using XTI_Forms;
using XTI_WebApp.CodeGeneration.CSharp;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace XTI_WebApp.ClientGenerator.CSharp;

public sealed class ComplexFieldClass
{
    private readonly string ns;
    private readonly Func<string, Stream> createStream;
    private readonly IComplexField template;
    private readonly string baseClassName;

    public ComplexFieldClass(string ns, Func<string, Stream> createStream, IComplexField template, string baseClassName)
    {
        this.ns = ns;
        this.createStream = createStream;
        this.template = template;
        this.baseClassName = baseClassName;
    }

    public async Task Output()
    {
        var complexField = createComplextFieldClass();
        var className = template.TypeName;
        await outputClass(complexField, className);
    }

    private CompilationUnitSyntax createComplextFieldClass()
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
                            SingletonList
                            (
                                classDeclaration()
                            )
                        )
                )
            );
    }

    private MemberDeclarationSyntax classDeclaration()
    {
        return ClassDeclaration(template.TypeName)
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
            .WithBaseList
            (
                BaseList
                (
                    SingletonSeparatedList<BaseTypeSyntax>
                    (
                        SimpleBaseType(IdentifierName(baseClassName))
                    )
                )
            )
            .WithMembers
            (
                List
                (
                    classMembers()
                )
            );
    }

    private IEnumerable<MemberDeclarationSyntax> classMembers()
    {
        var members = new List<MemberDeclarationSyntax>();
        members.Add(constructor());
        foreach (var field in template.Fields)
        {
            members.Add(fieldDeclarationStatement(field));
        }
        return members;
    }

    private MemberDeclarationSyntax constructor() =>
        ConstructorDeclaration(Identifier(template.TypeName))
            .WithModifiers
            (
                TokenList(Token(SyntaxKind.PublicKeyword))
            )
            .WithParameterList
            (
                ParameterList
                (
                    SeparatedList<ParameterSyntax>
                    (
                        constructorArgs()
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
                        SeparatedList<ArgumentSyntax>
                        (
                            constructorBaseArgs()
                        )
                    )
                )
            )
            .WithBody
            (
                Block(constructorBody())
            );

    private IEnumerable<SyntaxNodeOrToken> constructorArgs()
    {
        var args = new List<SyntaxNodeOrToken>();
        if (baseClassName == "ComplexField")
        {
            args.Add
            (
                Parameter(Identifier("prefix"))
                    .WithType
                    (
                        PredefinedType(Token(SyntaxKind.StringKeyword))
                    )
            );
            args.Add(Token(SyntaxKind.CommaToken));
            args.Add
            (
                Parameter(Identifier("name"))
                    .WithType
                    (
                        PredefinedType(Token(SyntaxKind.StringKeyword))
                    )
            );
        }
        return args;
    }

    private IEnumerable<SyntaxNodeOrToken> constructorBaseArgs()
    {
        var args = new List<SyntaxNodeOrToken>();
        if (baseClassName == "ComplexField")
        {
            args.Add(Argument(IdentifierName("prefix")));
            args.Add(Token(SyntaxKind.CommaToken));
            args.Add(Argument(IdentifierName("name")));
        }
        else if (template is FormModel formTemplate)
        {
            args.Add
            (
                Argument
                (
                    LiteralExpression
                    (
                        SyntaxKind.StringLiteralExpression,
                        Literal(formTemplate.Name)
                    )
                )
            );
        }
        return args;
    }

    private IEnumerable<StatementSyntax> constructorBody()
    {
        var statements = new List<StatementSyntax>();
        foreach (var field in template.Fields)
        {
            statements.AddRange(initializeField(field));
        }
        return statements;
    }

    private IEnumerable<StatementSyntax> initializeField(FieldModel field)
    {
        var statements = new List<StatementSyntax>();
        statements.Add(addFieldStatement(field));
        return statements;
    }

    private ExpressionStatementSyntax addFieldStatement(FieldModel field)
    {
        return ExpressionStatement
        (
            AssignmentExpression
            (
                SyntaxKind.SimpleAssignmentExpression,
                IdentifierName(field.Name),
                InvocationExpression(IdentifierName(GetAddFieldName(field)))
                    .WithArgumentList
                    (
                        ArgumentList
                        (
                            SeparatedList<ArgumentSyntax>
                            (
                                GetAddFieldArguments(field)
                            )
                        )
                    )
                )
            );
    }

    private string GetAddFieldName(FieldModel field)
    {
        string methodName = "Add";
        if (field is SimpleFieldModel simpleField)
        {
            if (simpleField.InputDataType == typeof(DateTimeOffset?))
            {
                methodName += "Date";
            }
            else if (simpleField.InputDataType == typeof(int?))
            {
                methodName += "Int32";
            }
            else if (simpleField.InputDataType == typeof(decimal?))
            {
                methodName += "Decimal";
            }
            else if (simpleField.InputDataType == typeof(bool?))
            {
                methodName += "Boolean";
            }
            else if (simpleField.InputDataType == typeof(string))
            {
                methodName += "Text";
            }
            else
            {
                throw new NotSupportedException($"Simple field of type '{simpleField.InputDataType?.Name}' is not supported");
            }
            if (field is InputFieldModel)
            {
                methodName += "Input";
            }
            else if (field is DropDownFieldModel)
            {
                methodName += "DropDown";
            }
            else if (field is HiddenFieldModel)
            {
                methodName += "Hidden";
            }
            else
            {
                throw new NotSupportedException($"Simple field of type {field.GetType()} is not supported");
            }
        }
        else
        {
            methodName += "Complex";
        }
        return methodName;
    }

    private PropertyDeclarationSyntax fieldDeclarationStatement(FieldModel field)
    {
        var typeSyntax = typeSyntaxFromField(field);
        return PropertyDeclaration
        (
            typeSyntax,
            Identifier(field.Name))
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
                            AccessorDeclaration
                            (
                                SyntaxKind.GetAccessorDeclaration
                            )
                            .WithSemicolonToken
                            (
                                Token(SyntaxKind.SemicolonToken)
                            )
                        )
                    )
                );
    }

    private SyntaxNodeOrToken[] GetAddFieldArguments(FieldModel field)
    {
        var arguments = new List<SyntaxNodeOrToken>
        {
            Argument
            (
                InvocationExpression(IdentifierName("nameof"))
                    .WithArgumentList
                    (
                        ArgumentList
                        (
                            SingletonSeparatedList
                            (
                                Argument(IdentifierName(field.Name))
                            )
                        )
                    )
            )
        };
        if (field is ComplexFieldModel complexField)
        {
            arguments.AddRange
            (
                new SyntaxNodeOrToken[]
                {
                    Token(SyntaxKind.CommaToken),
                    Argument
                    (
                        ParenthesizedLambdaExpression()
                        .WithParameterList
                        (
                            ParameterList
                            (
                                SeparatedList<ParameterSyntax>
                                (
                                    new SyntaxNodeOrToken[]
                                    {
                                        Parameter(Identifier("p")),
                                        Token(SyntaxKind.CommaToken),
                                        Parameter(Identifier("n"))
                                    }
                                )
                            )
                        )
                        .WithExpressionBody
                        (
                            ObjectCreationExpression(IdentifierName(complexField.TypeName))
                            .WithArgumentList
                            (
                                ArgumentList
                                (
                                    SeparatedList<ArgumentSyntax>
                                    (
                                        new SyntaxNodeOrToken[]
                                        {
                                            Argument(IdentifierName("p")),
                                            Token(SyntaxKind.CommaToken),
                                            Argument(IdentifierName("n"))
                                        }
                                    )
                                )
                            )
                        )
                    )
                }
            );
        }
        return arguments.ToArray();
    }

    private TypeSyntax typeSyntaxFromField(FieldModel field)
    {
        TypeSyntax typeSyntax;
        if (field is ComplexFieldModel complexField)
        {
            typeSyntax = IdentifierName(Identifier(complexField.TypeName));
        }
        else if (field is SimpleFieldModel simpleField)
        {
            typeSyntax = typeSyntaxFromSimpleField(simpleField);
        }
        else
        {
            throw new NotSupportedException($"Field of type {field.GetType()} is not supported");
        }
        return typeSyntax;
    }

    private static GenericNameSyntax typeSyntaxFromSimpleField(SimpleFieldModel field)
    {
        TypeSyntax typeSyntax;
        if (field.InputDataType == typeof(DateTimeOffset?))
        {
            typeSyntax = NullableType(IdentifierName("DateTimeOffset"));
        }
        else
        {
            SyntaxKind syntaxKind;
            if (field.InputDataType == typeof(int?))
            {
                syntaxKind = SyntaxKind.IntKeyword;
            }
            else if (field.InputDataType == typeof(decimal?))
            {
                syntaxKind = SyntaxKind.DecimalKeyword;
            }
            else if (field.InputDataType == typeof(bool?))
            {
                syntaxKind = SyntaxKind.BoolKeyword;
            }
            else if (field.InputDataType == typeof(string))
            {
                syntaxKind = SyntaxKind.StringKeyword;
            }
            else
            {
                throw new NotSupportedException($"Simple field of type '{field.InputDataType?.Name}' is not supported");
            }
            typeSyntax = PredefinedType(Token(syntaxKind));
            if (field.InputDataType != typeof(string))
            {
                typeSyntax = NullableType(typeSyntax);
            }
        }
        string specificName;
        if (field is InputFieldModel)
        {
            specificName = "InputField";
        }
        else if (field is DropDownFieldModel)
        {
            specificName = "DropDownField";
        }
        else if (field is HiddenFieldModel)
        {
            specificName = "HiddenField";
        }
        else
        {
            throw new NotSupportedException($"Simple field of type {field.GetType()} is not supported");
        }
        return GenericName(Identifier(specificName))
            .WithTypeArgumentList
            (
                TypeArgumentList
                (
                    SingletonSeparatedList
                    (
                        typeSyntax
                    )
                )
            );
    }

    private Task outputClass(CompilationUnitSyntax compilationUnitSyntax, string className)
    {
        var cSharpFile = new CSharpFile(compilationUnitSyntax, createStream, className);
        return cSharpFile.Output();
    }
}