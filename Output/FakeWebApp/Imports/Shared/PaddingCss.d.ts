import { CssClass } from "./CssClass";
export declare type PaddingAmount = 0 | 1 | 2 | 3 | 4 | 5;
export declare type PaddingDirection = '' | 'b' | 't' | 'e' | 's';
export declare class PaddingCssForBreakpoint {
    private readonly breakpoint;
    private readonly amounts;
    constructor(breakpoint: string);
    start(amount?: PaddingAmount): void;
    end(amount?: PaddingAmount): void;
    top(amount?: PaddingAmount): void;
    bottom(amount?: PaddingAmount): void;
    all(amount?: PaddingAmount): void;
    cssClass(): CssClass;
    private getCss;
    toString(): CssClass;
}
export declare class PaddingCss {
    static xs(config: (margin: PaddingCssForBreakpoint) => void): PaddingCss;
    static sm(config: (margin: PaddingCssForBreakpoint) => void): PaddingCss;
    static md(config: (margin: PaddingCssForBreakpoint) => void): PaddingCss;
    static lg(config: (margin: PaddingCssForBreakpoint) => void): PaddingCss;
    static xl(config: (margin: PaddingCssForBreakpoint) => void): PaddingCss;
    static xxl(config: (margin: PaddingCssForBreakpoint) => void): PaddingCss;
    private breakpoints;
    xs(config: (margin: PaddingCssForBreakpoint) => void): this;
    sm(config: (margin: PaddingCssForBreakpoint) => void): this;
    md(config: (margin: PaddingCssForBreakpoint) => void): this;
    lg(config: (margin: PaddingCssForBreakpoint) => void): this;
    xl(config: (margin: PaddingCssForBreakpoint) => void): this;
    xxl(config: (margin: PaddingCssForBreakpoint) => void): this;
    cssClass(): CssClass;
    toString(): string;
}
