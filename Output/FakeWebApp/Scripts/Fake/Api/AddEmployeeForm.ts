import { AddEmployeeFormViewModel } from "./AddEmployeeFormViewModel";
import { Form } from 'XtiShared/Forms/Form';
import { DropDownFieldItem } from "XtiShared/Forms/DropDownFieldItem";
import { AddressInput } from './AddressInput';

export class AddEmployeeForm extends Form {
	constructor(private readonly vm: AddEmployeeFormViewModel) {
		super('AddEmployeeForm');
		this.Name.caption.setCaption('Name');
		this.Name.setMaxLength(100);
		this.BirthDate.caption.setCaption('Birth Date');
		this.Department.caption.setCaption('Department');
		this.Department.constraints.mustNotBeNull();
		this.Department.setItemCaption('Select...');
		this.Department.setItems(
			new DropDownFieldItem(1, 'HR'),
			new DropDownFieldItem(2, 'IT')
		);
		this.Address.caption.setCaption('Address');
		this.SSN.caption.setCaption('SSN');
		this.SSN.constraints.mustBeAbove(0, 'Must be greater than 0');
		this.SSN.constraints.mustBeBelow(1000000000, 'Must be less than 1000000000');
		this.SSN.protect();
		this.HireDate.caption.setCaption('Hire Date');
		this.HireDate.setItems(
			new DropDownFieldItem(new Date(Date.UTC(2021, 0, 7, 5, 0, 0, 0)), 'Yesterday'),
			new DropDownFieldItem(new Date(Date.UTC(2021, 0, 8, 5, 0, 0, 0)), 'Today'),
			new DropDownFieldItem(new Date(Date.UTC(2021, 0, 9, 5, 0, 0, 0)), 'Tomorrow')
		);
		this.IsTemp.caption.setCaption('Is Temp');
		this.IsTemp.setItems(
			new DropDownFieldItem(true, 'Yes'),
			new DropDownFieldItem(false, 'No')
		);
	}
	readonly Name = this.addTextInputField('Name', this.vm.Name);
	readonly BirthDate = this.addDateInputField('BirthDate', this.vm.BirthDate);
	readonly Department = this.addDropDownField<number>('Department', this.vm.Department);
	readonly Address = this.addField(new AddressInput(this.getName(), 'Address', this.vm.Address));
	readonly SSN = this.addNumberInputField('SSN', this.vm.SSN);
	readonly HireDate = this.addDropDownField<Date>('HireDate', this.vm.HireDate);
	readonly IsTemp = this.addDropDownField<boolean>('IsTemp', this.vm.IsTemp);
}