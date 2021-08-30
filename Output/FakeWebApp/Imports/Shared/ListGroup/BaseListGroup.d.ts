import { BaseList } from "../Html/BaseList";
import { ListGroupItem } from "./ListGroupItem";
export declare abstract class BaseListGroup extends BaseList {
    constructor(vm: IListViewModel);
    makeFlush(): void;
    protected abstract createItemVM(): IListItemViewModel;
    protected createItem(itemVM: IListItemViewModel): ListGroupItem;
}
