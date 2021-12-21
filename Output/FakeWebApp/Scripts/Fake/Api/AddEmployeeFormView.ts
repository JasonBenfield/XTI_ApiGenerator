// Generated code
import { BaseFormView } from 'XtiShared/Forms/BaseFormView';
import { AddressInputView } from './AddressInputView';

export class AddEmployeeFormView extends BaseFormView {
	readonly EmployeeName = this.addInputFormGroup();
	readonly BirthDate = this.addInputFormGroup();
	readonly Department = this.addDropDownFormGroup<number>();
	readonly Address = this.addFormGroup(new AddressInputView());
	readonly SSN = this.addInputFormGroup();
	readonly HireDate = this.addDropDownFormGroup<Date>();
	readonly IsTemp = this.addDropDownFormGroup<boolean>();
	readonly EmployeeID = this.addInputFormGroup();
}