// Generated Code
namespace FakeWebApp.ApiControllers;
[Authorize]
[Route("odata/EmployeeQuery")]
public sealed partial class EmployeeQueryController : XtiODataController<QueryEmployeesRequest, Employee>
{
    public EmployeeQueryController(FakeAppApi api) : base(api.Group("EmployeeQuery"))
    {
    }
}