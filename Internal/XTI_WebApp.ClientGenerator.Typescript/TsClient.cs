using System.Text.RegularExpressions;
using XTI_App.Abstractions;
using XTI_App.Api;
using XTI_Forms;
using XTI_WebApp.CodeGeneration;

namespace XTI_WebApp.ClientGenerator.Typescript;

public sealed class TsClient : CodeGenerator
{
    private readonly AppVersionKey versionKey;
    private readonly Func<string, Stream> createStream;

    private static readonly Regex whitespaceRegex = new Regex("\\s+");

    public TsClient(AppVersionKey versionKey, Func<string, Stream> createStream)
    {
        this.versionKey = versionKey;
        this.createStream = createStream;
    }

    public async Task Output(AppApiTemplate appTemplate)
    {
        await generateApp(appTemplate);
        var formTemplates = appTemplate.FormTemplates(ApiCodeGenerators.TypeScript);
        var complexFieldTemplates = formTemplates.SelectMany(ft => ft.Form.ComplexFieldTemplates).Distinct();
        foreach (var complexFieldTemplate in complexFieldTemplates)
        {
            await generateComplexField(complexFieldTemplate, false);
            await generateComplexFieldView(complexFieldTemplate, false);
        }
        foreach (var formTemplate in formTemplates)
        {
            await generateComplexField(formTemplate.Form, true);
            await generateComplexFieldView(formTemplate.Form, true);
        }
        foreach (var group in appTemplate.GroupTemplates)
        {
            await generateGroup(group);
        }
        await generateEntities(appTemplate);
        foreach (var numericValueTemplate in appTemplate.NumericValueTemplates(ApiCodeGenerators.TypeScript))
        {
            await generateNumericValue(numericValueTemplate);
        }
    }

