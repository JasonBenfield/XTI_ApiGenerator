// Generated Code
namespace FakeWebAppClient;
public sealed partial class FakeAppClient : AppClient
{
    public const string DefaultVersion = "V0";
    public FakeAppClient(IHttpClientFactory httpClientFactory, IXtiToken xtiToken, AppClientUrl clientUrl, string version = DefaultVersion) : base(httpClientFactory, clientUrl, "Fake", string.IsNullOrWhiteSpace(version) ? DefaultVersion : version)
    {
        this.xtiToken = xtiToken;
        User = GetGroup((_clientFactory, _token, _url) => new UserGroup(_clientFactory, _token, _url));
        UserCache = GetGroup((_clientFactory, _token, _url) => new UserCacheGroup(_clientFactory, _token, _url));
        Employee = GetGroup((_clientFactory, _token, _url) => new EmployeeGroup(_clientFactory, _token, _url));
        Product = GetGroup((_clientFactory, _token, _url) => new ProductGroup(_clientFactory, _token, _url));
    }

    public UserGroup User { get; }

    public UserCacheGroup UserCache { get; }

    public EmployeeGroup Employee { get; }

    public ProductGroup Product { get; }
}