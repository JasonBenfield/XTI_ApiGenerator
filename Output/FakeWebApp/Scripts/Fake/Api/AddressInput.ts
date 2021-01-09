import { AddressInputViewModel } from "./AddressInputViewModel";
import { ComplexField } from 'XtiShared/Forms/ComplexField';

export class AddressInput extends ComplexField {
	constructor(prefix: string, name: string, private readonly vm: AddressInputViewModel) {
		super(prefix, name, vm.caption, vm.value);
		this.ID1.caption.setCaption('ID 1');
		this.ID2.caption.setCaption('ID 2');
		this.Line1.caption.setCaption('Line 1');
		this.Line1.setMaxLength(30);
		this.City.caption.setCaption('City');
		this.State.caption.setCaption('State');
		this.Zip.caption.setCaption('Zip');
	}
	readonly ID1 = this.addHiddenTextField('ID1', this.vm.value.ID1);
	readonly ID2 = this.addHiddenNumberField('ID2', this.vm.value.ID2);
	readonly Line1 = this.addTextInputField('Line1', this.vm.value.Line1);
	readonly City = this.addTextInputField('City', this.vm.value.City);
	readonly State = this.addTextInputField('State', this.vm.value.State);
	readonly Zip = this.addNumberInputField('Zip', this.vm.value.Zip);
}