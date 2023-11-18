"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
exports.EmployeeType = exports.EmployeeTypes = void 0;
// Generated code
const NumericValue_1 = require("@jasonbenfield/sharedwebapp/NumericValue");
const NumericValues_1 = require("@jasonbenfield/sharedwebapp/NumericValues");
class EmployeeTypes extends NumericValues_1.NumericValues {
    constructor(None, Temp, Permanent) {
        super([None, Temp, Permanent]);
        this.None = None;
        this.Temp = Temp;
        this.Permanent = Permanent;
    }
}
exports.EmployeeTypes = EmployeeTypes;
class EmployeeType extends NumericValue_1.NumericValue {
    constructor(Value, DisplayText) {
        super(Value, DisplayText);
    }
}
exports.EmployeeType = EmployeeType;
EmployeeType.values = new EmployeeTypes(new EmployeeType(0, 'None'), new EmployeeType(10, 'Temp'), new EmployeeType(15, 'Permanent'));
//# sourceMappingURL=EmployeeType.js.map