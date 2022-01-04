"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
exports.AddEmployeeFormView = void 0;
var tslib_1 = require("tslib");
// Generated code
var BaseFormView_1 = require("@jasonbenfield/sharedwebapp/Forms/BaseFormView");
var AddressInputView_1 = require("./AddressInputView");
var AddEmployeeFormView = /** @class */ (function (_super) {
    tslib_1.__extends(AddEmployeeFormView, _super);
    function AddEmployeeFormView() {
        var _this = _super !== null && _super.apply(this, arguments) || this;
        _this.EmployeeName = _this.addInputFormGroup();
        _this.BirthDate = _this.addInputFormGroup();
        _this.Department = _this.addDropDownFormGroup();
        _this.Address = _this.addFormGroup(new AddressInputView_1.AddressInputView());
        _this.SSN = _this.addInputFormGroup();
        _this.HireDate = _this.addDropDownFormGroup();
        _this.IsTemp = _this.addDropDownFormGroup();
        _this.EmployeeID = _this.addInputFormGroup();
        return _this;
    }
    return AddEmployeeFormView;
}(BaseFormView_1.BaseFormView));
exports.AddEmployeeFormView = AddEmployeeFormView;
//# sourceMappingURL=AddEmployeeFormView.js.map