import { Heading2 } from './Heading2';
import { Heading2ViewModel } from './Heading2ViewModel';
import { TextSpan } from './TextSpan';
export declare class TextHeading2 extends Heading2 {
    constructor(text?: string, vm?: Heading2ViewModel);
    protected readonly vm: Heading2ViewModel;
    readonly textSpan: TextSpan;
    setText(text: string): void;
}
