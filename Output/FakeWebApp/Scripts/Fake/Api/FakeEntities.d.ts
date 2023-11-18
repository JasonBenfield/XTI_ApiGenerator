// Generated code

interface ILinkModel {
	LinkName: string;
	DisplayText: string;
	Url: string;
}
interface IEmployee {
	ID: number;
	Name: string;
	BirthDate: Date;
	EmployeeType: IEmployeeType;
	Departments: number[];
	CurrentProduct: IProduct;
	Rates: Record<string,number>;
	Status: Status;
	TimeEmployed: string;
}
interface IProduct {
	ID: number;
	Quantity: number;
	Price: number;
}
interface IWebContentResult {
	Content: string;
	ContentType: string;
}
interface IQueryEmployeesRequest {
	Department: string;
}
interface IAddProductModel {
	Name: string;
	Quantity: number;
	Price: number;
	Attachment: File;
	SubProduct: IAddSubProductModel;
}
interface IAddSubProductModel {
	Attachments: File[];
}
interface IEmployeeType {
	Value: number;
	DisplayText: string;
}