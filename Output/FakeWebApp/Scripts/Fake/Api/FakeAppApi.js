"use strict";
// Generated code
Object.defineProperty(exports, "__esModule", { value: true });
exports.FakeAppApi = void 0;
var tslib_1 = require("tslib");
var AppApi_1 = require("@jasonbenfield/sharedwebapp/Api/AppApi");
var UserGroup_1 = require("./UserGroup");
var UserCacheGroup_1 = require("./UserCacheGroup");
var EmployeeGroup_1 = require("./EmployeeGroup");
var ProductGroup_1 = require("./ProductGroup");
var FakeAppApi = /** @class */ (function (_super) {
    tslib_1.__extends(FakeAppApi, _super);
    function FakeAppApi(events) {
        var _this = _super.call(this, events, 'Fake') || this;
        _this.User = _this.addGroup(function (evts, resourceUrl) { return new UserGroup_1.UserGroup(evts, resourceUrl); });
        _this.UserCache = _this.addGroup(function (evts, resourceUrl) { return new UserCacheGroup_1.UserCacheGroup(evts, resourceUrl); });
        _this.Employee = _this.addGroup(function (evts, resourceUrl) { return new EmployeeGroup_1.EmployeeGroup(evts, resourceUrl); });
        _this.Product = _this.addGroup(function (evts, resourceUrl) { return new ProductGroup_1.ProductGroup(evts, resourceUrl); });
        return _this;
    }
    return FakeAppApi;
}(AppApi_1.AppApi));
exports.FakeAppApi = FakeAppApi;
//# sourceMappingURL=FakeAppApi.js.map