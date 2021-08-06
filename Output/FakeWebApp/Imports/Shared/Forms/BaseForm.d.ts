import { AppApiAction } from "../AppApiAction";
import { FormSaveResult } from "./FormSaveResult";
import { FormComponentViewModel } from "../Html/FormComponentViewModel";
import { FormComponent } from "../Html/FormComponent";
import { ComplexFieldLayout } from "./ComplexFieldLayout";
export declare class BaseForm extends FormComponent {
    private readonly name;
    constructor(name: string, vm?: FormComponentViewModel);
    private onErrorSelected;
    private layout;
    useLayout(layout: ComplexFieldLayout): void;
    executeLayout(): void;
    private readonly formGroups;
    private readonly modalError;
    forEachFormGroup(action: (field: IFormGroupField) => void): void;
    getName(): string;
    getField(name: string): IField | this;
    protected addHiddenTextFormGroup(name: string): import("./TextInputFormGroup").TextInputFormGroup;
    protected addHiddenNumberFormGroup(name: string): import("./NumberInputFormGroup").NumberInputFormGroup;
    protected addHiddenDateFormGroup(name: string): import("./DateInputFormGroup").DateInputFormGroup;
    protected addTextInputFormGroup(name: string): import("./TextInputFormGroup").TextInputFormGroup;
    protected addNumberInputFormGroup(name: string): import("./NumberInputFormGroup").NumberInputFormGroup;
    protected addDateInputFormGroup(name: string): import("./DateInputFormGroup").DateInputFormGroup;
    protected addTextDropDownFormGroup(name: string): import("./TextDropDownFormGroup").TextDropDownFormGroup;
    protected addNumberDropDownFormGroup(name: string): import("./NumberDropDownFormGroup").NumberDropDownFormGroup;
    protected addDateDropDownFormGroup(name: string): import("./DateDropDownFormGroup").DateDropDownFormGroup;
    protected addBooleanDropDownFormGroup(name: string): import("./BooleanDropDownFormGroup").BooleanDropDownFormGroup;
    protected addDropDownFormGroup<T>(name: string): import("./DropDownFormGroup").DropDownFormGroup<T>;
    protected addFormGroup<TField extends IFormGroupField>(formGroup: TField): TField;
    save<TResult>(action: AppApiAction<any, TResult>): Promise<FormSaveResult<any>>;
    private validate;
    import(values: Record<string, any>): void;
    private export;
}
