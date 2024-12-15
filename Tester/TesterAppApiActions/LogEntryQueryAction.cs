using Microsoft.AspNetCore.OData.Query;
using XTI_ODataQuery.Api;

namespace TesterAppApiActions;

public sealed class LogEntryQueryRequest
{
}

public sealed class ExpandedLogEntry
{
}

internal sealed class LogEntryQueryAction : QueryAction<LogEntryQueryRequest, ExpandedLogEntry>
{
    public Task<IQueryable<ExpandedLogEntry>> Execute(ODataQueryOptions<ExpandedLogEntry> options, LogEntryQueryRequest model)
    {
        return Task.FromResult(new ExpandedLogEntry[0].AsQueryable());
    }
}
