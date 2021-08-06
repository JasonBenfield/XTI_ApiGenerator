import { AggregateComponent } from "../Html/AggregateComponent";
import { HtmlComponent } from "../Html/HtmlComponent";
export declare class ListGroupItem extends HtmlComponent implements IListItem {
    constructor(vm: IListItemViewModel);
    addToList(list: IList): this;
    readonly content: AggregateComponent;
    addContent<TItem extends IComponent>(item: TItem): TItem;
    protected readonly vm: IListItemViewModel;
    private contextClass;
    makePrimary(): void;
    makeSecondary(): void;
    makeLight(): void;
    makeDark(): void;
    makeDanger(): void;
    private setContextClass;
    private active;
    activate(): void;
    deactivate(): void;
    private setActive;
}
