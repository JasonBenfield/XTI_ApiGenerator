import { AppApiAction } from "../AppApiAction";
import { BaseFormView } from "./BaseFormView";
import { DropDownFormGroupView } from "./DropDownFormGroupView";
import { FormSaveResult } from "./FormSaveResult";
import { InputFormGroupView } from "./InputFormGroupView";
export declare class BaseForm {
    private readonly name;
    protected readonly view: BaseFormView;
    private readonly formGroups;
    private readonly modalError;
    constructor(name: string, view: BaseFormView);
    private onErrorSelected;
    forEachFormGroup(action: (field: IField) => void): void;
    getName(): string;
    getField(name: string): IField | this;
    protected addHiddenTextFormGroup(name: string, view: InputFormGroupView): import("./TextInputFormGroup").TextInputFormGroup;
    protected addHiddenNumberFormGroup(name: string, view: InputFormGroupView): import("./NumberInputFormGroup").NumberInputFormGroup;
    protected addHiddenDateFormGroup(name: string, view: InputFormGroupView): import("./DateInputFormGroup").DateInputFormGroup;
    protected addTextInputFormGroup(name: string, view: InputFormGroupView): import("./TextInputFormGroup").TextInputFormGroup;
    protected addNumberInputFormGroup(name: string, view: InputFormGroupView): import("./NumberInputFormGroup").NumberInputFormGroup;
    protected addDateInputFormGroup(name: string, view: InputFormGroupView): import("./DateInputFormGroup").DateInputFormGroup;
    protected addTextDropDownFormGroup(name: string, view: DropDownFormGroupView<string>): import("./TextDropDownFormGroup").TextDropDownFormGroup;
    protected addNumberDropDownFormGroup(name: string, view: DropDownFormGroupView<number>): import("./NumberDropDownFormGroup").NumberDropDownFormGroup;
    protected addDateDropDownFormGroup(name: string, view: DropDownFormGroupView<Date>): import("./DateDropDownFormGroup").DateDropDownFormGroup;
    protected addBooleanDropDownFormGroup(name: string, view: DropDownFormGroupView<boolean>): import("./BooleanDropDownFormGroup").BooleanDropDownFormGroup;
    protected addDropDownFormGroup<T>(name: string, view: DropDownFormGroupView<T>): import("./DropDownFormGroup").DropDownFormGroup<T>;
    protected addFormGroup<TField extends IField>(formGroup: TField): TField;
    save<TResult>(action: AppApiAction<any, TResult>): Promise<FormSaveResult<any>>;
    private validate;
    import(values: Record<string, any>): void;
    private export;
}
