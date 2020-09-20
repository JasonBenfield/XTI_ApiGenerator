namespace XTI_ApiGeneratorApp.Extensions
{
    public sealed class OutputOptions
    {
        public static readonly string Output = "Output";

        public string TsOutputFolder { get; set; }
        public CsClientOptions CsClient { get; set; }
        public CsControllerOptions CsController { get; set; }
    }

    public sealed class CsControllerOptions
    {
        public string OutputFolder { get; set; }
        public string Namespace { get; set; }
        public string[] AdditionalNamespaces { get; set; }
    }

    public sealed class CsClientOptions
    {
        public string OutputFolder { get; set; }
        public string Namespace { get; set; }
    }
}
