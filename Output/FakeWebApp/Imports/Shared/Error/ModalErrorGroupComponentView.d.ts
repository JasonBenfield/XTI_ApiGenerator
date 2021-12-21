import { Block } from '../Html/Block';
import { ListGroupView } from '../ListGroup/ListGroupView';
export declare class ModalErrorGroupComponentView extends Block {
    private readonly hr;
    private readonly caption;
    readonly errors: ListGroupView;
    constructor();
    showHR(): void;
    hideHR(): void;
    setCaption(caption: string): void;
}
