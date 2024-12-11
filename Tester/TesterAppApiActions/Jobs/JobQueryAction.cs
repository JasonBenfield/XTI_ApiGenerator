using Microsoft.AspNetCore.OData.Query;
using XTI_ODataQuery.Api;

namespace TesterAppApiActions.Jobs;

public sealed class JobQueryRequest
{
}

public sealed class ExpandedJobEntry
{
}

internal sealed class JobQueryAction : QueryAction<JobQueryRequest, ExpandedJobEntry>
{
    public Task<IQueryable<ExpandedJobEntry>> Execute(ODataQueryOptions<ExpandedJobEntry> options, JobQueryRequest model)
    {
        return Task.FromResult(new ExpandedJobEntry[0].AsQueryable());
    }
}
