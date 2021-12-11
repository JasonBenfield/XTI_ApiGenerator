// Generated Code
namespace FakeWebAppClient;
public sealed partial class FakeAppClient : AppClient
{
    public FakeAppClient(IHttpClientFactory httpClientFactory, IXtiToken xtiToken, string baseUrl, string version = DefaultVersion) : base(httpClientFactory, baseUrl, "Fake", string.IsNullOrWhiteSpace(version) ? DefaultVersion : version)
    {
        this.xtiToken = xtiToken;
        User = new UserGroup(httpClientFactory, xtiToken, url);
        UserCache = new UserCacheGroup(httpClientFactory, xtiToken, url);
        Employee = new EmployeeGroup(httpClientFactory, xtiToken, url);
        Product = new ProductGroup(httpClientFactory, xtiToken, url);
    }

    public const string DefaultVersion = "V0";
    public UserGroup User { get; }

    public UserCacheGroup UserCache { get; }

    public EmployeeGroup Employee { get; }

    public ProductGroup Product { get; }
}