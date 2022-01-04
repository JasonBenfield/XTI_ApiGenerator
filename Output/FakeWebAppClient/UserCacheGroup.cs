// Generated Code
namespace FakeWebAppClient;
public sealed partial class UserCacheGroup : AppClientGroup
{
    public UserCacheGroup(IHttpClientFactory httpClientFactory, IXtiToken xtiToken, AppClientUrl clientUrl) : base(httpClientFactory, xtiToken, clientUrl, "UserCache")
    {
    }

    public Task<EmptyActionResult> ClearCache(string model) => Post<EmptyActionResult, string>("ClearCache", "", model);
}