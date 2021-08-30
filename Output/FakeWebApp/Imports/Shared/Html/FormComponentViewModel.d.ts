import { HtmlComponentViewModel } from "./HtmlComponentViewModel";
import { AggregateComponentViewModel } from "./AggregateComponentViewModel";
import * as ko from 'knockout';
import { SimpleEvent } from "../Events";
export declare class FormComponentViewModel extends HtmlComponentViewModel {
    readonly content: AggregateComponentViewModel;
    constructor(content?: AggregateComponentViewModel);
    readonly action: ko.Observable<string>;
    readonly method: ko.Observable<string>;
    private readonly _submitted;
    readonly submitted: SimpleEvent;
    private isSubmitOverridden;
    overrideSubmit(): void;
    submit(_: any, event: any): Promise<void>;
}
