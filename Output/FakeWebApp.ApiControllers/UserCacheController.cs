// Generated Code
namespace FakeWebApp.ApiControllers;
[Authorize]
public sealed partial class UserCacheController : Controller
{
    private readonly FakeAppApi api;
    public UserCacheController(FakeAppApi api)
    {
        this.api = api;
    }

    [HttpPost]
    public Task<ResultContainer<EmptyActionResult>> ClearCache([FromBody] string requestData, CancellationToken ct)
    {
        return api.UserCache.ClearCache.Execute(requestData, ct);
    }
}