"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var ContextualClass_1 = require("./ContextualClass");
var AggregateComponent_1 = require("./Html/AggregateComponent");
var DropdownBlock_1 = require("./Dropdown/DropdownBlock");
var FlexColumn_1 = require("./Html/FlexColumn");
var Heading1_1 = require("./Html/Heading1");
var TextSmall_1 = require("./Html/TextSmall");
var TextSpan_1 = require("./Html/TextSpan");
var Toolbar_1 = require("./Html/Toolbar");
var PageViewModel_1 = require("./PageViewModel");
var FaIcon_1 = require("./FaIcon");
var ModalErrorComponent_1 = require("./Error/ModalErrorComponent");
var PaddingCss_1 = require("./PaddingCss");
var Block_1 = require("./Html/Block");
var PageLoader_1 = require("./PageLoader");
var AppApiEvents_1 = require("./AppApiEvents");
var ConsoleLog_1 = require("./ConsoleLog");
var AlignCss_1 = require("./AlignCss");
var Page = /** @class */ (function () {
    function Page(vm) {
        if (vm === void 0) { vm = new PageViewModel_1.PageViewModel(); }
        this.vm = vm;
        this.outerContent = new AggregateComponent_1.AggregateComponent(this.vm.content);
        this.modalError = new ModalErrorComponent_1.ModalErrorComponent(this.vm.modalError);
        var frame = this.outerContent.addContent(new FlexColumn_1.FlexColumn());
        frame.flexFill();
        var toolbar = frame.addContent(new Toolbar_1.Toolbar());
        toolbar.setBackgroundContext(ContextualClass_1.ContextualClass.primary);
        toolbar.setPadding(PaddingCss_1.PaddingCss.xs(function (p) { return p.all(3); }));
        toolbar.columnStart.setTextContext(ContextualClass_1.ContextualClass.light);
        var heading = toolbar.columnStart.addContent(new Heading1_1.Heading1());
        heading.addContent(new TextSpan_1.TextSpan(pageContext.AppTitle));
        heading.addContent(new TextSmall_1.TextSmall(pageContext.PageTitle));
        toolbar.columnEnd.addCssFrom(new AlignCss_1.AlignCss().self(function (a) { return a.xs('center'); }).cssClass());
        var dropdown = toolbar.columnEnd.addContent(new DropdownBlock_1.DropdownBlock());
        if (!pageContext.IsAuthenticated) {
            dropdown.hide();
        }
        dropdown.button.setContext(ContextualClass_1.ContextualClass.light);
        dropdown.button.useOutlineStyle();
        dropdown.button.addContent(new FaIcon_1.FaIcon('user'));
        dropdown.addSpanItem()
            .configure(function (li) {
            li.span.addContent(new TextSpan_1.TextSpan(pageContext.UserName));
        });
        this.logoutMenuItem = dropdown.addLinkItem();
        this.logoutMenuItem.link.addContent(new TextSpan_1.TextSpan('Logout'));
        this.content = frame.addContent(new Block_1.Block());
        this.content.flexFill();
        var documentTitle = pageContext.AppTitle;
        if (pageContext.PageTitle) {
            documentTitle = documentTitle + " - " + pageContext.PageTitle;
        }
        document.title = documentTitle;
    }
    Page.prototype.setLogoutUrl = function (url) {
        this.logoutMenuItem.link.setHref(url);
    };
    Page.prototype.addItem = function (itemVM, item) {
        return this.content.addItem(itemVM, item);
    };
    Page.prototype.insertItem = function (index, itemVM, item) {
        return this.content.insertItem(index, itemVM, item);
    };
    Page.prototype.removeItem = function (item) {
        return this.content.removeItem(item);
    };
    Page.prototype.show = function () {
        this.content.show();
    };
    Page.prototype.hide = function () {
        this.content.hide();
    };
    Page.prototype.insertContent = function (index, item) {
        return item.insertIntoContainer(this.content, index);
    };
    Page.prototype.addContent = function (item) {
        return item.addToContainer(this.content);
    };
    Page.prototype.load = function () {
        new PageLoader_1.PageLoader().loadPage(this.vm);
    };
    Page.prototype.api = function (apiCtor) {
        var _this = this;
        var api = this.createApi(apiCtor);
        new AppApiEvents_1.AppApiEvents(function (err) {
            new ConsoleLog_1.ConsoleLog().error(err.toString());
            _this.modalError.show(err.getErrors(), err.getCaption());
        });
        return api;
    };
    Page.prototype.createApi = function (apiCtor) {
        return new apiCtor();
    };
    return Page;
}());
exports.Page = Page;
//# sourceMappingURL=Page.js.map