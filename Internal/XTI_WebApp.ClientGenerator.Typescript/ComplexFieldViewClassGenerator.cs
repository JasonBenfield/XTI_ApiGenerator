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
            tsFile.AddLine("import { BaseFormView } from '@jasonbenfield/sharedwebapp/Views/BaseFormView';");
        }
        else
        {
            tsFile.AddLine("import { ComplexFieldFormGroupView } from '@jasonbenfield/sharedwebapp/Views/ComplexFieldFormGroupView';");
        }
        tsFile.AddLine("import { SimpleFieldFormGroupInputView, SimpleFieldFormGroupSelectView } from '@jasonbenfield/sharedwebapp/Views/FormGroup';");
        tsFile.AddLine("import { IFormGroupLayout } from '@jasonbenfield/sharedwebapp/Views/Types';");
        tsFile.AddLine("import { BasicComponentView } from '@jasonbenfield/sharedwebapp/Views/BasicComponentView';");
        var fields = complexField.Fields;
        foreach (var field in fields.OfType<ComplexFieldModel>().Select(f => f.TypeName).Distinct())
        {
            tsFile.AddLine($"import {{ {field}View }} from './{field}View';");
        }
        tsFile.AddLine();
        tsFile.AddLine($"export interface I{className} {{");
        tsFile.Indent();
        var lastField = fields.Last();
        foreach (var field in fields)
        {
            tsFile.AddLine($"{field.Name}: ");
            if (field is IComplexField complex)
            {
                tsFile.Append($"{complex.TypeName}View");
            }
            else if (field is DropDownFieldModel dropDownField)
            {
                tsFile.Append("SimpleFieldFormGroupSelectView");
            }
            else if (field is SimpleFieldModel hiddenField)
            {
                tsFile.Append("SimpleFieldFormGroupInputView");
            }
            else
            {
                throw new NotSupportedException($"Simple field of type {field.GetType()} is not supported");
            }
            tsFile.Append(";");
        }
        tsFile.Outdent();
        tsFile.AddLine("}");
        tsFile.AddLine();
        tsFile.AddLine($"export class Default{className}Layout implements IFormGroupLayout<I{className}> {{");
        tsFile.Indent();
        tsFile.AddLine($"addFormGroups(form: {className}) {{");
        tsFile.Indent();
        tsFile.AddLine("return {");
        tsFile.Indent();
        foreach (var field in fields)
        {
            tsFile.AddLine($"{field.Name}: ");
            if (field is IComplexField complex)
            {
                tsFile.Append($"form.addFormGroup({complex.TypeName}View)");
            }
            else if (field is DropDownFieldModel)
            {
                tsFile.Append("form.addDropDownFormGroup()");
            }
            else if (field is SimpleFieldModel)
            {
                tsFile.Append("form.addInputFormGroup()");
            }
            else
            {
                throw new NotSupportedException($"Simple field of type {field.GetType()} is not supported");
            }
            if(field != lastField)
            {
                tsFile.Append(",");
            }
        }
        tsFile.Outdent();
        tsFile.AddLine("}");
        tsFile.Outdent();
        tsFile.AddLine("}");
        tsFile.Outdent();
        tsFile.AddLine("}");
        tsFile.AddLine();
        var baseClass = isForm ? "BaseFormView" : "ComplexFieldFormGroupView";
        tsFile.AddLine($"export class {className} extends {baseClass} {{");
        tsFile.Indent();
        tsFile.AddLine($"private formGroups: I{className};");
        tsFile.AddLine();
        tsFile.AddLine("constructor(container: BasicComponentView) {");
        tsFile.Indent();
        tsFile.AddLine("super(container);");
        tsFile.Outdent();
        tsFile.AddLine("}");
        tsFile.AddLine();
        tsFile.AddLine($"addContent(layout?: IFormGroupLayout<I{className}>) {{");
        tsFile.Indent();
        tsFile.AddLine("if (!layout) {");
        tsFile.Indent();
        tsFile.AddLine($"layout = new Default{className}Layout();");
        tsFile.Outdent();
        tsFile.AddLine("}");
        tsFile.AddLine("this.formGroups = layout.addFormGroups(this);");
        tsFile.Outdent();
        tsFile.AddLine("}");
        tsFile.AddLine();
        foreach (var field in fields)
        {
            tsFile.AddLine($"get {field.Name}() {{ return this.formGroups.{field.Name}; }}");
        }
        tsFile.Outdent();
        tsFile.AddLine("}");
        return tsFile.Output();
    }
}
