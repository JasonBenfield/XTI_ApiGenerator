// Generated code

interface IUserStartRequest {
	ReturnUrl: string;
}
interface IClearUserCacheRequest {
	UserID: number;
	UserName: string;
}
interface IEmptyActionResult {
}
interface IEmptyRequest {
}
interface IEmployee {
	ID: number;
	Name: string;
	BirthDate: Date;
	EmployeeType: IEmployeeType;
}
interface IAddProductModel {
	Name: string;
	Quantity: number;
	Price: number;
}
interface IProduct {
	ID: number;
	Quantity: number;
	Price: number;
}
interface IEmployeeType {
	Value: number;
	DisplayText: string;
}