"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var ComponentTemplate_1 = require("./ComponentTemplate");
var ko = require("knockout");
var PageViewModel = /** @class */ (function () {
    function PageViewModel(template) {
        this.componentName = ko.observable('page-body');
        new ComponentTemplate_1.ComponentTemplate(this.componentName(), template).register();
    }
    return PageViewModel;
}());
exports.PageViewModel = PageViewModel;
//# sourceMappingURL=PageViewModel.js.map