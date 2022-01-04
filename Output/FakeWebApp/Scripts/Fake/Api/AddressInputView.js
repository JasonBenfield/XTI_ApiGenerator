"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
exports.AddressInputView = void 0;
var tslib_1 = require("tslib");
// Generated code
var ComplexFieldFormGroupView_1 = require("@jasonbenfield/sharedwebapp/Forms/ComplexFieldFormGroupView");
var AddressInputView = /** @class */ (function (_super) {
    tslib_1.__extends(AddressInputView, _super);
    function AddressInputView() {
        var _this = _super !== null && _super.apply(this, arguments) || this;
        _this.ID1 = _this.addInputFormGroup();
        _this.ID2 = _this.addInputFormGroup();
        _this.Line1 = _this.addInputFormGroup();
        _this.City = _this.addInputFormGroup();
        _this.State = _this.addInputFormGroup();
        _this.Zip = _this.addInputFormGroup();
        return _this;
    }
    return AddressInputView;
}(ComplexFieldFormGroupView_1.ComplexFieldFormGroupView));
exports.AddressInputView = AddressInputView;
//# sourceMappingURL=AddressInputView.js.map