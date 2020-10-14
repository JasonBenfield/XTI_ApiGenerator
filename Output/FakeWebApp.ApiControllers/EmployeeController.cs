// Generated Code
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using XTI_App.Api;
using XTI_WebApp.Api;
using FakeWebApp.Api;
using XTI_App;

namespace FakeWebApp.ApiControllers
{
    [Authorize]
    public class EmployeeController : Controller
    {
        public EmployeeController(FakeAppApi api, XtiPath xtiPath)
        {
            this.api = api;
            this.xtiPath = xtiPath;
        }

        private readonly FakeAppApi api;
        private readonly XtiPath xtiPath;
        public async Task<IActionResult> Index()
        {
            var result = await api.Group("Employee").Action<EmptyRequest, AppActionViewResult>("Index").Execute(xtiPath.Modifier, new EmptyRequest());
            return View(result.Data.ViewName);
        }

        [HttpPost]
        public Task<ResultContainer<int>> AddEmployee([FromBody] AddEmployeeModel model)
        {
            return api.Group("Employee").Action<AddEmployeeModel, int>("AddEmployee").Execute(xtiPath.Modifier, model);
        }

        [HttpPost]
        public Task<ResultContainer<Employee>> Employee([FromBody] int model)
        {
            return api.Group("Employee").Action<int, Employee>("Employee").Execute(xtiPath.Modifier, model);
        }
    }
}