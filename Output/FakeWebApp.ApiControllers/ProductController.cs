// Generated Code
namespace FakeWebApp.ApiControllers;
[Authorize]
public class ProductController : Controller
{
    private readonly FakeAppApi api;
    public ProductController(FakeAppApi api)
    {
        this.api = api;
    }

    public async Task<IActionResult> Index(CancellationToken ct)
    {
        var result = await api.Group("Product").Action<EmptyRequest, WebViewResult>("Index").Execute(new EmptyRequest(), ct);
        return View(result.Data.ViewName);
    }

    [HttpPost]
    public Task<ResultContainer<string>> GetInfo(CancellationToken ct)
    {
        return api.Group("Product").Action<EmptyRequest, string>("GetInfo").Execute(new EmptyRequest(), ct);
    }

    [HttpPost]
    public Task<ResultContainer<int>> AddProduct([FromBody] AddProductModel model, CancellationToken ct)
    {
        return api.Group("Product").Action<AddProductModel, int>("AddProduct").Execute(model, ct);
    }

    [HttpPost]
    public Task<ResultContainer<Product>> Product([FromBody] int model, CancellationToken ct)
    {
        return api.Group("Product").Action<int, Product>("Product").Execute(model, ct);
    }
}