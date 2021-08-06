// Generated code
import { ComplexFieldFormGroup } from 'XtiShared/Forms/ComplexFieldFormGroup';
import { BlockViewModel } from 'XtiShared/Html/BlockViewModel';

export class AddressInput extends ComplexFieldFormGroup {
	constructor(prefix: string, name: string, vm: BlockViewModel = new BlockViewModel()) {
		super(prefix, name, vm);
		this.ID1.setCaption('ID 1');
		this.ID2.setCaption('ID 2');
		this.Line1.setCaption('Line 1');
		this.Line1.setMaxLength(30);
		this.City.setCaption('City');
		this.State.setCaption('State');
		this.Zip.setCaption('Zip');
	}
	readonly ID1 = this.addHiddenTextFormGroup('ID1');
	readonly ID2 = this.addHiddenNumberFormGroup('ID2');
	readonly Line1 = this.addTextInputFormGroup('Line1');
	readonly City = this.addTextInputFormGroup('City');
	readonly State = this.addTextInputFormGroup('State');
	readonly Zip = this.addNumberInputFormGroup('Zip');
}