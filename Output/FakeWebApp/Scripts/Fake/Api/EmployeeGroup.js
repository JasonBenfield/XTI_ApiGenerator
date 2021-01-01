"use strict";
// Generated code
Object.defineProperty(exports, "__esModule", { value: true });
exports.EmployeeGroup = void 0;
var tslib_1 = require("tslib");
var AppApiGroup_1 = require("XtiShared/AppApiGroup");
var EmployeeGroup = /** @class */ (function (_super) {
    tslib_1.__extends(EmployeeGroup, _super);
    function EmployeeGroup(events, resourceUrl) {
        var _this = _super.call(this, events, resourceUrl, 'Employee') || this;
        _this.Index = _this.createView('Index');
        _this.AddEmployeeAction = _this.createAction('AddEmployee', 'AddEmployee');
        _this.EmployeeAction = _this.createAction('Employee', 'Get Employee Information');
        return _this;
    }
    EmployeeGroup.prototype.AddEmployee = function (model, errorOptions) {
        return this.AddEmployeeAction.execute(model, errorOptions || {});
    };
    EmployeeGroup.prototype.Employee = function (model, errorOptions) {
        return this.EmployeeAction.execute(model, errorOptions || {});
    };
    return EmployeeGroup;
}(AppApiGroup_1.AppApiGroup));
exports.EmployeeGroup = EmployeeGroup;
//# sourceMappingURL=EmployeeGroup.js.map