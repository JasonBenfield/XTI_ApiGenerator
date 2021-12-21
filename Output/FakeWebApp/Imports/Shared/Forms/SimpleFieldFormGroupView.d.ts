import { BlockViewModel } from "../Html/BlockViewModel";
import { FormGroupView } from "../Html/FormGroupView";
import { ListGroupView } from "../ListGroup/ListGroupView";
export declare class SimpleFieldFormGroupView extends FormGroupView {
    readonly alertList: ListGroupView;
    private readonly dropdown;
    constructor(vm?: BlockViewModel);
    showDropDown(): void;
    hideDropDown(): void;
}