    private Task generateComplexField(IComplexField complexField, bool isForm)
    {
        var className = complexField.TypeName;
        var tsFile = new TypeScriptFile(className, createStream);
        if (isForm)
        {
            tsFile.AddLine("import { BaseForm } from '@jasonbenfield/sharedwebapp/Forms/BaseForm';");
        }
        else
        {
            tsFile.AddLine("import { ComplexFieldFormGroup } from '@jasonbenfield/sharedwebapp/Forms/ComplexFieldFormGroup';");
        }
        tsFile.AddLine($"import {{ {className}View }} from './{className}View';");
        var fields = complexField.Fields;
        if (fields.OfType<DropDownFieldModel>().Any())
        {
            tsFile.AddLine("import { DropDownFieldItem } from \"@jasonbenfield/sharedwebapp/Forms/DropDownFieldItem\";");
        }
        foreach (var field in fields.OfType<ComplexFieldModel>().Select(f => f.TypeName).Distinct())
        {
            tsFile.AddLine($"import {{ {field} }} from './{field}';");
        }
        tsFile.AddLine();
        var baseClass = isForm ? "BaseForm" : "ComplexFieldFormGroup";
        tsFile.AddLine($"export class {className} extends {baseClass} {{");
        tsFile.Indent();
        tsFile.AddLine($"protected readonly view: {className}View;");
        tsFile.AddLine();
        var ctorArgs = isForm ? "" : "prefix: string, name: string, ";
        var vmClassName = isForm ? "FormComponentViewModel" : "BlockViewModel";
        tsFile.AddLine($"constructor({ctorArgs}view: {className}View) {{");
        tsFile.Indent();
        var superArgs = isForm ? $"'{className}'" : "prefix, name";
        tsFile.AddLine($"super({superArgs}, view);");
        foreach (var field in fields)
        {
            if (!string.IsNullOrWhiteSpace(field.Caption))
            {
                tsFile.AddLine($"this.{field.Name}.setCaption('{field.Caption}');");
            }
            if (field is SimpleFieldModel simpleField)
            {
                if (!simpleField.IsNullAllowed)
                {
                    tsFile.AddLine($"this.{field.Name}.constraints.mustNotBeNull();");
                }
                foreach (var constraint in simpleField.Constraints)
                {
                    if (constraint is NotWhitespaceConstraintModel)
                    {
                        tsFile.AddLine($"this.{field.Name}.constraints.mustNotBeWhitespace('{constraint.FailureMessage}');");
                    }
                    else if (constraint is LowerRangeConstraintModel lowerRange)
                    {
                        var lowerValue = valueToJsLiteral(lowerRange.Value);
                        if (lowerRange.IsIncluded)
                        {
                            tsFile.AddLine($"this.{field.Name}.constraints.mustBeOnOrAbove({lowerRange.Value}, '{constraint.FailureMessage}');");
                        }
                        else
                        {
                            tsFile.AddLine($"this.{field.Name}.constraints.mustBeAbove({lowerRange.Value}, '{constraint.FailureMessage}');");
                        }
                    }
                    else if (constraint is UpperRangeConstraintModel upperRange)
                    {
                        var upperValue = valueToJsLiteral(upperRange.Value);
                        if (upperRange.IsIncluded)
                        {
                            tsFile.AddLine($"this.{field.Name}.constraints.mustBeOnOrBelow({upperRange.Value}, '{constraint.FailureMessage}');");
                        }
                        else
                        {
                            tsFile.AddLine($"this.{field.Name}.constraints.mustBeBelow({upperRange.Value}, '{constraint.FailureMessage}');");
                        }
                    }
                }
            }
            if (field is InputFieldModel inputField)
            {
                if (inputField.MaxLength.HasValue)
                {
                    tsFile.AddLine($"this.{field.Name}.setMaxLength({inputField.MaxLength});");
                }
                if (inputField.IsProtected)
                {
                    tsFile.AddLine($"this.{field.Name}.protect();");
                }
            }
            else if (field is DropDownFieldModel dropDownField)
            {
                if (!string.IsNullOrWhiteSpace(dropDownField.ItemCaption))
                {
                    tsFile.AddLine($"this.{field.Name}.setItemCaption('{dropDownField.ItemCaption}');");
                }
                var dropDownItems = dropDownField.Items;
                if (dropDownItems?.Any() == true)
                {
                    tsFile.AddLine($"this.{field.Name}.setItems(");
                    tsFile.Indent();
                    var inputType = dropDownField.InputDataType;
                    var lastItem = dropDownItems.Last();
                    foreach (var dropDownItem in dropDownItems)
                    {
                        var itemValue = valueToJsLiteral(dropDownItem.Value);
                        tsFile.AddLine($"new DropDownFieldItem({itemValue}, '{dropDownItem.DisplayText}')");
                        if (dropDownItem != lastItem)
                        {
                            tsFile.Append(",");
                        }
                    }
                    tsFile.Outdent();
                    tsFile.AddLine(");");
                }
            }
        }
        tsFile.Outdent();
        tsFile.AddLine("}");
        foreach (var field in fields)
        {
            tsFile.AddLine("");
            tsFile.Append($"readonly {field.Name} = ");
            if (field is IComplexField complex)
            {
                tsFile.Append($"this.addFormGroup(new {complex.TypeName}(this.getName(), '{field.Name}', this.view.{field.Name}))");
            }
            else
            {
                string addFormGroup;
                if (field is InputFieldModel inputField)
                {
                    var inputType = inputField.InputDataType;
                    if (inputType == typeof(int?) || inputType == typeof(decimal?))
                    {
                        addFormGroup = "addNumberInputFormGroup";
                    }
                    else if (inputType == typeof(DateTimeOffset?))
                    {
                        addFormGroup = "addDateInputFormGroup";
                    }
                    else if (inputType == typeof(string))
                    {
                        addFormGroup = "addTextInputFormGroup";
                    }
                    else
                    {
                        throw new NotSupportedException($"Simple field with input type {inputType} is not supported");
                    }
                }
                else if (field is DropDownFieldModel dropDownField)
                {
                    var inputType = dropDownField.InputDataType;
                    var tsInputType = getTsType(inputType);
                    if (inputType == typeof(int?) || inputType == typeof(decimal?))
                    {
                        addFormGroup = "addNumberDropDownFormGroup";
                    }
                    else if (inputType == typeof(DateTimeOffset?))
                    {
                        addFormGroup = "addDateDropDownFormGroup";
                    }
                    else if (inputType == typeof(string))
                    {
                        addFormGroup = "addTextDropDownFormGroup";
                    }
                    else if (inputType == typeof(bool?))
                    {
                        addFormGroup = "addBooleanDropDownFormGroup";
                    }
                    else
                    {
                        throw new NotSupportedException($"DropDown field with input type {inputType} is not supported");
                    }
                }
                else if (field is SimpleFieldModel hiddenField)
                {
                    var inputType = hiddenField.InputDataType;
                    if (inputType == typeof(int?) || inputType == typeof(decimal?))
                    {
                        addFormGroup = "addHiddenNumberFormGroup";
                    }
                    else if (inputType == typeof(DateTimeOffset?))
                    {
                        addFormGroup = "addHiddenDateFormGroup";
                    }
                    else if (inputType == typeof(string))
                    {
                        addFormGroup = "addHiddenTextFormGroup";
                    }
                    else
                    {
                        throw new NotSupportedException($"Simple field with input type {inputType} is not supported");
                    }
                }
                else
                {
                    throw new NotSupportedException($"Simple field of type {field.GetType()} is not supported");
                }
                tsFile.Append($"this.{addFormGroup}('{field.Name}', this.view.{field.Name})");
            }
            tsFile.Append(";");
        }
        tsFile.Outdent();
        tsFile.AddLine("}");
        return tsFile.Output();
    }

