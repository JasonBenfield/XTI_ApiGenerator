using System.Threading.Tasks;
using XTI_App.Api;

namespace XTI_WebApp.CodeGeneration
{
    public interface CodeGenerator
    {
        Task Output(AppApiTemplate appTemplate);
    }
}
