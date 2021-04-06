import { HtmlContainerComponent } from "./HtmlContainerComponent";
export declare class ListItem extends HtmlContainerComponent implements IListItem {
    constructor(vm?: IListItemViewModel);
    protected readonly vm: IListItemViewModel;
    addToList(list: IList): this;
}
