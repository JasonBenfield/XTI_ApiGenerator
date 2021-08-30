"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var tslib_1 = require("tslib");
var ListBlockViewModel_1 = require("../Html/ListBlockViewModel");
var BaseListGroup_1 = require("./BaseListGroup");
var LinkListItemViewModel_1 = require("./LinkListItemViewModel");
var LinkListGroup = /** @class */ (function (_super) {
    tslib_1.__extends(LinkListGroup, _super);
    function LinkListGroup(vm) {
        if (vm === void 0) { vm = new ListBlockViewModel_1.ListBlockViewModel(); }
        return _super.call(this, vm) || this;
    }
    LinkListGroup.prototype.createItemVM = function () {
        return new LinkListItemViewModel_1.LinkListItemViewModel();
    };
    return LinkListGroup;
}(BaseListGroup_1.BaseListGroup));
exports.LinkListGroup = LinkListGroup;
//# sourceMappingURL=LinkListGroup.js.map