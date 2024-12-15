namespace XTI_TestServiceAppApi.Jobs;

internal sealed class RunTestJob2Action : AppAction<EmptyRequest, EmptyActionResult>
{
    public async Task<EmptyActionResult> Execute(EmptyRequest model, CancellationToken ct)
    {
        return new EmptyActionResult();
    }
}