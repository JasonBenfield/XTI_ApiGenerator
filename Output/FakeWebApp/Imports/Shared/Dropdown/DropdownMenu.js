"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var tslib_1 = require("tslib");
var UnorderedList_1 = require("../Html/UnorderedList");
var UnorderedListViewModel_1 = require("../Html/UnorderedListViewModel");
var DropdownMenu = /** @class */ (function (_super) {
    tslib_1.__extends(DropdownMenu, _super);
    function DropdownMenu(vm) {
        if (vm === void 0) { vm = new UnorderedListViewModel_1.UnorderedListViewModel(); }
        var _this = _super.call(this, vm) || this;
        _this.addCssName('dropdown-menu dropdown-menu-right');
        return _this;
    }
    return DropdownMenu;
}(UnorderedList_1.UnorderedList));
exports.DropdownMenu = DropdownMenu;
//# sourceMappingURL=DropdownMenu.js.map