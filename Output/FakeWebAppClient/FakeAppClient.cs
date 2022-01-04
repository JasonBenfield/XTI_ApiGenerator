// Generated Code
namespace FakeWebAppClient;
public sealed partial class FakeAppClient : AppClient
{
    public const string DefaultVersion = "V0";
    public FakeAppClient(IHttpClientFactory httpClientFactory, IXtiToken xtiToken, AppClientUrl clientUrl, string version = DefaultVersion) : base(httpClientFactory, clientUrl, "Fake", string.IsNullOrWhiteSpace(version) ? DefaultVersion : version)
    {
        this.xtiToken = xtiToken;
        User = new UserGroup(httpClientFactory, xtiToken, clientUrl);
        SetJsonSerializerOptions(User);
        UserCache = new UserCacheGroup(httpClientFactory, xtiToken, clientUrl);
        SetJsonSerializerOptions(UserCache);
        Employee = new EmployeeGroup(httpClientFactory, xtiToken, clientUrl);
        SetJsonSerializerOptions(Employee);
        Product = new ProductGroup(httpClientFactory, xtiToken, clientUrl);
        SetJsonSerializerOptions(Product);
    }

    public UserGroup User { get; }

    public UserCacheGroup UserCache { get; }

    public EmployeeGroup Employee { get; }

    public ProductGroup Product { get; }
}