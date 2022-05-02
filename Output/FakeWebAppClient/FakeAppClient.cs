// Generated Code
namespace FakeWebAppClient;
public sealed partial class FakeAppClient : AppClient
{
    public FakeAppClient(IHttpClientFactory httpClientFactory, XtiTokenAccessor xtiTokenAccessor, AppClientUrl clientUrl, FakeAppClientVersion version) : base(httpClientFactory, xtiTokenAccessor, clientUrl, "Fake", version.Value)
    {
        User = GetGroup((_clientFactory, _tokenAccessor, _url) => new UserGroup(_clientFactory, _tokenAccessor, _url));
        UserCache = GetGroup((_clientFactory, _tokenAccessor, _url) => new UserCacheGroup(_clientFactory, _tokenAccessor, _url));
        Employee = GetGroup((_clientFactory, _tokenAccessor, _url) => new EmployeeGroup(_clientFactory, _tokenAccessor, _url));
        Product = GetGroup((_clientFactory, _tokenAccessor, _url) => new ProductGroup(_clientFactory, _tokenAccessor, _url));
    }

    public FakeRoleNames RoleNames { get; } = FakeRoleNames.Instance;
    public string AppName { get; } = "Fake";
    public UserGroup User { get; }

    public UserCacheGroup UserCache { get; }

    public EmployeeGroup Employee { get; }

    public ProductGroup Product { get; }
}