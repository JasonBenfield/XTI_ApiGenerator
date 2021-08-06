import { HtmlComponent } from "./HtmlComponent";
export declare abstract class BaseList extends HtmlComponent implements IList {
    constructor(vm: IListViewModel);
    protected readonly vm: IListViewModel;
    readonly items: IListItem[];
    clear(): void;
    addItem(): IListItem;
    add<TListItem extends IListItem>(itemVM: IListItemViewModel, create: (vm: IListItemViewModel) => TListItem): TListItem;
    addListItem<TListItem extends IListItem>(itemVM: IListItemViewModel, item: TListItem): TListItem;
    setItems<TSourceItem>(sourceItems: TSourceItem[], config: (sourceItem: TSourceItem, listItem: IListItem) => void): void;
    protected abstract createItemVM(): IListItemViewModel;
    protected abstract createItem(itemVM: IListItemViewModel): IListItem;
}
