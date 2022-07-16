// Generated code

interface IUserStartRequest {
	ReturnUrl: string;
}
interface IEmptyRequest {
}
interface ILogoutRequest {
	ReturnUrl: string;
}
interface IEmptyActionResult {
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
interface IWebFileResult {
	FileStream: IStream;
	ContentType: string;
	DownloadName: string;
}
interface IStream {
	CanRead: boolean;
	CanWrite: boolean;
	CanSeek: boolean;
	CanTimeout: boolean;
	Length: number;
	Position: number;
	ReadTimeout: number;
	WriteTimeout: number;
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
}
interface IEmployeeType {
	Value: number;
	DisplayText: string;
}