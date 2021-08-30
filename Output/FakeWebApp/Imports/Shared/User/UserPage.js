"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var tslib_1 = require("tslib");
var UrlBuilder_1 = require("../UrlBuilder");
var xtistart_1 = require("xtistart");
var WebPage_1 = require("../WebPage");
var MessageAlert_1 = require("../MessageAlert");
var PageViewModel_1 = require("../PageViewModel");
var Page_1 = require("../Page");
var Startup_1 = require("../Startup");
var UserPage = /** @class */ (function (_super) {
    tslib_1.__extends(UserPage, _super);
    function UserPage(vm) {
        var _this = _super.call(this, vm) || this;
        _this.alert = _this.addContent(new MessageAlert_1.MessageAlert());
        _this.goToReturnUrl();
        return _this;
    }
    UserPage.prototype.goToReturnUrl = function () {
        this.alert.info('Opening Page...');
        var urlBuilder = UrlBuilder_1.UrlBuilder.current();
        var returnUrl = urlBuilder.getQueryValue('returnUrl');
        if (returnUrl) {
            returnUrl = decodeURIComponent(returnUrl);
        }
        returnUrl = Startup_1.defaultApi ? Startup_1.defaultApi.url.addPart(returnUrl).value() : '/';
        new WebPage_1.WebPage(returnUrl).open();
    };
    return UserPage;
}(Page_1.Page));
exports.UserPage = UserPage;
xtistart_1.startup(new UserPage(new PageViewModel_1.PageViewModel()));
//# sourceMappingURL=UserPage.js.map