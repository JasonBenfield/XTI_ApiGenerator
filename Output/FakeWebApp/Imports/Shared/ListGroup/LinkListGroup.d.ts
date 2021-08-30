import { ListBlockViewModel } from "../Html/ListBlockViewModel";
import { BaseListGroup } from "./BaseListGroup";
import { LinkListItemViewModel } from "./LinkListItemViewModel";
export declare class LinkListGroup extends BaseListGroup {
    constructor(vm?: ListBlockViewModel);
    protected createItemVM(): LinkListItemViewModel;
}
