"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
exports.AddEmployeeForm = void 0;
var tslib_1 = require("tslib");
var Form_1 = require("XtiShared/Forms/Form");
var DropDownFieldItem_1 = require("XtiShared/Forms/DropDownFieldItem");
var AddressInput_1 = require("./AddressInput");
var AddEmployeeForm = /** @class */ (function (_super) {
    tslib_1.__extends(AddEmployeeForm, _super);
    function AddEmployeeForm(vm) {
        var _this = _super.call(this, 'AddEmployeeForm') || this;
        _this.vm = vm;
        _this.EmployeeName = _this.addTextInputField('EmployeeName', _this.vm.EmployeeName);
        _this.BirthDate = _this.addDateInputField('BirthDate', _this.vm.BirthDate);
        _this.Department = _this.addDropDownField('Department', _this.vm.Department);
        _this.Address = _this.addField(new AddressInput_1.AddressInput(_this.getName(), 'Address', _this.vm.Address));
        _this.SSN = _this.addNumberInputField('SSN', _this.vm.SSN);
        _this.HireDate = _this.addDropDownField('HireDate', _this.vm.HireDate);
        _this.IsTemp = _this.addDropDownField('IsTemp', _this.vm.IsTemp);
        _this.EmployeeName.caption.setCaption('Employee Name');
        _this.EmployeeName.setMaxLength(100);
        _this.BirthDate.caption.setCaption('Birth Date');
        _this.Department.caption.setCaption('Department');
        _this.Department.constraints.mustNotBeNull();
        _this.Department.setItemCaption('Select...');
        _this.Department.setItems(new DropDownFieldItem_1.DropDownFieldItem(1, 'HR'), new DropDownFieldItem_1.DropDownFieldItem(2, 'IT'));
        _this.Address.caption.setCaption('Address');
        _this.SSN.caption.setCaption('SSN');
        _this.SSN.constraints.mustBeAbove(0, 'Must be greater than 0');
        _this.SSN.constraints.mustBeBelow(1000000000, 'Must be less than 1000000000');
        _this.SSN.protect();
        _this.HireDate.caption.setCaption('Hire Date');
        _this.HireDate.setItems(new DropDownFieldItem_1.DropDownFieldItem(new Date(Date.UTC(2021, 0, 22, 5, 0, 0, 0)), 'Yesterday'), new DropDownFieldItem_1.DropDownFieldItem(new Date(Date.UTC(2021, 0, 23, 5, 0, 0, 0)), 'Today'), new DropDownFieldItem_1.DropDownFieldItem(new Date(Date.UTC(2021, 0, 24, 5, 0, 0, 0)), 'Tomorrow'));
        _this.IsTemp.caption.setCaption('Is Temp');
        _this.IsTemp.setItems(new DropDownFieldItem_1.DropDownFieldItem(true, 'Yes'), new DropDownFieldItem_1.DropDownFieldItem(false, 'No'));
        return _this;
    }
    return AddEmployeeForm;
}(Form_1.Form));
exports.AddEmployeeForm = AddEmployeeForm;
//# sourceMappingURL=AddEmployeeForm.js.map