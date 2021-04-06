"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var tslib_1 = require("tslib");
var UnorderedListViewModel_1 = require("../Html/UnorderedListViewModel");
var ListGroup_1 = require("../ListGroup/ListGroup");
var CardListGroup = /** @class */ (function (_super) {
    tslib_1.__extends(CardListGroup, _super);
    function CardListGroup(vm) {
        if (vm === void 0) { vm = new UnorderedListViewModel_1.UnorderedListViewModel(); }
        var _this = _super.call(this, vm) || this;
        _this.makeFlush();
        return _this;
    }
    return CardListGroup;
}(ListGroup_1.ListGroup));
exports.CardListGroup = CardListGroup;
//# sourceMappingURL=CardListGroup.js.map