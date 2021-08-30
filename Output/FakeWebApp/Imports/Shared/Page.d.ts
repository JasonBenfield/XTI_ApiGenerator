import { PageViewModel } from "./PageViewModel";
import { ModalErrorComponent } from "./Error/ModalErrorComponent";
import { Block } from "./Html/Block";
import { AppApi } from "./AppApi";
export declare class Page implements IPage {
    protected readonly vm: PageViewModel;
    constructor(vm?: PageViewModel);
    private readonly logoutMenuItem;
    setLogoutUrl(url: string): void;
    addItem<TItemVM extends IComponentViewModel, TItem extends IComponent>(itemVM: TItemVM, item: TItem): TItem;
    insertItem<TItemVM extends IComponentViewModel, TItem extends IComponent>(index: number, itemVM: TItemVM, item: TItem): TItem;
    removeItem<TItem extends IComponent>(item: TItem): any;
    show(): void;
    hide(): void;
    private readonly outerContent;
    readonly content: Block;
    readonly modalError: ModalErrorComponent;
    insertContent<TItem extends IComponent>(index: number, item: TItem): TItem;
    addContent<TItem extends IComponent>(item: TItem): TItem;
    load(): void;
    protected api<TApi extends AppApi>(apiCtor: constructor<TApi>): TApi;
    protected createApi<TApi extends AppApi>(apiCtor: constructor<TApi>): TApi;
}
