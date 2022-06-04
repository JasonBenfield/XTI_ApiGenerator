﻿using XTI_App.Abstractions;
using XTI_App.Api;
using XTI_Core;
using XTI_WebApp.Api;

namespace FakeWebApp.Api;

public static class FakeAppKey
{
    public static readonly AppKey AppKey = new AppKey("Fake", AppType.Values.WebApp);
}
public sealed class FakeAppApi : WebAppApiWrapper
{

    public FakeAppApi(IAppApiUser user, ResourceAccess access, IServiceProvider sp)
        : base(new AppApi(FakeAppKey.AppKey, user, access), sp)
    {
        Employee = new EmployeeGroup(source.AddGroup(nameof(Employee)));
        Product = new ProductGroup(source.AddGroup(nameof(Product)));
    }
    public EmployeeGroup Employee { get; }
    public ProductGroup Product { get; }
}

public sealed class EmployeeGroup : AppApiGroupWrapper
{
    public EmployeeGroup(AppApiGroup source)
        : base(source)
    {
        var actions = new WebAppApiActionFactory(source);
        Index = source.AddAction(actions.DefaultView());
        AddEmployee = source.AddAction
        (
            actions.Action
            (
                nameof(AddEmployee),
                () => new AddEmployeeValidation(),
                () => new AddEmployeeAction()
            )
        );
        AddEmployeeFormView = source.AddAction
        (
            actions.PartialView
            (
                nameof(AddEmployeeFormView),
                () => new AddEmployeeFormViewAction()
            )
        );
        AddEmployeeForm = source.AddAction
        (
            actions.Action
            (
                nameof(AddEmployeeForm),
                () => new AddEmployeeFormAction()
            )
        );
        Employee = source.AddAction
        (
            actions.Action
            (
                nameof(Employee),
                () => new EmployeeAction(),
                "Get Employee Information"
            )
        );
    }
    public AppApiAction<EmptyRequest, WebViewResult> Index { get; }
    public AppApiAction<AddEmployeeForm, int> AddEmployee { get; }
    public AppApiAction<EmptyRequest, IDictionary<string, object?>> AddEmployeeForm { get; }
    public AppApiAction<EmptyRequest, WebPartialViewResult> AddEmployeeFormView { get; }
    public AppApiAction<int, Employee> Employee { get; }
}

public sealed class AddEmployeeAction : AppAction<AddEmployeeForm, int>
{
    public Task<int> Execute(AddEmployeeForm model, CancellationToken ct)
    {
        return Task.FromResult(1);
    }
}

public sealed class AddEmployeeFormAction : AppAction<EmptyRequest, IDictionary<string, object?>>
{
    public Task<IDictionary<string, object?>> Execute(EmptyRequest model, CancellationToken ct)
    {
        var form = new AddEmployeeForm();
        return Task.FromResult(form.Export());
    }
}

public sealed class AddEmployeeFormViewAction : AppAction<EmptyRequest, WebPartialViewResult>
{
    public Task<WebPartialViewResult> Execute(EmptyRequest model, CancellationToken ct)
    {
        return Task.FromResult(new WebPartialViewResult("AddEmployeeForm"));
    }
}

public sealed class AddEmployeeValidation : AppActionValidation<AddEmployeeForm>
{
    public Task Validate(ErrorList errors, AddEmployeeForm model, CancellationToken ct)
    {
        if (string.IsNullOrWhiteSpace(model.EmployeeName.Value()))
        {
            errors.Add("Name is required");
        }
        return Task.CompletedTask;
    }
}

public sealed class EmployeeAction : AppAction<int, Employee>
{
    public Task<Employee> Execute(int id, CancellationToken ct)
    {
        return Task.FromResult(new Employee { ID = id, Name = "Someone", BirthDate = DateTime.Today });
    }
}

public sealed class ProductGroup : AppApiGroupWrapper
{
    public ProductGroup(AppApiGroup source)
        : base(source)
    {
        var actions = new WebAppApiActionFactory(source);
        Index = source.AddAction(actions.DefaultView());
        GetInfo = source.AddAction
        (
            actions.Action
            (
                nameof(GetInfo),
                () => new GetInfoAction()
            )
        );
        AddProduct = source.AddAction
        (
            actions.Action
            (
                nameof(AddProduct),
                () => new AddProductValidation(),
                () => new AddProductAction()
            )
        );
        Product = source.AddAction
        (
            actions.Action
            (
                nameof(Product),
                () => new ProductAction(),
                "Get Product Information"
            )
        );
    }
    public AppApiAction<EmptyRequest, WebViewResult> Index { get; }
    public AppApiAction<EmptyRequest, string> GetInfo { get; }
    public AppApiAction<AddProductModel, int> AddProduct { get; }
    public AppApiAction<int, Product> Product { get; }
}

public sealed class GetInfoAction : AppAction<EmptyRequest, string>
{
    public Task<string> Execute(EmptyRequest model, CancellationToken ct)
    {
        return Task.FromResult("");
    }
}

public sealed class AddProductAction : AppAction<AddProductModel, int>
{
    public Task<int> Execute(AddProductModel model, CancellationToken ct)
    {
        return Task.FromResult(1);
    }
}

public sealed class AddProductValidation : AppActionValidation<AddProductModel>
{
    public Task Validate(ErrorList errors, AddProductModel model, CancellationToken ct)
    {
        if (string.IsNullOrWhiteSpace(model.Name))
        {
            errors.Add("Name is required");
        }
        return Task.CompletedTask;
    }
}

public sealed class ProductAction : AppAction<int, Product>
{
    public Task<Product> Execute(int id, CancellationToken ct)
    {
        return Task.FromResult(new Product { ID = id, Quantity = 2, Price = 23.42M });
    }
}