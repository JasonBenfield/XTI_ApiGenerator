// Generated code
import { BaseForm } from 'XtiShared/Forms/BaseForm';
import { FormComponentViewModel } from 'XtiShared/Html/FormComponentViewModel';
import { DropDownFieldItem } from "XtiShared/Forms/DropDownFieldItem";
import { AddressInput } from './AddressInput';

export class AddEmployeeForm extends BaseForm {
	constructor(vm: FormComponentViewModel = new FormComponentViewModel()) {
		super('AddEmployeeForm', vm);
		this.EmployeeName.setCaption('Employee Name');
		this.EmployeeName.setMaxLength(100);
		this.BirthDate.setCaption('Birth Date');
		this.Department.setCaption('Department');
		this.Department.constraints.mustNotBeNull();
		this.Department.setItemCaption('Select...');
		this.Department.setItems(
			new DropDownFieldItem(1, 'HR'),
			new DropDownFieldItem(2, 'IT')
		);
		this.Address.setCaption('Address');
		this.SSN.setCaption('SSN');
		this.SSN.constraints.mustBeAbove(0, 'Must be greater than 0');
		this.SSN.constraints.mustBeBelow(1000000000, 'Must be less than 1000000000');
		this.SSN.protect();
		this.HireDate.setCaption('Hire Date');
		this.HireDate.setItems(
			new DropDownFieldItem(new Date(Date.UTC(2021, 7, 28, 4, 0, 0, 0)), 'Yesterday'),
			new DropDownFieldItem(new Date(Date.UTC(2021, 7, 29, 4, 0, 0, 0)), 'Today'),
			new DropDownFieldItem(new Date(Date.UTC(2021, 7, 30, 4, 0, 0, 0)), 'Tomorrow')
		);
		this.IsTemp.setCaption('Is Temp');
		this.IsTemp.setItems(
			new DropDownFieldItem(true, 'Yes'),
			new DropDownFieldItem(false, 'No')
		);
		this.EmployeeID.setCaption('Employee ID');
	}
	readonly EmployeeName = this.addTextInputFormGroup('EmployeeName');
	readonly BirthDate = this.addDateInputFormGroup('BirthDate');
	readonly Department = this.addNumberDropDownFormGroup('Department');
	readonly Address = this.addFormGroup(new AddressInput(this.getName(), 'Address'));
	readonly SSN = this.addNumberInputFormGroup('SSN');
	readonly HireDate = this.addDateDropDownFormGroup('HireDate');
	readonly IsTemp = this.addBooleanDropDownFormGroup('IsTemp');
	readonly EmployeeID = this.addHiddenNumberFormGroup('EmployeeID');
}