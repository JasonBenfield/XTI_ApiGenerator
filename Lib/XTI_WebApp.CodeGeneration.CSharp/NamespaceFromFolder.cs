namespace XTI_WebApp.CodeGeneration.CSharp;

public sealed class NamespaceFromFolder
{
    private readonly string outputFolder;

    public NamespaceFromFolder(string outputFolder)
    {
        this.outputFolder = outputFolder;
    }

    public string Value()
    {
        var dirInfo = new DirectoryInfo(outputFolder);
        var csprojFiles = Directory.GetFiles(outputFolder, "*.csproj")
            .Select(f => Path.GetFileNameWithoutExtension(f) ?? "");
        return csprojFiles
            .FirstOrDefault(f => f.Equals(dirInfo.Name, StringComparison.OrdinalIgnoreCase))
            ?? csprojFiles.First();
    }
}