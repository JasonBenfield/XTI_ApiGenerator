"use strict";
// Generated code
Object.defineProperty(exports, "__esModule", { value: true });
exports.ProductGroup = void 0;
const AppClientGroup_1 = require("@jasonbenfield/sharedwebapp/Http/AppClientGroup");
class ProductGroup extends AppClientGroup_1.AppClientGroup {
    constructor(events, resourceUrl) {
        super(events, resourceUrl, 'Product');
        this.Index = this.createView('Index');
        this.GetInfoAction = this.createAction('GetInfo', 'Get Info');
        this.AddProductAction = this.createAction('AddProduct', 'Add Product');
        this.ProductAction = this.createAction('Product', 'Get Product Information');
    }
    GetInfo(errorOptions) {
        return this.GetInfoAction.execute({}, errorOptions || {});
    }
    AddProduct(model, errorOptions) {
        return this.AddProductAction.execute(model, errorOptions || {});
    }
    Product(model, errorOptions) {
        return this.ProductAction.execute(model, errorOptions || {});
    }
}
exports.ProductGroup = ProductGroup;
//# sourceMappingURL=ProductGroup.js.map