import { Column } from "../Grid/Column";
import { Block } from "./Block";
import { BlockViewModel } from "./BlockViewModel";
export declare class Toolbar extends Block {
    constructor(vm?: BlockViewModel);
    readonly columnStart: Column;
    readonly columnEnd: Column;
}
