// Generated Code
namespace FakeWebAppClient;
public sealed partial class FakeAppClientFactory
{
    private readonly IHttpClientFactory httpClientFactory;
    private readonly XtiTokenAccessorFactory xtiTokenAccessorFactory;
    private readonly AppClientUrl clientUrl;
    private readonly AppClientOptions options;
    private readonly FakeAppClientVersion version;
    public FakeAppClientFactory(IHttpClientFactory httpClientFactory, XtiTokenAccessorFactory xtiTokenAccessorFactory, AppClientUrl clientUrl, AppClientOptions options, FakeAppClientVersion version)
    {
        this.httpClientFactory = httpClientFactory;
        this.xtiTokenAccessorFactory = xtiTokenAccessorFactory;
        this.clientUrl = clientUrl;
        this.options = options;
        this.version = version;
    }

    public FakeAppClient Create() => new FakeAppClient(httpClientFactory, xtiTokenAccessorFactory, clientUrl, options, version);
}