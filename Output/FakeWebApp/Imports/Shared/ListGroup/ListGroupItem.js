"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var tslib_1 = require("tslib");
var ContextualClass_1 = require("../ContextualClass");
var AggregateComponent_1 = require("../Html/AggregateComponent");
var HtmlComponent_1 = require("../Html/HtmlComponent");
var ListGroupItem = /** @class */ (function (_super) {
    tslib_1.__extends(ListGroupItem, _super);
    function ListGroupItem(vm) {
        var _this = _super.call(this, vm) || this;
        _this.content = new AggregateComponent_1.AggregateComponent(_this.vm.content);
        _this.contextClass = ContextualClass_1.ContextualClass.default;
        _this.active = '';
        _this.addCssName('list-group-item');
        if (vm.isClickable) {
            _this.addCssName('list-group-item-action');
        }
        return _this;
    }
    ListGroupItem.prototype.addToList = function (list) {
        list.addListItem(this.vm, this);
        return this;
    };
    ListGroupItem.prototype.addContent = function (item) {
        return item.addToContainer(this.content);
    };
    ListGroupItem.prototype.makePrimary = function () {
        this.setContextClass(ContextualClass_1.ContextualClass.primary);
    };
    ListGroupItem.prototype.makeSecondary = function () {
        this.setContextClass(ContextualClass_1.ContextualClass.secondary);
    };
    ListGroupItem.prototype.makeLight = function () {
        this.setContextClass(ContextualClass_1.ContextualClass.light);
    };
    ListGroupItem.prototype.makeDark = function () {
        this.setContextClass(ContextualClass_1.ContextualClass.dark);
    };
    ListGroupItem.prototype.makeDanger = function () {
        this.setContextClass(ContextualClass_1.ContextualClass.danger);
    };
    ListGroupItem.prototype.setContextClass = function (contextClass) {
        if (this.contextClass !== ContextualClass_1.ContextualClass.default) {
            this.removeCssName(this.contextClass.toString());
        }
        this.contextClass = contextClass;
        this.addCssName(this.contextClass.toString());
    };
    ListGroupItem.prototype.activate = function () {
        this.setActive('active');
    };
    ListGroupItem.prototype.deactivate = function () {
        this.setActive('');
    };
    ListGroupItem.prototype.setActive = function (active) {
        if (this.active) {
            this.removeCssName(this.active);
        }
        this.active = active;
        this.addCssName(this.active);
    };
    return ListGroupItem;
}(HtmlComponent_1.HtmlComponent));
exports.ListGroupItem = ListGroupItem;
//# sourceMappingURL=ListGroupItem.js.map