"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var tslib_1 = require("tslib");
var ListBlockViewModel_1 = require("../Html/ListBlockViewModel");
var LinkListGroup_1 = require("../ListGroup/LinkListGroup");
var CardLinkListGroup = /** @class */ (function (_super) {
    tslib_1.__extends(CardLinkListGroup, _super);
    function CardLinkListGroup(vm) {
        if (vm === void 0) { vm = new ListBlockViewModel_1.ListBlockViewModel(); }
        var _this = _super.call(this, vm) || this;
        _this.makeFlush();
        return _this;
    }
    return CardLinkListGroup;
}(LinkListGroup_1.LinkListGroup));
exports.CardLinkListGroup = CardLinkListGroup;
//# sourceMappingURL=CardLinkListGroup.js.map