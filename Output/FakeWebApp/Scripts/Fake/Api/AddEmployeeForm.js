"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
exports.AddEmployeeForm = void 0;
var tslib_1 = require("tslib");
// Generated code
var BaseForm_1 = require("@jasonbenfield/sharedwebapp/Forms/BaseForm");
var DropDownFieldItem_1 = require("@jasonbenfield/sharedwebapp/Forms/DropDownFieldItem");
var AddressInput_1 = require("./AddressInput");
var AddEmployeeForm = /** @class */ (function (_super) {
    tslib_1.__extends(AddEmployeeForm, _super);
    function AddEmployeeForm(view) {
        var _this = _super.call(this, 'AddEmployeeForm', view) || this;
        _this.EmployeeName = _this.addTextInputFormGroup('EmployeeName', _this.view.EmployeeName);
        _this.BirthDate = _this.addDateInputFormGroup('BirthDate', _this.view.BirthDate);
        _this.Department = _this.addNumberDropDownFormGroup('Department', _this.view.Department);
        _this.Address = _this.addFormGroup(new AddressInput_1.AddressInput(_this.getName(), 'Address', _this.view.Address));
        _this.SSN = _this.addNumberInputFormGroup('SSN', _this.view.SSN);
        _this.HireDate = _this.addDateDropDownFormGroup('HireDate', _this.view.HireDate);
        _this.IsTemp = _this.addBooleanDropDownFormGroup('IsTemp', _this.view.IsTemp);
        _this.EmployeeID = _this.addHiddenNumberFormGroup('EmployeeID', _this.view.EmployeeID);
        _this.EmployeeName.setCaption('Employee Name');
        _this.EmployeeName.setMaxLength(100);
        _this.BirthDate.setCaption('Birth Date');
        _this.Department.setCaption('Department');
        _this.Department.constraints.mustNotBeNull();
        _this.Department.setItemCaption('Select...');
        _this.Department.setItems(new DropDownFieldItem_1.DropDownFieldItem(1, 'HR'), new DropDownFieldItem_1.DropDownFieldItem(2, 'IT'));
        _this.Address.setCaption('Address');
        _this.SSN.setCaption('SSN');
        _this.SSN.constraints.mustBeAbove(0, 'Must be greater than 0');
        _this.SSN.constraints.mustBeBelow(1000000000, 'Must be less than 1000000000');
        _this.SSN.protect();
        _this.HireDate.setCaption('Hire Date');
        _this.HireDate.setItems(new DropDownFieldItem_1.DropDownFieldItem(new Date(Date.UTC(2022, 0, 2, 5, 0, 0, 0)), 'Yesterday'), new DropDownFieldItem_1.DropDownFieldItem(new Date(Date.UTC(2022, 0, 3, 5, 0, 0, 0)), 'Today'), new DropDownFieldItem_1.DropDownFieldItem(new Date(Date.UTC(2022, 0, 4, 5, 0, 0, 0)), 'Tomorrow'));
        _this.IsTemp.setCaption('Is Temp');
        _this.IsTemp.setItems(new DropDownFieldItem_1.DropDownFieldItem(true, 'Yes'), new DropDownFieldItem_1.DropDownFieldItem(false, 'No'));
        _this.EmployeeID.setCaption('Employee ID');
        return _this;
    }
    return AddEmployeeForm;
}(BaseForm_1.BaseForm));
exports.AddEmployeeForm = AddEmployeeForm;
//# sourceMappingURL=AddEmployeeForm.js.map