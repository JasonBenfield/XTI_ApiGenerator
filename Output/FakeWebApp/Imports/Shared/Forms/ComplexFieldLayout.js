"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
exports.ComplexFieldLayout = void 0;
var BaseFormView_1 = require("./BaseFormView");
var ComplexFieldLayout = /** @class */ (function () {
    function ComplexFieldLayout(complexField) {
        this.complexField = complexField;
    }
    ComplexFieldLayout.prototype.execute = function () {
        this.executeLayout(this.complexField);
    };
    ComplexFieldLayout.prototype.executeLayout = function (complexField) {
        if (complexField instanceof BaseFormView_1.BaseFormView) {
            complexField.forEachFormGroup(function (fg) {
                fg.addToContainer(complexField);
            });
        }
        else {
            complexField.forEachFormGroup(function (fg) {
                fg.addToContainer(complexField.valueColumn);
            });
        }
    };
    return ComplexFieldLayout;
}());
exports.ComplexFieldLayout = ComplexFieldLayout;
//# sourceMappingURL=ComplexFieldLayout.js.map