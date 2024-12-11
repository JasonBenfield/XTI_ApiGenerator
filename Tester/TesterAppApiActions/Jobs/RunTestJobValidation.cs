using XTI_Core;

namespace XTI_TestServiceAppApi.Jobs;

internal sealed class RunTestJobValidation : AppActionValidation<EmptyRequest>
{
    public Task Validate(ErrorList errors, EmptyRequest model, CancellationToken stoppingToken)
    {
        return Task.CompletedTask;
    }
}