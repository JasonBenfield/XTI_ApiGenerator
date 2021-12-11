// Generated Code
namespace FakeWebAppClient;
public sealed partial class UserCacheGroup : AppClientGroup
{
    public UserCacheGroup(IHttpClientFactory httpClientFactory, IXtiToken xtiToken, string baseUrl) : base(httpClientFactory, xtiToken, baseUrl, "UserCache")
    {
    }

    public Task<EmptyActionResult> ClearCache(string model) => Post<EmptyActionResult, string>("ClearCache", "", model);
}