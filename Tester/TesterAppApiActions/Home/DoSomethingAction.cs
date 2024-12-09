namespace XTI_TestServiceAppApi.Home;

public sealed class DoSomethingAction : AppAction<EmptyRequest, EmptyActionResult>
{
    public Task<EmptyActionResult> Execute(EmptyRequest model, CancellationToken ct)
    {
        Console.WriteLine($"{DateTime.Now:M/dd/yy HH:mm:ss} Doing Something");
        return Task.FromResult(new EmptyActionResult());
    }
}