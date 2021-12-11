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

    public async Task<IActionResult> Index()
    {
        var result = await api.Group("Employee").Action<EmptyRequest, WebViewResult>("Index").Execute(new EmptyRequest());
        return View(result.Data.ViewName);
    }

    [HttpPost]
    public Task<ResultContainer<int>> AddEmployee([FromBody] AddEmployeeForm model)
    {
        return api.Group("Employee").Action<AddEmployeeForm, int>("AddEmployee").Execute(model);
    }

    [ResponseCache(CacheProfileName = "Default")]
    public async Task<IActionResult> AddEmployeeFormView()
    {
        var result = await api.Group("Employee").Action<EmptyRequest, WebPartialViewResult>("AddEmployeeFormView").Execute(new EmptyRequest());
        return PartialView(result.Data.ViewName);
    }

    [HttpPost]
    public Task<ResultContainer<IDictionary<string, object>>> AddEmployeeForm()
    {
        return api.Group("Employee").Action<EmptyRequest, IDictionary<string, object>>("AddEmployeeForm").Execute(new EmptyRequest());
    }

    [HttpPost]
    public Task<ResultContainer<Employee>> Employee([FromBody] int model)
    {
        return api.Group("Employee").Action<int, Employee>("Employee").Execute(model);
    }
}