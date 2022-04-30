// Generated Code
namespace FakeWebAppClient;
public sealed partial class ProductActions
{
    internal ProductActions(AppClientUrl appClientUrl)
    {
        Index = new AppClientGetAction<EmptyRequest>(appClientUrl, "Index");
    }

    public AppClientGetAction<EmptyRequest> Index { get; }
}