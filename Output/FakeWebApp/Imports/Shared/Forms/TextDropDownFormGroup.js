"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
exports.TextDropDownFormGroup = void 0;
var tslib_1 = require("tslib");
var ConstraintCollection_1 = require("./ConstraintCollection");
var DropDownFormGroup_1 = require("./DropDownFormGroup");
var TextDropDownFormGroup = /** @class */ (function (_super) {
    tslib_1.__extends(TextDropDownFormGroup, _super);
    function TextDropDownFormGroup(prefix, name, view) {
        var _this = _super.call(this, prefix, name, view) || this;
        _this.constraints = new ConstraintCollection_1.TextConstraintCollection();
        return _this;
    }
    TextDropDownFormGroup.prototype.validateConstraints = function (fieldErrors) {
        this.constraints.validate(fieldErrors, this);
    };
    return TextDropDownFormGroup;
}(DropDownFormGroup_1.DropDownFormGroup));
exports.TextDropDownFormGroup = TextDropDownFormGroup;
//# sourceMappingURL=TextDropDownFormGroup.js.map