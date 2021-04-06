"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var CssClass_1 = require("./CssClass");
var PaddingCssForBreakpoint = /** @class */ (function () {
    function PaddingCssForBreakpoint(breakpoint) {
        this.breakpoint = breakpoint;
        this.amounts = {};
    }
    PaddingCssForBreakpoint.prototype.start = function (amount) {
        if (amount === void 0) { amount = 0; }
        this.amounts.start = amount;
    };
    PaddingCssForBreakpoint.prototype.end = function (amount) {
        if (amount === void 0) { amount = 0; }
        this.amounts.end = amount;
    };
    PaddingCssForBreakpoint.prototype.top = function (amount) {
        if (amount === void 0) { amount = 0; }
        this.amounts.top = amount;
    };
    PaddingCssForBreakpoint.prototype.bottom = function (amount) {
        if (amount === void 0) { amount = 0; }
        this.amounts.bottom = amount;
    };
    PaddingCssForBreakpoint.prototype.all = function (amount) {
        if (amount === void 0) { amount = 0; }
        this.amounts.all = amount;
    };
    PaddingCssForBreakpoint.prototype.cssClass = function () {
        var css = new CssClass_1.CssClass();
        if (this.amounts.start !== undefined) {
            css.addName(this.getCss('s', this.amounts.start));
        }
        if (this.amounts.end !== undefined) {
            css.addName(this.getCss('e', this.amounts.end));
        }
        if (this.amounts.top !== undefined) {
            css.addName(this.getCss('t', this.amounts.top));
        }
        if (this.amounts.bottom !== undefined) {
            css.addName(this.getCss('b', this.amounts.bottom));
        }
        if (this.amounts.all !== undefined) {
            css.addName(this.getCss('', this.amounts.all));
        }
        return css;
    };
    PaddingCssForBreakpoint.prototype.getCss = function (direction, amount) {
        var css = 'p';
        if (direction) {
            css += direction;
        }
        if (amount === null || amount === undefined) {
            amount = 0;
        }
        if (this.breakpoint && this.breakpoint !== 'xs') {
            css += "-" + this.breakpoint;
        }
        css += "-" + amount;
        return css;
    };
    PaddingCssForBreakpoint.prototype.toString = function () {
        return this.cssClass();
    };
    return PaddingCssForBreakpoint;
}());
exports.PaddingCssForBreakpoint = PaddingCssForBreakpoint;
var PaddingCss = /** @class */ (function () {
    function PaddingCss() {
        this.breakpoints = {};
    }
    PaddingCss.xs = function (config) {
        return new PaddingCss().xs(config);
    };
    PaddingCss.sm = function (config) {
        return new PaddingCss().sm(config);
    };
    PaddingCss.md = function (config) {
        return new PaddingCss().md(config);
    };
    PaddingCss.lg = function (config) {
        return new PaddingCss().lg(config);
    };
    PaddingCss.xl = function (config) {
        return new PaddingCss().xl(config);
    };
    PaddingCss.xxl = function (config) {
        return new PaddingCss().xxl(config);
    };
    PaddingCss.prototype.xs = function (config) {
        this.breakpoints.xs = new PaddingCssForBreakpoint('xs');
        config(this.breakpoints.xs);
        return this;
    };
    PaddingCss.prototype.sm = function (config) {
        this.breakpoints.sm = new PaddingCssForBreakpoint('sm');
        config(this.breakpoints.sm);
        return this;
    };
    PaddingCss.prototype.md = function (config) {
        this.breakpoints.sm = new PaddingCssForBreakpoint('sm');
        config(this.breakpoints.sm);
        return this;
    };
    PaddingCss.prototype.lg = function (config) {
        this.breakpoints.sm = new PaddingCssForBreakpoint('sm');
        config(this.breakpoints.sm);
        return this;
    };
    PaddingCss.prototype.xl = function (config) {
        this.breakpoints.sm = new PaddingCssForBreakpoint('sm');
        config(this.breakpoints.sm);
        return this;
    };
    PaddingCss.prototype.xxl = function (config) {
        this.breakpoints.sm = new PaddingCssForBreakpoint('sm');
        config(this.breakpoints.sm);
        return this;
    };
    PaddingCss.prototype.cssClass = function () {
        var css = new CssClass_1.CssClass();
        css.addFrom(this.breakpoints.xs && this.breakpoints.xs.cssClass());
        css.addFrom(this.breakpoints.sm && this.breakpoints.sm.cssClass());
        css.addFrom(this.breakpoints.md && this.breakpoints.md.cssClass());
        css.addFrom(this.breakpoints.lg && this.breakpoints.lg.cssClass());
        css.addFrom(this.breakpoints.xl && this.breakpoints.xl.cssClass());
        css.addFrom(this.breakpoints.xxl && this.breakpoints.xxl.cssClass());
        return css;
    };
    PaddingCss.prototype.toString = function () {
        return this.cssClass.toString();
    };
    return PaddingCss;
}());
exports.PaddingCss = PaddingCss;
//# sourceMappingURL=PaddingCss.js.map