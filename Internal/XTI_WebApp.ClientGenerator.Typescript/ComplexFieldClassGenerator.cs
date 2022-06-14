using XTI_Forms;

namespace XTI_WebApp.ClientGenerator.Typescript;

internal sealed class ComplexFieldClassGenerator
{
    private readonly Func<string, Stream> createStream;
    private readonly IComplexField complexField;
    private readonly bool isForm;

    public ComplexFieldClassGenerator(Func<string, Stream> createStream, IComplexField complexField, bool isForm)
    {
        this.createStream = createStream;
        this.complexField = complexField;
        this.isForm = isForm;
    }

    public Task Output()
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
                    var tsInputType = new TsType(inputType).Value;
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

}
