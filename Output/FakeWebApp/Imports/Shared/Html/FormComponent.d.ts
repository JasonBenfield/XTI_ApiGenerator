import { ButtonCommandItem } from "../Command/ButtonCommandItem";
import { AggregateComponent } from "./AggregateComponent";
import { FormComponentViewModel } from "./FormComponentViewModel";
import { HtmlContainerComponent } from "./HtmlContainerComponent";
export declare class FormComponent extends HtmlContainerComponent {
    constructor(vm?: FormComponentViewModel);
    readonly vm: FormComponentViewModel;
    readonly submitted: import("../Events").SimpleEvent;
    overrideSubmit(): void;
    readonly content: AggregateComponent;
    setAction(action: string): void;
    setMethod(method: string): void;
    addOffscreenSubmit(): ButtonCommandItem;
}
