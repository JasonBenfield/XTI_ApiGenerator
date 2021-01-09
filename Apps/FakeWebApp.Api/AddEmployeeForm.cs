using System;
using XTI_Forms;

namespace FakeWebApp.Api
{
    public sealed class AddEmployeeForm : Form
    {
        public AddEmployeeForm() : base("AddEmployeeForm")
        {
            Name = AddTextInput(nameof(Name));
            Name.MaxLength = 100;
            Name.SetValue("");
            BirthDate = AddDateInput(nameof(BirthDate));
            Department = AddInt32DropDown
            (
                nameof(Department),
                new DropDownItem<int?>(1, "HR"),
                new DropDownItem<int?>(2, "IT")
            );
            Department.ItemCaption = "Select...";
            Department.MustNotBeNull();
            Address = AddComplex(nameof(Address), (p, n) => new AddressInput(p, n, null));
            SSN = AddInt32Input(nameof(SSN));
            SSN.IsProtected = true;
            SSN.AddConstraints(Int32RangeConstraint.FromAbove(0).ToBelow(1000000000));
            HireDate = AddDateDropDown(
                nameof(HireDate),
                new DropDownItem<DateTimeOffset?>(DateTimeOffset.Now.Date.AddDays(-1), "Yesterday"),
                new DropDownItem<DateTimeOffset?>(DateTimeOffset.Now.Date, "Today"),
                new DropDownItem<DateTimeOffset?>(DateTimeOffset.Now.Date.AddDays(1), "Tomorrow")
            );
            IsTemp = AddBooleanDropDown(nameof(IsTemp));
        }

        public InputField<string> Name { get; }
        public InputField<DateTimeOffset?> BirthDate { get; }
        public DropDownField<int?> Department { get; }
        public AddressInput Address { get; }
        public InputField<int?> SSN { get; }
        public DropDownField<DateTimeOffset?> HireDate { get; }
        public DropDownField<bool?> IsTemp { get; }
    }

}
