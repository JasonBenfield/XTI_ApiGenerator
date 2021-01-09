using XTI_Forms;

namespace FakeWebApp.Api
{
    public sealed class AddressInput : ComplexField
    {
        public AddressInput(string prefix, string name)
            : base(prefix, name)
        {
            ID1 = AddTextHidden(nameof(ID1), "ID_1");
            ID2 = AddInt32Hidden(nameof(ID2), 2);
            Line1 = AddTextInput(nameof(Line1));
            Line1.MaxLength = 30;
            City = AddTextInput(nameof(City));
            State = AddTextInput(nameof(State));
            Zip = AddInt32Input(nameof(Zip));
        }

        public HiddenField<string> ID1 { get; }
        public HiddenField<int?> ID2 { get; }
        public InputField<string> Line1 { get; }
        public InputField<string> City { get; }
        public InputField<string> State { get; }
        public InputField<int?> Zip { get; }
    }
}
