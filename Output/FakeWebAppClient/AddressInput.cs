// Generated code
using XTI_WebAppClient.Forms;
using System;

namespace FakeWebAppClient
{
    public sealed partial class AddressInput : ComplexField
    {
        public AddressInput(string prefix, string name): base(prefix, name)
        {
            ID1 = AddField(new HiddenField<string>(FieldName, nameof(ID1)));
            ID2 = AddField(new HiddenField<int>(FieldName, nameof(ID2)));
            Line1 = AddField(new InputField<string>(FieldName, nameof(Line1)));
            City = AddField(new InputField<string>(FieldName, nameof(City)));
            State = AddField(new InputField<string>(FieldName, nameof(State)));
            Zip = AddField(new InputField<int>(FieldName, nameof(Zip)));
        }

        public HiddenField<string> ID1 { get; }

        public HiddenField<int> ID2 { get; }

        public InputField<string> Line1 { get; }

        public InputField<string> City { get; }

        public InputField<string> State { get; }

        public InputField<int> Zip { get; }
    }
}