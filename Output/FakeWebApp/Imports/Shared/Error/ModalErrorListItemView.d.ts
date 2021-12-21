import { LinkListGroupItem } from "../ListGroup/LinkListGroupItemView";
export declare class ModalErrorListItemView extends LinkListGroupItem {
    private readonly caption;
    private readonly message;
    constructor();
    hideCaption(): void;
    showCaption(): void;
    setCaption(caption: string): void;
    setMessage(message: string): void;
}
