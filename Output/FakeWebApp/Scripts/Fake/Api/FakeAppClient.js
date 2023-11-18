"use strict";
// Generated code
Object.defineProperty(exports, "__esModule", { value: true });
exports.FakeAppClient = void 0;
const AppClient_1 = require("@jasonbenfield/sharedwebapp/Http/AppClient");
const AppClientQuery_1 = require("@jasonbenfield/sharedwebapp/Http/AppClientQuery");
const EmployeeGroup_1 = require("./EmployeeGroup");
const ProductGroup_1 = require("./ProductGroup");
class FakeAppClient extends AppClient_1.AppClient {
    constructor(events) {
        super(events, 'Fake', pageContext.EnvironmentName === 'Production' || pageContext.EnvironmentName === 'Staging' ? 'V00000' : 'Current');
        this.Employee = this.addGroup((evts, resourceUrl) => new EmployeeGroup_1.EmployeeGroup(evts, resourceUrl));
        this.EmployeeQuery = this.addODataGroup((evts, resourceUrl) => new AppClientQuery_1.AppClientQuery(evts, resourceUrl.odata('EmployeeQuery'), 'EmployeeQuery'));
        this.Product = this.addGroup((evts, resourceUrl) => new ProductGroup_1.ProductGroup(evts, resourceUrl));
    }
}
exports.FakeAppClient = FakeAppClient;
//# sourceMappingURL=FakeAppClient.js.map