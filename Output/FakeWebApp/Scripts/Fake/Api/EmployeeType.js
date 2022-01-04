"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
exports.EmployeeType = exports.EmployeeTypes = void 0;
var tslib_1 = require("tslib");
// Generated code
var NumericValue_1 = require("@jasonbenfield/sharedwebapp/NumericValue");
var NumericValues_1 = require("@jasonbenfield/sharedwebapp/NumericValues");
var EmployeeTypes = /** @class */ (function (_super) {
    tslib_1.__extends(EmployeeTypes, _super);
    function EmployeeTypes(None, Temp, Permanent) {
        var _this = _super.call(this, [None, Temp, Permanent]) || this;
        _this.None = None;
        _this.Temp = Temp;
        _this.Permanent = Permanent;
        return _this;
    }
    return EmployeeTypes;
}(NumericValues_1.NumericValues));
exports.EmployeeTypes = EmployeeTypes;
var EmployeeType = /** @class */ (function (_super) {
    tslib_1.__extends(EmployeeType, _super);
    function EmployeeType(Value, DisplayText) {
        return _super.call(this, Value, DisplayText) || this;
    }
    EmployeeType.values = new EmployeeTypes(new EmployeeType(0, 'None'), new EmployeeType(10, 'Temp'), new EmployeeType(15, 'Permanent'));
    return EmployeeType;
}(NumericValue_1.NumericValue));
exports.EmployeeType = EmployeeType;
//# sourceMappingURL=EmployeeType.js.map