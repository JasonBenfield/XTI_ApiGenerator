import { HtmlComponent } from "./HtmlComponent";
import { TextBlockViewModel } from "./TextBlockViewModel";
export declare class TextBlock extends HtmlComponent {
    constructor(text?: string, vm?: TextBlockViewModel);
    readonly vm: TextBlockViewModel;
    setText(text: string): void;
}
