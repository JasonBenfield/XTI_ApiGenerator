// Generated code
import * as ko from 'knockout';
import { ComplexFieldViewModel } from "XtiShared/Forms/ComplexFieldViewModel";
import { ComplexFieldValueViewModel } from "XtiShared/Forms/FieldValueViewModel";
import { InputFieldViewModel } from "XtiShared/Forms/InputFieldViewModel";

export class AddressInputValueViewModel extends ComplexFieldValueViewModel {
	readonly componentName = ko.observable('AddressInput');
	readonly ID1 = this.addValue(new InputFieldViewModel());
	readonly ID2 = this.addValue(new InputFieldViewModel());
	readonly Line1 = this.addValue(new InputFieldViewModel());
	readonly City = this.addValue(new InputFieldViewModel());
	readonly State = this.addValue(new InputFieldViewModel());
	readonly Zip = this.addValue(new InputFieldViewModel());
}

export class AddressInputViewModel extends ComplexFieldViewModel {
	constructor() {
		super(new AddressInputValueViewModel());
	}
	readonly value: AddressInputValueViewModel;
}