import { ButtonCommandItem } from "../Command/ButtonCommandItem";
import { HtmlComponent } from "../Html/HtmlComponent";
import { ModalComponentViewModel } from "./ModalComponentViewModel";
export declare class ModalConfirmComponentView extends HtmlComponent {
    private readonly modal;
    private readonly title;
    private readonly message;
    readonly noButton: ButtonCommandItem;
    readonly yesButton: ButtonCommandItem;
    readonly closed: IEventHandler<any>;
    constructor(vm?: ModalComponentViewModel);
    setTitle(title: string): void;
    showTitle(): void;
    hideTitle(): void;
    setMessage(message: string): void;
    showModal(): void;
    hideModal(): void;
}
