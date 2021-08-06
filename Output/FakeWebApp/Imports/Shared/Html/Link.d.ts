import { HtmlContainerComponent } from "./HtmlContainerComponent";
import { LinkViewModel } from "./LinkViewModel";
export declare class Link extends HtmlContainerComponent {
    constructor(vm?: LinkViewModel);
    protected readonly vm: LinkViewModel;
    readonly clicked: import("../Events").DefaultEventHandler<any>;
    setHref(href: string): void;
    enable(): void;
    disable(): void;
}
