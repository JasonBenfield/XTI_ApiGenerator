using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using XTI_App.Api;
using XTI_WebApp.CodeGeneration;

namespace XTI_WebApp.ClientGenerator.Typescript
{
    public sealed class TsClient : CodeGenerator
    {
        private readonly DefaultVersion defaultVersion;
        private readonly Func<string, Stream> createStream;

        public TsClient(DefaultVersion defaultVersion, Func<string, Stream> createStream)
        {
            this.defaultVersion = defaultVersion;
            this.createStream = createStream;
        }

        public async Task Output(AppApiTemplate appTemplate)
        {
            var appClassName = $"{appTemplate.Name}AppApi";
            using (var stream = createStream($"{appClassName}.ts"))
            {
                var str = new StringBuilder();
                str.Append("// Generated code");
                str.Append("\r\n");
                str.Append("\r\nimport { AppApi } from \"../../Shared/AppApi\";");
                str.Append("\r\nimport { AppApiEvents } from \"../../Shared/AppApiEvents\";");
                foreach (var groupTemplate in appTemplate.GroupTemplates)
                {
                    var groupClassName = getGroupClassName(groupTemplate);
                    str.Append($"\r\nimport {{ {groupClassName} }} from \"./{groupClassName}\";");
                }
                str.Append("\r\n");
                str.Append($"\r\nexport class {appClassName} extends AppApi {{");
                var versionKey = await defaultVersion.Value(appTemplate.AppKey);
                str.Append($"\r\n\tpublic static readonly DefaultVersion = '{versionKey.Value}';");
                str.Append("\r\n");
                str.Append($"\r\n\tconstructor(events: AppApiEvents, baseUrl: string, version: string = '') {{");
                str.Append($"\r\n\t\tsuper(events, baseUrl, '{appTemplate.Name}', version || {appClassName}.DefaultVersion);");
                foreach (var groupTemplate in appTemplate.GroupTemplates)
                {
                    str.Append($"\r\n\t\tthis.{groupTemplate.Name} = this.addGroup((evts, resourceUrl) => new {groupTemplate.Name}Group(evts, resourceUrl));");
                }
                str.Append("\r\n\t}");
                str.Append("\r\n");
                foreach (var group in appTemplate.GroupTemplates)
                {
                    str.Append($"\r\n\treadonly {group.Name}: {group.Name}Group;");
                }
                str.Append("\r\n}");
                var bytes = Encoding.UTF8.GetBytes(str.ToString());
                await stream.WriteAsync(bytes, 0, bytes.Length);
            }
            foreach (var group in appTemplate.GroupTemplates)
            {
                var groupClassName = getGroupClassName(group);
                using (var stream = createStream($"{groupClassName}.ts"))
                {
                    var str = new StringBuilder();
                    str.Append("// Generated code");
                    str.Append("\r\n");
                    str.Append("\r\nimport { AppApiGroup } from \"../../Shared/AppApiGroup\";");
                    str.Append("\r\nimport { AppApiAction } from \"../../Shared/AppApiAction\";");
                    str.Append("\r\nimport { AppApiView } from \"../../Shared/AppApiView\";");
                    str.Append("\r\nimport { AppApiEvents } from \"../../Shared/AppApiEvents\";");
                    str.Append("\r\nimport { AppResourceUrl } from \"../../Shared/AppResourceUrl\";");
                    str.Append("\r\n");
                    var implementsClause = group.IsUser()
                        ? "implements IUserGroup "
                        : "";
                    str.Append($"\r\nexport class {groupClassName} extends AppApiGroup {implementsClause}{{");
                    str.Append("\r\n\tconstructor(events: AppApiEvents, resourceUrl: AppResourceUrl) {");
                    str.Append($"\r\n\t\tsuper(events, resourceUrl, '{group.Name}');");
                    foreach (var action in group.ActionTemplates)
                    {
                        if (action.IsView() || action.IsRedirect())
                        {
                            var modelType = getTsType(action.ModelTemplate);
                            str.Append($"\r\n\t\tthis.{action.Name} = this.createView<{modelType}>('{action.Name}');");
                        }
                        else
                        {
                            var genericArgs = getGenericArguments(action);
                            str.Append($"\r\n\t\tthis.{action.Name}Action = this.createAction{genericArgs}('{action.Name}', '{action.FriendlyName}');");
                        }
                    }
                    str.Append("\r\n\t}");
                    str.Append("\r\n");
                    foreach (var action in group.ActionTemplates)
                    {
                        if (action.IsView() || action.IsRedirect())
                        {
                            var modelType = getTsType(action.ModelTemplate);
                            str.Append($"\r\n\treadonly {action.Name}: AppApiView<{modelType}>;");
                        }
                        else
                        {
                            var genericArgs = getGenericArguments(action);
                            str.Append($"\r\n\tprivate readonly {action.Name}Action: AppApiAction{genericArgs};");
                        }
                    }
                    str.Append("\r\n");
                    foreach (var action in group.ActionTemplates)
                    {
                        if (!action.IsView() && !action.IsRedirect())
                        {
                            var modelType = getTsType(action.ModelTemplate.DataType);
                            var modelDecl = action.HasEmptyModel() ? "" : $"model: {modelType}, ";
                            var modelParm = action.HasEmptyModel() ? "{}" : "model";
                            str.Append($"\r\n\t{action.Name}({modelDecl}errorOptions?: IActionErrorOptions) {{");
                            str.Append($"\r\n\t\treturn this.{action.Name}Action.execute({modelParm}, errorOptions || {{}});");
                            str.Append("\r\n\t}");
                        }
                    }
                    str.Append("\r\n}");
                    var bytes = Encoding.UTF8.GetBytes(str.ToString());
                    await stream.WriteAsync(bytes, 0, bytes.Length);
                }
            }
            using (var stream = createStream($"{appTemplate.Name}Entities.d.ts"))
            {
                var str = new StringBuilder();
                str.Append("// Generated code");
                str.Append("\r\n");
                foreach (var objectTemplate in appTemplate.ObjectTemplates())
                {
                    str.Append($"\r\ninterface I{objectTemplate.DataType.Name} {{");
                    foreach (var property in objectTemplate.PropertyTemplates)
                    {
                        var tsType = getTsType(property.ValueTemplate);
                        str.Append($"\r\n\t{property.Name}: {tsType};");
                    }
                    str.Append("\r\n}");
                }
                var bytes = Encoding.UTF8.GetBytes(str.ToString());
                await stream.WriteAsync(bytes, 0, bytes.Length);
            }
        }

        private static string getGroupClassName(AppApiGroupTemplate group) => $"{group.Name}Group";

        private string getGenericArguments(AppApiActionTemplate action)
        {
            var modelType = getTsType(action.ModelTemplate);
            var resultType = getTsType(action.ResultTemplate);
            return $"<{modelType},{resultType}>";
        }

        private string getTsType(ValueTemplate valueTemplate)
        {
            if (valueTemplate is ArrayValueTemplate arrTempl)
            {
                var elType = getTsType(arrTempl.ElementTemplate.DataType);
                return $"{elType}[]";
            }
            return getTsType(valueTemplate.DataType);
        }

        private string getTsType(Type type)
        {
            string tsType;
            if (type == typeof(string))
            {
                tsType = "string";
            }
            else if
            (
                type == typeof(short) || type == typeof(int) || type == typeof(long)
                || type == typeof(double) || type == typeof(decimal) || type == typeof(float)
            )
            {
                tsType = "number";
            }
            else if (type == typeof(DateTime))
            {
                tsType = "Date";
            }
            else if (type == typeof(bool))
            {
                tsType = "boolean";
            }
            else
            {
                tsType = $"I{type.Name}";
            }
            return tsType;
        }
    }
}
