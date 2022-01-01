// Generated Code
namespace FakeWebAppClient;
public sealed partial class FakeAppClient : AppClient
{
    public const string DefaultVersion = "V0";
    public FakeAppClient(IHttpClientFactory httpClientFactory, IXtiToken xtiToken, string baseUrl, string version = DefaultVersion) : base(httpClientFactory, baseUrl, "Fake", string.IsNullOrWhiteSpace(version) ? DefaultVersion : version)
    {
        this.xtiToken = xtiToken;
        User = new UserGroup(httpClientFactory, xtiToken, url);
        SetJsonSerializerOptions(User);
        UserCache = new UserCacheGroup(httpClientFactory, xtiToken, url);
        SetJsonSerializerOptions(UserCache);
        Employee = new EmployeeGroup(httpClientFactory, xtiToken, url);
        SetJsonSerializerOptions(Employee);
        Product = new ProductGroup(httpClientFactory, xtiToken, url);
        SetJsonSerializerOptions(Product);
    }

    public UserGroup User { get; }

    public UserCacheGroup UserCache { get; }

    public EmployeeGroup Employee { get; }

    public ProductGroup Product { get; }
}