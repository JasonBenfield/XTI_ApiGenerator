"use strict";
// Generated code
Object.defineProperty(exports, "__esModule", { value: true });
exports.EmployeeGroup = void 0;
const AppClientGroup_1 = require("@jasonbenfield/sharedwebapp/Http/AppClientGroup");
class EmployeeGroup extends AppClientGroup_1.AppClientGroup {
    constructor(events, resourceUrl) {
        super(events, resourceUrl, 'Employee');
        this.Index = this.createView('Index');
        this.AddEmployeeAction = this.createAction('AddEmployee', 'Add Employee');
        this.AddEmployeeFormView = this.createView('AddEmployeeFormView');
        this.AddEmployeeFormAction = this.createAction('AddEmployeeForm', 'Add Employee Form');
        this.EmployeeAction = this.createAction('Employee', 'Get Employee Information');
        this.DownloadAttachment = this.createView('DownloadAttachment');
        this.GetContentAction = this.createAction('GetContent', 'Get Content');
    }
    AddEmployee(model, errorOptions) {
        return this.AddEmployeeAction.execute(model, errorOptions || {});
    }
    AddEmployeeForm(errorOptions) {
        return this.AddEmployeeFormAction.execute({}, errorOptions || {});
    }
    Employee(model, errorOptions) {
        return this.EmployeeAction.execute(model, errorOptions || {});
    }
    GetContent(errorOptions) {
        return this.GetContentAction.execute({}, errorOptions || {});
    }
}
exports.EmployeeGroup = EmployeeGroup;
//# sourceMappingURL=EmployeeGroup.js.map