"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
exports.AddEmployeeFormView = exports.DefaultAddEmployeeFormViewLayout = void 0;
// Generated code
const BaseFormView_1 = require("@jasonbenfield/sharedwebapp/Views/BaseFormView");
const AddressInputView_1 = require("./AddressInputView");
class DefaultAddEmployeeFormViewLayout {
    addFormGroups(form) {
        return {
            EmployeeName: form.addInputFormGroup(),
            BirthDate: form.addInputFormGroup(),
            Department: form.addDropDownFormGroup(),
            Address: form.addFormGroup(AddressInputView_1.AddressInputView),
            SSN: form.addInputFormGroup(),
            HireDate: form.addDropDownFormGroup(),
            IsTemp: form.addDropDownFormGroup(),
            EmployeeID: form.addInputFormGroup()
        };
    }
}
exports.DefaultAddEmployeeFormViewLayout = DefaultAddEmployeeFormViewLayout;
class AddEmployeeFormView extends BaseFormView_1.BaseFormView {
    constructor(container) {
        super(container);
    }
    addContent(layout) {
        if (!layout) {
            layout = new DefaultAddEmployeeFormViewLayout();
        }
        this.formGroups = layout.addFormGroups(this);
    }
    get EmployeeName() { return this.formGroups.EmployeeName; }
    get BirthDate() { return this.formGroups.BirthDate; }
    get Department() { return this.formGroups.Department; }
    get Address() { return this.formGroups.Address; }
    get SSN() { return this.formGroups.SSN; }
    get HireDate() { return this.formGroups.HireDate; }
    get IsTemp() { return this.formGroups.IsTemp; }
    get EmployeeID() { return this.formGroups.EmployeeID; }
}
exports.AddEmployeeFormView = AddEmployeeFormView;
//# sourceMappingURL=AddEmployeeFormView.js.map