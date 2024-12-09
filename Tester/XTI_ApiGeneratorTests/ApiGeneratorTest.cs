using XTI_ApiGenerator;
using XTI_Core.Extensions;

namespace XTI_ApiGeneratorTests;

public sealed class ApiGeneratorTest
{
    [Test]
    public void ShouldGenerateGroupsForAppDefinition()
    {
        var appDefinition = GenerateAppDefinition();
        appDefinition.WriteToConsole();
        Assert.That(appDefinition.Groups.Length, Is.EqualTo(3));
        Assert.That
        (
            appDefinition.Groups.Select(g => g.Name),
            Is.EquivalentTo(new[] { "Home", "Jobs", "Whatever" })
        );
    }

    [Test]
    public void ShouldGenerateActionForGroupDefinition()
    {
        var appDefinition = GenerateAppDefinition();
        appDefinition.WriteToConsole();
        var homeGroup = appDefinition.Groups
            .FirstOrDefault(g => g.Name.Equals("Home", StringComparison.OrdinalIgnoreCase)) ?? 
            new();
        Assert.That(homeGroup.Actions.Length, Is.EqualTo(1));
        Assert.That(homeGroup.Actions[0].Name, Is.EqualTo("DoSomething"));
        Assert.That(homeGroup.Actions[0].ClassName, Is.EqualTo("DoSomethingAction"));
        Assert.That(homeGroup.Actions[0].RequestDataName, Is.EqualTo("EmptyRequest"));
        Assert.That(homeGroup.Actions[0].ResultDataName, Is.EqualTo("EmptyActionResult"));
    }

    [Test]
    public void ShouldGenerateGroupBuilderClass()
    {
        var appDefinition = GenerateAppDefinition();
        var homeGroup = appDefinition.Groups
            .FirstOrDefault(g => g.Name.Equals("Home", StringComparison.OrdinalIgnoreCase)) ??
            new();
        var groupBuilderClass = new GeneratedGroupBuilderClass(homeGroup, "XTI_ApiGeneratorTests").Value();
        Console.WriteLine(groupBuilderClass.Contents);
    }

    [Test]
    public void ShouldGenerateGroupClass()
    {
        var appDefinition = GenerateAppDefinition();
        var homeGroup = appDefinition.Groups
            .FirstOrDefault(g => g.Name.Equals("Home", StringComparison.OrdinalIgnoreCase)) ??
            new();
        var groupClass = new GeneratedGroupClass(homeGroup, "XTI_ApiGeneratorTests").Value();
        Console.WriteLine(groupClass.Contents);
    }

    [Test]
    public void ShouldGenerateAppBuilderClass()
    {
        var appDefinition = GenerateAppDefinition();
        var appBuilderClass = new GeneratedAppBuilderClass(appDefinition, "XTI_ApiGeneratorTests").Value();
        Console.WriteLine(appBuilderClass.Contents);
    }

    [Test]
    public void ShouldGenerateAppClass()
    {
        var appDefinition = GenerateAppDefinition();
        var appClass = new GeneratedAppClass(appDefinition, "XTI_ApiGeneratorTests").Value();
        Console.WriteLine(appClass.Contents);
    }

    private static AppDefinition GenerateAppDefinition()
    {
        var projectDir = Path.GetFullPath
        (
            Path.Combine(TestContext.CurrentContext.TestDirectory, "../../../../TesterAppApiActions")
        );
        var generatedAppDefinition = new GeneratedAppDefinition("Tester", "Web App", projectDir);
        var appDefinition = generatedAppDefinition.Value();
        return appDefinition;
    }

}
