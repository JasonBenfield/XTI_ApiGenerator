// Generated Code
namespace FakeWebAppClient;
public sealed partial class UserCacheGroup : AppClientGroup
{
    public UserCacheGroup(IHttpClientFactory httpClientFactory, XtiTokenAccessor xtiTokenAccessor, AppClientUrl clientUrl, AppClientOptions options) : base(httpClientFactory, xtiTokenAccessor, clientUrl, options, "UserCache")
    {
        Actions = new UserCacheGroupActions(ClearCache: CreatePostAction<string, EmptyActionResult>("ClearCache"));
    }

    public UserCacheGroupActions Actions { get; }

    public Task<EmptyActionResult> ClearCache(string model, CancellationToken ct = default) => Actions.ClearCache.Post("", model, ct);
    public sealed record UserCacheGroupActions(AppClientPostAction<string, EmptyActionResult> ClearCache);
}