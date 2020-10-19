// Generated code

import { AppApiGroup } from "../../Shared/AppApiGroup";
import { AppApiAction } from "../../Shared/AppApiAction";
import { AppApiView } from "../../Shared/AppApiView";
import { AppApiEvents } from "../../Shared/AppApiEvents";
import { AppResourceUrl } from "../../Shared/AppResourceUrl";

export class ProductGroup extends AppApiGroup {
	constructor(events: AppApiEvents, resourceUrl: AppResourceUrl) {
		super(events, resourceUrl, 'Product');
		this.Index = this.createView<IEmptyRequest>('Index');
		this.GetInfoAction = this.createAction<IEmptyRequest,string>('GetInfo', 'GetInfo');
		this.AddProductAction = this.createAction<IAddProductModel,number>('AddProduct', 'AddProduct');
		this.ProductAction = this.createAction<number,IProduct>('Product', 'Get Product Information');
	}

	readonly Index: AppApiView<IEmptyRequest>;
	private readonly GetInfoAction: AppApiAction<IEmptyRequest,string>;
	private readonly AddProductAction: AppApiAction<IAddProductModel,number>;
	private readonly ProductAction: AppApiAction<number,IProduct>;

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