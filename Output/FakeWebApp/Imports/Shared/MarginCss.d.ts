import { CssClass } from "./CssClass";
export declare type MarginAmount = 0 | 1 | 2 | 3 | 4 | 5 | 'auto';
export declare class MarginCssForBreakpoint {
    private readonly breakpoint;
    private readonly amounts;
    constructor(breakpoint: string);
    start(amount?: MarginAmount): void;
    end(amount?: MarginAmount): void;
    top(amount?: MarginAmount): void;
    bottom(amount?: MarginAmount): void;
    all(amount?: MarginAmount): void;
    cssClass(): CssClass;
    private getCss;
    toString(): CssClass;
}
export declare class MarginCss {
    static xs(config: (margin: MarginCssForBreakpoint) => void): MarginCss;
    static sm(config: (margin: MarginCssForBreakpoint) => void): MarginCss;
    static md(config: (margin: MarginCssForBreakpoint) => void): MarginCss;
    static lg(config: (margin: MarginCssForBreakpoint) => void): MarginCss;
    static xl(config: (margin: MarginCssForBreakpoint) => void): MarginCss;
    static xxl(config: (margin: MarginCssForBreakpoint) => void): MarginCss;
    private breakpoints;
    xs(config: (margin: MarginCssForBreakpoint) => void): this;
    sm(config: (margin: MarginCssForBreakpoint) => void): this;
    md(config: (margin: MarginCssForBreakpoint) => void): this;
    lg(config: (margin: MarginCssForBreakpoint) => void): this;
    xl(config: (margin: MarginCssForBreakpoint) => void): this;
    xxl(config: (margin: MarginCssForBreakpoint) => void): this;
    cssClass(): CssClass;
    toString(): string;
}
