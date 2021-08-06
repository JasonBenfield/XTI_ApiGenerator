"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var tslib_1 = require("tslib");
var AggregateComponent_1 = require("./AggregateComponent");
var HtmlContainerComponent_1 = require("./HtmlContainerComponent");
var LinkViewModel_1 = require("./LinkViewModel");
var Link = /** @class */ (function (_super) {
    tslib_1.__extends(Link, _super);
    function Link(vm) {
        if (vm === void 0) { vm = new LinkViewModel_1.LinkViewModel(); }
        var _this = _super.call(this, vm, new AggregateComponent_1.AggregateComponent(vm.content)) || this;
        _this.clicked = _this.vm.clicked;
        _this.setHref('javascript:;');
        return _this;
    }
    Link.prototype.setHref = function (href) {
        this.vm.href(href);
    };
    Link.prototype.enable = function () {
        this.vm.isEnabled(true);
    };
    Link.prototype.disable = function () {
        this.vm.isEnabled(false);
    };
    return Link;
}(HtmlContainerComponent_1.HtmlContainerComponent));
exports.Link = Link;
//# sourceMappingURL=Link.js.map