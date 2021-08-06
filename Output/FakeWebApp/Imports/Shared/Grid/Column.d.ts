import { ColumnCss } from "../ColumnCss";
import { Block } from "../Html/Block";
import { BlockViewModel } from "../Html/BlockViewModel";
export declare class Column extends Block {
    constructor(vm?: BlockViewModel);
    protected readonly vm: BlockViewModel;
    private columnCss;
    setColumnCss(columnCss: ColumnCss): void;
    truncate(): void;
}
