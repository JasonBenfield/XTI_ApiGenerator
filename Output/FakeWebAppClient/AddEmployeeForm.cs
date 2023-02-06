// Generated Code
namespace FakeWebAppClient;
public sealed partial class AddEmployeeForm : Form
{
    public AddEmployeeForm() : base("AddEmployeeForm")
    {
        EmployeeName = AddTextInput(nameof(EmployeeName));
        BirthDate = AddDateInput(nameof(BirthDate));
        Department = AddInt32DropDown(nameof(Department));
        Address = AddComplex(nameof(Address), (p, n) => new AddressInput(p, n));
        SSN = AddInt32Input(nameof(SSN));
        HireDate = AddDateDropDown(nameof(HireDate));
        IsTemp = AddBooleanDropDown(nameof(IsTemp));
        EmployeeID = AddInt32Hidden(nameof(EmployeeID));
    }

    public InputField<string> EmployeeName { get; }

    public InputField<DateTimeOffset?> BirthDate { get; }

    public DropDownField<int?> Department { get; }

    public AddressInput Address { get; }

    public InputField<int?> SSN { get; }

    public DropDownField<DateTimeOffset?> HireDate { get; }

    public DropDownField<bool?> IsTemp { get; }

    public HiddenField<int?> EmployeeID { get; }
}