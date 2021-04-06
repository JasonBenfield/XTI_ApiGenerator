"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
exports.AddressInput = void 0;
var tslib_1 = require("tslib");
// Generated code
var ComplexFieldFormGroup_1 = require("XtiShared/Forms/ComplexFieldFormGroup");
var BlockViewModel_1 = require("XtiShared/Html/BlockViewModel");
var AddressInput = /** @class */ (function (_super) {
    tslib_1.__extends(AddressInput, _super);
    function AddressInput(prefix, name, vm) {
        if (vm === void 0) { vm = new BlockViewModel_1.BlockViewModel(); }
        var _this = _super.call(this, prefix, name, vm) || this;
        _this.ID1 = _this.addHiddenTextFormGroup('ID1');
        _this.ID2 = _this.addHiddenNumberFormGroup('ID2');
        _this.Line1 = _this.addTextInputFormGroup('Line1');
        _this.City = _this.addTextInputFormGroup('City');
        _this.State = _this.addTextInputFormGroup('State');
        _this.Zip = _this.addNumberInputFormGroup('Zip');
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