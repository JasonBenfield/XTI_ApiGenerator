// Generated code

import { AppApi } from "../../Shared/AppApi";
import { AppApiEvents } from "../../Shared/AppApiEvents";
import { UserGroup } from "./UserGroup";
import { EmployeeGroup } from "./EmployeeGroup";
import { ProductGroup } from "./ProductGroup";

export class FakeAppApi extends AppApi {
	constructor(events: AppApiEvents, baseUrl: string, version: string = 'V1006') {
		super(events, baseUrl, 'Fake', version);
		this.User = this.addGroup((evts, resourceUrl) => new UserGroup(evts, resourceUrl));
		this.Employee = this.addGroup((evts, resourceUrl) => new EmployeeGroup(evts, resourceUrl));
		this.Product = this.addGroup((evts, resourceUrl) => new ProductGroup(evts, resourceUrl));
	}

	readonly User: UserGroup;
	readonly Employee: EmployeeGroup;
	readonly Product: ProductGroup;
}