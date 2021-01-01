"use strict";
// Generated code
Object.defineProperty(exports, "__esModule", { value: true });
exports.ProductGroup = void 0;
var tslib_1 = require("tslib");
var AppApiGroup_1 = require("XtiShared/AppApiGroup");
var ProductGroup = /** @class */ (function (_super) {
    tslib_1.__extends(ProductGroup, _super);
    function ProductGroup(events, resourceUrl) {
        var _this = _super.call(this, events, resourceUrl, 'Product') || this;
        _this.Index = _this.createView('Index');
        _this.GetInfoAction = _this.createAction('GetInfo', 'GetInfo');
        _this.AddProductAction = _this.createAction('AddProduct', 'AddProduct');
        _this.ProductAction = _this.createAction('Product', 'Get Product Information');
        return _this;
    }
    ProductGroup.prototype.GetInfo = function (errorOptions) {
        return this.GetInfoAction.execute({}, errorOptions || {});
    };
    ProductGroup.prototype.AddProduct = function (model, errorOptions) {
        return this.AddProductAction.execute(model, errorOptions || {});
    };
    ProductGroup.prototype.Product = function (model, errorOptions) {
        return this.ProductAction.execute(model, errorOptions || {});
    };
    return ProductGroup;
}(AppApiGroup_1.AppApiGroup));
exports.ProductGroup = ProductGroup;
//# sourceMappingURL=ProductGroup.js.map