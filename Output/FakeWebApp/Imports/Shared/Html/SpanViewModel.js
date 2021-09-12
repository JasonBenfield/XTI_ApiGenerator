"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var tslib_1 = require("tslib");
var HtmlComponentViewModel_1 = require("./HtmlComponentViewModel");
var template = require("./Span.html");
var ComponentTemplate_1 = require("../ComponentTemplate");
var AggregateComponentViewModel_1 = require("./AggregateComponentViewModel");
var SpanViewModel = /** @class */ (function (_super) {
    tslib_1.__extends(SpanViewModel, _super);
    function SpanViewModel() {
        var _this = _super.call(this, new ComponentTemplate_1.ComponentTemplate('span', template)) || this;
        _this.content = new AggregateComponentViewModel_1.AggregateComponentViewModel();
        return _this;
    }
    return SpanViewModel;
}(HtmlComponentViewModel_1.HtmlComponentViewModel));
exports.SpanViewModel = SpanViewModel;
//# sourceMappingURL=SpanViewModel.js.map