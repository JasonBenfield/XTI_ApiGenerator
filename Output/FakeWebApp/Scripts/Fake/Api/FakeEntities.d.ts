// Generated code

interface IUserStartRequest {
	ReturnUrl: string;
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
	Departments: number[];
	CurrentProduct: IProduct;
	Rates: Record<string,number>;
}
interface IProduct {
	ID: number;
	Quantity: number;
	Price: number;
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