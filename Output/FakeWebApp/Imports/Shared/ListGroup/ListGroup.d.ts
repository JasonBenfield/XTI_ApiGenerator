import { BaseListView } from "./BaseListView";
export declare class ListGroup {
    private readonly view;
    private readonly itemsWithView;
    private readonly _itemClicked;
    readonly itemClicked: import("../Events").DefaultEventHandler<IListItemView>;
    constructor(view: BaseListView);
    private onItemClicked;
    clearItems(): void;
    addItem<TItem>(sourceItem: any, createItem: (sourceItem: any, itemView: IListItemView) => TItem): TItem;
    setItems<TItem>(sourceItems: any[], createItem: (sourceItem: any, itemView: IListItemView) => TItem): TItem[];
}
