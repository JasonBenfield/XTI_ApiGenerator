// Generated code

import { AppApi } from "XtiShared/AppApi";
import { AppApiEvents } from "XtiShared/AppApiEvents";
import { UserGroup } from "./UserGroup";
import { EmployeeGroup } from "./EmployeeGroup";
import { ProductGroup } from "./ProductGroup";


export class FakeAppApi extends AppApi {
	public static readonly DefaultVersion = 'V2';
	
	constructor(events: AppApiEvents, baseUrl: string, version: string = '') {
		super(events, baseUrl, 'Fake', version || FakeAppApi.DefaultVersion);
		this.User = this.addGroup((evts, resourceUrl) => new UserGroup(evts, resourceUrl));
		this.Employee = this.addGroup((evts, resourceUrl) => new EmployeeGroup(evts, resourceUrl));
		this.Product = this.addGroup((evts, resourceUrl) => new ProductGroup(evts, resourceUrl));
	}
	
	readonly User: UserGroup;
	readonly Employee: EmployeeGroup;
	readonly Product: ProductGroup;
	}