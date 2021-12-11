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

    public async Task<IActionResult> Index()
    {
        var result = await api.Group("Product").Action<EmptyRequest, WebViewResult>("Index").Execute(new EmptyRequest());
        return View(result.Data.ViewName);
    }

    [HttpPost]
    public Task<ResultContainer<string>> GetInfo()
    {
        return api.Group("Product").Action<EmptyRequest, string>("GetInfo").Execute(new EmptyRequest());
    }

    [HttpPost]
    public Task<ResultContainer<int>> AddProduct([FromBody] AddProductModel model)
    {
        return api.Group("Product").Action<AddProductModel, int>("AddProduct").Execute(model);
    }

    [HttpPost]
    public Task<ResultContainer<Product>> Product([FromBody] int model)
    {
        return api.Group("Product").Action<int, Product>("Product").Execute(model);
    }
}