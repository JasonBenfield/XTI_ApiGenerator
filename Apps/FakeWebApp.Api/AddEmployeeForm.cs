using System;
using XTI_Forms;

namespace FakeWebApp.Api
{
    public sealed class AddEmployeeForm : Form
    {
        public AddEmployeeForm() : base("AddEmployeeForm")
        {
            EmployeeName = AddTextInput(nameof(EmployeeName));
            EmployeeName.MaxLength = 100;
            EmployeeName.SetValue("");
            BirthDate = AddDateInput(nameof(BirthDate));
            Department = AddInt32DropDown
            (
                nameof(Department),
                new DropDownItem<int?>(1, "HR"),
                new DropDownItem<int?>(2, "IT")
            );
            Department.ItemCaption = "Select...";
            Department.MustNotBeNull();
            Address = AddComplex(nameof(Address), (p, n) => new AddressInput(p, n));
            SSN = AddInt32Input(nameof(SSN));
            SSN.IsProtected = true;
            SSN.AddConstraints(Int32RangeConstraint.FromAbove(0).ToBelow(1000000000));
            HireDate = AddDateDropDown
            (
                nameof(HireDate),
                new DropDownItem<DateTimeOffset?>(DateTimeOffset.Now.Date.AddDays(-1), "Yesterday"),
                new DropDownItem<DateTimeOffset?>(DateTimeOffset.Now.Date, "Today"),
                new DropDownItem<DateTimeOffset?>(DateTimeOffset.Now.Date.AddDays(1), "Tomorrow")
            );
            IsTemp = AddBooleanDropDown(nameof(IsTemp));
            EmployeeID = AddInt32Hidden(nameof(EmployeeID), 32);
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

}
