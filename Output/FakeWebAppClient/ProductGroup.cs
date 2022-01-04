// Generated Code
namespace FakeWebAppClient;
public sealed partial class ProductGroup : AppClientGroup
{
    public ProductGroup(IHttpClientFactory httpClientFactory, IXtiToken xtiToken, AppClientUrl clientUrl) : base(httpClientFactory, xtiToken, clientUrl, "Product")
    {
    }

    public Task<string> GetInfo() => Post<string, EmptyRequest>("GetInfo", "", new EmptyRequest());
    public Task<int> AddProduct(AddProductModel model) => Post<int, AddProductModel>("AddProduct", "", model);
    public Task<Product> Product(int model) => Post<Product, int>("Product", "", model);
}