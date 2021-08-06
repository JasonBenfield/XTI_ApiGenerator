"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var CssClass_1 = require("../CssClass");
var HtmlComponent = /** @class */ (function () {
    function HtmlComponent(vm) {
        this.vm = vm;
        this.bgContextCss = '';
        this.textContextCss = '';
        this.margin = null;
        this.padding = null;
        this.css = new CssClass_1.CssClass();
    }
    HtmlComponent.prototype.addToContainer = function (container) {
        return container.addItem(this.vm, this);
    };
    HtmlComponent.prototype.insertIntoContainer = function (container, index) {
        return container.insertItem(index, this.vm, this);
    };
    HtmlComponent.prototype.removeFromContainer = function (container) {
        return container.removeItem(this);
    };
    HtmlComponent.prototype.configure = function (action) {
        action(this);
        return this;
    };
    HtmlComponent.prototype.setID = function (id) {
        this.vm.id(id);
    };
    HtmlComponent.prototype.setName = function (name) {
        this.vm.name(name);
    };
    HtmlComponent.prototype.setBackgroundContext = function (contextClass) {
        var css = contextClass.append('bg');
        this.replaceCssName(this.bgContextCss, css);
        this.bgContextCss = css;
    };
    HtmlComponent.prototype.setTextContext = function (contextClass) {
        var css = contextClass.append('text');
        this.replaceCssName(this.textContextCss, css);
        this.textContextCss = css;
    };
    HtmlComponent.prototype.setMargin = function (margin) {
        if (this.margin) {
            this.css.removeName(this.margin.cssClass.toString());
        }
        this.margin = margin;
        if (this.margin) {
            this.addCssFrom(this.margin.cssClass());
        }
    };
    HtmlComponent.prototype.setPadding = function (padding) {
        if (this.padding) {
            this.css.removeName(this.padding.cssClass.toString());
        }
        this.padding = padding;
        if (this.padding) {
            this.addCssFrom(this.padding.cssClass());
        }
    };
    HtmlComponent.prototype.replaceCss = function (css) {
        this.clearCss();
        return this.addCssFrom(css);
    };
    HtmlComponent.prototype.clearCss = function () {
        this.css.clear();
        this.updateVmCss();
    };
    HtmlComponent.prototype.addCssFrom = function (css) {
        this.css.addFrom(css);
        this.updateVmCss();
    };
    HtmlComponent.prototype.replaceCssName = function (nameToRemove, nameToAdd) {
        this.css.removeName(nameToRemove);
        this.css.addName(nameToAdd);
        this.updateVmCss();
    };
    HtmlComponent.prototype.addCssName = function (name) {
        this.css.addName(name);
        this.updateVmCss();
    };
    HtmlComponent.prototype.removeCssName = function (name) {
        this.css.removeName(name);
        this.updateVmCss();
    };
    HtmlComponent.prototype.updateVmCss = function () {
        this.vm.css(this.css.toString());
    };
    HtmlComponent.prototype.setTitle = function (title) {
        this.vm.title(title);
    };
    HtmlComponent.prototype.show = function () {
        this.vm.isVisible(true);
    };
    HtmlComponent.prototype.hide = function () {
        this.vm.isVisible(false);
    };
    return HtmlComponent;
}());
exports.HtmlComponent = HtmlComponent;
//# sourceMappingURL=HtmlComponent.js.map