"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var tslib_1 = require("tslib");
var ModalErrorComponentViewModel_1 = require("./Error/ModalErrorComponentViewModel");
var ko = require("knockout");
var tsyringe_1 = require("tsyringe");
var PageFrameViewModel = /** @class */ (function () {
    function PageFrameViewModel(page, modalError, logoutUrl) {
        this.page = page;
        this.modalError = modalError;
        this.appTitle = ko.observable('');
        this.pageTitle = ko.observable('');
        this.isAuthenticated = ko.observable(false);
        this.userName = ko.observable('');
        this.logoutUrl = ko.observable('');
        this.appTitle(pageContext.AppTitle);
        this.pageTitle(pageContext.PageTitle);
        var title;
        if (pageContext.PageTitle) {
            title = pageContext.AppTitle + " - " + pageContext.PageTitle;
        }
        else {
            title = pageContext.AppTitle;
        }
        document.title = title;
        this.isAuthenticated(pageContext.IsAuthenticated);
        this.userName(pageContext.UserName);
        this.logoutUrl(logoutUrl.value());
    }
    PageFrameViewModel = tslib_1.__decorate([
        tsyringe_1.singleton(),
        tslib_1.__param(0, tsyringe_1.inject('PageVM')),
        tslib_1.__param(2, tsyringe_1.inject('LogoutUrl')),
        tslib_1.__metadata("design:paramtypes", [Object, ModalErrorComponentViewModel_1.ModalErrorComponentViewModel, Object])
    ], PageFrameViewModel);
    return PageFrameViewModel;
}());
exports.PageFrameViewModel = PageFrameViewModel;
//# sourceMappingURL=PageFrameViewModel.js.map