import { Column } from "../Grid/Column";
import { LabelColumn } from "../Grid/LabelColumn";
import { Block } from "../Html/Block";
import { BlockViewModel } from "../Html/BlockViewModel";
import { InputGroup } from "../Html/InputGroup";
export declare class FormGroupView extends Block implements IFormGroup {
    readonly captionColumn: LabelColumn;
    private readonly labelTextSpan;
    readonly inputGroup: InputGroup;
    constructor(vm?: BlockViewModel);
    setCaption(caption: string): void;
    readonly valueColumn: Column;
}
