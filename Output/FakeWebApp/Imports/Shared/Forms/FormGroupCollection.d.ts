import { BooleanDropDownFormGroup } from "./BooleanDropDownFormGroup";
import { DateDropDownFormGroup } from "./DateDropDownFormGroup";
import { DateInputFormGroup } from "./DateInputFormGroup";
import { DropDownFormGroup } from "./DropDownFormGroup";
import { NumberDropDownFormGroup } from "./NumberDropDownFormGroup";
import { NumberInputFormGroup } from "./NumberInputFormGroup";
import { TextDropDownFormGroup } from "./TextDropDownFormGroup";
import { TextInputFormGroup } from "./TextInputFormGroup";
export declare class FormGroupCollection {
    private readonly name;
    constructor(name: string);
    readonly values: IFormGroupField[];
    formGroups(): IFormGroupField[];
    addHiddenTextFormGroup(name: string): TextInputFormGroup;
    addHiddenNumberFormGroup(name: string): NumberInputFormGroup;
    addHiddenDateFormGroup(name: string): DateInputFormGroup;
    private hideFormGroup;
    addTextInputFormGroup(name: string): TextInputFormGroup;
    addNumberInputFormGroup(name: string): NumberInputFormGroup;
    addDateInputFormGroup(name: string): DateInputFormGroup;
    addTextDropDownFormGroup(name: string): TextDropDownFormGroup;
    addNumberDropDownFormGroup(name: string): NumberDropDownFormGroup;
    addDateDropDownFormGroup(name: string): DateDropDownFormGroup;
    addBooleanDropDownFormGroup(name: string): BooleanDropDownFormGroup;
    addDropDownFormGroup<T>(name: string): DropDownFormGroup<T>;
    addFormGroup<TFormGroup extends IFormGroupField>(formGroup: TFormGroup): TFormGroup;
    executeLayout(): void;
    forEach(action: (field: IFormGroupField) => void): void;
    getField(name: string): IField;
    clearErrors(): void;
    validate(errors: IErrorList): void;
    import(values: Record<string, any>): void;
    export(values: Record<string, any>): void;
}
