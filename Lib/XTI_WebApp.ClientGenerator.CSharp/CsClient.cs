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

namespace XTI_WebApp.ClientGenerator.CSharp
{
    public sealed class CsClient : CodeGenerator
    {
        private readonly DefaultVersion defaultVersion;
        private readonly string ns;
        private readonly Func<string, Stream> createStream;

        public CsClient
        (
            DefaultVersion defaultVersion,
            string ns,
            Func<string, Stream> createStream
        )
        {
            this.defaultVersion = defaultVersion;
            this.ns = ns;
            this.createStream = createStream;
        }

        private static readonly string[] ModelsToOmit = new[]
        {
            "LoginCredentials",
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

        private async Task outputGroup(AppApiGroupTemplate groupTemplate)
        {
            var groupClient = createGroupClient(groupTemplate);
            var className = getGroupClassName(groupTemplate);
            await outputClass(groupClient, className);
        }

        private async Task outputApp(AppApiTemplate appTemplate)
        {
            var appClient = await createAppClient(appTemplate);
            var className = getAppClassName(appTemplate);
            await outputClass(appClient, className);
        }

        private async Task outputClass(CompilationUnitSyntax compilationUnitSyntax, string className)
        {
            var cSharpFile = new CSharpFile(compilationUnitSyntax, createStream, className);
            await cSharpFile.Output();
        }

        private CompilationUnitSyntax createApiObject(ObjectValueTemplate objTemplate)
        {
            return CompilationUnit()
                .WithUsings
                (
                    List
                    (
                        new[]
                        {
                            UsingDirective(IdentifierName("System"))
                                .WithUsingKeyword
                                (
                                    Token
                                    (
                                        TriviaList(Comment("// Generated Code")),
                                        SyntaxKind.UsingKeyword,
                                        TriviaList()
                                    )
                                ),
                            UsingDirective(IdentifierName("System.Collections.Generic"))
                        }
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
                                    ClassDeclaration(objTemplate.DataType.Name)
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
                                                getProperties(objTemplate.PropertyTemplates)
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

        private static string getGroupClassName(AppApiGroupTemplate groupTemplate) => $"{groupTemplate.Name}Group";

        private CompilationUnitSyntax createGroupClient(AppApiGroupTemplate groupTemplate)
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
                                    groupClass(groupTemplate)
                                )
                            )
                    )
                );
        }

