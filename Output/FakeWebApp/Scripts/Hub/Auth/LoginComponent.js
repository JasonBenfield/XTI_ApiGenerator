"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
exports.LoginComponent = exports.LoginResult = void 0;
var tslib_1 = require("tslib");
var AwaitableComponent_1 = require("../AwaitableComponent");
var TextInput_1 = require("../TextInput");
var Command_1 = require("../Command");
var ColumnCss_1 = require("../ColumnCss");
var UrlBuilder_1 = require("../UrlBuilder");
var tsyringe_1 = require("tsyringe");
var HubAppApi_1 = require("../Api/HubAppApi");
var LoginResult = /** @class */ (function () {
    function LoginResult(token) {
        this.token = token;
    }
    return LoginResult;
}());
exports.LoginResult = LoginResult;
var LoginComponent = /** @class */ (function (_super) {
    tslib_1.__extends(LoginComponent, _super);
    function LoginComponent(vm) {
        var _this = _super.call(this, vm.component) || this;
        _this.vm = vm;
        _this.userName = new TextInput_1.TextInput(_this.vm.userName);
        _this.password = new TextInput_1.PasswordInput(_this.vm.password);
        _this.loginCommand = new Command_1.AsyncCommand(_this.vm.loginCommand, _this.login.bind(_this));
        _this.userName.setColumns(new ColumnCss_1.ColumnCss(3), new ColumnCss_1.ColumnCss(0));
        _this.password.setColumns(new ColumnCss_1.ColumnCss(3), new ColumnCss_1.ColumnCss(0));
        _this.loginCommand.setText('Login');
        return _this;
    }
    LoginComponent.prototype.login = function () {
        return tslib_1.__awaiter(this, void 0, void 0, function () {
            var model, hub, form, userNameInput, passwordInput, urlBuilder, startUrlInput, returnUrlInput;
            return tslib_1.__generator(this, function (_a) {
                switch (_a.label) {
                    case 0:
                        this.alert.info('Verifying login...');
                        _a.label = 1;
                    case 1:
                        _a.trys.push([1, , 3, 4]);
                        model = {
                            UserName: this.userName.getValue(),
                            Password: this.password.getValue()
                        };
                        hub = tsyringe_1.container.resolve(HubAppApi_1.HubAppApi);
                        return [4 /*yield*/, hub.Auth.Login(model)];
                    case 2:
                        _a.sent();
                        this.alert.info('Opening page...');
                        form = document.createElement('form');
                        form.action = hub.Auth.Start.getUrl(null).getUrl();
                        form.style.position = 'absolute';
                        form.style.top = '-100px';
                        form.style.left = '-100px';
                        form.method = 'POST';
                        userNameInput = this.createInput('UserName', model.UserName, 'text');
                        passwordInput = this.createInput('Password', model.Password, 'password');
                        urlBuilder = UrlBuilder_1.UrlBuilder.current();
                        startUrlInput = this.createInput('StartUrl', urlBuilder.getQueryValue('startUrl'));
                        returnUrlInput = this.createInput('ReturnUrl', urlBuilder.getQueryValue('returnUrl'));
                        form.append(userNameInput, passwordInput, startUrlInput, returnUrlInput);
                        document.body.append(form);
                        form.submit();
                        return [3 /*break*/, 4];
                    case 3:
                        this.alert.clear();
                        return [7 /*endfinally*/];
                    case 4: return [2 /*return*/];
                }
            });
        });
    };
    LoginComponent.prototype.createInput = function (name, value, type) {
        if (type === void 0) { type = 'hidden'; }
        var input = document.createElement('input');
        input.type = type;
        input.name = name;
        input.value = value;
        return input;
    };
    return LoginComponent;
}(AwaitableComponent_1.BaseComponent));
exports.LoginComponent = LoginComponent;
//# sourceMappingURL=LoginComponent.js.map