// Generated Code
namespace FakeWebApp.ApiControllers;
[Authorize]
public class EmployeeController : Controller
{
    private readonly FakeAppApi api;
    public EmployeeController(FakeAppApi api)
    {
        this.api = api;
    }

    public async Task<IActionResult> Index(CancellationToken ct)
    {
        var result = await api.Group("Employee").Action<EmptyRequest, WebViewResult>("Index").Execute(new EmptyRequest(), ct);
        return View(result.Data.ViewName);
    }

    [HttpPost]
    public Task<ResultContainer<int>> AddEmployee([FromBody] AddEmployeeForm model, CancellationToken ct)
    {
        return api.Group("Employee").Action<AddEmployeeForm, int>("AddEmployee").Execute(model, ct);
    }

    [ResponseCache(CacheProfileName = "Default")]
    public async Task<IActionResult> AddEmployeeFormView(CancellationToken ct)
    {
        var result = await api.Group("Employee").Action<EmptyRequest, WebPartialViewResult>("AddEmployeeFormView").Execute(new EmptyRequest(), ct);
        return PartialView(result.Data.ViewName);
    }

    [HttpPost]
    public Task<ResultContainer<IDictionary<string, object>>> AddEmployeeForm(CancellationToken ct)
    {
        return api.Group("Employee").Action<EmptyRequest, IDictionary<string, object>>("AddEmployeeForm").Execute(new EmptyRequest(), ct);
    }

    [HttpPost]
    public Task<ResultContainer<Employee>> Employee([FromBody] int model, CancellationToken ct)
    {
        return api.Group("Employee").Action<int, Employee>("Employee").Execute(model, ct);
    }
}