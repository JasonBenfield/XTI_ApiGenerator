// Generated code
using XTI_WebAppClient.Forms;
using System;

namespace FakeWebAppClient
{
    public sealed partial class AddEmployeeForm : Form
    {
        public AddEmployeeForm(string name): base(name)
        {
            EmployeeName = AddField(new InputField<string>(FieldName, nameof(EmployeeName)));
            BirthDate = AddField(new InputField<DateTimeOffset?>(FieldName, nameof(BirthDate)));
            Department = AddField(new DropDownField<int>(FieldName, nameof(Department)));
            Address = AddField(new AddressInput(FieldName, nameof(Address)));
            SSN = AddField(new InputField<int>(FieldName, nameof(SSN)));
            HireDate = AddField(new DropDownField<DateTimeOffset?>(FieldName, nameof(HireDate)));
            IsTemp = AddField(new DropDownField<bool>(FieldName, nameof(IsTemp)));
            EmployeeID = AddField(new HiddenField<int>(FieldName, nameof(EmployeeID)));
        }

        public InputField<string> EmployeeName
        {
            get;
        }

        public InputField<DateTimeOffset?> BirthDate
        {
            get;
        }

        public DropDownField<int> Department
        {
            get;
        }

        public AddressInput Address
        {
            get;
        }

        public InputField<int> SSN
        {
            get;
        }

        public DropDownField<DateTimeOffset?> HireDate
        {
            get;
        }

        public DropDownField<bool> IsTemp
        {
            get;
        }

        public HiddenField<int> EmployeeID
        {
            get;
        }
    }
}