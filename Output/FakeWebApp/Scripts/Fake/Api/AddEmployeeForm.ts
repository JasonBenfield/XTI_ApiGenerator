// Generated code
import * as xti from "@jasonbenfield/sharedwebapp/Common";
import { BaseForm } from '@jasonbenfield/sharedwebapp/Forms/BaseForm';
import { AddEmployeeFormView } from './AddEmployeeFormView';
import { DropDownFieldItem } from "@jasonbenfield/sharedwebapp/Forms/DropDownFieldItem";
import { AddressInput } from './AddressInput';

export class AddEmployeeForm extends BaseForm {
	protected readonly view: AddEmployeeFormView;
	
	constructor(view: AddEmployeeFormView) {
		super('AddEmployeeForm', view);
		this.EmployeeName.setCaption('Employee Name');
		this.EmployeeName.setMaxLength(100);
		this.BirthDate.setCaption('Birth Date');
		this.Department.setCaption('Department');
		this.Department.constraints.mustNotBeNull();
		this.Department.setItems(
			'Select...',
			[
				new DropDownFieldItem(1, 'HR'),
				new DropDownFieldItem(2, 'IT')
			]
		);
		this.Address.setCaption('Address');
		this.SSN.setCaption('SSN');
		this.SSN.constraints.mustBeAbove(0, 'Must be greater than 0');
		this.SSN.constraints.mustBeBelow(1000000000, 'Must be less than 1000000000');
		this.SSN.protect();
		this.HireDate.setCaption('Hire Date');
		this.HireDate.setItems(
			'',
			[
				new DropDownFieldItem(new xti.DateOnly(2024, xti.Month.fromValue(11), 12), 'Yesterday'),
				new DropDownFieldItem(new xti.DateOnly(2024, xti.Month.fromValue(11), 13), 'Today'),
				new DropDownFieldItem(new xti.DateOnly(2024, xti.Month.fromValue(11), 14), 'Tomorrow')
			]
		);
		this.IsTemp.setCaption('Is Temp');
		this.IsTemp.setItems(
			'',
			[
				new DropDownFieldItem(true, 'Yes'),
				new DropDownFieldItem(false, 'No')
			]
		);
	}
	readonly EmployeeName = this.addTextInputFormGroup('EmployeeName', this.view.EmployeeName);
	readonly BirthDate = this.addDateInputFormGroup('BirthDate', this.view.BirthDate);
	readonly Department = this.addNumberDropDownFormGroup('Department', this.view.Department);
	readonly Address = this.addFormGroup(new AddressInput(this.getName(), 'Address', this.view.Address));
	readonly SSN = this.addNumberInputFormGroup('SSN', this.view.SSN);
	readonly HireDate = this.addDateDropDownFormGroup('HireDate', this.view.HireDate);
	readonly IsTemp = this.addBooleanDropDownFormGroup('IsTemp', this.view.IsTemp);
	readonly EmployeeID = this.addHiddenNumber('EmployeeID', this.view.EmployeeID);
}