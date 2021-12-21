import { Heading6 } from './Heading6';
import { Heading6ViewModel } from './Heading6ViewModel';
import { TextSpan } from './TextSpan';
export declare class TextHeading6 extends Heading6 {
    constructor(text?: string, vm?: Heading6ViewModel);
    protected readonly vm: Heading6ViewModel;
    readonly textSpan: TextSpan;
    setText(text: string): void;
}
