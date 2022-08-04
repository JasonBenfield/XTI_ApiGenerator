using Microsoft.AspNetCore.OData.Query;
using Microsoft.Extensions.DependencyInjection;
using XTI_App.Abstractions;
using XTI_App.Api;
using XTI_Core;
using XTI_ODataQuery.Api;
using XTI_WebApp.Api;

namespace FakeWebApp.Api;

public static class FakeAppKey
{
    public static readonly AppKey AppKey = AppKey.WebApp("Fake");
}
public sealed class FakeAppApi : WebAppApiWrapper
{

    public FakeAppApi(IAppApiUser user, ResourceAccess access, IServiceProvider sp)
        : base(new AppApi(FakeAppKey.AppKey, user, access), sp)
    {
        Employee = new EmployeeGroup(source.AddGroup(nameof(Employee), new ModifierCategoryName("Department")));
        EmployeeQuery = new ODataGroup<QueryEmployeesRequest, Employee>
        (
            source.AddGroup(nameof(EmployeeQuery)),
            () => sp.GetRequiredService<QueryEmployeesAction>()
        );
        Product = new ProductGroup(source.AddGroup(nameof(Product)));
    }
    public EmployeeGroup Employee { get; }
    public ODataGroup<QueryEmployeesRequest, Employee> EmployeeQuery { get; }
    public ProductGroup Product { get; }
}

public sealed class EmployeeGroup : AppApiGroupWrapper
{
    public EmployeeGroup(AppApiGroup source)
        : base(source)
    {
        Index = source.AddAction(nameof(Index), () => ViewAppAction<EmptyRequest>.Index());
        AddEmployee = source.AddAction
        (
            nameof(AddEmployee),
            () => new AddEmployeeAction(),
            () => new AddEmployeeValidation()
        );
        AddEmployeeFormView = source.AddAction
        (
            nameof(AddEmployeeFormView),
            () => new AddEmployeeFormViewAction()
        );
        AddEmployeeForm = source.AddAction
        (
            nameof(AddEmployeeForm),
            () => new AddEmployeeFormAction()
        );
        Employee = source.AddAction
        (
            nameof(Employee),
            () => new EmployeeAction(),
            friendlyName: "Get Employee Information"
        );
        DownloadAttachment = source.AddAction
        (
            nameof(DownloadAttachment), () => new DownloadAttachmentAction()
        );
        GetContent = source.AddAction
        (
            nameof(GetContent), () => new GetContentAction()
        );
    }
    public AppApiAction<EmptyRequest, WebViewResult> Index { get; }
    public AppApiAction<AddEmployeeForm, int> AddEmployee { get; }
    public AppApiAction<EmptyRequest, IDictionary<string, object?>> AddEmployeeForm { get; }
    public AppApiAction<EmptyRequest, WebPartialViewResult> AddEmployeeFormView { get; }
    public AppApiAction<int, Employee> Employee { get; }
    public AppApiAction<EmptyRequest, WebFileResult> DownloadAttachment { get; }
    public AppApiAction<EmptyRequest, WebContentResult> GetContent { get; }
}

public sealed record QueryEmployeesRequest(string Department);

public sealed class QueryEmployeesAction : QueryAction<QueryEmployeesRequest, Employee>
{
    public Task<IQueryable<Employee>> Execute(ODataQueryOptions<Employee> options, QueryEmployeesRequest model) =>
        Task.FromResult
        (
            Enumerable.Range(1, 3)
                .Select(i => new Employee { ID = i })
                .AsQueryable()
        );
}

public sealed class GetContentAction : AppAction<EmptyRequest, WebContentResult>
{
    public Task<WebContentResult> Execute(EmptyRequest model, CancellationToken stoppingToken) =>
        Task.FromResult(new WebContentResult("Whatever"));
}

public sealed class DownloadAttachmentAction : AppAction<EmptyRequest, WebFileResult>
{
    public Task<WebFileResult> Execute(EmptyRequest model, CancellationToken stoppingToken)
    {
        var stream = new MemoryStream();
        return Task.FromResult
        (
            new WebFileResult(stream, "text/plain", "attachment.txt")
        );
    }
}

public sealed class AddEmployeeAction : AppAction<AddEmployeeForm, int>
{
    public Task<int> Execute(AddEmployeeForm model, CancellationToken ct)
    {
        return Task.FromResult(1);
    }
}

public sealed class AddEmployeeFormAction : AppAction<EmptyRequest, IDictionary<string, object?>>
{
    public Task<IDictionary<string, object?>> Execute(EmptyRequest model, CancellationToken ct)
    {
        var form = new AddEmployeeForm();
        return Task.FromResult(form.Export());
    }
}

public sealed class AddEmployeeFormViewAction : AppAction<EmptyRequest, WebPartialViewResult>
{
    public Task<WebPartialViewResult> Execute(EmptyRequest model, CancellationToken ct)
    {
        return Task.FromResult(new WebPartialViewResult("AddEmployeeForm"));
    }
}

public sealed class AddEmployeeValidation : AppActionValidation<AddEmployeeForm>
{
    public Task Validate(ErrorList errors, AddEmployeeForm model, CancellationToken ct)
    {
        if (string.IsNullOrWhiteSpace(model.EmployeeName.Value()))
        {
            errors.Add("Name is required");
        }
        return Task.CompletedTask;
    }
}

public sealed class EmployeeAction : AppAction<int, Employee>
{
    public Task<Employee> Execute(int id, CancellationToken ct)
    {
        return Task.FromResult(new Employee { ID = id, Name = "Someone", BirthDate = DateTime.Today });
    }
}

public sealed class ProductGroup : AppApiGroupWrapper
{
    public ProductGroup(AppApiGroup source)
        : base(source)
    {
        Index = source.AddAction(nameof(Index), () => ViewAppAction<EmptyRequest>.Index());
        GetInfo = source.AddAction
        (
            nameof(GetInfo),
            () => new GetInfoAction()
        );
        AddProduct = source.AddAction
        (
            nameof(AddProduct),
            () => new AddProductAction(),
            () => new AddProductValidation()
        );
        Product = source.AddAction
        (
            nameof(Product),
            () => new ProductAction(),
            friendlyName: "Get Product Information"
        );
    }
    public AppApiAction<EmptyRequest, WebViewResult> Index { get; }
    public AppApiAction<EmptyRequest, string> GetInfo { get; }
    public AppApiAction<AddProductModel, int> AddProduct { get; }
    public AppApiAction<int, Product> Product { get; }
}

public sealed class GetInfoAction : AppAction<EmptyRequest, string>
{
    public Task<string> Execute(EmptyRequest model, CancellationToken ct)
    {
        return Task.FromResult("");
    }
}

public sealed class AddProductAction : AppAction<AddProductModel, int>
{
    public Task<int> Execute(AddProductModel model, CancellationToken ct)
    {
        return Task.FromResult(1);
    }
}

public sealed class AddProductValidation : AppActionValidation<AddProductModel>
{
    public Task Validate(ErrorList errors, AddProductModel model, CancellationToken ct)
    {
        if (string.IsNullOrWhiteSpace(model.Name))
        {
            errors.Add("Name is required");
        }
        return Task.CompletedTask;
    }
}

public sealed class ProductAction : AppAction<int, Product>
{
    public Task<Product> Execute(int id, CancellationToken ct)
    {
        return Task.FromResult(new Product { ID = id, Quantity = 2, Price = 23.42M });
    }
}