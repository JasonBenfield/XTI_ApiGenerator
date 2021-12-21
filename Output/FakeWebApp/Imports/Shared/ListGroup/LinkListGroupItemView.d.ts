import { LinkListItemViewModel } from "./LinkListItemViewModel";
import { ListGroupItemView } from "./ListGroupItemView";
export declare class LinkListGroupItem extends ListGroupItemView {
    private readonly link;
    constructor(vm: LinkListItemViewModel);
    setHref(href: string): void;
    enable(): void;
    disable(): void;
}
