// Generated Code
namespace FakeWebAppClient;
public sealed partial class ProductGroup : AppClientGroup
{
    public ProductGroup(IHttpClientFactory httpClientFactory, XtiTokenAccessor xtiTokenAccessor, AppClientUrl clientUrl) : base(httpClientFactory, xtiTokenAccessor, clientUrl, "Product")
    {
        Actions = new ProductActions(clientUrl);
    }

    public ProductActions Actions { get; }

    public Task<string> GetInfo() => Post<string, EmptyRequest>("GetInfo", "", new EmptyRequest());
    public Task<int> AddProduct(AddProductModel model) => Post<int, AddProductModel>("AddProduct", "", model);
    public Task<Product> Product(int model) => Post<Product, int>("Product", "", model);
}