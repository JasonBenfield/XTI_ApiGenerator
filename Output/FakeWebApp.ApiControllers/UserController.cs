// Generated Code
namespace FakeWebApp.ApiControllers;
[Authorize]
public sealed partial class UserController : Controller
{
    private readonly FakeAppApi api;
    public UserController(FakeAppApi api)
    {
        this.api = api;
    }

    [HttpPost]
    public Task<ResultContainer<ResourcePathAccess[]>> GetUserAccess([FromBody] ResourcePath[] requestData, CancellationToken ct)
    {
        return api.User.GetUserAccess.Execute(requestData, ct);
    }

    public async Task<IActionResult> UserProfile(CancellationToken ct)
    {
        var result = await api.User.UserProfile.Execute(new EmptyRequest(), ct);
        return Redirect(result.Data!.Url);
    }

    [HttpPost]
    public Task<ResultContainer<LinkModel[]>> GetMenuLinks([FromBody] string requestData, CancellationToken ct)
    {
        return api.User.GetMenuLinks.Execute(requestData, ct);
    }

    public async Task<IActionResult> Logout(LogoutRequest requestData, CancellationToken ct)
    {
        var result = await api.User.Logout.Execute(requestData, ct);
        return Redirect(result.Data!.Url);
    }
}