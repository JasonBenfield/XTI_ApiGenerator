namespace XTI_TestServiceAppApi.Jobs;

public sealed class RunTestJobAction : AppAction<EmptyRequest, EmptyActionResult>
{
    public async Task<EmptyActionResult> Execute(EmptyRequest model, CancellationToken ct)
    {
        return new EmptyActionResult();
    }
}