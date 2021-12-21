import { ContextualClass } from './ContextualClass';
import { Block } from './Html/Block';
import { BlockViewModel } from './Html/BlockViewModel';
export declare class MessageAlertView extends Block {
    private readonly alert;
    private readonly textBlock;
    constructor(vm?: BlockViewModel);
    setContext(context: ContextualClass): void;
    setMessage(message: string): void;
}
