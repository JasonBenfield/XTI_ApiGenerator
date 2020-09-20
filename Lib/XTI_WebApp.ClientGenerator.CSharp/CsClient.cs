using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using XTI_WebApp.Api;
using XTI_WebApp.CodeGeneration;
using XTI_WebApp.CodeGeneration.CSharp;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace XTI_WebApp.ClientGenerator.CSharp
{
    public sealed class CsClient : CodeGenerator
    {
        public CsClient(string ns, Func<string, Stream> createStream)
        {
            this.ns = ns;
            this.createStream = createStream;
        }

        private readonly string ns;
        private readonly Func<string, Stream> createStream;

        private static readonly string[] ModelsToOmit = new[]
        {
            "LoginModel",
            "LoginResult",
            "EmptyRequest"
        };

        public async Task Output(AppApiTemplate appTemplate)
        {
            var objTemplates = appTemplate.ObjectTemplates()
                .Where
                (
                    obj =>
                        !ModelsToOmit.Any
                        (
                            m => obj.DataType.Name.Equals(m, StringComparison.OrdinalIgnoreCase)
                        )
                );
            foreach (var objTemplate in objTemplates)
            {
                await outputObjTemplate(objTemplate);
            }
            foreach (var groupTemplate in appTemplate.GroupTemplates)
            {
                await outputGroup(groupTemplate);
            }
            await outputApp(appTemplate);
        }

        private async Task outputObjTemplate(ObjectValueTemplate objTemplate)
        {
            var apiObject = createApiObject(objTemplate);
            var className = objTemplate.DataType.Name;
            await outputClass(apiObject, className);
        }

        private async Task outputGroup(AppApiGroupTemplate group)
        {
            var groupClient = createGroupClient(group);
            var className = getGroupClassName(group);
            await outputClass(groupClient, className);
        }

        private async Task outputApp(AppApiTemplate app)
        {
            var appClient = createAppClient(app);
            var className = getAppClassName(app);
            await outputClass(appClient, className);
        }

        private async Task outputClass(CompilationUnitSyntax compilationUnitSyntax, string className)
        {
            var cSharpFile = new CSharpFile(compilationUnitSyntax, createStream, className);
            await cSharpFile.Output();
        }

        private CompilationUnitSyntax createApiObject(ObjectValueTemplate obj)
        {
            return CompilationUnit()
                .WithUsings
                (
                    SingletonList
                    (
                        UsingDirective(IdentifierName("System"))
                            .WithUsingKeyword
                            (
                                Token
                                (
                                    TriviaList(Comment("// Generated Code")),
                                    SyntaxKind.UsingKeyword,
                                    TriviaList()
                                )
                            )
                    )
                )
                .WithMembers
                (
                    SingletonList<MemberDeclarationSyntax>
                    (
                        NamespaceDeclaration(IdentifierName(ns))
                            .WithMembers
                            (
                                SingletonList<MemberDeclarationSyntax>
                                (
                                    ClassDeclaration(obj.DataType.Name)
                                        .WithModifiers
                                        (
                                            TokenList
                                            (
                                                new[]
                                                {
                                                    Token(SyntaxKind.PublicKeyword),
                                                    Token(SyntaxKind.SealedKeyword)
                                                }
                                            )
                                        )
                                        .WithMembers
                                        (
                                            List
                                            (
                                                getProperties(obj.PropertyTemplates)
                                            )
                                        )
                                    )
                                )
                            )
                    );
        }

        private static IEnumerable<MemberDeclarationSyntax> getProperties(IEnumerable<ObjectPropertyTemplate> properties)
        {
            return properties.Select
            (
                property =>
                    PropertyDeclaration
                    (
                        new TypeSyntaxFromValueTemplate(property.ValueTemplate).Value(),
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
                    )
            );
        }

        private static string getGroupClassName(AppApiGroupTemplate group) => $"{group.Name}Group";

        private CompilationUnitSyntax createGroupClient(AppApiGroupTemplate group)
        {
            return CompilationUnit()
                .WithUsings
                (
                    groupUsings()
                )
                .WithMembers
                (
                    SingletonList
                    (
                        (MemberDeclarationSyntax)NamespaceDeclaration(IdentifierName(ns))
                            .WithMembers
                            (
                                List
                                (
                                    groupClass(group)
                                )
                            )
                    )
                );
        }

        private static MemberDeclarationSyntax[] groupClass(AppApiGroupTemplate group)
        {
            var groupClass = new List<MemberDeclarationSyntax>();
            groupClass.Add
            (
                ClassDeclaration(getGroupClassName(group))
                    .WithModifiers
                    (
                        TokenList
                        (
                            new[]
                            {
                                Token(SyntaxKind.PublicKeyword),
                                Token(SyntaxKind.SealedKeyword)
                            }
                        )
                    )
                    .WithBaseList
                    (
                        BaseList
                        (
                            SeparatedList
                            (
                                groupBaseList(group)
                            )
                        )
                    )
                    .WithMembers
                    (
                        List
                        (
                            groupMembers(group)
                        )
                    )
            );
            return groupClass.ToArray();
        }

        private static MemberDeclarationSyntax[] groupMembers(AppApiGroupTemplate group)
        {
            var members = new List<MemberDeclarationSyntax>();
            members.Add(groupCtor(group));
            foreach (var action in group.ActionTemplates.Where(a => !a.IsRedirect() && !a.IsView()))
            {
                members.Add
                (
                    MethodDeclaration
                    (
                        GenericName(Identifier("Task"))
                            .WithTypeArgumentList
                            (
                                TypeArgumentList
                                (
                                    SingletonSeparatedList
                                    (
                                        new TypeSyntaxFromValueTemplate(action.ResultTemplate).Value()
                                    )
                                )
                            ),
                            Identifier(action.Name)
                    )
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
                                actionArgs(action)
                            )
                        )
                    )
                    .WithExpressionBody
                    (
                        ArrowExpressionClause
                        (
                            InvocationExpression
                            (
                                GenericName(Identifier("Post"))
                                .WithTypeArgumentList
                                (
                                    TypeArgumentList
                                    (
                                        SeparatedList<TypeSyntax>
                                        (
                                            new SyntaxNodeOrToken[]
                                            {
                                                new TypeSyntaxFromValueTemplate(action.ResultTemplate).Value(),
                                                Token(SyntaxKind.CommaToken),
                                                new TypeSyntaxFromValueTemplate(action.ModelTemplate).Value()
                                            }
                                        )
                                    )
                                )
                            )
                            .WithArgumentList
                            (
                                ArgumentList
                                (
                                    SeparatedList<ArgumentSyntax>
                                    (
                                        postArgs(action)
                                    )
                                )
                            )
                        )
                    )
                    .WithSemicolonToken(Token(SyntaxKind.SemicolonToken))
                );
            }
            return members.ToArray();
        }

        private static SyntaxNodeOrToken[] postArgs(AppApiActionTemplate action)
        {
            var args = new List<SyntaxNodeOrToken>();
            args.AddRange
            (
                new SyntaxNodeOrToken[]
                {
                    Argument
                    (
                        LiteralExpression
                        (
                            SyntaxKind.StringLiteralExpression,
                            Literal(action.Name)
                        )
                    ),
                    Token(SyntaxKind.CommaToken)
                }
            );
            if (action.HasEmptyModel())
            {
                args.Add
                (
                    Argument
                    (
                        ObjectCreationExpression(IdentifierName("EmptyRequest"))
                            .WithArgumentList(ArgumentList())
                    )
                );
            }
            else
            {
                args.Add(Argument(IdentifierName("model")));
            }
            return args.ToArray();
        }

        private static ParameterSyntax[] actionArgs(AppApiActionTemplate action)
        {
            var parameters = new List<ParameterSyntax>();
            if (!action.HasEmptyModel())
            {
                parameters.Add
                (
                    Parameter(Identifier("model"))
                        .WithType
                        (
                            new TypeSyntaxFromValueTemplate(action.ModelTemplate).Value()
                        )
                );
            }
            return parameters.ToArray();
        }

        private static MemberDeclarationSyntax groupCtor(AppApiGroupTemplate group)
        {
            return ConstructorDeclaration(Identifier(getGroupClassName(group)))
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
                            new SyntaxNodeOrToken[]
                            {
                                Parameter(Identifier("httpClientFactory"))
                                    .WithType(IdentifierName("IHttpClientFactory")),
                                Token(SyntaxKind.CommaToken),
                                Parameter(Identifier("xtiToken"))
                                    .WithType(IdentifierName("XtiToken")),
                                Token(SyntaxKind.CommaToken),
                                Parameter(Identifier("baseUrl"))
                                    .WithType(PredefinedType(Token(SyntaxKind.StringKeyword)))
                            }
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
                                new SyntaxNodeOrToken[]
                                {
                                    Argument(IdentifierName("httpClientFactory")),
                                    Token(SyntaxKind.CommaToken),
                                    Argument(IdentifierName("xtiToken")),
                                    Token(SyntaxKind.CommaToken),
                                    Argument(IdentifierName("baseUrl")),
                                    Token(SyntaxKind.CommaToken),
                                    Argument
                                    (
                                        LiteralExpression
                                        (
                                            SyntaxKind.StringLiteralExpression,
                                            Literal(group.Name)
                                        )
                                    )
                                }
                            )
                        )
                    )
                )
                .WithBody
                (
                    Block()
                );
        }

        private static BaseTypeSyntax[] groupBaseList(AppApiGroupTemplate group)
        {
            var baseTypes = new List<BaseTypeSyntax>();
            baseTypes.Add(SimpleBaseType(IdentifierName("AppClientGroup")));
            if (group.Name.Equals("Auth", StringComparison.OrdinalIgnoreCase))
            {
                baseTypes.Add(SimpleBaseType(IdentifierName("IAuthClientGroup")));
            }
            return baseTypes.ToArray();
        }

        private static SyntaxList<UsingDirectiveSyntax> groupUsings()
        {
            return List
            (
                new UsingDirectiveSyntax[]
                {
                    UsingDirective(IdentifierName("XTI_WebAppClient"))
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
                                    IdentifierName("System"),
                                    IdentifierName("Net")
                                ),
                                IdentifierName("Http")
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
            );
        }

        private static SyntaxList<UsingDirectiveSyntax> appUsings()
        {
            return List
            (
                new UsingDirectiveSyntax[]
                {
                    UsingDirective(IdentifierName("XTI_WebAppClient"))
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
                                IdentifierName("System"),
                                IdentifierName("Net")
                            ),
                            IdentifierName("Http")
                        )
                    )
                }
            );
        }

        private string getAppClassName(AppApiTemplate app) => $"{app.Name}AppClient";

        private CompilationUnitSyntax createAppClient(AppApiTemplate app)
        {
            return CompilationUnit()
                .WithUsings
                (
                    appUsings()
                )
                .WithMembers
                (
                    SingletonList
                    (
                        (MemberDeclarationSyntax)NamespaceDeclaration(IdentifierName(ns))
                            .WithMembers
                            (
                                SingletonList
                                (
                                    (MemberDeclarationSyntax)ClassDeclaration(getAppClassName(app))
                                        .WithModifiers
                                        (
                                            TokenList
                                            (
                                                new[]
                                                {
                                                    Token(SyntaxKind.PublicKeyword),
                                                    Token(SyntaxKind.SealedKeyword)
                                                }
                                            )
                                        )
                                        .WithBaseList
                                        (
                                            BaseList
                                            (
                                                SeparatedList
                                                (
                                                    appBaseList(app)
                                                )
                                            )
                                        )
                                        .WithMembers
                                        (
                                            List
                                            (
                                                appMembers(app)
                                            )
                                        )
                                    )
                                )
                            )
                    );
        }

        private MemberDeclarationSyntax[] appMembers(AppApiTemplate app)
        {
            var members = new List<MemberDeclarationSyntax>();
            members.Add(appCtor(app));
            foreach (var group in app.GroupTemplates)
            {
                members.Add
                (
                    PropertyDeclaration
                    (
                        IdentifierName
                        (
                            group.Name.Equals("Auth", StringComparison.OrdinalIgnoreCase)
                                ? $"IAuthClientGroup"
                                : $"{group.Name}Group"
                        ),
                        Identifier(group.Name)
                    )
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
                                AccessorDeclaration(SyntaxKind.GetAccessorDeclaration)
                                    .WithSemicolonToken(Token(SyntaxKind.SemicolonToken))
                            )
                        )
                    )
                );
            }
            return members.ToArray();
        }

        private ConstructorDeclarationSyntax appCtor(AppApiTemplate app)
        {
            return ConstructorDeclaration(Identifier(getAppClassName(app)))
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
                            appCtorArgs(app)
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
                                appBaseCtorArgs(app)
                            )
                        )
                    )
                )
                .WithBody
                (
                    Block
                    (
                        List
                        (
                            appCtorBodyStatements(app)
                        )
                    )
                );
        }

        private static StatementSyntax[] appCtorBodyStatements(AppApiTemplate app)
        {
            var statements = new List<StatementSyntax>();
            if (app.IsHub())
            {
                statements.Add
                (
                    ExpressionStatement
                    (
                        AssignmentExpression
                        (
                            SyntaxKind.SimpleAssignmentExpression,
                            IdentifierName("xtiToken"),
                            ObjectCreationExpression(IdentifierName("XtiToken"))
                                .WithArgumentList
                                (
                                    ArgumentList
                                    (
                                        SeparatedList<ArgumentSyntax>
                                        (
                                            new SyntaxNodeOrToken[]
                                            {
                                                Argument(ThisExpression()),
                                                Token(SyntaxKind.CommaToken),
                                                Argument(IdentifierName("credentials"))
                                            }
                                        )
                                    )
                                )
                        )
                    )
                );
            }
            else
            {
                statements.Add
                (
                    ExpressionStatement
                    (
                        AssignmentExpression
                        (
                            SyntaxKind.SimpleAssignmentExpression,
                            MemberAccessExpression
                            (
                                SyntaxKind.SimpleMemberAccessExpression,
                                ThisExpression(),
                                IdentifierName("xtiToken")
                            ),
                            IdentifierName("xtiToken")
                        )
                    )
                );
            }
            foreach (var group in app.GroupTemplates)
            {
                statements.Add
                (
                    ExpressionStatement
                    (
                        AssignmentExpression
                        (
                            SyntaxKind.SimpleAssignmentExpression,
                            IdentifierName(group.Name),
                            ObjectCreationExpression(IdentifierName(getGroupClassName(group)))
                                .WithArgumentList
                                (
                                    ArgumentList
                                    (
                                        SeparatedList<ArgumentSyntax>
                                        (
                                            new SyntaxNodeOrToken[]
                                            {
                                                Argument(IdentifierName("httpClientFactory")),
                                                Token(SyntaxKind.CommaToken),
                                                Argument(IdentifierName("xtiToken")),
                                                Token(SyntaxKind.CommaToken),
                                                Argument(IdentifierName("url"))
                                            }
                                        )
                                    )
                                )
                        )
                    )
                );
            }
            return statements.ToArray();
        }

        private static BaseTypeSyntax[] appBaseList(AppApiTemplate app)
        {
            var baseTypes = new List<BaseTypeSyntax>();
            baseTypes.Add(SimpleBaseType(IdentifierName("AppClient")));
            if (app.IsHub())
            {
                baseTypes.Add(SimpleBaseType(IdentifierName("IAuthClient")));
            }
            return baseTypes.ToArray();
        }

        private static SyntaxNodeOrToken[] appCtorArgs(AppApiTemplate app)
        {
            var args = new List<SyntaxNodeOrToken>();
            args.AddRange
            (
                new SyntaxNodeOrToken[]
                {
                    Parameter(Identifier("httpClientFactory"))
                        .WithType(IdentifierName("IHttpClientFactory")),
                    Token(SyntaxKind.CommaToken),
                }
            );
            if (app.IsHub())
            {
                args.AddRange
                (
                    new SyntaxNodeOrToken[]
                    {
                        Parameter(Identifier("credentials"))
                            .WithType(IdentifierName("XtiCredentials")),
                        Token(SyntaxKind.CommaToken)
                    }
                );
            }
            else
            {
                args.AddRange
                (
                    new SyntaxNodeOrToken[]
                    {
                        Parameter(Identifier("xtiToken"))
                            .WithType(IdentifierName("XtiToken")),
                        Token(SyntaxKind.CommaToken)
                    }
                );
            }
            args.AddRange
            (
                new SyntaxNodeOrToken[]
                {
                    Parameter(Identifier("baseUrl"))
                        .WithType(PredefinedType(Token(SyntaxKind.StringKeyword)))
                }
            );
            return args.ToArray();
        }

        private SyntaxNodeOrToken[] appBaseCtorArgs(AppApiTemplate app)
        {
            var args = new List<SyntaxNodeOrToken>();
            args.AddRange
            (
                new SyntaxNodeOrToken[]
                {
                    Argument(IdentifierName("httpClientFactory")),
                    Token(SyntaxKind.CommaToken),
                    Argument(IdentifierName("baseUrl")),
                    Token(SyntaxKind.CommaToken),
                    Argument
                    (
                        LiteralExpression
                        (
                            SyntaxKind.StringLiteralExpression,
                            Literal(app.Name)
                        )
                    )
                }
            );
            return args.ToArray();
        }

    }
}
