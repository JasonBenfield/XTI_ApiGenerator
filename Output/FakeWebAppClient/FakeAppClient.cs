// Generated Code
using XTI_WebAppClient;
using System.Net.Http;

namespace FakeWebAppClient
{
    public sealed partial class FakeAppClient : AppClient
    {
        public FakeAppClient(IHttpClientFactory httpClientFactory, XtiToken xtiToken, string baseUrl, string version = DefaultVersion): base(httpClientFactory, baseUrl, "Fake", string.IsNullOrWhiteSpace(version) ? DefaultVersion : version)
        {
            this.xtiToken = xtiToken;
            User = new UserGroup(httpClientFactory, xtiToken, url);
            Employee = new EmployeeGroup(httpClientFactory, xtiToken, url);
            Product = new ProductGroup(httpClientFactory, xtiToken, url);
        }

        public const string DefaultVersion = "V1006";
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