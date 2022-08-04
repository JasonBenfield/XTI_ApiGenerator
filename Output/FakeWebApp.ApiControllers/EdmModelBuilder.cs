// Generated Code
using Microsoft.OData.Edm;
using Microsoft.OData.ModelBuilder;

namespace FakeWebApp.ApiControllers;
public sealed partial class EdmModelBuilder
{
    private readonly ODataConventionModelBuilder odataBuilder = new();
    public EdmModelBuilder()
    {
        EmployeeQuery = odataBuilder.EntitySet<Employee>("EmployeeQuery");
        init();
    }

    partial void init();
    public EntitySetConfiguration<Employee> EmployeeQuery { get; }

    public IEdmModel GetEdmModel() => odataBuilder.GetEdmModel();
}