namespace XTI_ApiGeneratorApp.Extensions;

public sealed class OutputOptions
{
    public static readonly string Output = "Output";

    public string DefaultVersion { get; set; } = "";
    public TsClientOptions TsClient { get; set; } = new TsClientOptions();
    public CsClientOptions CsClient { get; set; } = new CsClientOptions();
    public CsControllerOptions CsController { get; set; } = new CsControllerOptions();
}

public sealed class TsClientOptions
{
    public bool Disable { get; set; }
    public string OutputFolder { get; set; } = "";
}

public sealed class CsControllerOptions
{
    public bool Disable { get; set; }
    public string OutputFolder { get; set; } = "";
}

public sealed class CsClientOptions
{
    public bool Disable { get; set; }
    public string OutputFolder { get; set; } = "";
    public bool OutputClientExtensions { get; set; } = true;
}