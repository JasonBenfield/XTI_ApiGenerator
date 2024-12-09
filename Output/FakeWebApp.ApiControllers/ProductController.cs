// Generated Code
namespace FakeWebApp.ApiControllers;
[Authorize]
public sealed partial class ProductController : Controller
{
    private readonly FakeAppApi api;
    public ProductController(FakeAppApi api)
    {
        this.api = api;
    }

    public async Task<IActionResult> Index(CancellationToken ct)
    {
        var result = await api.Product.Index.Execute(new EmptyRequest(), ct);
        return View(result.Data!.ViewName);
    }

    [HttpPost]
    public Task<ResultContainer<string>> GetInfo(CancellationToken ct)
    {
        return api.Product.GetInfo.Execute(new EmptyRequest(), ct);
    }

    [HttpPost]
    public Task<ResultContainer<int>> AddProduct(AddProductModel requestData, CancellationToken ct)
    {
        return api.Product.AddProduct.Execute(requestData, ct);
    }

    [HttpPost]
    public Task<ResultContainer<Product>> Product([FromBody] int requestData, CancellationToken ct)
    {
        return api.Product.Product.Execute(requestData, ct);
    }
}