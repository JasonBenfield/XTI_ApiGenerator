using System.Threading.Tasks;
using XTI_App;

namespace XTI_WebApp.CodeGeneration
{
    public sealed class DefaultVersion
    {
        private readonly AppFactory appFactory;
        private readonly string defaultVersionKey;

        public DefaultVersion(AppFactory appFactory, string defaultVersionKey)
        {
            this.appFactory = appFactory;
            this.defaultVersionKey = defaultVersionKey;
        }

        public async Task<AppVersionKey> Value(AppKey appKey)
        {
            AppVersionKey versionKey;
            if (string.IsNullOrWhiteSpace(defaultVersionKey))
            {
                var app = await appFactory.Apps().App(appKey);
                var currentVersion = await app.CurrentVersion();
                versionKey = currentVersion.Key();
            }
            else
            {
                versionKey = AppVersionKey.Parse(defaultVersionKey);
            }
            return versionKey;
        }
    }
}
