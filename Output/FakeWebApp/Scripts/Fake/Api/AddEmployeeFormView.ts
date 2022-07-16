// Generated code
import { BaseFormView } from '@jasonbenfield/sharedwebapp/Views/BaseFormView';
import { SimpleFieldFormGroupInputView, SimpleFieldFormGroupSelectView } from '@jasonbenfield/sharedwebapp/Views/FormGroup';
import { IFormGroupLayout } from '@jasonbenfield/sharedwebapp/Views/Types';
import { BasicComponentView } from '@jasonbenfield/sharedwebapp/Views/BasicComponentView';
import { AddressInputView } from './AddressInputView';

export interface IAddEmployeeFormView {
	EmployeeName: SimpleFieldFormGroupInputView;
	BirthDate: SimpleFieldFormGroupInputView;
	Department: SimpleFieldFormGroupSelectView;
	Address: AddressInputView;
	SSN: SimpleFieldFormGroupInputView;
	HireDate: SimpleFieldFormGroupSelectView;
	IsTemp: SimpleFieldFormGroupSelectView;
	EmployeeID: SimpleFieldFormGroupInputView;
}

export class DefaultAddEmployeeFormViewLayout implements IFormGroupLayout<IAddEmployeeFormView> {
	addFormGroups(form: AddEmployeeFormView) {
		return {
			EmployeeName: form.addInputFormGroup(),
			BirthDate: form.addInputFormGroup(),
			Department: form.addDropDownFormGroup(),
			Address: form.addFormGroup(AddressInputView),
			SSN: form.addInputFormGroup(),
			HireDate: form.addDropDownFormGroup(),
			IsTemp: form.addDropDownFormGroup(),
			EmployeeID: form.addInputFormGroup()
		}
	}
}

export class AddEmployeeFormView extends BaseFormView {
	private formGroups: IAddEmployeeFormView;
	
	constructor(container: BasicComponentView) {
		super(container);
	}
	
	addContent(layout?: IFormGroupLayout<IAddEmployeeFormView>) {
		if (!layout) {
			layout = new DefaultAddEmployeeFormViewLayout();
		}
		this.formGroups = layout.addFormGroups(this);
	}
	
	get EmployeeName() { return this.formGroups.EmployeeName; }
	get BirthDate() { return this.formGroups.BirthDate; }
	get Department() { return this.formGroups.Department; }
	get Address() { return this.formGroups.Address; }
	get SSN() { return this.formGroups.SSN; }
	get HireDate() { return this.formGroups.HireDate; }
	get IsTemp() { return this.formGroups.IsTemp; }
	get EmployeeID() { return this.formGroups.EmployeeID; }
}