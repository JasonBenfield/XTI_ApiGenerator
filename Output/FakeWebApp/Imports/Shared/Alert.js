"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var tslib_1 = require("tslib");
var ko = require("knockout");
var _ = require("lodash");
var ContextualClass_1 = require("./ContextualClass");
var template = require("./Templates/Alert.html");
var ComponentTemplate_1 = require("./ComponentTemplate");
var AlertViewModel = /** @class */ (function () {
    function AlertViewModel() {
        var _this = this;
        this.componentName = ko.observable('alert');
        this.contextualClass = new ContextualClass_1.ContextualClassViewModel();
        this.message = ko.observable('');
        this.hasMessage = ko.pureComputed(function () { return Boolean(_this.message()); });
        this.hasSuccessMessage = ko.pureComputed(function () {
            var hasMessage = _this.hasMessage();
            var isSuccess = _this.contextualClass.isSuccess();
            return hasMessage && isSuccess;
        });
        this.hasInfoMessage = ko.pureComputed(function () {
            var hasMessage = _this.hasMessage();
            var isInfo = _this.contextualClass.isInfo();
            return hasMessage && isInfo;
        });
        this.hasWarningMessage = ko.pureComputed(function () {
            var hasMessage = _this.hasMessage();
            var isWarning = _this.contextualClass.isWarning();
            return hasMessage && isWarning;
        });
        this.hasDangerMessage = ko.pureComputed(function () {
            var hasMessage = _this.hasMessage();
            var isDanger = _this.contextualClass.isDanger();
            return hasMessage && isDanger;
        });
        new ComponentTemplate_1.ComponentTemplate(this.componentName(), template).register();
    }
    return AlertViewModel;
}());
exports.AlertViewModel = AlertViewModel;
var Alert = /** @class */ (function () {
    function Alert(vm) {
        var _this = this;
        this.vm = vm;
        this.debouncedSetMessage = _.debounce(function (message) {
            _this.vm.message(message);
        }, 500);
    }
    Alert.prototype.clear = function () {
        this.setMessage('');
    };
    Alert.prototype.success = function (message) {
        ContextualClass_1.ContextualClass.success(this.vm.contextualClass);
        this.setMessage(message);
    };
    Alert.prototype.info = function (message) {
        ContextualClass_1.ContextualClass.info(this.vm.contextualClass);
        this.setMessage(message);
    };
    Alert.prototype.infoAction = function (message, a) {
        return tslib_1.__awaiter(this, void 0, void 0, function () {
            return tslib_1.__generator(this, function (_a) {
                switch (_a.label) {
                    case 0:
                        this.info(message);
                        _a.label = 1;
                    case 1:
                        _a.trys.push([1, , 3, 4]);
                        return [4 /*yield*/, a()];
                    case 2:
                        _a.sent();
                        return [3 /*break*/, 4];
                    case 3:
                        this.clear();
                        return [7 /*endfinally*/];
                    case 4: return [2 /*return*/];
                }
            });
        });
    };
    Alert.prototype.warning = function (message) {
        ContextualClass_1.ContextualClass.warning(this.vm.contextualClass);
        this.setMessage(message);
    };
    Alert.prototype.danger = function (message) {
        ContextualClass_1.ContextualClass.danger(this.vm.contextualClass);
        this.setMessage(message);
    };
    Alert.prototype.setMessage = function (message) {
        message = _.trim(message);
        if (message) {
            this.vm.message(message);
        }
        this.debouncedSetMessage(message);
    };
    return Alert;
}());
exports.Alert = Alert;
//# sourceMappingURL=Alert.js.map