using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XTI_App.Api;
using XTI_WebApp.CodeGeneration;
using XTI_WebApp.CodeGeneration.CSharp;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace XTI_WebApp.ClientGenerator.CSharp
{
    public sealed class ApiAppClass
    {
        private readonly string ns;
        private readonly Func<string, Stream> createStream;
        private readonly AppApiTemplate template;
        private readonly DefaultVersion defaultVersion;

        public ApiAppClass(string ns, Func<string, Stream> createStream, AppApiTemplate template, DefaultVersion defaultVersion)
        {
            this.ns = ns;
            this.createStream = createStream;
            this.template = template;
            this.defaultVersion = defaultVersion;
        }

        public async Task Output()
        {
            var appClient = await createAppClient();
            var className = getAppClassName();
            await outputClass(appClient, className);
        }

        private static string getGroupClassName(AppApiGroupTemplate groupTemplate) => $"{groupTemplate.Name}Group";

        private static SyntaxList<UsingDirectiveSyntax> appUsings()
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
            return List(usings);
        }

        private string getAppClassName() => $"{template.Name}AppClient";

        private async Task<CompilationUnitSyntax> createAppClient()
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
                                    (MemberDeclarationSyntax)ClassDeclaration(getAppClassName())
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
                                                await appMembers()
                                            )
                                        )
                                    )
                                )
                            )
                    );
        }

        private async Task<MemberDeclarationSyntax[]> appMembers()
        {
            var members = new List<MemberDeclarationSyntax>();
            members.Add(appCtor());
            members.Add(await defaultVersionDeclaration());
            foreach (var group in template.GroupTemplates)
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

        private async Task<MemberDeclarationSyntax> defaultVersionDeclaration()
        {
            var versionKey = await defaultVersion.Value(template.AppKey);
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

        private ConstructorDeclarationSyntax appCtor()
        {
            return ConstructorDeclaration(Identifier(getAppClassName()))
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
                            appCtorArgs()
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
                                appBaseCtorArgs()
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
                            appCtorBodyStatements()
                        )
                    )
                );
        }

        private StatementSyntax[] appCtorBodyStatements()
        {
            var statements = new List<StatementSyntax>();
            if (template.IsAuthenticator())
            {
                statements.Add
                (
                    ExpressionStatement
                    (
                        AssignmentExpression
                        (
                            SyntaxKind.SimpleAssignmentExpression,
                            IdentifierName("xtiToken"),
                            InvocationExpression
                            (
                                MemberAccessExpression
                                (
                                    SyntaxKind.SimpleMemberAccessExpression,
                                    IdentifierName("tokenFactory"),
                                    IdentifierName("Create")
                                )
                            )
                            .WithArgumentList
                            (
                                ArgumentList
                                (
                                    SingletonSeparatedList
                                    (
                                        Argument(ThisExpression())
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
            foreach (var group in template.GroupTemplates)
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

        private SyntaxNodeOrToken[] appCtorArgs()
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
            if (template.IsAuthenticator())
            {
                args.AddRange
                (
                    new SyntaxNodeOrToken[]
                    {
                        Parameter(Identifier("tokenFactory"))
                            .WithType(IdentifierName("IXtiTokenFactory")),
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

        private SyntaxNodeOrToken[] appBaseCtorArgs()
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
                            Literal(template.Name)
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

        private Task outputClass(CompilationUnitSyntax compilationUnitSyntax, string className)
        {
            var cSharpFile = new CSharpFile(compilationUnitSyntax, createStream, className);
            return cSharpFile.Output();
        }

    }
}
