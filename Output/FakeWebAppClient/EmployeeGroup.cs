// Generated Code
using XTI_WebAppClient;
using System.Net.Http;
using System.Threading.Tasks;

namespace FakeWebAppClient
{
    public sealed partial class EmployeeGroup : AppClientGroup
    {
        public EmployeeGroup(IHttpClientFactory httpClientFactory, XtiToken xtiToken, string baseUrl): base(httpClientFactory, xtiToken, baseUrl, "Employee")
        {
        }

        public Task<int> AddEmployee(string modifier, AddEmployeeModel model) => Post<int, AddEmployeeModel>("AddEmployee", modifier, model);
        public Task<Employee> Employee(string modifier, int model) => Post<Employee, int>("Employee", modifier, model);
    }
}