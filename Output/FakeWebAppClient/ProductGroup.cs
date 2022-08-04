// Generated Code
namespace FakeWebAppClient;
public sealed partial class ProductGroup : AppClientGroup
{
    public ProductGroup(IHttpClientFactory httpClientFactory, XtiTokenAccessor xtiTokenAccessor, AppClientUrl clientUrl, AppClientOptions options) : base(httpClientFactory, xtiTokenAccessor, clientUrl, options, "Product")
    {
        Actions = new ProductGroupActions(Index: CreateGetAction<EmptyRequest>("Index"), GetInfo: CreatePostAction<EmptyRequest, string>("GetInfo"), AddProduct: CreatePostAction<AddProductModel, int>("AddProduct"), Product: CreatePostAction<int, Product>("Product"));
    }

    public ProductGroupActions Actions { get; }

    public Task<string> GetInfo() => Actions.GetInfo.Post("", new EmptyRequest());
    public Task<int> AddProduct(AddProductModel model) => Actions.AddProduct.Post("", model);
    public Task<Product> Product(int model) => Actions.Product.Post("", model);
    public sealed record ProductGroupActions(AppClientGetAction<EmptyRequest> Index, AppClientPostAction<EmptyRequest, string> GetInfo, AppClientPostAction<AddProductModel, int> AddProduct, AppClientPostAction<int, Product> Product);
}