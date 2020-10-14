// Generated Code
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using XTI_WebApp.Api;
using FakeWebApp.Api;
using XTI_App;
using XTI_App.Api;

namespace FakeWebApp.ApiControllers
{
    [Authorize]
    public class UserController : Controller
    {
        public UserController(FakeAppApi api, XtiPath xtiPath)
        {
            this.api = api;
            this.xtiPath = xtiPath;
        }

        private readonly FakeAppApi api;
        private readonly XtiPath xtiPath;
        public async Task<IActionResult> Index(UserStartRequest model)
        {
            var result = await api.Group("User").Action<UserStartRequest, AppActionViewResult>("Index").Execute(xtiPath.Modifier, model);
            return View(result.Data.ViewName);
        }
    }
}