using System.Threading.Tasks;
using XTI_App.Abstractions;

namespace XTI_WebApp.CodeGeneration
{
    public sealed class DefaultVersion
    {
        private readonly string defaultVersionKey;

        public DefaultVersion(string defaultVersionKey)
        {
            this.defaultVersionKey = defaultVersionKey;
        }

        public Task<AppVersionKey> Value(AppKey _)
            => Task.FromResult(AppVersionKey.Parse(defaultVersionKey));
    }
}
