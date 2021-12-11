// Generated Code
namespace FakeWebApp.ApiControllers;
[Authorize]
public class UserCacheController : Controller
{
    private readonly FakeAppApi api;
    public UserCacheController(FakeAppApi api)
    {
        this.api = api;
    }

    [HttpPost]
    public Task<ResultContainer<EmptyActionResult>> ClearCache([FromBody] string model)
    {
        return api.Group("UserCache").Action<string, EmptyActionResult>("ClearCache").Execute(model);
    }
}