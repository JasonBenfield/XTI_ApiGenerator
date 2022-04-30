using XTI_App.Api;

namespace XTI_WebApp.ClientGenerator.CSharp;

internal static class Extensions
{
    public static AppApiActionTemplate[] ActionsForGetMethod(this AppApiGroupTemplate group) =>
        group.ActionTemplates.Where(at => at.IsView() || at.IsPartialView() || at.IsRedirect()).ToArray();
}
