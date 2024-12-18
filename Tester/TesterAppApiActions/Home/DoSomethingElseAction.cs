namespace XTI_TestServiceAppApi.Home;

public sealed class DoSomethingElseAction : AppAction<EmptyRequest, IDictionary<string, object?>>
{
    public Task<IDictionary<string, object?>> Execute(EmptyRequest model, CancellationToken ct)
    {
        IDictionary<string, object?> dict = new Dictionary<string, object?>();
        return Task.FromResult(dict);
    }
}