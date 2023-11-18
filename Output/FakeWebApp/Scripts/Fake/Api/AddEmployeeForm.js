"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
exports.AddEmployeeForm = void 0;
// Generated code
const BaseForm_1 = require("@jasonbenfield/sharedwebapp/Forms/BaseForm");
const DropDownFieldItem_1 = require("@jasonbenfield/sharedwebapp/Forms/DropDownFieldItem");
const AddressInput_1 = require("./AddressInput");
class AddEmployeeForm extends BaseForm_1.BaseForm {
    constructor(view) {
        super('AddEmployeeForm', view);
        this.EmployeeName = this.addTextInputFormGroup('EmployeeName', this.view.EmployeeName);
        this.BirthDate = this.addDateInputFormGroup('BirthDate', this.view.BirthDate);
        this.Department = this.addNumberDropDownFormGroup('Department', this.view.Department);
        this.Address = this.addFormGroup(new AddressInput_1.AddressInput(this.getName(), 'Address', this.view.Address));
        this.SSN = this.addNumberInputFormGroup('SSN', this.view.SSN);
        this.HireDate = this.addDateDropDownFormGroup('HireDate', this.view.HireDate);
        this.IsTemp = this.addBooleanDropDownFormGroup('IsTemp', this.view.IsTemp);
        this.EmployeeID = this.addHiddenNumberFormGroup('EmployeeID', this.view.EmployeeID);
        this.EmployeeName.setCaption('Employee Name');
        this.EmployeeName.setMaxLength(100);
        this.BirthDate.setCaption('Birth Date');
        this.Department.setCaption('Department');
        this.Department.constraints.mustNotBeNull();
        this.Department.setItemCaption('Select...');
        this.Department.setItems(new DropDownFieldItem_1.DropDownFieldItem(1, 'HR'), new DropDownFieldItem_1.DropDownFieldItem(2, 'IT'));
        this.Address.setCaption('Address');
        this.SSN.setCaption('SSN');
        this.SSN.constraints.mustBeAbove(0, 'Must be greater than 0');
        this.SSN.constraints.mustBeBelow(1000000000, 'Must be less than 1000000000');
        this.SSN.protect();
        this.HireDate.setCaption('Hire Date');
        this.HireDate.setItems(new DropDownFieldItem_1.DropDownFieldItem(new Date(Date.UTC(2023, 10, 10, 5, 0, 0, 0)), 'Yesterday'), new DropDownFieldItem_1.DropDownFieldItem(new Date(Date.UTC(2023, 10, 11, 5, 0, 0, 0)), 'Today'), new DropDownFieldItem_1.DropDownFieldItem(new Date(Date.UTC(2023, 10, 12, 5, 0, 0, 0)), 'Tomorrow'));
        this.IsTemp.setCaption('Is Temp');
        this.IsTemp.setItems(new DropDownFieldItem_1.DropDownFieldItem(true, 'Yes'), new DropDownFieldItem_1.DropDownFieldItem(false, 'No'));
        this.EmployeeID.setCaption('Employee ID');
    }
}
exports.AddEmployeeForm = AddEmployeeForm;
//# sourceMappingURL=AddEmployeeForm.js.map