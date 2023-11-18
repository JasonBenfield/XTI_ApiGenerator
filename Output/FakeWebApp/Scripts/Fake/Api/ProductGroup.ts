// Generated code

import { AppClientGroup } from "@jasonbenfield/sharedwebapp/Http/AppClientGroup";
import { AppClientAction } from "@jasonbenfield/sharedwebapp/Http/AppClientAction";
import { AppClientView } from "@jasonbenfield/sharedwebapp/Http/AppClientView";
import { AppClientEvents } from "@jasonbenfield/sharedwebapp/Http/AppClientEvents";
import { AppResourceUrl } from "@jasonbenfield/sharedwebapp/Http/AppResourceUrl";

export class ProductGroup extends AppClientGroup {
	constructor(events: AppClientEvents, resourceUrl: AppResourceUrl) {
		super(events, resourceUrl, 'Product');
		this.Index = this.createView<IEmptyRequest>('Index');
		this.GetInfoAction = this.createAction<IEmptyRequest,string>('GetInfo', 'Get Info');
		this.AddProductAction = this.createAction<IAddProductModel,number>('AddProduct', 'Add Product');
		this.ProductAction = this.createAction<number,IProduct>('Product', 'Get Product Information');
	}
	
	readonly Index: AppClientView<IEmptyRequest>;
	readonly GetInfoAction: AppClientAction<IEmptyRequest,string>;
	readonly AddProductAction: AppClientAction<IAddProductModel,number>;
	readonly ProductAction: AppClientAction<number,IProduct>;
	
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