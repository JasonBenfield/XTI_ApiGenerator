"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
exports.AddressInputView = exports.DefaultAddressInputViewLayout = void 0;
// Generated code
const ComplexFieldFormGroupView_1 = require("@jasonbenfield/sharedwebapp/Views/ComplexFieldFormGroupView");
class DefaultAddressInputViewLayout {
    addFormGroups(form) {
        return {
            ID1: form.addInputFormGroup(),
            ID2: form.addInputFormGroup(),
            Line1: form.addInputFormGroup(),
            City: form.addInputFormGroup(),
            State: form.addInputFormGroup(),
            Zip: form.addInputFormGroup()
        };
    }
}
exports.DefaultAddressInputViewLayout = DefaultAddressInputViewLayout;
class AddressInputView extends ComplexFieldFormGroupView_1.ComplexFieldFormGroupView {
    constructor(container) {
        super(container);
    }
    addContent(layout) {
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
exports.AddressInputView = AddressInputView;
//# sourceMappingURL=AddressInputView.js.map