import { MessageAlertView } from './MessageAlertView';
export declare class MessageAlert {
    private readonly view;
    private _message;
    constructor(view: MessageAlertView);
    get message(): string;
    get hasMessage(): boolean;
    private readonly _messageChanged;
    readonly messageChanged: import("./Events").DefaultEventHandler<string>;
    clear(): void;
    success(message: string): void;
    info(message: string): void;
    infoAction(message: string, a: () => Promise<any>): Promise<void>;
    warning(message: string): void;
    danger(message: string): void;
    private setMessage;
    private debouncedSetMessage;
    private updateVmMessage;
}
