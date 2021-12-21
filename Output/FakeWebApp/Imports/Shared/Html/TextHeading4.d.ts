import { Heading4 } from './Heading4';
import { Heading4ViewModel } from './Heading4ViewModel';
import { TextSpan } from './TextSpan';
export declare class TextHeading4 extends Heading4 {
    constructor(text?: string, vm?: Heading4ViewModel);
    protected readonly vm: Heading4ViewModel;
    readonly textSpan: TextSpan;
    setText(text: string): void;
}
