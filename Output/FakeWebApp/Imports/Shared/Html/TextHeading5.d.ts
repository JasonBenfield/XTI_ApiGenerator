import { Heading5 } from './Heading5';
import { Heading5ViewModel } from './Heading5ViewModel';
import { TextSpan } from './TextSpan';
export declare class TextHeading5 extends Heading5 {
    constructor(text?: string, vm?: Heading5ViewModel);
    protected readonly vm: Heading5ViewModel;
    readonly textSpan: TextSpan;
    setText(text: string): void;
}
