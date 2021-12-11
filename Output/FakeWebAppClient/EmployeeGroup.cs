// Generated Code
namespace FakeWebAppClient;
public sealed partial class EmployeeGroup : AppClientGroup
{
    public EmployeeGroup(IHttpClientFactory httpClientFactory, IXtiToken xtiToken, string baseUrl) : base(httpClientFactory, xtiToken, baseUrl, "Employee")
    {
    }

    public Task<int> AddEmployee(AddEmployeeForm model) => Post<int, AddEmployeeForm>("AddEmployee", "", model);
    public Task<IDictionary<string, object>> AddEmployeeForm() => Post<IDictionary<string, object>, EmptyRequest>("AddEmployeeForm", "", new EmptyRequest());
    public Task<Employee> Employee(int model) => Post<Employee, int>("Employee", "", model);
}