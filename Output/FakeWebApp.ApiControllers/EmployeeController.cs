// Generated Code
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Collections.Generic;
using XTI_App.Api;
using XTI_WebApp.Api;
using FakeWebApp.Api;
using XTI_App;

namespace FakeWebApp.ApiControllers
{
    [Authorize]
    public class EmployeeController : Controller
    {
        public EmployeeController(FakeAppApi api)
        {
            this.api = api;
        }

        private readonly FakeAppApi api;
        public async Task<IActionResult> Index()
        {
            var result = await api.Group("Employee").Action<EmptyRequest, AppActionViewResult>("Index").Execute(new EmptyRequest());
            return View(result.Data.ViewName);
        }

        [HttpPost]
        public Task<ResultContainer<int>> AddEmployee([FromBody] AddEmployeeForm model)
        {
            return api.Group("Employee").Action<AddEmployeeForm, int>("AddEmployee").Execute(model);
        }

        [HttpPost]
        public Task<ResultContainer<IDictionary<string, object?>>> AddEmployeeForm()
        {
            return api.Group("Employee").Action<EmptyRequest, IDictionary<string, object?>>("AddEmployeeForm").Execute(new EmptyRequest());
        }

        [HttpPost]
        public Task<ResultContainer<Employee>> Employee([FromBody] int model)
        {
            return api.Group("Employee").Action<int, Employee>("Employee").Execute(model);
        }
    }
}