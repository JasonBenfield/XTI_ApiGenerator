// Generated Code
namespace FakeWebApp.ApiControllers;
[Authorize]
public sealed partial class EmployeeController : Controller
{
    private readonly FakeAppApi api;
    public EmployeeController(FakeAppApi api)
    {
        this.api = api;
    }

    public async Task<IActionResult> Index(CancellationToken ct)
    {
        var result = await api.Employee.Index.Execute(new EmptyRequest(), ct);
        return View(result.Data!.ViewName);
    }

    [HttpPost]
    public Task<ResultContainer<int>> AddEmployee([FromBody] AddEmployeeForm requestData, CancellationToken ct)
    {
        return api.Employee.AddEmployee.Execute(requestData, ct);
    }

    [ResponseCache(CacheProfileName = "Default")]
    public async Task<IActionResult> AddEmployeeFormView(CancellationToken ct)
    {
        var result = await api.Employee.AddEmployeeFormView.Execute(new EmptyRequest(), ct);
        return PartialView(result.Data!.ViewName);
    }

    [HttpPost]
    public Task<ResultContainer<IDictionary<string, object>>> AddEmployeeForm(CancellationToken ct)
    {
        return api.Employee.AddEmployeeForm.Execute(new EmptyRequest(), ct);
    }

    [HttpPost]
    public Task<ResultContainer<Employee>> Employee([FromBody] int requestData, CancellationToken ct)
    {
        return api.Employee.Employee.Execute(requestData, ct);
    }

    public async Task<IActionResult> DownloadAttachment(CancellationToken ct)
    {
        var result = await api.Employee.DownloadAttachment.Execute(new EmptyRequest(), ct);
        return File(result.Data!.FileStream, result.Data!.ContentType, result.Data!.DownloadName);
    }

    [HttpPost]
    public async Task<IActionResult> GetContent(CancellationToken ct)
    {
        var result = await api.Employee.GetContent.Execute(new EmptyRequest(), ct);
        return Content(result.Data!.Content, result.Data.ContentType);
    }
}