namespace XTI_TestServiceAppApi.Whatever;

public sealed class DoSomethingAction : AppAction<EmptyRequest, EmptyActionResult>
{
    public async Task<EmptyActionResult> Execute(EmptyRequest model, CancellationToken ct)
    {
        return new EmptyActionResult();
    }
}