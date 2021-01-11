// Generated code
import * as ko from 'knockout';
import { InputFieldViewModel } from "XtiShared/Forms/InputFieldViewModel";
import { DropDownFieldViewModel } from "XtiShared/Forms/DropDownFieldViewModel";
import { AddressInputViewModel } from './AddressInputViewModel';

export class AddEmployeeFormViewModel {
	readonly componentName = ko.observable('AddEmployeeForm');
	readonly EmployeeName = new InputFieldViewModel();
	readonly BirthDate = new InputFieldViewModel();
	readonly Department = new DropDownFieldViewModel();
	readonly Address = new AddressInputViewModel();
	readonly SSN = new InputFieldViewModel();
	readonly HireDate = new DropDownFieldViewModel();
	readonly IsTemp = new DropDownFieldViewModel();
}
