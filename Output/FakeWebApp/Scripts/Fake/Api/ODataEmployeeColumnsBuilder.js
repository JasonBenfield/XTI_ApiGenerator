"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
exports.ODataEmployeeColumnsBuilder = exports.ODataEmployeeColumnViewsBuilder = void 0;
// Generated code
const ODataColumnBuilder_1 = require("@jasonbenfield/sharedwebapp/OData/ODataColumnBuilder");
const ODataColumnViewBuilder_1 = require("@jasonbenfield/sharedwebapp/OData/ODataColumnViewBuilder");
const SourceType_1 = require("@jasonbenfield/sharedwebapp/OData/SourceType");
class ODataEmployeeColumnViewsBuilder {
    constructor() {
        this.ID = new ODataColumnViewBuilder_1.ODataColumnViewBuilder();
        this.Name = new ODataColumnViewBuilder_1.ODataColumnViewBuilder();
        this.BirthDate = new ODataColumnViewBuilder_1.ODataColumnViewBuilder();
        this.EmployeeType = new ODataColumnViewBuilder_1.ODataColumnViewBuilder();
        this.Departments = new ODataColumnViewBuilder_1.ODataColumnViewBuilder();
        this.CurrentProduct = new ODataColumnViewBuilder_1.ODataColumnViewBuilder();
        this.Rates = new ODataColumnViewBuilder_1.ODataColumnViewBuilder();
        this.Status = new ODataColumnViewBuilder_1.ODataColumnViewBuilder();
        this.TimeEmployed = new ODataColumnViewBuilder_1.ODataColumnViewBuilder();
    }
}
exports.ODataEmployeeColumnViewsBuilder = ODataEmployeeColumnViewsBuilder;
class ODataEmployeeColumnsBuilder {
    constructor(views) {
        this.ID = new ODataColumnBuilder_1.ODataColumnBuilder('ID', new SourceType_1.SourceType('Int32'), views.ID);
        this.Name = new ODataColumnBuilder_1.ODataColumnBuilder('Name', new SourceType_1.SourceType('String'), views.Name);
        this.BirthDate = new ODataColumnBuilder_1.ODataColumnBuilder('BirthDate', new SourceType_1.SourceType('DateTime'), views.BirthDate);
        this.BirthDate.setDisplayText('Birth Date');
        this.EmployeeType = new ODataColumnBuilder_1.ODataColumnBuilder('EmployeeType', new SourceType_1.SourceType('EmployeeType'), views.EmployeeType);
        this.EmployeeType.setDisplayText('Employee Type');
        this.Departments = new ODataColumnBuilder_1.ODataColumnBuilder('Departments', new SourceType_1.SourceType('Int32[]'), views.Departments);
        this.CurrentProduct = new ODataColumnBuilder_1.ODataColumnBuilder('CurrentProduct', new SourceType_1.SourceType('Product'), views.CurrentProduct);
        this.CurrentProduct.setDisplayText('Current Product');
        this.Rates = new ODataColumnBuilder_1.ODataColumnBuilder('Rates', new SourceType_1.SourceType('IDictionary`2'), views.Rates);
        this.Status = new ODataColumnBuilder_1.ODataColumnBuilder('Status', new SourceType_1.SourceType('Status'), views.Status);
        this.TimeEmployed = new ODataColumnBuilder_1.ODataColumnBuilder('TimeEmployed', new SourceType_1.SourceType('TimeSpan'), views.TimeEmployed);
        this.TimeEmployed.setDisplayText('Time Employed');
    }
    build() {
        return {
            ID: this.ID.build(),
            Name: this.Name.build(),
            BirthDate: this.BirthDate.build(),
            EmployeeType: this.EmployeeType.build(),
            Departments: this.Departments.build(),
            CurrentProduct: this.CurrentProduct.build(),
            Rates: this.Rates.build(),
            Status: this.Status.build(),
            TimeEmployed: this.TimeEmployed.build()
        };
    }
}
exports.ODataEmployeeColumnsBuilder = ODataEmployeeColumnsBuilder;
//# sourceMappingURL=ODataEmployeeColumnsBuilder.js.map