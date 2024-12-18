// Generated Code
namespace FakeWebAppClient;
public sealed partial class ProductGroup : AppClientGroup
{
    public ProductGroup(IHttpClientFactory httpClientFactory, XtiTokenAccessor xtiTokenAccessor, AppClientUrl clientUrl, AppClientOptions options) : base(httpClientFactory, xtiTokenAccessor, clientUrl, options, "Product")
    {
        Actions = new ProductGroupActions(Index: CreateGetAction<EmptyRequest>("Index"), GetInfo: CreatePostAction<EmptyRequest, string>("GetInfo"), AddProduct: CreatePostAction<AddProductModel, int>("AddProduct"), Product: CreatePostAction<int, Product>("Product"));
    }

    public ProductGroupActions Actions { get; }

    public Task<string> GetInfo(CancellationToken ct = default) => Actions.GetInfo.Post("", new EmptyRequest(), ct);
    public Task<int> AddProduct(AddProductModel requestData, CancellationToken ct = default) => Actions.AddProduct.Post("", requestData, ct);
    public Task<Product> Product(int requestData, CancellationToken ct = default) => Actions.Product.Post("", requestData, ct);
    public sealed record ProductGroupActions(AppClientGetAction<EmptyRequest> Index, AppClientPostAction<EmptyRequest, string> GetInfo, AppClientPostAction<AddProductModel, int> AddProduct, AppClientPostAction<int, Product> Product);
}