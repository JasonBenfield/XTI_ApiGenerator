// Generated Code
namespace FakeWebAppClient;
public sealed partial class EmployeeActions
{
    internal EmployeeActions(AppClientUrl appClientUrl)
    {
        Index = new AppClientGetAction<EmptyRequest>(appClientUrl, "Index");
        AddEmployeeFormView = new AppClientGetAction<EmptyRequest>(appClientUrl, "AddEmployeeFormView");
    }

    public AppClientGetAction<EmptyRequest> Index { get; }

    public AppClientGetAction<EmptyRequest> AddEmployeeFormView { get; }
}