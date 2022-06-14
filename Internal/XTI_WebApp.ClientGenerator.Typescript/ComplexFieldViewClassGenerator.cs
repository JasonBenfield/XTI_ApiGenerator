using XTI_Forms;

namespace XTI_WebApp.ClientGenerator.Typescript;

internal sealed class ComplexFieldViewClassGenerator
{
    private readonly Func<string, Stream> createStream;
    private readonly IComplexField complexField;
    private readonly bool isForm;

    public ComplexFieldViewClassGenerator(Func<string, Stream> createStream, IComplexField complexField, bool isForm)
    {
        this.createStream = createStream;
        this.complexField = complexField;
        this.isForm = isForm;
    }

    public Task Output()
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
                    var tsInputType = new TsType(inputType).Value;
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
}
