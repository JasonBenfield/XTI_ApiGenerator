// Generated Code
namespace FakeWebAppClient;
public sealed partial class FakeAppClient : AppClient
{
    public FakeAppClient(IHttpClientFactory httpClientFactory, XtiTokenAccessor xtiTokenAccessor, AppClientUrl clientUrl, FakeAppClientVersion version) : base(httpClientFactory, xtiTokenAccessor, clientUrl, "Fake", version.Value)
    {
        Employee = CreateGroup((_clientFactory, _tokenAccessor, _url, _options) => new EmployeeGroup(_clientFactory, _tokenAccessor, _url, _options));
        EmployeeQuery = CreateODataGroup<QueryEmployeesRequest, Employee>("EmployeeQuery");
        Product = CreateGroup((_clientFactory, _tokenAccessor, _url, _options) => new ProductGroup(_clientFactory, _tokenAccessor, _url, _options));
    }

    public FakeRoleNames RoleNames { get; } = FakeRoleNames.Instance;
    public string AppName { get; } = "Fake";
    public EmployeeGroup Employee { get; }

    public AppClientODataGroup<QueryEmployeesRequest, Employee> EmployeeQuery { get; }

    public ProductGroup Product { get; }
}