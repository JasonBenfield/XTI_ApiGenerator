"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var tslib_1 = require("tslib");
var BaseList_1 = require("../Html/BaseList");
var ListGroupItem_1 = require("./ListGroupItem");
var BaseListGroup = /** @class */ (function (_super) {
    tslib_1.__extends(BaseListGroup, _super);
    function BaseListGroup(vm) {
        var _this = _super.call(this, vm) || this;
        _this.addCssName('list-group');
        return _this;
    }
    BaseListGroup.prototype.makeFlush = function () {
        this.addCssName('list-group-flush');
    };
    BaseListGroup.prototype.createItem = function (itemVM) {
        return new ListGroupItem_1.ListGroupItem(itemVM);
    };
    return BaseListGroup;
}(BaseList_1.BaseList));
exports.BaseListGroup = BaseListGroup;
//# sourceMappingURL=BaseListGroup.js.map