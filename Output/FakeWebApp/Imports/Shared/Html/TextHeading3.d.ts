import { Heading3 } from './Heading3';
import { Heading3ViewModel } from './Heading3ViewModel';
import { TextSpan } from './TextSpan';
export declare class TextHeading3 extends Heading3 {
    constructor(text?: string, vm?: Heading3ViewModel);
    protected readonly vm: Heading3ViewModel;
    readonly textSpan: TextSpan;
    setText(text: string): void;
}
