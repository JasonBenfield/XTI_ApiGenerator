import { ListBlockViewModel } from "../Html/ListBlockViewModel";
import { BaseListGroup } from "./BaseListGroup";
import { ButtonListItemViewModel } from "./ButtonListItemViewModel";
export declare class ButtonListGroup extends BaseListGroup {
    constructor(vm?: ListBlockViewModel);
    protected createItemVM(): ButtonListItemViewModel;
}
