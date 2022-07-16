// Generated code
import { ComplexFieldFormGroupView } from '@jasonbenfield/sharedwebapp/Views/ComplexFieldFormGroupView';
import { SimpleFieldFormGroupInputView, SimpleFieldFormGroupSelectView } from '@jasonbenfield/sharedwebapp/Views/FormGroup';
import { IFormGroupLayout } from '@jasonbenfield/sharedwebapp/Views/Types';
import { BasicComponentView } from '@jasonbenfield/sharedwebapp/Views/BasicComponentView';

export interface IAddressInputView {
	ID1: SimpleFieldFormGroupInputView;
	ID2: SimpleFieldFormGroupInputView;
	Line1: SimpleFieldFormGroupInputView;
	City: SimpleFieldFormGroupInputView;
	State: SimpleFieldFormGroupInputView;
	Zip: SimpleFieldFormGroupInputView;
}

export class DefaultAddressInputViewLayout implements IFormGroupLayout<IAddressInputView> {
	addFormGroups(form: AddressInputView) {
		return {
			ID1: form.addInputFormGroup(),
			ID2: form.addInputFormGroup(),
			Line1: form.addInputFormGroup(),
			City: form.addInputFormGroup(),
			State: form.addInputFormGroup(),
			Zip: form.addInputFormGroup()
		}
	}
}

export class AddressInputView extends ComplexFieldFormGroupView {
	private formGroups: IAddressInputView;
	
	constructor(container: BasicComponentView) {
		super(container);
	}
	
	addContent(layout?: IFormGroupLayout<IAddressInputView>) {
		if (!layout) {
			layout = new DefaultAddressInputViewLayout();
		}
		this.formGroups = layout.addFormGroups(this);
	}
	
	get ID1() { return this.formGroups.ID1; }
	get ID2() { return this.formGroups.ID2; }
	get Line1() { return this.formGroups.Line1; }
	get City() { return this.formGroups.City; }
	get State() { return this.formGroups.State; }
	get Zip() { return this.formGroups.Zip; }
}