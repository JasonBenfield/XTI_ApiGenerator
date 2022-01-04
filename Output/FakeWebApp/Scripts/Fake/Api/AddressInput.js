"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
exports.AddressInput = void 0;
var tslib_1 = require("tslib");
// Generated code
var ComplexFieldFormGroup_1 = require("@jasonbenfield/sharedwebapp/Forms/ComplexFieldFormGroup");
var AddressInput = /** @class */ (function (_super) {
    tslib_1.__extends(AddressInput, _super);
    function AddressInput(prefix, name, view) {
        var _this = _super.call(this, prefix, name, view) || this;
        _this.ID1 = _this.addHiddenTextFormGroup('ID1', _this.view.ID1);
        _this.ID2 = _this.addHiddenNumberFormGroup('ID2', _this.view.ID2);
        _this.Line1 = _this.addTextInputFormGroup('Line1', _this.view.Line1);
        _this.City = _this.addTextInputFormGroup('City', _this.view.City);
        _this.State = _this.addTextInputFormGroup('State', _this.view.State);
        _this.Zip = _this.addNumberInputFormGroup('Zip', _this.view.Zip);
        _this.ID1.setCaption('ID 1');
        _this.ID2.setCaption('ID 2');
        _this.Line1.setCaption('Line 1');
        _this.Line1.setMaxLength(30);
        _this.City.setCaption('City');
        _this.State.setCaption('State');
        _this.Zip.setCaption('Zip');
        return _this;
    }
    return AddressInput;
}(ComplexFieldFormGroup_1.ComplexFieldFormGroup));
exports.AddressInput = AddressInput;
//# sourceMappingURL=AddressInput.js.map