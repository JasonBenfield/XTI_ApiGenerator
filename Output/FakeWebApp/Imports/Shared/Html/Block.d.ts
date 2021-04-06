import { BlockViewModel } from './BlockViewModel';
import { HtmlContainerComponent } from './HtmlContainerComponent';
export declare class Block extends HtmlContainerComponent {
    constructor(vm?: BlockViewModel);
    protected readonly vm: BlockViewModel;
    flexFill(): void;
    positionRelative(): void;
    positionAbsoluteFill(): void;
    scrollable(): void;
}
