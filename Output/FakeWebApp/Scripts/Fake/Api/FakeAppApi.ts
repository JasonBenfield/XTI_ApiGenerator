// Generated code

import { AppApi } from "@jasonbenfield/sharedwebapp/Api/AppApi";
import { AppApiEvents } from "@jasonbenfield/sharedwebapp/Api/AppApiEvents";
import { UserGroup } from "./UserGroup";
import { UserCacheGroup } from "./UserCacheGroup";
import { EmployeeGroup } from "./EmployeeGroup";
import { ProductGroup } from "./ProductGroup";


export class FakeAppApi extends AppApi {
	public static readonly DefaultVersion = 'V0';
	
	constructor(events: AppApiEvents, baseUrl: string, version: string = '') {
		super(events, baseUrl, 'Fake', version || FakeAppApi.DefaultVersion);
		this.User = this.addGroup((evts, resourceUrl) => new UserGroup(evts, resourceUrl));
		this.UserCache = this.addGroup((evts, resourceUrl) => new UserCacheGroup(evts, resourceUrl));
		this.Employee = this.addGroup((evts, resourceUrl) => new EmployeeGroup(evts, resourceUrl));
		this.Product = this.addGroup((evts, resourceUrl) => new ProductGroup(evts, resourceUrl));
	}
	
	readonly User: UserGroup;
	readonly UserCache: UserCacheGroup;
	readonly Employee: EmployeeGroup;
	readonly Product: ProductGroup;
}