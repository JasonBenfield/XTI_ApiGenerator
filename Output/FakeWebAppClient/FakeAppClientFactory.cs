// Generated Code
namespace FakeWebAppClient;
public sealed partial class FakeAppClientFactory
{
    private readonly IHttpClientFactory httpClientFactory;
    private readonly XtiTokenAccessorFactory xtiTokenAccessorFactory;
    private readonly AppClientUrl clientUrl;
    private readonly IAppClientSessionKey sessionKey;
    private readonly IAppClientRequestKey requestKey;
    private readonly FakeAppClientVersion version;
    public FakeAppClientFactory(IHttpClientFactory httpClientFactory, XtiTokenAccessorFactory xtiTokenAccessorFactory, AppClientUrl clientUrl, IAppClientSessionKey sessionKey, IAppClientRequestKey requestKey, FakeAppClientVersion version)
    {
        this.httpClientFactory = httpClientFactory;
        this.xtiTokenAccessorFactory = xtiTokenAccessorFactory;
        this.clientUrl = clientUrl;
        this.sessionKey = sessionKey;
        this.requestKey = requestKey;
        this.version = version;
    }

    public FakeAppClient Create() => new FakeAppClient(httpClientFactory, xtiTokenAccessorFactory, clientUrl, sessionKey, requestKey, version);
}