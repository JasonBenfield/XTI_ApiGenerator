namespace XTI_TestServiceAppApi.Jobs;

internal sealed class RunTestJobAction : AppAction<EmptyRequest, EmptyActionResult>
{
    public async Task<EmptyActionResult> Execute(EmptyRequest model, CancellationToken ct)
    {
        return new EmptyActionResult();
    }
}