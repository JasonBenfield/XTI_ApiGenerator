using XTI_App.Api;

namespace FakeWebApp.Api;

public sealed class FakeAppApiFactory : AppApiFactory
{
    private readonly IServiceProvider sp;

    public FakeAppApiFactory(IServiceProvider sp)
    {
        this.sp = sp;
    }

    protected override IAppApi _Create(IAppApiUser user) => 
        new FakeAppApi(user, ResourceAccess.AllowAuthenticated(), sp);
}