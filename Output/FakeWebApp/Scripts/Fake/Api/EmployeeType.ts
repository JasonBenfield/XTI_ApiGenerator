// Generated code
import { NumericValue } from 'XtiShared/NumericValue';
import { NumericValues } from 'XtiShared/NumericValues';

export class EmployeeTypes extends NumericValues<EmployeeType> {
	constructor(
		public readonly None: EmployeeType,
		public readonly Temp: EmployeeType,
		public readonly Permanent: EmployeeType
	) {
		super([None,Temp,Permanent]);
	}
}

export class EmployeeType extends NumericValue implements IEmployeeType {
	public static readonly values = new EmployeeTypes(
		new EmployeeType(0, 'None'),
		new EmployeeType(10, 'Temp'),
		new EmployeeType(15, 'Permanent')
	);
	
	private constructor(Value: number, DisplayText: string) {
		super(Value, DisplayText);
	}
}