// Generated code

import { AppApiGroup } from "XtiShared/AppApiGroup";
import { AppApiAction } from "XtiShared/AppApiAction";
import { AppApiView } from "XtiShared/AppApiView";
import { AppApiEvents } from "XtiShared/AppApiEvents";
import { AppResourceUrl } from "XtiShared/AppResourceUrl";
import { AddEmployeeForm } from "./AddEmployeeForm";

export class EmployeeGroup extends AppApiGroup {
	constructor(events: AppApiEvents, resourceUrl: AppResourceUrl) {
		super(events, resourceUrl, 'Employee');
		this.Index = this.createView<IEmptyRequest>('Index');
		this.AddEmployeeAction = this.createAction<AddEmployeeForm,number>('AddEmployee', 'Add Employee');
		this.AddEmployeeFormView = this.createView<IEmptyRequest>('AddEmployeeFormView');
		this.AddEmployeeFormAction = this.createAction<IEmptyRequest,Record<string,object>>('AddEmployeeForm', 'Add Employee Form');
		this.EmployeeAction = this.createAction<number,IEmployee>('Employee', 'Get Employee Information');
	}
	
	readonly Index: AppApiView<IEmptyRequest>;
	readonly AddEmployeeAction: AppApiAction<AddEmployeeForm,number>;
	readonly AddEmployeeFormView: AppApiView<IEmptyRequest>;
	readonly AddEmployeeFormAction: AppApiAction<IEmptyRequest,Record<string,object>>;
	readonly EmployeeAction: AppApiAction<number,IEmployee>;
	
	AddEmployee(model: AddEmployeeForm, errorOptions?: IActionErrorOptions) {
		return this.AddEmployeeAction.execute(model, errorOptions || {});
	}
	AddEmployeeForm(errorOptions?: IActionErrorOptions) {
		return this.AddEmployeeFormAction.execute({}, errorOptions || {});
	}
	Employee(model: number, errorOptions?: IActionErrorOptions) {
		return this.EmployeeAction.execute(model, errorOptions || {});
	}
}