    private Task generateComplexFieldView(IComplexField complexField, bool isForm)
    {
        var className = $"{complexField.TypeName}View";
        var tsFile = new TypeScriptFile(className, createStream);
        if (isForm)
        {
            tsFile.AddLine("import { BaseFormView } from '@jasonbenfield/sharedwebapp/Forms/BaseFormView';");
        }
        else
        {
            tsFile.AddLine("import { ComplexFieldFormGroupView } from '@jasonbenfield/sharedwebapp/Forms/ComplexFieldFormGroupView';");
        }
        var fields = complexField.Fields;
        foreach (var field in fields.OfType<ComplexFieldModel>().Select(f => f.TypeName).Distinct())
        {
            tsFile.AddLine($"import {{ {field}View }} from './{field}View';");
        }
        tsFile.AddLine();
        var baseClass = isForm ? "BaseFormView" : "ComplexFieldFormGroupView";
        tsFile.AddLine($"export class {className} extends {baseClass} {{");
        tsFile.Indent();
        foreach (var field in fields)
        {
            tsFile.AddLine("");
            tsFile.Append($"readonly {field.Name} = ");
            if (field is IComplexField complex)
            {
                tsFile.Append($"this.addFormGroup(new {complex.TypeName}View())");
            }
            else
            {
                string addFormGroup;
                if (field is InputFieldModel inputField)
                {
                    addFormGroup = "addInputFormGroup";
                }
                else if (field is DropDownFieldModel dropDownField)
                {
                    var inputType = dropDownField.InputDataType;
                    var tsInputType = getTsType(inputType);
                    addFormGroup = $"addDropDownFormGroup<{tsInputType}>";
                }
                else if (field is SimpleFieldModel hiddenField)
                {
                    addFormGroup = "addInputFormGroup";
                }
                else
                {
                    throw new NotSupportedException($"Simple field of type {field.GetType()} is not supported");
                }
                tsFile.Append($"this.{addFormGroup}()");
            }
            tsFile.Append(";");
        }
        tsFile.Outdent();
        tsFile.AddLine("}");
        return tsFile.Output();
    }

