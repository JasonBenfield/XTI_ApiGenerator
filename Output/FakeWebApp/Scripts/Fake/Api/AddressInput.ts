// Generated code
import * as xti from "@jasonbenfield/sharedwebapp/Common";
import { ComplexFieldFormGroup } from '@jasonbenfield/sharedwebapp/Forms/ComplexFieldFormGroup';
import { AddressInputView } from './AddressInputView';

export class AddressInput extends ComplexFieldFormGroup {
	protected readonly view: AddressInputView;
	
	constructor(prefix: string, name: string, view: AddressInputView) {
		super(prefix, name, view);
		this.Line1.setCaption('Line 1');
		this.Line1.setMaxLength(30);
		this.City.setCaption('City');
		this.State.setCaption('State');
		this.Zip.setCaption('Zip');
	}
	readonly ID1 = this.addHiddenText('ID1', this.view.ID1);
	readonly ID2 = this.addHiddenNumber('ID2', this.view.ID2);
	readonly Line1 = this.addTextInputFormGroup('Line1', this.view.Line1);
	readonly City = this.addTextInputFormGroup('City', this.view.City);
	readonly State = this.addTextInputFormGroup('State', this.view.State);
	readonly Zip = this.addNumberInputFormGroup('Zip', this.view.Zip);
}