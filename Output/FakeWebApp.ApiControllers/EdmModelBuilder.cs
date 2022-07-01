using Microsoft.OData.Edm;
using Microsoft.OData.ModelBuilder;

namespace FakeWebApp.ApiControllers;
public sealed class EdmModelBuilder
{
    public IEdmModel GetEdmModel()
    {
        var odataBuilder = new ODataConventionModelBuilder();
        odataBuilder.EntitySet<Employee>("EmployeeQuery");
        return odataBuilder.GetEdmModel();
    }
}