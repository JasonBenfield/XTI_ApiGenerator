import { DateConstraintCollection } from "./ConstraintCollection";
import { InputFieldViewModel } from "./InputFieldViewModel";
import { SimpleField } from "./SimpleField";
import { TextToDateViewValue } from "./TextToDateViewValue";
export declare class DateInputField extends SimpleField {
    static hidden(prefix: string, name: string, vm: InputFieldViewModel, viewValue?: TextToDateViewValue): DateInputField;
    constructor(prefix: string, name: string, vm: InputFieldViewModel, viewValue?: any);
    readonly constraints: DateConstraintCollection;
    setValue(value: Date): void;
    getValue(): Date;
}
