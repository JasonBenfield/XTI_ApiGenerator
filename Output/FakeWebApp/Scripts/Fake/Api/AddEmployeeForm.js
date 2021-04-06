"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
exports.AddEmployeeForm = void 0;
var tslib_1 = require("tslib");
// Generated code
var BaseForm_1 = require("XtiShared/Forms/BaseForm");
var FormComponentViewModel_1 = require("XtiShared/Html/FormComponentViewModel");
var DropDownFieldItem_1 = require("XtiShared/Forms/DropDownFieldItem");
var AddressInput_1 = require("./AddressInput");
var AddEmployeeForm = /** @class */ (function (_super) {
    tslib_1.__extends(AddEmployeeForm, _super);
    function AddEmployeeForm(vm) {
        if (vm === void 0) { vm = new FormComponentViewModel_1.FormComponentViewModel(); }
        var _this = _super.call(this, 'AddEmployeeForm', vm) || this;
        _this.EmployeeName = _this.addTextInputFormGroup('EmployeeName');
        _this.BirthDate = _this.addDateInputFormGroup('BirthDate');
        _this.Department = _this.addNumberDropDownFormGroup('Department');
        _this.Address = _this.addFormGroup(new AddressInput_1.AddressInput(_this.getName(), 'Address'));
        _this.SSN = _this.addNumberInputFormGroup('SSN');
        _this.HireDate = _this.addDateDropDownFormGroup('HireDate');
        _this.IsTemp = _this.addBooleanDropDownFormGroup('IsTemp');
        _this.EmployeeID = _this.addHiddenNumberFormGroup('EmployeeID');
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
        _this.HireDate.setItems(new DropDownFieldItem_1.DropDownFieldItem(new Date(Date.UTC(2021, 1, 10, 5, 0, 0, 0)), 'Yesterday'), new DropDownFieldItem_1.DropDownFieldItem(new Date(Date.UTC(2021, 1, 11, 5, 0, 0, 0)), 'Today'), new DropDownFieldItem_1.DropDownFieldItem(new Date(Date.UTC(2021, 1, 12, 5, 0, 0, 0)), 'Tomorrow'));
        _this.IsTemp.setCaption('Is Temp');
        _this.IsTemp.setItems(new DropDownFieldItem_1.DropDownFieldItem(true, 'Yes'), new DropDownFieldItem_1.DropDownFieldItem(false, 'No'));
        _this.EmployeeID.setCaption('Employee ID');
        return _this;
    }
    return AddEmployeeForm;
}(BaseForm_1.BaseForm));
exports.AddEmployeeForm = AddEmployeeForm;
//# sourceMappingURL=AddEmployeeForm.js.map