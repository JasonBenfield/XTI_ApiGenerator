// Generated Code
namespace FakeWebAppClient;
public sealed partial class FakeAppClientFactory
{
    private readonly IHttpClientFactory httpClientFactory;
    private readonly XtiTokenAccessorFactory xtiTokenAccessorFactory;
    private readonly AppClientUrl clientUrl;
    private readonly IAppClientRequestKey requestKey;
    private readonly FakeAppClientVersion version;
    public FakeAppClientFactory(IHttpClientFactory httpClientFactory, XtiTokenAccessorFactory xtiTokenAccessorFactory, AppClientUrl clientUrl, IAppClientRequestKey requestKey, FakeAppClientVersion version)
    {
        this.httpClientFactory = httpClientFactory;
        this.xtiTokenAccessorFactory = xtiTokenAccessorFactory;
        this.clientUrl = clientUrl;
        this.requestKey = requestKey;
        this.version = version;
    }

    public FakeAppClient Create() => new FakeAppClient(httpClientFactory, xtiTokenAccessorFactory, clientUrl, requestKey, version);
}