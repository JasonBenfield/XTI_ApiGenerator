namespace XTI_TestServiceAppApi.Home;

public sealed class DoSomething2Result
{
}

public sealed class DoSomething2Action : AppAction<int, DoSomething2Result[]>
{
    public Task<DoSomething2Result[]> Execute(int model, CancellationToken ct)
    {
        return Task.FromResult(new DoSomething2Result[0]);
    }
}