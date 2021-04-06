"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var tslib_1 = require("tslib");
var ListBlockViewModel_1 = require("../Html/ListBlockViewModel");
var BaseListGroup_1 = require("./BaseListGroup");
var ButtonListItemViewModel_1 = require("./ButtonListItemViewModel");
var ButtonListGroup = /** @class */ (function (_super) {
    tslib_1.__extends(ButtonListGroup, _super);
    function ButtonListGroup(vm) {
        if (vm === void 0) { vm = new ListBlockViewModel_1.ListBlockViewModel(); }
        return _super.call(this, vm) || this;
    }
    ButtonListGroup.prototype.createItemVM = function () {
        return new ButtonListItemViewModel_1.ButtonListItemViewModel();
    };
    return ButtonListGroup;
}(BaseListGroup_1.BaseListGroup));
exports.ButtonListGroup = ButtonListGroup;
//# sourceMappingURL=ButtonListGroup.js.map