    private string valueToJsLiteral(object? value)
    {
        string jsLiteral;
        if (value == null)
        {
            jsLiteral = "null";
        }
        else
        {
            if (value is string)
            {
                jsLiteral = $"'{value}'";
            }
            else if (value is DateTime? || value is DateTimeOffset?)
            {
                DateTime? dateTime;
                if (value is DateTime?)
                {
                    dateTime = (DateTime?)value;
                }
                else
                {
                    var dateTimeOffset = (DateTimeOffset?)value;
                    dateTime = dateTimeOffset.Value.DateTime;
                }
                dateTime = dateTime.Value.ToUniversalTime();
                jsLiteral = $"new Date(Date.UTC({dateTime.Value.Year}, {dateTime.Value.Month - 1}, {dateTime.Value.Day}, {dateTime.Value.Hour}, {dateTime.Value.Minute}, {dateTime.Value.Second}, {dateTime.Value.Millisecond}))";
            }
            else if (value is bool?)
            {
                jsLiteral = value?.ToString()?.ToLower() ?? "";
            }
            else
            {
                jsLiteral = $"{value}";
            }
        }
        return jsLiteral;
    }

    private async Task generateApp(AppApiTemplate appTemplate)
    {
        var appClassName = $"{appTemplate.Name}AppApi";
        var tsFile = new TypeScriptFile(appClassName, createStream);
        tsFile.AddLine();
        tsFile.AddLine("import { AppApi } from \"@jasonbenfield/sharedwebapp/Api/AppApi\";");
        tsFile.AddLine("import { AppApiEvents } from \"@jasonbenfield/sharedwebapp/Api/AppApiEvents\";");
        foreach (var groupTemplate in appTemplate.GroupTemplates)
        {
            var groupClassName = getGroupClassName(groupTemplate);
            tsFile.AddLine($"import {{ {groupClassName} }} from \"./{groupClassName}\";");
        }
        tsFile.AddLine();
        tsFile.AddLine($"\r\nexport class {appClassName} extends AppApi {{");
        tsFile.Indent();
        tsFile.AddLine($"public static readonly DefaultVersion = '{versionKey.Value}';");
        tsFile.AddLine();
        tsFile.AddLine($"constructor(events: AppApiEvents, baseUrl: string, version: string = '') {{");
        tsFile.Indent();
        tsFile.AddLine($"super(events, baseUrl, '{appTemplate.Name}', version || {appClassName}.DefaultVersion);");
        foreach (var groupTemplate in appTemplate.GroupTemplates)
        {
            tsFile.AddLine($"this.{groupTemplate.Name} = this.addGroup((evts, resourceUrl) => new {groupTemplate.Name}Group(evts, resourceUrl));");
        }
        tsFile.Outdent();
        tsFile.AddLine("}");
        tsFile.AddLine();
        foreach (var group in appTemplate.GroupTemplates)
        {
            tsFile.AddLine($"readonly {group.Name}: {group.Name}Group;");
        }
        tsFile.Outdent();
        tsFile.AddLine("}");
        await tsFile.Output();
    }

    private Task generateEntities(AppApiTemplate appTemplate)
    {
        var tsFile = new TypeScriptFile($"{appTemplate.Name}Entities.d.ts", createStream);
        tsFile.AddLine();
        foreach (var objectTemplate in appTemplate.ObjectTemplates(ApiCodeGenerators.TypeScript))
        {
            tsFile.AddLine($"interface I{objectTemplate.DataType.Name} {{");
            tsFile.Indent();
            foreach (var property in objectTemplate.PropertyTemplates)
            {
                var tsType = getTsType(property.ValueTemplate);
                tsFile.AddLine($"{property.Name}: {tsType};");
            }
            tsFile.Outdent();
            tsFile.AddLine("}");
        }
        foreach (var numericValueTemplate in appTemplate.NumericValueTemplates(ApiCodeGenerators.TypeScript))
        {
            tsFile.AddLine($"interface I{numericValueTemplate.DataType.Name} {{");
            tsFile.Indent();
            tsFile.AddLine($"Value: number;");
            tsFile.AddLine($"DisplayText: string;");
            tsFile.Outdent();
            tsFile.AddLine("}");
        }
        return tsFile.Output();
    }

