"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
exports.AddEmployeeFormViewModel = void 0;
// Generated code
var InputFieldViewModel_1 = require("XtiShared/Forms/InputFieldViewModel");
var DropDownFieldViewModel_1 = require("XtiShared/Forms/DropDownFieldViewModel");
var AddressInputViewModel_1 = require("./AddressInputViewModel");
var AddEmployeeFormViewModel = /** @class */ (function () {
    function AddEmployeeFormViewModel() {
        this.EmployeeName = new InputFieldViewModel_1.InputFieldViewModel();
        this.BirthDate = new InputFieldViewModel_1.InputFieldViewModel();
        this.Department = new DropDownFieldViewModel_1.DropDownFieldViewModel();
        this.Address = new AddressInputViewModel_1.AddressInputViewModel();
        this.SSN = new InputFieldViewModel_1.InputFieldViewModel();
        this.HireDate = new DropDownFieldViewModel_1.DropDownFieldViewModel();
        this.IsTemp = new DropDownFieldViewModel_1.DropDownFieldViewModel();
    }
    return AddEmployeeFormViewModel;
}());
exports.AddEmployeeFormViewModel = AddEmployeeFormViewModel;
//# sourceMappingURL=AddEmployeeFormViewModel.js.map