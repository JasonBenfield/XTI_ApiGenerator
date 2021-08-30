"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var tslib_1 = require("tslib");
var ListItemViewModel_1 = require("../Html/ListItemViewModel");
var UnorderedListViewModel_1 = require("../Html/UnorderedListViewModel");
var BaseListGroup_1 = require("./BaseListGroup");
var ListGroup = /** @class */ (function (_super) {
    tslib_1.__extends(ListGroup, _super);
    function ListGroup(vm) {
        if (vm === void 0) { vm = new UnorderedListViewModel_1.UnorderedListViewModel(); }
        return _super.call(this, vm) || this;
    }
    ListGroup.prototype.createItemVM = function () {
        return new ListItemViewModel_1.ListItemViewModel();
    };
    return ListGroup;
}(BaseListGroup_1.BaseListGroup));
exports.ListGroup = ListGroup;
//# sourceMappingURL=ListGroup.js.map