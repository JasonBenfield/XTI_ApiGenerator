using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using XTI_App.Api;
using XTI_WebApp.CodeGeneration;
using XTI_WebApp.CodeGeneration.CSharp;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace XTI_WebApp.ControllerGenerator
{
    public sealed class CsControllers : CodeGenerator
    {
        public CsControllers(string ns, IEnumerable<string> additionalNamespaces, Func<string, Stream> createStream)
        {
            this.ns = ns;
            this.additionalNamespaces = additionalNamespaces;
            this.createStream = createStream;
        }

        private readonly string ns;
        private readonly IEnumerable<string> additionalNamespaces;
        private readonly Func<string, Stream> createStream;

        public async Task Output(AppApiTemplate appTemplate)
        {
            foreach (var group in appTemplate.GroupTemplates)
            {
                var controller = createController(appTemplate, group);
                var controllerClassName = getControllerClassName(group);
                var cSharpFile = new CSharpFile(controller, createStream, controllerClassName);
                await cSharpFile.Output();
            }
        }

        private CompilationUnitSyntax createController(AppApiTemplate app, AppApiGroupTemplate group)
        {
            var nsToken = NamespaceDeclaration(ParseName(ns));
            var apiClassName = $"{app.Name}AppApi";
            var controllerClassName = getControllerClassName(group);
            var classToken = ClassDeclaration(controllerClassName)
                .AddModifiers(Token(SyntaxKind.PublicKeyword));
            var actionDeclarations = group.ActionTemplates.Select(a => actionDeclaration(group, a));
            var code = withUsings
            (
                CompilationUnit(),
                group
            )
            .WithMembers
            (
                SingletonList<MemberDeclarationSyntax>
                (
                    namespaceDeclaration()
                    .WithMembers
                    (
                        SingletonList<MemberDeclarationSyntax>
                        (
                            classDeclaration(controllerClassName, group.Access.IsAnonymousAllowed)
                            .WithMembers
                            (
                                List
                                (
                                    new MemberDeclarationSyntax[]
                                    {
                                        constructorDeclaration(apiClassName, getControllerClassName(group)),
                                        apiFieldDeclaration(apiClassName),
                                        xtiPathFieldDeclaration()
                                    }
                                    .Union(actionDeclarations)
                                )
                            )
                        )
                    )
                )
            );
            return code;
        }

        private static string getControllerClassName(AppApiGroupTemplate group)
        {
            return $"{group.Name}Controller";
        }

        private MethodDeclarationSyntax actionDeclaration(AppApiGroupTemplate group, AppApiActionTemplate action)
        {
            if (action.IsRedirect())
            {
                return actionDeclarationForRedirect(group, action);
            }
            if (action.IsView())
            {
                return actionDeclarationForView(group, action);
            }
            return actionDeclarationForPost(group, action);
        }

        private MethodDeclarationSyntax actionDeclarationForRedirect(AppApiGroupTemplate group, AppApiActionTemplate action)
        {
            var actionDeclaration = MethodDeclaration
            (
                SyntaxFactory
                    .GenericName(Identifier("Task"))
                    .WithTypeArgumentList
                    (
                        TypeArgumentList
                        (
                            SingletonSeparatedList<TypeSyntax>
                            (
                                IdentifierName("IActionResult")
                            )
                        )
                    ),
                    Identifier(action.Name)
            )
            .WithModifiers
            (
                TokenList
                (
                    new[]
                    {
                        Token(SyntaxKind.PublicKeyword),
                        Token(SyntaxKind.AsyncKeyword)
                    }
                )
            )
            .WithBody
            (
                Block
                (
                    LocalDeclarationStatement
                    (
                        VariableDeclaration
                        (
                            IdentifierName("var")
                        )
                        .WithVariables
                        (
                            SingletonSeparatedList
                            (
                                VariableDeclarator(Identifier("result"))
                                    .WithInitializer
                                    (
                                        EqualsValueClause
                                        (
                                            AwaitExpression
                                            (
                                                executeInvocationExpression(group, action)
                                            )
                                        )
                                    )
                            )
                        )
                    ),
                    ReturnStatement
                    (
                        InvocationExpression(IdentifierName("Redirect"))
                            .WithArgumentList
                            (
                                ArgumentList
                                (
                                    SingletonSeparatedList
                                    (
                                        Argument
                                        (
                                            MemberAccessExpression
                                            (
                                                SyntaxKind.SimpleMemberAccessExpression,
                                                MemberAccessExpression
                                                (
                                                    SyntaxKind.SimpleMemberAccessExpression,
                                                    IdentifierName("result"),
                                                    IdentifierName("Data")
                                                ),
                                                IdentifierName("Url")
                                            )
                                        )
                                    )
                                )
                            )
                    )
                )
            );
            if (!action.HasEmptyModel())
            {
                actionDeclaration = actionDeclaration
                    .WithParameterList
                    (
                        ParameterList
                        (
                            SingletonSeparatedList
                            (
                                Parameter(Identifier("model"))
                                    .WithType(typeSyntax(action.ModelTemplate))
                            )
                        )
                    );
            }
            return actionDeclaration;
        }

        private MethodDeclarationSyntax actionDeclarationForView(AppApiGroupTemplate group, AppApiActionTemplate action)
        {
            var actionDeclaration = MethodDeclaration
            (
                GenericName(Identifier("Task"))
                    .WithTypeArgumentList
                    (
                        TypeArgumentList
                        (
                            SingletonSeparatedList<TypeSyntax>
                            (
                                IdentifierName("IActionResult")
                            )
                        )
                    ),
                    Identifier(action.Name)
            )
            .WithModifiers
            (
                TokenList
                (
                    new[]
                    {
                        Token(SyntaxKind.PublicKeyword),
                        Token(SyntaxKind.AsyncKeyword)
                    }
                )
            )
            .WithBody
            (
                Block
                (
                    LocalDeclarationStatement
                    (
                        VariableDeclaration(IdentifierName("var"))
                            .WithVariables
                            (
                                SingletonSeparatedList
                                (
                                    VariableDeclarator(Identifier("result"))
                                        .WithInitializer
                                        (
                                            EqualsValueClause
                                            (
                                                AwaitExpression
                                                (
                                                    executeInvocationExpression(group, action)
                                                )
                                            )
                                        )
                                )
                            )
                    ),
                    ReturnStatement
                    (
                        InvocationExpression(IdentifierName("View"))
                            .WithArgumentList
                            (
                                ArgumentList
                                (
                                    SingletonSeparatedList
                                    (
                                        Argument
                                        (
                                            MemberAccessExpression
                                            (
                                                SyntaxKind.SimpleMemberAccessExpression,
                                                MemberAccessExpression
                                                (
                                                    SyntaxKind.SimpleMemberAccessExpression,
                                                    IdentifierName("result"),
                                                    IdentifierName("Data")
                                                ),
                                                IdentifierName("ViewName")
                                            )
                                        )
                                    )
                                )
                            )
                    )
                )
            );
            if (!action.HasEmptyModel())
            {
                actionDeclaration = actionDeclaration
                    .WithParameterList
                    (
                        ParameterList
                        (
                            SingletonSeparatedList
                            (
                                Parameter(Identifier("model"))
                                    .WithType(typeSyntax(action.ModelTemplate))
                            )
                        )
                    );
            }
            return actionDeclaration;
        }

        private MethodDeclarationSyntax actionDeclarationForPost(AppApiGroupTemplate group, AppApiActionTemplate action)
        {
            var actionDeclaration =
                MethodDeclaration
                (
                    GenericName(Identifier("Task")
                )
                .WithTypeArgumentList
                (
                    TypeArgumentList
                    (
                        SingletonSeparatedList<TypeSyntax>
                        (
                            GenericName(Identifier("ResultContainer"))
                                .WithTypeArgumentList
                                (
                                    TypeArgumentList
                                    (
                                        SingletonSeparatedList
                                        (
                                            typeSyntax(action.ResultTemplate)
                                        )
                                    )
                                )
                        )
                    )
                ),
                Identifier(action.Name)
            )
            .WithAttributeLists
            (
                SingletonList
                (
                    AttributeList
                    (
                        SingletonSeparatedList
                        (
                            Attribute(IdentifierName("HttpPost"))
                        )
                    )
                )
            )
            .WithModifiers
            (
                TokenList(Token(SyntaxKind.PublicKeyword))
            );
            if (!action.HasEmptyModel())
            {
                actionDeclaration = actionDeclaration
                    .WithParameterList
                    (
                        ParameterList
                        (
                            SingletonSeparatedList
                            (
                                Parameter(Identifier("model"))
                                    .WithType(typeSyntax(action.ModelTemplate))
                                    .WithAttributeLists
                                    (
                                        SingletonList
                                        (
                                            AttributeList
                                            (
                                                SingletonSeparatedList
                                                (
                                                    Attribute(IdentifierName("FromBody"))
                                                )
                                            )
                                        )
                                    )
                            )
                        )
                    );
            }
            return actionDeclaration
                .WithBody
                (
                    Block
                    (
                        SingletonList<StatementSyntax>
                        (
                            ReturnStatement
                            (
                                executeInvocationExpression(group, action)
                            )
                        )
                    )
                );
        }

        private InvocationExpressionSyntax executeInvocationExpression(AppApiGroupTemplate group, AppApiActionTemplate action)
        {
            return InvocationExpression
            (
                MemberAccessExpression
                (
                    SyntaxKind.SimpleMemberAccessExpression,
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
                                    IdentifierName("api"),
                                    IdentifierName("Group")
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
                                            LiteralExpression
                                            (
                                                SyntaxKind.StringLiteralExpression,
                                                Literal(group.Name)
                                            )
                                        )
                                    )
                                )
                            ),
                            GenericName(Identifier("Action"))
                                .WithTypeArgumentList
                                (
                                    TypeArgumentList
                                    (
                                        SeparatedList<TypeSyntax>
                                        (
                                            new SyntaxNodeOrToken[]
                                            {
                                                typeSyntax(action.ModelTemplate),
                                                Token(SyntaxKind.CommaToken),
                                                typeSyntax(action.ResultTemplate)
                                            }
                                        )
                                    )
                                )
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
                                    LiteralExpression
                                    (
                                        SyntaxKind.StringLiteralExpression,
                                        Literal(action.Name)
                                    )
                                )
                            )
                        )
                    ),
                    IdentifierName("Execute")
                )
            )
            .WithArgumentList
            (
                ArgumentList
                (
                    SeparatedList
                    (
                        new ArgumentSyntax[]
                        {
                            Argument
                            (
                                MemberAccessExpression
                                (
                                    SyntaxKind.SimpleMemberAccessExpression,
                                    IdentifierName("xtiPath"),
                                    IdentifierName("Modifier")
                                )
                            ),
                            Argument
                            (
                                action.HasEmptyModel()
                                    ? (ExpressionSyntax)newEmptyRequest()
                                    : IdentifierName("model")
                            )
                        }
                    )
                )
            );
        }

        private static ObjectCreationExpressionSyntax newEmptyRequest()
        {
            return ObjectCreationExpression(IdentifierName("EmptyRequest"))
                .WithArgumentList(ArgumentList());
        }

        private static FieldDeclarationSyntax apiFieldDeclaration(string apiClassName)
        {
            return
                FieldDeclaration
                (
                    VariableDeclaration(IdentifierName(apiClassName))
                        .WithVariables
                        (
                            SingletonSeparatedList
                            (
                                VariableDeclarator(Identifier("api"))
                            )
                        )
                )
                .WithModifiers
                (
                    TokenList
                    (
                        Token(SyntaxKind.PrivateKeyword),
                        Token(SyntaxKind.ReadOnlyKeyword)
                    )
                );
        }

        private static FieldDeclarationSyntax xtiPathFieldDeclaration()
        {
            return
                FieldDeclaration
                (
                    VariableDeclaration(IdentifierName("XtiPath"))
                        .WithVariables
                        (
                            SingletonSeparatedList
                            (
                                VariableDeclarator(Identifier("xtiPath"))
                            )
                        )
                )
                .WithModifiers
                (
                    TokenList
                    (
                        Token(SyntaxKind.PrivateKeyword),
                        Token(SyntaxKind.ReadOnlyKeyword)
                    )
                );
        }

        private static ConstructorDeclarationSyntax constructorDeclaration(string apiClassName, string groupClassName)
        {
            return
                ConstructorDeclaration(Identifier(groupClassName))
                .WithModifiers
                (
                    TokenList(Token(SyntaxKind.PublicKeyword))
                )
                .WithParameterList
                (
                    ParameterList
                    (
                        SeparatedList
                        (
                            new ParameterSyntax[]
                            {
                                Parameter(Identifier("api"))
                                    .WithType(IdentifierName(apiClassName)),
                                Parameter(Identifier("xtiPath"))
                                    .WithType(IdentifierName("XtiPath"))
                            }
                        )
                    )
                )
                .WithBody
                (
                    Block
                    (
                        SeparatedList
                        (
                            new StatementSyntax[]
                            {
                                ExpressionStatement
                                (
                                    AssignmentExpression
                                    (
                                        SyntaxKind.SimpleAssignmentExpression,
                                        MemberAccessExpression
                                        (
                                            SyntaxKind.SimpleMemberAccessExpression,
                                            ThisExpression(),
                                            IdentifierName("api")
                                        ),
                                        IdentifierName("api")
                                    )
                                ),
                                ExpressionStatement
                                (
                                    AssignmentExpression
                                    (
                                        SyntaxKind.SimpleAssignmentExpression,
                                        MemberAccessExpression
                                        (
                                            SyntaxKind.SimpleMemberAccessExpression,
                                            ThisExpression(),
                                            IdentifierName("xtiPath")
                                        ),
                                        IdentifierName("xtiPath")
                                    )
                                )
                            }
                        )
                    )
                );
        }

        private static ClassDeclarationSyntax classDeclaration(string controllerName, bool isAnonymousAllowed)
        {
            return
                ClassDeclaration(controllerName)
                .WithAttributeLists
                (
                    SingletonList
                    (
                        AttributeList
                        (
                            SingletonSeparatedList
                            (
                                Attribute
                                (
                                    IdentifierName
                                    (
                                        isAnonymousAllowed ? "AllowAnonymous" : "Authorize"
                                    )
                                )
                            )
                        )
                    )
                )
                .WithModifiers
                (
                    TokenList(Token(SyntaxKind.PublicKeyword))
                )
                .WithBaseList
                (
                    BaseList
                    (
                        SingletonSeparatedList<BaseTypeSyntax>
                        (
                            SimpleBaseType(IdentifierName("Controller"))
                        )
                    )
                );
        }

        private NamespaceDeclarationSyntax namespaceDeclaration()
        {
            return NamespaceDeclaration(IdentifierName(ns));
        }

        private CompilationUnitSyntax withUsings(CompilationUnitSyntax compilationUnit, AppApiGroupTemplate group)
        {
            var code = compilationUnit
                .WithUsings
                (
                    List
                    (
                        new UsingDirectiveSyntax[]
                        {
                            UsingDirective
                            (
                                QualifiedName
                                (
                                    QualifiedName
                                    (
                                        IdentifierName("Microsoft"),
                                        IdentifierName("AspNetCore")
                                    ),
                                    IdentifierName("Authorization")
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
                            ),
                            UsingDirective
                            (
                                QualifiedName
                                (
                                    QualifiedName
                                    (
                                        IdentifierName("Microsoft"),
                                        IdentifierName("AspNetCore")
                                    ),
                                    IdentifierName("Mvc")
                                )
                            ),
                            UsingDirective
                            (
                                QualifiedName
                                (
                                    QualifiedName
                                    (
                                        IdentifierName("System"),
                                        IdentifierName("Threading")
                                    ),
                                    IdentifierName("Tasks")
                                )
                            )
                        }
                    )
                );
            var namespaces = group.ObjectTemplates()
                .Select(ot => ot.DataType.Namespace)
                .Union(additionalNamespaces ?? new string[] { })
                .Union(new[] { "XTI_App", "XTI_App.Api" })
                .Distinct();
            foreach (var ns in namespaces)
            {
                code = code.AddUsings(UsingDirective(IdentifierName(ns)));
            }
            return code;
        }

        private TypeSyntax typeSyntax(ValueTemplate valueTemplate)
        {
            return new TypeSyntaxFromValueTemplate(valueTemplate).Value();
        }
    }
}
