// Generated code

import { AppApiGroup } from "@jasonbenfield/sharedwebapp/Api/AppApiGroup";
import { AppApiAction } from "@jasonbenfield/sharedwebapp/Api/AppApiAction";
import { AppApiView } from "@jasonbenfield/sharedwebapp/Api/AppApiView";
import { AppApiEvents } from "@jasonbenfield/sharedwebapp/Api/AppApiEvents";
import { AppResourceUrl } from "@jasonbenfield/sharedwebapp/Api/AppResourceUrl";

export class ProductGroup extends AppApiGroup {
	constructor(events: AppApiEvents, resourceUrl: AppResourceUrl) {
		super(events, resourceUrl, 'Product');
		this.Index = this.createView<IEmptyRequest>('Index');
		this.GetInfoAction = this.createAction<IEmptyRequest,string>('GetInfo', 'Get Info');
		this.AddProductAction = this.createAction<IAddProductModel,number>('AddProduct', 'Add Product');
		this.ProductAction = this.createAction<number,IProduct>('Product', 'Get Product Information');
	}
	
	readonly Index: AppApiView<IEmptyRequest>;
	readonly GetInfoAction: AppApiAction<IEmptyRequest,string>;
	readonly AddProductAction: AppApiAction<IAddProductModel,number>;
	readonly ProductAction: AppApiAction<number,IProduct>;
	
	GetInfo(errorOptions?: IActionErrorOptions) {
		return this.GetInfoAction.execute({}, errorOptions || {});
	}
	AddProduct(model: IAddProductModel, errorOptions?: IActionErrorOptions) {
		return this.AddProductAction.execute(model, errorOptions || {});
	}
	Product(model: number, errorOptions?: IActionErrorOptions) {
		return this.ProductAction.execute(model, errorOptions || {});
	}
}