    private Task generateGroup(AppApiGroupTemplate group)
    {
        var groupClassName = getGroupClassName(group);
        var tsFile = new TypeScriptFile(groupClassName, createStream);
        tsFile.AddLine();
        tsFile.AddLine("import { AppApiGroup } from \"@jasonbenfield/sharedwebapp/Api/AppApiGroup\";");
        tsFile.AddLine("import { AppApiAction } from \"@jasonbenfield/sharedwebapp/Api/AppApiAction\";");
        tsFile.AddLine("import { AppApiView } from \"@jasonbenfield/sharedwebapp/Api/AppApiView\";");
        tsFile.AddLine("import { AppApiEvents } from \"@jasonbenfield/sharedwebapp/Api/AppApiEvents\";");
        tsFile.AddLine("import { AppResourceUrl } from \"@jasonbenfield/sharedwebapp/Api/AppResourceUrl\";");
        foreach (var form in group.FormTemplates())
        {
            tsFile.AddLine($"import {{ {form.Form.TypeName} }} from \"./{form.Form.TypeName}\";");
        }
        tsFile.AddLine();
        var implementsClause = group.IsUser()
            ? "implements IUserGroup "
            : "";
        tsFile.AddLine($"export class {groupClassName} extends AppApiGroup {implementsClause}{{");
        tsFile.Indent();
        tsFile.AddLine("constructor(events: AppApiEvents, resourceUrl: AppResourceUrl) {");
        tsFile.Indent();
        tsFile.AddLine($"super(events, resourceUrl, '{group.Name}');");
        foreach (var action in group.ActionTemplates)
        {
            if (action.IsView() || action.IsPartialView() || action.IsRedirect())
            {
                var modelType = getTsType(action.ModelTemplate);
                tsFile.AddLine($"this.{action.Name} = this.createView<{modelType}>('{action.Name}');");
            }
            else
            {
                var genericArgs = getGenericArguments(action);
                tsFile.AddLine($"this.{action.Name}Action = this.createAction{genericArgs}('{action.Name}', '{action.FriendlyName}');");
            }
        }
        tsFile.Outdent();
        tsFile.AddLine("}");
        tsFile.AddLine();
        foreach (var action in group.ActionTemplates)
        {
            if (action.IsView() || action.IsPartialView() || action.IsRedirect())
            {
                var modelType = getTsType(action.ModelTemplate);
                tsFile.AddLine($"readonly {action.Name}: AppApiView<{modelType}>;");
            }
            else
            {
                var genericArgs = getGenericArguments(action);
                tsFile.AddLine($"readonly {action.Name}Action: AppApiAction{genericArgs};");
            }
        }
        tsFile.AddLine();
        foreach (var action in group.ActionTemplates)
        {
            if (!action.IsView() && !action.IsPartialView() && !action.IsRedirect())
            {
                var modelType = getTsType(action.ModelTemplate.DataType);
                var modelDecl = action.HasEmptyModel() ? "" : $"model: {modelType}, ";
                var modelParm = action.HasEmptyModel() ? "{}" : "model";
                tsFile.AddLine($"{action.Name}({modelDecl}errorOptions?: IActionErrorOptions) {{");
                tsFile.Indent();
                tsFile.AddLine($"return this.{action.Name}Action.execute({modelParm}, errorOptions || {{}});");
                tsFile.Outdent();
                tsFile.AddLine("}");
            }
        }
        tsFile.Outdent();
        tsFile.AddLine("}");
        return tsFile.Output();
    }

    private static string getGroupClassName(AppApiGroupTemplate group) => $"{group.Name}Group";

