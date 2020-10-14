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
    public class ProductController : Controller
    {
        public ProductController(FakeAppApi api, XtiPath xtiPath)
        {
            this.api = api;
            this.xtiPath = xtiPath;
        }

        private readonly FakeAppApi api;
        private readonly XtiPath xtiPath;
        public async Task<IActionResult> Index()
        {
            var result = await api.Group("Product").Action<EmptyRequest, AppActionViewResult>("Index").Execute(xtiPath.Modifier, new EmptyRequest());
            return View(result.Data.ViewName);
        }

        [HttpPost]
        public Task<ResultContainer<string>> GetInfo()
        {
            return api.Group("Product").Action<EmptyRequest, string>("GetInfo").Execute(xtiPath.Modifier, new EmptyRequest());
        }

        [HttpPost]
        public Task<ResultContainer<int>> AddProduct([FromBody] AddProductModel model)
        {
            return api.Group("Product").Action<AddProductModel, int>("AddProduct").Execute(xtiPath.Modifier, model);
        }

        [HttpPost]
        public Task<ResultContainer<Product>> Product([FromBody] int model)
        {
            return api.Group("Product").Action<int, Product>("Product").Execute(xtiPath.Modifier, model);
        }
    }
}