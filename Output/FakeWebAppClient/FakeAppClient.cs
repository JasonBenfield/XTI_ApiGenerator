// Generated Code
using XTI_WebAppClient;
using System.Net.Http;

namespace FakeWebAppClient
{
    public sealed partial class FakeAppClient : AppClient
    {
        public FakeAppClient(IHttpClientFactory httpClientFactory, XtiToken xtiToken, string baseUrl, string version = "V1006"): base(httpClientFactory, baseUrl, "Fake", version)
        {
            this.xtiToken = xtiToken;
            User = new UserGroup(httpClientFactory, xtiToken, url);
            Employee = new EmployeeGroup(httpClientFactory, xtiToken, url);
            Product = new ProductGroup(httpClientFactory, xtiToken, url);
        }

        public UserGroup User
        {
            get;
        }

        public EmployeeGroup Employee
        {
            get;
        }

        public ProductGroup Product
        {
            get;
        }
    }
}