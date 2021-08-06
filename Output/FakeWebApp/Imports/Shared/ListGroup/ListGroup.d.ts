import { ListItemViewModel } from "../Html/ListItemViewModel";
import { UnorderedListViewModel } from "../Html/UnorderedListViewModel";
import { BaseListGroup } from "./BaseListGroup";
export declare class ListGroup extends BaseListGroup {
    constructor(vm?: UnorderedListViewModel);
    protected createItemVM(): ListItemViewModel;
}
