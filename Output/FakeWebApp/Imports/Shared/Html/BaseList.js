"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var tslib_1 = require("tslib");
var Enumerable_1 = require("../Enumerable");
var HtmlComponent_1 = require("./HtmlComponent");
var BaseList = /** @class */ (function (_super) {
    tslib_1.__extends(BaseList, _super);
    function BaseList(vm) {
        var _this = _super.call(this, vm) || this;
        _this.items = [];
        return _this;
    }
    BaseList.prototype.clear = function () {
        this.items.splice(0, this.items.length);
        this.vm.items([]);
        this.vm.hasItems(false);
    };
    BaseList.prototype.addItem = function () {
        var itemVM = this.createItemVM();
        return this.add(itemVM, this.createItem);
    };
    BaseList.prototype.add = function (itemVM, create) {
        return this.addListItem(itemVM, create(itemVM));
    };
    BaseList.prototype.addListItem = function (itemVM, item) {
        this.vm.items.push(itemVM);
        this.vm.hasItems(true);
        this.items.push(item);
        return item;
    };
    BaseList.prototype.setItems = function (sourceItems, config) {
        var _a;
        var _this = this;
        var itemVMs = [];
        var items = new Enumerable_1.MappedArray(sourceItems, function (sourceItem) {
            var itemVM = _this.createItemVM();
            itemVMs.push(itemVM);
            var item = _this.createItem(itemVM);
            config(sourceItem, item);
            return item;
        }).value();
        (_a = this.items).splice.apply(_a, tslib_1.__spreadArrays([0, this.items.length], items));
        this.vm.items(itemVMs);
        this.vm.hasItems(itemVMs.length > 0);
    };
    return BaseList;
}(HtmlComponent_1.HtmlComponent));
exports.BaseList = BaseList;
//# sourceMappingURL=BaseList.js.map