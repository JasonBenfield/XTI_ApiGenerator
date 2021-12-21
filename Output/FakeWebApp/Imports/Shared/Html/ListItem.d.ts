import { HtmlContainerComponent } from "./HtmlContainerComponent";
export declare class ListItem extends HtmlContainerComponent implements IListItemView {
    protected readonly vm: IListItemViewModel;
    private data;
    constructor(vm?: IListItemViewModel);
    getData<T>(): T;
    setData(data: any): void;
    addToList(list: IListView): this;
    removeFromList(list: IListView): this;
}
