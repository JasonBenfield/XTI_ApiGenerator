"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
exports.AddressInput = void 0;
var tslib_1 = require("tslib");
var ComplexField_1 = require("XtiShared/Forms/ComplexField");
var AddressInput = /** @class */ (function (_super) {
    tslib_1.__extends(AddressInput, _super);
    function AddressInput(prefix, name, vm) {
        var _this = _super.call(this, prefix, name, vm.caption, vm.value) || this;
        _this.vm = vm;
        _this.ID1 = _this.addHiddenTextField('ID1', _this.vm.value.ID1);
        _this.ID2 = _this.addHiddenNumberField('ID2', _this.vm.value.ID2);
        _this.Line1 = _this.addTextInputField('Line1', _this.vm.value.Line1);
        _this.City = _this.addTextInputField('City', _this.vm.value.City);
        _this.State = _this.addTextInputField('State', _this.vm.value.State);
        _this.Zip = _this.addNumberInputField('Zip', _this.vm.value.Zip);
        _this.ID1.caption.setCaption('ID 1');
        _this.ID2.caption.setCaption('ID 2');
        _this.Line1.caption.setCaption('Line 1');
        _this.Line1.setMaxLength(30);
        _this.City.caption.setCaption('City');
        _this.State.caption.setCaption('State');
        _this.Zip.caption.setCaption('Zip');
        return _this;
    }
    return AddressInput;
}(ComplexField_1.ComplexField));
exports.AddressInput = AddressInput;
//# sourceMappingURL=AddressInput.js.map