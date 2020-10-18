using XTI_App.Api;

namespace FakeWebApp.Api
{
    public sealed class FakeAppApiTemplateFactory : IAppApiTemplateFactory
    {
        public AppApiTemplate Create()
        {
            var api = new FakeAppApi(new AppApiSuperUser(), "Current");
            return api.Template();
        }
    }
}
