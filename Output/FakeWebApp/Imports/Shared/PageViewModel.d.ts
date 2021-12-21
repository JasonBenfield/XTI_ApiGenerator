import { ComponentViewModel } from './ComponentViewModel';
import { AggregateComponentViewModel } from './Html/AggregateComponentViewModel';
import { ModalComponentViewModel } from './Modal/ModalComponentViewModel';
export declare class PageViewModel extends ComponentViewModel implements IPageViewModel {
    constructor();
    readonly content: AggregateComponentViewModel;
    readonly modalError: ModalComponentViewModel;
}
