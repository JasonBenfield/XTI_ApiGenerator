using XTI_App.Api;

namespace XTI_WebApp.CodeGeneration;

public static class AppApiGroupTemplateExtensions
{
    public static bool IsODataGroup(this AppApiGroupTemplate groupTemplate) => 
        groupTemplate.QueryableTemplates().Any();
}
