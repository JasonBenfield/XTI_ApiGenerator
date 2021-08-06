export declare class AggregateComponent implements IAggregateComponent {
    private readonly vm;
    constructor(vm: IAggregateComponentViewModel);
    private readonly items;
    configure(action: (c: this) => void): this;
    addItemsTo(other: IAggregateComponent): void;
    clear(): void;
    prependItem<TItem extends IComponent, TItemVM extends IComponentViewModel>(itemVM: TItemVM, create: (vm: TItemVM) => TItem): TItem;
    insertItemBefore<TItem extends IComponent>(otherItem: IComponent, item: TItem): TItem;
    insertItemAfter<TItem extends IComponent>(otherItem: IComponent, item: TItem): TItem;
    moveItemBefore(sourceItem: IComponent, targetItem: IComponent): IComponent;
    moveItemAfter(sourceItem: IComponent, targetItem: IComponent): IComponent;
    private indexOf;
    addContent<TItem extends IComponent>(item: TItem): TItem;
    insertContent<TItem extends IComponent>(index: number, item: TItem): TItem;
    addItem<TItemVM extends IComponentViewModel, TItem extends IComponent>(itemVM: TItemVM, item: TItem): TItem;
    insertItem<TItemVM extends IComponentViewModel, TItem extends IComponent>(index: number, itemVM: TItemVM, item: TItem): TItem;
    removeItem<TItem extends IComponent>(item: TItem): void;
    private splice;
    show(): void;
    hide(): void;
}
