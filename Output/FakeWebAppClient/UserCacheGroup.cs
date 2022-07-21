// Generated Code
namespace FakeWebAppClient;
public sealed partial class UserCacheGroup : AppClientGroup
{
    public UserCacheGroup(IHttpClientFactory httpClientFactory, XtiTokenAccessor xtiTokenAccessor, AppClientUrl clientUrl, AppClientOptions options) : base(httpClientFactory, xtiTokenAccessor, clientUrl, options, "UserCache")
    {
    }

    public UserCacheGroupActions Actions { get; }

    public Task<EmptyActionResult> ClearCache(string model) => Actions.ClearCache.Post("", model);
    public sealed record UserCacheGroupActions(AppClientPostAction<string, EmptyActionResult> ClearCache);
}