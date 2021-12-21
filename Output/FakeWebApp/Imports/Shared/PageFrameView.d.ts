import { ModalErrorComponentView } from "./Error/ModalErrorComponentView";
import { Block } from "./Html/Block";
import { TextSmall } from "./Html/TextSmall";
import { TextSpan } from "./Html/TextSpan";
import { Toolbar } from "./Html/Toolbar";
import { PageViewModel } from "./PageViewModel";
export declare class PageFrameView implements IPageFrame {
    private readonly vm;
    readonly toolbar: Toolbar;
    readonly appTitle: TextSpan;
    readonly pageTitle: TextSmall;
    private readonly outerContent;
    readonly content: Block;
    readonly modalError: ModalErrorComponentView;
    private readonly logoutMenuItem;
    constructor(vm?: PageViewModel);
    setName(name: string): void;
    addItem<TItemVM extends IComponentViewModel, TItem extends IComponent>(itemVM: TItemVM, item: TItem): TItem;
    insertItem<TItemVM extends IComponentViewModel, TItem extends IComponent>(index: number, itemVM: TItemVM, item: TItem): TItem;
    removeItem<TItem extends IComponent>(item: TItem): any;
    show(): void;
    hide(): void;
    insertContent<TItem extends IComponent>(index: number, item: TItem): TItem;
    addContent<TItem extends IComponent>(item: TItem): TItem;
    load(): void;
}
