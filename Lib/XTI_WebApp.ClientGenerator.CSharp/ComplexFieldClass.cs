using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
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

    private IEnumerable<UsingDirectiveSyntax> usings()
    {
        return new[]
        {
                UsingDirective
                (
                    QualifiedName
                    (
                        IdentifierName("XTI_WebAppClient"),
                        IdentifierName("Forms")
                    )
                )
                .WithUsingKeyword
                (
                    Token
                    (
                        TriviaList(Comment("// Generated code")),
                        SyntaxKind.UsingKeyword,
                        TriviaList()
                    )
                ),
                UsingDirective(IdentifierName("System"))
            };
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

    private MemberDeclarationSyntax constructor()
    {
        return ConstructorDeclaration(Identifier(template.TypeName))
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
    }

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
        }
        args.Add
        (
            Parameter(Identifier("name"))
                .WithType
                (
                    PredefinedType(Token(SyntaxKind.StringKeyword))
                )
        );
        return args;
    }

    private IEnumerable<SyntaxNodeOrToken> constructorBaseArgs()
    {
        var args = new List<SyntaxNodeOrToken>();
        if (baseClassName == "ComplexField")
        {
            args.Add(Argument(IdentifierName("prefix")));
            args.Add(Token(SyntaxKind.CommaToken));
        }
        args.Add(Argument(IdentifierName("name")));
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
                InvocationExpression(IdentifierName("AddField"))
                    .WithArgumentList
                    (
                        ArgumentList
                        (
                            SingletonSeparatedList
                            (
                                Argument
                                (
                                    addFieldArgument(field)
                                )
                            )
                        )
                    )
                )
            );
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

    private ExpressionSyntax addFieldArgument(FieldModel field)
    {
        var expression = ObjectCreationExpression
        (
            typeSyntaxFromField(field)
        );
        expression = expression.WithArgumentList
        (
            ArgumentList
            (
                SeparatedList<ArgumentSyntax>
                (
                    new SyntaxNodeOrToken[]
                    {
                            Argument(IdentifierName("FieldName")),
                            Token(SyntaxKind.CommaToken),
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
                    }
                )
            )
        );
        return expression;
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