"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
exports.AddressInputViewModel = exports.AddressInputValueViewModel = void 0;
var tslib_1 = require("tslib");
// Generated code
var ko = require("knockout");
var ComplexFieldViewModel_1 = require("XtiShared/Forms/ComplexFieldViewModel");
var FieldValueViewModel_1 = require("XtiShared/Forms/FieldValueViewModel");
var InputFieldViewModel_1 = require("XtiShared/Forms/InputFieldViewModel");
var AddressInputValueViewModel = /** @class */ (function (_super) {
    tslib_1.__extends(AddressInputValueViewModel, _super);
    function AddressInputValueViewModel() {
        var _this = _super !== null && _super.apply(this, arguments) || this;
        _this.componentName = ko.observable('AddressInput');
        _this.ID1 = _this.addValue(new InputFieldViewModel_1.InputFieldViewModel());
        _this.ID2 = _this.addValue(new InputFieldViewModel_1.InputFieldViewModel());
        _this.Line1 = _this.addValue(new InputFieldViewModel_1.InputFieldViewModel());
        _this.City = _this.addValue(new InputFieldViewModel_1.InputFieldViewModel());
        _this.State = _this.addValue(new InputFieldViewModel_1.InputFieldViewModel());
        _this.Zip = _this.addValue(new InputFieldViewModel_1.InputFieldViewModel());
        return _this;
    }
    return AddressInputValueViewModel;
}(FieldValueViewModel_1.ComplexFieldValueViewModel));
exports.AddressInputValueViewModel = AddressInputValueViewModel;
var AddressInputViewModel = /** @class */ (function (_super) {
    tslib_1.__extends(AddressInputViewModel, _super);
    function AddressInputViewModel() {
        return _super.call(this, new AddressInputValueViewModel()) || this;
    }
    return AddressInputViewModel;
}(ComplexFieldViewModel_1.ComplexFieldViewModel));
exports.AddressInputViewModel = AddressInputViewModel;
//# sourceMappingURL=AddressInputViewModel.js.map