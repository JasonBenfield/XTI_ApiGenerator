"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var tslib_1 = require("tslib");
var HtmlComponent_1 = require("./HtmlComponent");
var ContextualClass_1 = require("../ContextualClass");
var SelectViewModel_1 = require("./SelectViewModel");
var Select = /** @class */ (function (_super) {
    tslib_1.__extends(Select, _super);
    function Select(vm) {
        if (vm === void 0) { vm = new SelectViewModel_1.SelectViewModel(); }
        var _this = _super.call(this, vm) || this;
        _this.border = ContextualClass_1.ContextualClass.default;
        return _this;
    }
    Select.prototype.enable = function () { this.vm.isEnabled(true); };
    Select.prototype.disable = function () { this.vm.isEnabled(false); };
    Select.prototype.setBorder = function (border) {
        var borderCss = this.getBorderCss(border);
        this.replaceCssName(this.getBorderCss(this.border), borderCss);
        this.border = border;
    };
    Select.prototype.getBorderCss = function (border) {
        return border === ContextualClass_1.ContextualClass.default ? '' : border.append('border');
    };
    Select.prototype.setItemCaption = function (itemCaption) {
        this.vm.itemsCaption(itemCaption);
    };
    Select.prototype.setItems = function (items) {
        this.vm.items(items);
    };
    Select.prototype.getValue = function () { return this.vm.value(); };
    Select.prototype.setValue = function (value) {
        this.vm.value(value);
    };
    return Select;
}(HtmlComponent_1.HtmlComponent));
exports.Select = Select;
//# sourceMappingURL=Select.js.map