import { ContextualClass } from "../ContextualClass";
import { AggregateComponent } from "../Html/AggregateComponent";
import { HtmlComponent } from "../Html/HtmlComponent";
export declare class ListGroupItemView extends HtmlComponent implements IListItemView {
    private data;
    readonly content: AggregateComponent;
    protected readonly vm: IListItemViewModel;
    private contextClass;
    private active;
    constructor(vm?: IListItemViewModel);
    getData<T>(): T;
    setData(data: any): void;
    addToList(list: IListView): this;
    removeFromList(list: IListView): this;
    addContent<TItem extends IComponent>(item: TItem): TItem;
    setContext(contextClass: ContextualClass): void;
    private getCss;
    activate(): void;
    deactivate(): void;
    private setActive;
}
