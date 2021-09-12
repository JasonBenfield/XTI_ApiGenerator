// Generated Code
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Collections.Generic;
using XTI_App.Api;
using FakeWebApp.Api;
using XTI_App;
using XTI_WebApp.Api;

namespace FakeWebApp.ApiControllers
{
    [Authorize]
    public class UserCacheController : Controller
    {
        public UserCacheController(FakeAppApi api)
        {
            this.api = api;
        }

        private readonly FakeAppApi api;
        [HttpPost]
        public Task<ResultContainer<EmptyActionResult>> ClearCache([FromBody] string model)
        {
            return api.Group("UserCache").Action<string, EmptyActionResult>("ClearCache").Execute(model);
        }
    }
}