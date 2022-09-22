// Generated code

import { AppApi } from "@jasonbenfield/sharedwebapp/Api/AppApi";
import { AppApiEvents } from "@jasonbenfield/sharedwebapp/Api/AppApiEvents";
import { AppApiQuery } from "@jasonbenfield/sharedwebapp/Api/AppApiQuery";
import { EmployeeGroup } from "./EmployeeGroup";
import { ProductGroup } from "./ProductGroup";


export class FakeAppApi extends AppApi {
	constructor(events: AppApiEvents) {
		super(events, 'Fake');
		this.Employee = this.addGroup((evts, resourceUrl) => new EmployeeGroup(evts, resourceUrl));
		this.EmployeeQuery = this.addODataGroup((evts, resourceUrl) => new AppApiQuery<IQueryEmployeesRequest, IEmployee>(evts, resourceUrl.odata('EmployeeQuery'), 'EmployeeQuery'));
		this.Product = this.addGroup((evts, resourceUrl) => new ProductGroup(evts, resourceUrl));
	}
	
	readonly Employee: EmployeeGroup;
	readonly EmployeeQuery: AppApiQuery<IQueryEmployeesRequest, IEmployee>;
	readonly Product: ProductGroup;
}