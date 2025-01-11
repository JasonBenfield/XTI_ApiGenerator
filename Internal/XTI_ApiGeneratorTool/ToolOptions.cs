using CommandLine;

namespace XTI_ApiGeneratorTool;

internal sealed class ToolOptions
{
    [Option(nameof(AssemblyPath), Required = false)]
    public string AssemblyPath { get; set; } = "";

    [Option(nameof(AssemblyNamespace), Required = false)]
    public string AssemblyNamespace { get; set; } = "";

    [Option(nameof(SolutionDirectory), Required = false)]
    public string SolutionDirectory { get; set; } = "";

    [Option(nameof(FactoryClassName), Required = false)]
    public string FactoryClassName { get; set; } = "";

    [Option(nameof(SkipControllers), Required = false)]
    public bool SkipControllers { get; set; }

    [Option(nameof(ControllersProjectPath), Required = false)]
    public string ControllersProjectPath { get; set; } = "";

    [Option(nameof(SkipCsClient), Required = false)]
    public bool SkipCsClient { get; set; }

    [Option(nameof(CsClientProjectPath), Required = false)]
    public string CsClientProjectPath { get; set; } = "";

    [Option(nameof(SkipTsClient), Required = false)]
    public bool SkipTsClient { get; set; }

    [Option(nameof(TsClientPath), Required = false)]
    public string TsClientPath { get; set; } = "";

    [Option(nameof(RepoOwner), Required = false)]
    public string RepoOwner { get; set; } = "";

    [Option(nameof(RepoName), Required = false)]
    public string RepoName { get; set; } = "";

}
