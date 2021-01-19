using XTI_App.Api;

namespace FakeWebApp.Api
{
    public sealed class FakeAppApiFactory : AppApiFactory
    {
        protected override IAppApi _Create(IAppApiUser user) => new FakeAppApi(user);
    }
}
