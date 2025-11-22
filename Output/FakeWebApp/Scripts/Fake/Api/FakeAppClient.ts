// Generated code

import { AppClient } from "@jasonbenfield/sharedwebapp/Http/AppClient";
import { AppClientEvents } from "@jasonbenfield/sharedwebapp/Http/AppClientEvents";
import { AppClientQuery } from "@jasonbenfield/sharedwebapp/Http/AppClientQuery";
import { EmployeeGroup } from "./EmployeeGroup";
import { ProductGroup } from "./ProductGroup";


export class FakeAppClient extends AppClient {
	constructor(events: AppClientEvents) {
		super(
			events, 
			'Fake', 
			pageContext.EnvironmentName === 'Production' || pageContext.EnvironmentName === 'Staging' ? 'V0' : 'Current'
		);
		this.Employee = this.addGroup((evts, resourceUrl) => new EmployeeGroup(evts, resourceUrl));
		this.EmployeeQuery = this.addODataGroup((evts, resourceUrl) => new AppClientQuery<IQueryEmployeesRequest, IEmployee>(evts, resourceUrl.odata('EmployeeQuery'), 'EmployeeQuery'));
		this.Product = this.addGroup((evts, resourceUrl) => new ProductGroup(evts, resourceUrl));
	}
	
	readonly Employee: EmployeeGroup;
	readonly EmployeeQuery: AppClientQuery<IQueryEmployeesRequest, IEmployee>;
	readonly Product: ProductGroup;
}