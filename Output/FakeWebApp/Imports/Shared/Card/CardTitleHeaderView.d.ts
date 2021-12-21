import { CardHeader } from './CardHeader';
import { BlockViewModel } from '../Html/BlockViewModel';
export declare class CardTitleHeaderView extends CardHeader {
    private readonly textBlock;
    constructor(vm?: BlockViewModel);
    setText(text: string): void;
}
