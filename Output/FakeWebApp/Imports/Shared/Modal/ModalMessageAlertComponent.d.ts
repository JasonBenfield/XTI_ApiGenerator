import { Command } from "../Command/Command";
import { MessageAlert } from "../MessageAlert";
import { ModalMessageAlertComponentView } from "./ModalMessageAlertComponentView";
export declare class ModalMessageAlertComponent {
    private readonly view;
    private readonly _alert;
    private readonly awaitable;
    constructor(view: ModalMessageAlertComponentView);
    private onClosed;
    setBackdrop(backdrop: boolean | 'static'): void;
    alert(action: (a: MessageAlert) => void): Promise<void>;
    readonly okCommand: Command;
    private ok;
}
