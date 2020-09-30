using XTI_App.Api;

namespace FakeApiGeneratorApp
{
    public sealed class FakeAppApiTemplateFactory
    {
        public AppApiTemplate Create()
        {
            var api = new FakeAppApi(new AppApiSuperUser());
            return api.Template();
        }
    }
}
