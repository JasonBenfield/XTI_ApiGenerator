import { FormGroupView } from "./FormGroupView";
export declare class FormGroup {
    protected readonly view: FormGroupView;
    private caption;
    constructor(view: FormGroupView);
    getCaption(): string;
    setCaption(caption: string): void;
}
