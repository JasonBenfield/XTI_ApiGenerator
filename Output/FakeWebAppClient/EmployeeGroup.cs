// Generated Code
namespace FakeWebAppClient;
public sealed partial class EmployeeGroup : AppClientGroup
{
    public EmployeeGroup(IHttpClientFactory httpClientFactory, XtiTokenAccessor xtiTokenAccessor, AppClientUrl clientUrl, AppClientOptions options) : base(httpClientFactory, xtiTokenAccessor, clientUrl, options, "Employee")
    {
        Actions = new EmployeeGroupActions(Index: CreateGetAction<EmptyRequest>("Index"), AddEmployee: CreatePostAction<AddEmployeeForm, int>("AddEmployee"), AddEmployeeFormView: CreateGetAction<EmptyRequest>("AddEmployeeFormView"), AddEmployeeForm: CreatePostAction<EmptyRequest, IDictionary<string, object>>("AddEmployeeForm"), Employee: CreatePostAction<int, Employee>("Employee"), DownloadAttachment: CreateFileAction<EmptyRequest>("DownloadAttachment"), GetContent: CreateContentAction<EmptyRequest>("GetContent"));
    }

    public EmployeeGroupActions Actions { get; }

    public Task<int> AddEmployee(string modifier, AddEmployeeForm model) => Actions.AddEmployee.Post(modifier, model);
    public Task<IDictionary<string, object>> AddEmployeeForm(string modifier) => Actions.AddEmployeeForm.Post(modifier, new EmptyRequest());
    public Task<Employee> Employee(string modifier, int model) => Actions.Employee.Post(modifier, model);
    public Task<AppClientFileResult> DownloadAttachment(string modifier) => Actions.DownloadAttachment.GetFile(modifier, new EmptyRequest());
    public Task<AppClientContentResult> GetContent(string modifier) => Actions.GetContent.Post(modifier, new EmptyRequest());
    public sealed record EmployeeGroupActions(AppClientGetAction<EmptyRequest> Index, AppClientPostAction<AddEmployeeForm, int> AddEmployee, AppClientGetAction<EmptyRequest> AddEmployeeFormView, AppClientPostAction<EmptyRequest, IDictionary<string, object>> AddEmployeeForm, AppClientPostAction<int, Employee> Employee, AppClientFileAction<EmptyRequest> DownloadAttachment, AppClientContentAction<EmptyRequest> GetContent);
}