import { ModalConfirmComponentView } from "./ModalConfirmComponentView";
export declare class ModalConfirmComponent {
    private readonly view;
    private readonly awaitable;
    private readonly yesCommand;
    private readonly noCommand;
    constructor(view: ModalConfirmComponentView);
    private onClosed;
    confirm(message: string, title?: string): Promise<boolean>;
    private yes;
    private no;
}
