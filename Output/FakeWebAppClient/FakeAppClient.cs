// Generated Code
using XTI_WebAppClient;
using System.Net.Http;

namespace FakeWebAppClient
{
    public sealed class FakeAppClient : AppClient
    {
        public FakeAppClient(IHttpClientFactory httpClientFactory, XtiToken xtiToken, string baseUrl): base(httpClientFactory, baseUrl, "Fake")
        {
            this.xtiToken = xtiToken;
            Employee = new EmployeeGroup(httpClientFactory, xtiToken, url);
            Product = new ProductGroup(httpClientFactory, xtiToken, url);
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