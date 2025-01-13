namespace FakeWebAppClient
{
    partial class FakeAppClient : ITest
    {
        public string Something { get; set; } = "";

        partial void Configure()
        {
            Something = "Whatever";
        }
    }
}
