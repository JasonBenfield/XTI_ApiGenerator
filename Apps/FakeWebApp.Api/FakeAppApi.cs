﻿using System;
using System.Threading.Tasks;
using XTI_App;
using XTI_App.Api;
using XTI_WebApp.Api;

namespace FakeWebApp.Api
{
    public sealed class FakeAppApi : WebAppApi
    {
        private static readonly string AppKeyValue = "Fake";
        public static readonly AppKey AppKey = new AppKey(AppKeyValue);

        public FakeAppApi(IAppApiUser user, string version, ResourceAccess access = null)
            : base(AppKeyValue, version, user, access)
        {
            Employee = AddGroup(u => new EmployeeGroup(this, u));
            Product = AddGroup(u => new ProductGroup(this, u));
        }
        public EmployeeGroup Employee { get; }
        public ProductGroup Product { get; }
    }

    public sealed class EmployeeGroup : AppApiGroup
    {
        public EmployeeGroup(AppApi api, IAppApiUser user)
            : base
            (
                  api,
                  new NameFromGroupClassName(nameof(EmployeeGroup)).Value,
                  true,
                  api.Access,
                  user,
                  (n, a, u) => new WebAppApiActionCollection(n, a, u)
            )
        {
            var actions = Actions<WebAppApiActionCollection>();
            Index = actions.AddDefaultView();
            AddEmployee = actions.AddAction
            (
                "AddEmployee",
                () => new AddEmployeeValidation(),
                () => new AddEmployeeAction()
            );
            Employee = actions.AddAction
            (
                "Employee",
                () => new EmployeeAction(),
                "Get Employee Information"
            );
        }
        public AppApiAction<EmptyRequest, AppActionViewResult> Index { get; }
        public AppApiAction<AddEmployeeModel, int> AddEmployee { get; }
        public AppApiAction<int, Employee> Employee { get; }
    }

    public sealed class AddEmployeeModel
    {
        public string Name { get; set; }
        public DateTime BirthDate { get; set; }
    }

    public sealed class AddEmployeeAction : AppAction<AddEmployeeModel, int>
    {
        public Task<int> Execute(AddEmployeeModel model)
        {
            return Task.FromResult(1);
        }
    }

    public sealed class AddEmployeeValidation : AppActionValidation<AddEmployeeModel>
    {
        public Task Validate(ErrorList errors, AddEmployeeModel model)
        {
            if (string.IsNullOrWhiteSpace(model.Name))
            {
                errors.Add("Name is required");
            }
            return Task.CompletedTask;
        }
    }

    public sealed class Employee
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public DateTime BirthDate { get; set; }
    }

    public sealed class EmployeeAction : AppAction<int, Employee>
    {
        public Task<Employee> Execute(int id)
        {
            return Task.FromResult(new Employee { ID = id, Name = "Someone", BirthDate = DateTime.Today });
        }
    }

    public sealed class ProductGroup : AppApiGroup
    {
        public ProductGroup(AppApi api, IAppApiUser user)
            : base
            (
                api,
                new NameFromGroupClassName(nameof(ProductGroup)).Value,
                false,
                api.Access,
                user,
                (n, a, u) => new WebAppApiActionCollection(n, a, u)
            )
        {
            var actions = Actions<WebAppApiActionCollection>();
            Index = actions.AddDefaultView();
            GetInfo = actions.AddAction
            (
                "GetInfo",
                () => new GetInfoAction()
            );
            AddProduct = actions.AddAction
            (
                "AddProduct",
                () => new AddProductValidation(),
                () => new AddProductAction()
            );
            Product = actions.AddAction
            (
                "Product",
                () => new ProductAction(),
                "Get Product Information"
            );
        }
        public AppApiAction<EmptyRequest, AppActionViewResult> Index { get; }
        public AppApiAction<EmptyRequest, string> GetInfo { get; }
        public AppApiAction<AddProductModel, int> AddProduct { get; }
        public AppApiAction<int, Product> Product { get; }
    }

    public sealed class GetInfoAction : AppAction<EmptyRequest, string>
    {
        public Task<string> Execute(EmptyRequest model)
        {
            return Task.FromResult("");
        }
    }

    public sealed class AddProductModel
    {
        public string Name { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
    }

    public sealed class AddProductAction : AppAction<AddProductModel, int>
    {
        public Task<int> Execute(AddProductModel model)
        {
            return Task.FromResult(1);
        }
    }

    public sealed class AddProductValidation : AppActionValidation<AddProductModel>
    {
        public Task Validate(ErrorList errors, AddProductModel model)
        {
            if (string.IsNullOrWhiteSpace(model.Name))
            {
                errors.Add("Name is required");
            }
            return Task.CompletedTask;
        }
    }

    public sealed class Product
    {
        public int ID { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
    }

    public sealed class ProductAction : AppAction<int, Product>
    {
        public Task<Product> Execute(int id)
        {
            return Task.FromResult(new Product { ID = id, Quantity = 2, Price = 23.42M });
        }
    }

}