        private static MemberDeclarationSyntax[] groupClass(AppApiGroupTemplate groupTemplate)
        {
            var groupClass = new List<MemberDeclarationSyntax>();
            groupClass.Add
            (
                ClassDeclaration(getGroupClassName(groupTemplate))
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
                            SeparatedList
                            (
                                groupBaseList(groupTemplate)
                            )
                        )
                    )
                    .WithMembers
                    (
                        List
                        (
                            groupMembers(groupTemplate)
                        )
                    )
            );
            return groupClass.ToArray();
        }

        private static MemberDeclarationSyntax[] groupMembers(AppApiGroupTemplate groupTemplate)
        {
            var members = new List<MemberDeclarationSyntax>();
            members.Add(groupCtor(groupTemplate));
            foreach (var action in groupTemplate.ActionTemplates.Where(a => !a.IsRedirect() && !a.IsView()))
            {
                members.AddRange
                (
                    new[]
                    {
                        actionDeclaration(groupTemplate, action)
                    }
                );
            }
            return members.ToArray();
        }

        private static MethodDeclarationSyntax actionDeclaration(AppApiGroupTemplate groupTemplate, AppApiActionTemplate action)
        {
            return MethodDeclaration
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
                        actionArgs(action, groupTemplate.HasModifier)
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
                                postArgs(action, groupTemplate.HasModifier)
                            )
                        )
                    )
                )
            )
            .WithSemicolonToken(Token(SyntaxKind.SemicolonToken));
        }

        private static SyntaxNodeOrToken[] postArgs(AppApiActionTemplate actionTemplate, bool includeModifier)
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
                                Literal(actionTemplate.Name)
                            )
                        ),
                        Token(SyntaxKind.CommaToken)
                }
            );
            if (includeModifier)
            {
                args.AddRange
                (
                    new SyntaxNodeOrToken[]
                    {
                        Argument(IdentifierName("modifier")),
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
                        Argument
                        (
                            LiteralExpression
                            (
                                SyntaxKind.StringLiteralExpression,
                                Literal("")
                            )
                        ),
                        Token(SyntaxKind.CommaToken)
                    }
                );
            }
            if (actionTemplate.HasEmptyModel())
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

        private static ParameterSyntax[] actionArgs(AppApiActionTemplate actionTemplate, bool includeModifier)
        {
            var parameters = new List<ParameterSyntax>();
            if (includeModifier)
            {
                parameters.Add
                (
                    Parameter(Identifier("modifier"))
                        .WithType(PredefinedType(Token(SyntaxKind.StringKeyword)))
                );
            }
            if (!actionTemplate.HasEmptyModel())
            {
                parameters.Add
                (
                    Parameter(Identifier("model"))
                        .WithType
                        (
                            new TypeSyntaxFromValueTemplate(actionTemplate.ModelTemplate).Value()
                        )
                );
            }
            return parameters.ToArray();
        }

        private static MemberDeclarationSyntax groupCtor(AppApiGroupTemplate groupTemplate)
        {
            return ConstructorDeclaration(Identifier(getGroupClassName(groupTemplate)))
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
                                    .WithType(IdentifierName("IXtiToken")),
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
                                            Literal(groupTemplate.Name)
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

        private static SyntaxList<UsingDirectiveSyntax> appUsings(AppApiTemplate appTemplate)
        {
            var usings = new List<UsingDirectiveSyntax>();
            usings.AddRange
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
            if (appTemplate.IsAuthenticator())
            {
                usings.Add
                (
                    UsingDirective
                    (
                        IdentifierName("XTI_Credentials")
                    )
                );
            }
            return List(usings);
        }

        private string getAppClassName(AppApiTemplate appTemplate) => $"{appTemplate.Name}AppClient";

        private async Task<CompilationUnitSyntax> createAppClient(AppApiTemplate appTemplate)
        {
            return CompilationUnit()
                .WithUsings
                (
                    appUsings(appTemplate)
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
                                    (MemberDeclarationSyntax)ClassDeclaration(getAppClassName(appTemplate))
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
                                                SeparatedList
                                                (
                                                    appBaseList()
                                                )
                                            )
                                        )
                                        .WithMembers
                                        (
                                            List
                                            (
                                                await appMembers(appTemplate)
                                            )
                                        )
                                    )
                                )
                            )
                    );
        }

        private async Task<MemberDeclarationSyntax[]> appMembers(AppApiTemplate appTemplate)
        {
            var members = new List<MemberDeclarationSyntax>();
            members.Add(appCtor(appTemplate));
            members.Add(await defaultVersionDeclaration(appTemplate));
            foreach (var group in appTemplate.GroupTemplates)
            {
                members.Add
                (
                    PropertyDeclaration
                    (
                        IdentifierName
                        (
                            $"{group.Name}Group"
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

        private async Task<MemberDeclarationSyntax> defaultVersionDeclaration(AppApiTemplate appTemplate)
        {
            var versionKey = await defaultVersion.Value(appTemplate.AppKey);
            return FieldDeclaration
            (
                VariableDeclaration
                (
                    PredefinedType(Token(SyntaxKind.StringKeyword))
                )
                .WithVariables
                (
                    SingletonSeparatedList
                    (
                        VariableDeclarator(Identifier("DefaultVersion"))
                            .WithInitializer
                            (
                                EqualsValueClause
                                (
                                    LiteralExpression
                                    (
                                        SyntaxKind.StringLiteralExpression,
                                        Literal(versionKey.Value)
                                    )
                                )
                            )
                    )
                )
            )
            .WithModifiers
            (
                TokenList
                (
                    new[]
                    {
                        Token(SyntaxKind.PublicKeyword),
                        Token(SyntaxKind.ConstKeyword)
                    }
                )
            );
        }

        private ConstructorDeclarationSyntax appCtor(AppApiTemplate appTemplate)
        {
            return ConstructorDeclaration(Identifier(getAppClassName(appTemplate)))
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
                            appCtorArgs(appTemplate)
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
                                appBaseCtorArgs(appTemplate)
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
                            appCtorBodyStatements(appTemplate)
                        )
                    )
                );
        }

        private static StatementSyntax[] appCtorBodyStatements(AppApiTemplate appTemplate)
        {
            var statements = new List<StatementSyntax>();
            if (appTemplate.IsAuthenticator())
            {
                statements.Add
                (
                    ExpressionStatement
                    (
                        AssignmentExpression
                        (
                            SyntaxKind.SimpleAssignmentExpression,
                            IdentifierName("xtiToken"),
                            ObjectCreationExpression(IdentifierName("IXtiToken"))
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
            foreach (var group in appTemplate.GroupTemplates)
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

        private static BaseTypeSyntax[] appBaseList()
        {
            var baseTypes = new List<BaseTypeSyntax>();
            baseTypes.Add(SimpleBaseType(IdentifierName("AppClient")));
            return baseTypes.ToArray();
        }

        private SyntaxNodeOrToken[] appCtorArgs(AppApiTemplate appTemplate)
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
            if (appTemplate.IsAuthenticator())
            {
                args.AddRange
                (
                    new SyntaxNodeOrToken[]
                    {
                        Parameter(Identifier("credentials"))
                            .WithType(IdentifierName("ICredentials")),
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
                            .WithType(IdentifierName("IXtiToken")),
                        Token(SyntaxKind.CommaToken)
                    }
                );
            }
            args.AddRange
            (
                new SyntaxNodeOrToken[]
                {
                    Parameter(Identifier("baseUrl"))
                        .WithType(PredefinedType(Token(SyntaxKind.StringKeyword))),
                    Token(SyntaxKind.CommaToken),
                    Parameter(Identifier("version"))
                        .WithType(PredefinedType(Token(SyntaxKind.StringKeyword)))
                        .WithDefault
                        (
                            EqualsValueClause
                            (
                                IdentifierName("DefaultVersion")
                            )
                        )
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
                    ),
                    Token(SyntaxKind.CommaToken),
                    Argument
                    (
                        ConditionalExpression
                        (
                            InvocationExpression
                            (
                                MemberAccessExpression
                                (
                                    SyntaxKind.SimpleMemberAccessExpression,
                                    PredefinedType(Token(SyntaxKind.StringKeyword)),
                                    IdentifierName("IsNullOrWhiteSpace")
                                )
                            )
                            .WithArgumentList
                            (
                                ArgumentList
                                (
                                    SingletonSeparatedList
                                    (
                                        Argument(IdentifierName("version"))
                                    )
                                )
                            ),
                            IdentifierName("DefaultVersion"),
                            IdentifierName("version")
                        )
                    )
                }
            );
            return args.ToArray();
        }

    }
}
