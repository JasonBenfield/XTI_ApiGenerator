// Generated code

import { AppApiGroup } from "../../Hub/AppApiGroup";
import { AppApiAction } from "../../Hub/AppApiAction";
import { AppApiView } from "../../Hub/AppApiView";
import { AppApiEvents } from "../../Hub/AppApiEvents";
import { AppResourceUrl } from "../../Hub/AppResourceUrl";

export class UserAdminGroup extends AppApiGroup {
	constructor(events: AppApiEvents, resourceUrl: AppResourceUrl) {
		super(events, resourceUrl, 'UserAdmin');
		this.AddUserAction = this.createAction<IAddUserModel,number>('AddUser', 'AddUser');
	}

	private readonly AddUserAction: AppApiAction<IAddUserModel,number>;

	AddUser(model: IAddUserModel, errorOptions?: IActionErrorOptions) {
		return this.AddUserAction.execute(model, errorOptions || {});
	}
}