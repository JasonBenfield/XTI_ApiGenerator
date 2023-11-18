"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
exports.AddressInput = void 0;
// Generated code
const ComplexFieldFormGroup_1 = require("@jasonbenfield/sharedwebapp/Forms/ComplexFieldFormGroup");
class AddressInput extends ComplexFieldFormGroup_1.ComplexFieldFormGroup {
    constructor(prefix, name, view) {
        super(prefix, name, view);
        this.ID1 = this.addHiddenTextFormGroup('ID1', this.view.ID1);
        this.ID2 = this.addHiddenNumberFormGroup('ID2', this.view.ID2);
        this.Line1 = this.addTextInputFormGroup('Line1', this.view.Line1);
        this.City = this.addTextInputFormGroup('City', this.view.City);
        this.State = this.addTextInputFormGroup('State', this.view.State);
        this.Zip = this.addNumberInputFormGroup('Zip', this.view.Zip);
        this.ID1.setCaption('ID 1');
        this.ID2.setCaption('ID 2');
        this.Line1.setCaption('Line 1');
        this.Line1.setMaxLength(30);
        this.City.setCaption('City');
        this.State.setCaption('State');
        this.Zip.setCaption('Zip');
    }
}
exports.AddressInput = AddressInput;
//# sourceMappingURL=AddressInput.js.map