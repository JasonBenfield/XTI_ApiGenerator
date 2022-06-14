// Generated Code
namespace FakeWebApp.ApiControllers;
[Authorize]
[Route("EmployeeQuery")]
public sealed partial class EmployeeQueryController : XtiODataController<Employee>
{
    public EmployeeQueryController(FakeAppApi api) : base(api.Group("EmployeeQuery"))
    {
    }
}