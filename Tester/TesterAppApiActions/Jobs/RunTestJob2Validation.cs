using XTI_Core;

namespace XTI_TestServiceAppApi.Jobs;

internal sealed class RunTestJob2Validation : AppActionValidation<EmptyRequest>
{
    public Task Validate(ErrorList errors, EmptyRequest model, CancellationToken stoppingToken)
    {
        return Task.CompletedTask;
    }
}