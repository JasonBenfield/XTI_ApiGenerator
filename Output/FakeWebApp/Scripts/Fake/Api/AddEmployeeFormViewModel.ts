// Generated code
import { InputFieldViewModel } from "XtiShared/Forms/InputFieldViewModel";
import { DropDownFieldViewModel } from "XtiShared/Forms/DropDownFieldViewModel";
import { AddressInputViewModel } from './AddressInputViewModel';

export class AddEmployeeFormViewModel {
	readonly Name = new InputFieldViewModel();
	readonly BirthDate = new InputFieldViewModel();
	readonly Department = new DropDownFieldViewModel();
	readonly Address = new AddressInputViewModel();
	readonly SSN = new InputFieldViewModel();
	readonly HireDate = new DropDownFieldViewModel();
	readonly IsTemp = new DropDownFieldViewModel();
}
