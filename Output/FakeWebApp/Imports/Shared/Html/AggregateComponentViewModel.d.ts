import { ComponentViewModel } from "../ComponentViewModel";
import * as ko from 'knockout';
export declare class AggregateComponentViewModel extends ComponentViewModel implements IAggregateComponentViewModel {
    constructor();
    readonly items: ko.ObservableArray<IComponentViewModel>;
    readonly isVisible: ko.Observable<boolean>;
}
