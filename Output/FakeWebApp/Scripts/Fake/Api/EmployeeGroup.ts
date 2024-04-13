// Generated code

import * as xti from "@jasonbenfield/sharedwebapp/Common";
import { AppClientGroup } from "@jasonbenfield/sharedwebapp/Http/AppClientGroup";
import { AppClientAction } from "@jasonbenfield/sharedwebapp/Http/AppClientAction";
import { AppClientView } from "@jasonbenfield/sharedwebapp/Http/AppClientView";
import { AppClientEvents } from "@jasonbenfield/sharedwebapp/Http/AppClientEvents";
import { AppResourceUrl } from "@jasonbenfield/sharedwebapp/Http/AppResourceUrl";
import { AddEmployeeForm } from "./AddEmployeeForm";

export class EmployeeGroup extends AppClientGroup {
	constructor(events: AppClientEvents, resourceUrl: AppResourceUrl) {
		super(events, resourceUrl, 'Employee');
		this.Index = this.createView<IEmptyRequest>('Index');
		this.AddEmployeeAction = this.createAction<AddEmployeeForm,number>('AddEmployee', 'Add Employee');
		this.AddEmployeeFormView = this.createView<IEmptyRequest>('AddEmployeeFormView');
		this.AddEmployeeFormAction = this.createAction<IEmptyRequest,Record<string,object>>('AddEmployeeForm', 'Add Employee Form');
		this.EmployeeAction = this.createAction<number,IEmployee>('Employee', 'Get Employee Information');
		this.DownloadAttachment = this.createView<IEmptyRequest>('DownloadAttachment');
		this.GetContentAction = this.createAction<IEmptyRequest,IWebContentResult>('GetContent', 'Get Content');
	}
	
	readonly Index: AppClientView<IEmptyRequest>;
	readonly AddEmployeeAction: AppClientAction<AddEmployeeForm,number>;
	readonly AddEmployeeFormView: AppClientView<IEmptyRequest>;
	readonly AddEmployeeFormAction: AppClientAction<IEmptyRequest,Record<string,object>>;
	readonly EmployeeAction: AppClientAction<number,IEmployee>;
	readonly DownloadAttachment: AppClientView<IEmptyRequest>;
	readonly GetContentAction: AppClientAction<IEmptyRequest,IWebContentResult>;
	
	AddEmployee(model: AddEmployeeForm, errorOptions?: IActionErrorOptions) {
		return this.AddEmployeeAction.execute(model, errorOptions || {});
	}
	AddEmployeeForm(errorOptions?: IActionErrorOptions) {
		return this.AddEmployeeFormAction.execute({}, errorOptions || {});
	}
	Employee(model: number, errorOptions?: IActionErrorOptions) {
		return this.EmployeeAction.execute(model, errorOptions || {});
	}
	GetContent(errorOptions?: IActionErrorOptions) {
		return this.GetContentAction.execute({}, errorOptions || {});
	}
}