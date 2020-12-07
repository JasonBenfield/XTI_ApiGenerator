namespace XTI_ApiGeneratorApp.Extensions
{
    public sealed class OutputOptions
    {
        public static readonly string Output = "Output";

        public TsClientOptions TsClient { get; set; }
        public CsClientOptions CsClient { get; set; }
        public CsControllerOptions CsController { get; set; }
    }

    public sealed class TsClientOptions
    {
        public bool Disable { get; set; }
        public string OutputFolder { get; set; }
    }

    public sealed class CsControllerOptions
    {
        public bool Disable { get; set; }
        public string OutputFolder { get; set; }
        public string Namespace { get; set; }
        public string[] AdditionalNamespaces { get; set; }
    }

    public sealed class CsClientOptions
    {
        public bool Disable { get; set; }
        public string OutputFolder { get; set; }
        public string Namespace { get; set; }
    }
}
