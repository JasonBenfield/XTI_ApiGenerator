// Generated code
import { ODataColumnBuilder } from "@jasonbenfield/sharedwebapp/OData/ODataColumnBuilder";
import { ODataColumnViewBuilder } from "@jasonbenfield/sharedwebapp/OData/ODataColumnViewBuilder";
import { SourceType } from "@jasonbenfield/sharedwebapp/OData/SourceType";
import { ODataColumns } from "@jasonbenfield/sharedwebapp/OData/Types";

export class ODataEmployeeColumnViewsBuilder {
	readonly ID = new ODataColumnViewBuilder();
	readonly Name = new ODataColumnViewBuilder();
	readonly BirthDate = new ODataColumnViewBuilder();
	readonly EmployeeType = new ODataColumnViewBuilder();
	readonly Departments = new ODataColumnViewBuilder();
	readonly CurrentProduct = new ODataColumnViewBuilder();
	readonly Rates = new ODataColumnViewBuilder();
	readonly Status = new ODataColumnViewBuilder();
	readonly TimeEmployed = new ODataColumnViewBuilder();
}

export class ODataEmployeeColumnsBuilder {
	constructor(views: ODataEmployeeColumnViewsBuilder) {
		this.ID = new ODataColumnBuilder('ID', new SourceType('Int32'), views.ID);
		this.Name = new ODataColumnBuilder('Name', new SourceType('String'), views.Name);
		this.BirthDate = new ODataColumnBuilder('BirthDate', new SourceType('DateTime'), views.BirthDate);
		this.BirthDate.setDisplayText('Birth Date');
		this.EmployeeType = new ODataColumnBuilder('EmployeeType', new SourceType('EmployeeType'), views.EmployeeType);
		this.EmployeeType.setDisplayText('Employee Type');
		this.Departments = new ODataColumnBuilder('Departments', new SourceType('Int32[]'), views.Departments);
		this.CurrentProduct = new ODataColumnBuilder('CurrentProduct', new SourceType('Product'), views.CurrentProduct);
		this.CurrentProduct.setDisplayText('Current Product');
		this.Rates = new ODataColumnBuilder('Rates', new SourceType('IDictionary`2'), views.Rates);
		this.Status = new ODataColumnBuilder('Status', new SourceType('Status'), views.Status);
		this.TimeEmployed = new ODataColumnBuilder('TimeEmployed', new SourceType('TimeSpan'), views.TimeEmployed);
		this.TimeEmployed.setDisplayText('Time Employed');
	}
	readonly ID: ODataColumnBuilder;
	readonly Name: ODataColumnBuilder;
	readonly BirthDate: ODataColumnBuilder;
	readonly EmployeeType: ODataColumnBuilder;
	readonly Departments: ODataColumnBuilder;
	readonly CurrentProduct: ODataColumnBuilder;
	readonly Rates: ODataColumnBuilder;
	readonly Status: ODataColumnBuilder;
	readonly TimeEmployed: ODataColumnBuilder;
	
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
		} as ODataColumns<IEmployee>;
	}
}