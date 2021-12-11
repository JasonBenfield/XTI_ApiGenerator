// Generated Code
namespace FakeWebApp.ApiControllers;
[Authorize]
public class UserController : Controller
{
    private readonly FakeAppApi api;
    public UserController(FakeAppApi api)
    {
        this.api = api;
    }

    public async Task<IActionResult> Index(UserStartRequest model)
    {
        var result = await api.Group("User").Action<UserStartRequest, WebViewResult>("Index").Execute(model);
        return View(result.Data.ViewName);
    }
}