    private Task generateNumericValue(NumericValueTemplate numericValueTemplate)
    {
        var tsFile = new TypeScriptFile($"{numericValueTemplate.DataType.Name}.ts", createStream);
        var className = numericValueTemplate.DataType.Name;
        tsFile.AddLine("import { NumericValue } from '@jasonbenfield/sharedwebapp/NumericValue';");
        tsFile.AddLine("import { NumericValues } from '@jasonbenfield/sharedwebapp/NumericValues';");
        tsFile.AddLine();
        tsFile.AddLine($"export class {className}s extends NumericValues<{className}> {{");
        tsFile.Indent();
        tsFile.AddLine("constructor(");
        tsFile.Indent();
        var valueNames = numericValueTemplate.Values.Select(v => whitespaceRegex.Replace(v.DisplayText, ""));
        var lastValueName = valueNames.Last();
        foreach (var valueName in valueNames)
        {
            tsFile.AddLine($"public readonly {valueName}: {className}");
            if (valueName != lastValueName)
            {
                tsFile.Append(",");
            }
        }
        tsFile.Outdent();
        tsFile.AddLine(") {");
        tsFile.Indent();
        var joinedValueNames = string.Join(",", valueNames);
        tsFile.AddLine($"super([{joinedValueNames}]);");
        tsFile.Outdent();
        tsFile.AddLine("}");
        tsFile.Outdent();
        tsFile.AddLine("}");
        tsFile.AddLine();
        tsFile.AddLine($"export class {className} extends NumericValue implements I{className} {{");
        tsFile.Indent();
        tsFile.AddLine($"public static readonly values = new {className}s(");
        tsFile.Indent();
        var lastValue = numericValueTemplate.Values.Last();
        foreach (var numericValue in numericValueTemplate.Values)
        {
            tsFile.AddLine($"new {className}({numericValue.Value}, '{numericValue.DisplayText.Replace("'", "\'")}')");
            if (numericValue != lastValue)
            {
                tsFile.Append(",");
            }
        }
        tsFile.Outdent();
        tsFile.AddLine(");");
        tsFile.AddLine();
        tsFile.AddLine("private constructor(Value: number, DisplayText: string) {");
        tsFile.Indent();
        tsFile.AddLine("super(Value, DisplayText);");
        tsFile.Outdent();
        tsFile.AddLine("}");
        tsFile.Outdent();
        tsFile.AddLine("}");
        return tsFile.Output();
    }

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

    private string getTsType(Type? type)
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
            || type == typeof(short?) || type == typeof(int?) || type == typeof(long?)
            || type == typeof(double?) || type == typeof(decimal?) || type == typeof(float?)
        )
        {
            tsType = "number";
        }
        else if
        (
            type == typeof(DateTime) || type == typeof(DateTimeOffset)
            || type == typeof(DateTime?) || type == typeof(DateTimeOffset?)
        )
        {
            tsType = "Date";
        }
        else if (type == typeof(bool) || type == typeof(bool?))
        {
            tsType = "boolean";
        }
        else if (type == typeof(object))
        {
            tsType = "object";
        }
        else if
        (
            type != null &&
            type.IsGenericType &&
            (
                type.GetGenericTypeDefinition() == typeof(IDictionary<,>) ||
                type.GetGenericTypeDefinition() == typeof(Dictionary<,>)
            )
        )
        {
            var dictArgs = type.GetGenericArguments();
            var keyType = getTsType(dictArgs[0]);
            var valueType = getTsType(dictArgs[1]);
            tsType = $"Record<{keyType},{valueType}>";
        }
        else if (typeof(Form).IsAssignableFrom(type))
        {
            tsType = type.Name;
        }
        else if(type != null)
        {
            tsType = $"I{type.Name}";
        }
        else 
        { 
            throw new ArgumentException($"Invalid type '{type}'"); 
        }
        return tsType;
    }
}