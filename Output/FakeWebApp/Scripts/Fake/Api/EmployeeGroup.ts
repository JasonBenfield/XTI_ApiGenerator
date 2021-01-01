// Generated code

import { AppApiGroup } from "XtiShared/AppApiGroup";
import { AppApiAction } from "XtiShared/AppApiAction";
import { AppApiView } from "XtiShared/AppApiView";
import { AppApiEvents } from "XtiShared/AppApiEvents";
import { AppResourceUrl } from "XtiShared/AppResourceUrl";

export class EmployeeGroup extends AppApiGroup {
	constructor(events: AppApiEvents, resourceUrl: AppResourceUrl) {
		super(events, resourceUrl, 'Employee');
		this.Index = this.createView<IEmptyRequest>('Index');
		this.AddEmployeeAction = this.createAction<IAddEmployeeModel,number>('AddEmployee', 'AddEmployee');
		this.EmployeeAction = this.createAction<number,IEmployee>('Employee', 'Get Employee Information');
	}

	readonly Index: AppApiView<IEmptyRequest>;
	private readonly AddEmployeeAction: AppApiAction<IAddEmployeeModel,number>;
	private readonly EmployeeAction: AppApiAction<number,IEmployee>;

	AddEmployee(model: IAddEmployeeModel, errorOptions?: IActionErrorOptions) {
		return this.AddEmployeeAction.execute(model, errorOptions || {});
	}
	Employee(model: number, errorOptions?: IActionErrorOptions) {
		return this.EmployeeAction.execute(model, errorOptions || {});
	}
}