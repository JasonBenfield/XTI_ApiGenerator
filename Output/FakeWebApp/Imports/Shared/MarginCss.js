"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var CssClass_1 = require("./CssClass");
var MarginCssForBreakpoint = /** @class */ (function () {
    function MarginCssForBreakpoint(breakpoint) {
        this.breakpoint = breakpoint;
        this.amounts = {};
    }
    MarginCssForBreakpoint.prototype.start = function (amount) {
        if (amount === void 0) { amount = 0; }
        this.amounts.start = amount;
    };
    MarginCssForBreakpoint.prototype.end = function (amount) {
        if (amount === void 0) { amount = 0; }
        this.amounts.end = amount;
    };
    MarginCssForBreakpoint.prototype.top = function (amount) {
        if (amount === void 0) { amount = 0; }
        this.amounts.top = amount;
    };
    MarginCssForBreakpoint.prototype.bottom = function (amount) {
        if (amount === void 0) { amount = 0; }
        this.amounts.bottom = amount;
    };
    MarginCssForBreakpoint.prototype.all = function (amount) {
        if (amount === void 0) { amount = 0; }
        this.amounts.all = amount;
    };
    MarginCssForBreakpoint.prototype.cssClass = function () {
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
    MarginCssForBreakpoint.prototype.getCss = function (direction, amount) {
        var css = 'm';
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
    MarginCssForBreakpoint.prototype.toString = function () {
        return this.cssClass();
    };
    return MarginCssForBreakpoint;
}());
exports.MarginCssForBreakpoint = MarginCssForBreakpoint;
var MarginCss = /** @class */ (function () {
    function MarginCss() {
        this.breakpoints = {};
    }
    MarginCss.xs = function (config) {
        return new MarginCss().xs(config);
    };
    MarginCss.sm = function (config) {
        return new MarginCss().sm(config);
    };
    MarginCss.md = function (config) {
        return new MarginCss().md(config);
    };
    MarginCss.lg = function (config) {
        return new MarginCss().lg(config);
    };
    MarginCss.xl = function (config) {
        return new MarginCss().xl(config);
    };
    MarginCss.xxl = function (config) {
        return new MarginCss().xxl(config);
    };
    MarginCss.prototype.xs = function (config) {
        this.breakpoints.xs = new MarginCssForBreakpoint('xs');
        config(this.breakpoints.xs);
        return this;
    };
    MarginCss.prototype.sm = function (config) {
        this.breakpoints.sm = new MarginCssForBreakpoint('sm');
        config(this.breakpoints.sm);
        return this;
    };
    MarginCss.prototype.md = function (config) {
        this.breakpoints.sm = new MarginCssForBreakpoint('sm');
        config(this.breakpoints.sm);
        return this;
    };
    MarginCss.prototype.lg = function (config) {
        this.breakpoints.sm = new MarginCssForBreakpoint('sm');
        config(this.breakpoints.sm);
        return this;
    };
    MarginCss.prototype.xl = function (config) {
        this.breakpoints.sm = new MarginCssForBreakpoint('sm');
        config(this.breakpoints.sm);
        return this;
    };
    MarginCss.prototype.xxl = function (config) {
        this.breakpoints.sm = new MarginCssForBreakpoint('sm');
        config(this.breakpoints.sm);
        return this;
    };
    MarginCss.prototype.cssClass = function () {
        var css = new CssClass_1.CssClass();
        css.addFrom(this.breakpoints.xs && this.breakpoints.xs.cssClass());
        css.addFrom(this.breakpoints.sm && this.breakpoints.sm.cssClass());
        css.addFrom(this.breakpoints.md && this.breakpoints.md.cssClass());
        css.addFrom(this.breakpoints.lg && this.breakpoints.lg.cssClass());
        css.addFrom(this.breakpoints.xl && this.breakpoints.xl.cssClass());
        css.addFrom(this.breakpoints.xxl && this.breakpoints.xxl.cssClass());
        return css;
    };
    MarginCss.prototype.toString = function () {
        return this.cssClass.toString();
    };
    return MarginCss;
}());
exports.MarginCss = MarginCss;
//# sourceMappingURL=MarginCss.js.map