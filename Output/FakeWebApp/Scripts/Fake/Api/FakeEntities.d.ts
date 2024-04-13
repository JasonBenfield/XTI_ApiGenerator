// Generated code

interface ILinkModel {
	LinkName: string;
	DisplayText: string;
	Url: string;
	IsAuthenticationRequired: boolean;
}
interface IEmployee {
	ID: number;
	Name: string;
	BirthDate: import('@jasonbenfield/sharedwebapp/Common').DateTimeOffset;
	EmployeeType: IEmployeeType;
	Departments: number[];
	CurrentProduct: IProduct;
	Rates: Record<string,number>;
	Status: Status;
	TimeEmployed: import('@jasonbenfield/sharedwebapp/Common').TimeSpan;
	DateHired: import('@jasonbenfield/sharedwebapp/Common').DateOnly;
	StartTime: import('@jasonbenfield/sharedwebapp/Common').TimeOnly;
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