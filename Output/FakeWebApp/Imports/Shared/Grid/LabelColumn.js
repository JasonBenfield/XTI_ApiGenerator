"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var tslib_1 = require("tslib");
var ColumnCss_1 = require("../ColumnCss");
var Label_1 = require("../Html/Label");
var LabelViewModel_1 = require("../Html/LabelViewModel");
var TextCss_1 = require("../TextCss");
var LabelColumn = /** @class */ (function (_super) {
    tslib_1.__extends(LabelColumn, _super);
    function LabelColumn(vm) {
        if (vm === void 0) { vm = new LabelViewModel_1.LabelViewModel(); }
        var _this = _super.call(this, vm) || this;
        _this.setColumnCss(ColumnCss_1.ColumnCss.xs());
        _this.addCssName('col-form-label');
        return _this;
    }
    LabelColumn.prototype.setColumnCss = function (columnCss) {
        this.replaceCssName(this.columnCss && this.columnCss.toString(), columnCss && columnCss.toString());
    };
    LabelColumn.prototype.truncate = function () {
        this.addCssFrom(new TextCss_1.TextCss().truncate().cssClass());
    };
    return LabelColumn;
}(Label_1.Label));
exports.LabelColumn = LabelColumn;
//# sourceMappingURL=LabelColumn.js.map