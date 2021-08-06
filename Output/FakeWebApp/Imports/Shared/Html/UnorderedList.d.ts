import { BaseList } from "./BaseList";
import { ListItem } from "./ListItem";
import { ListItemViewModel } from "./ListItemViewModel";
import { UnorderedListViewModel } from "./UnorderedListViewModel";
export declare class UnorderedList extends BaseList {
    constructor(vm?: UnorderedListViewModel);
    protected readonly vm: UnorderedListViewModel;
    protected createItemVM(): ListItemViewModel;
    protected createItem(itemVM: ListItemViewModel): ListItem;
}
