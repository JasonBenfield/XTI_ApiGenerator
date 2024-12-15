using XTI_ApiGeneratorTask;

namespace XTI_ApiGeneratorTests;

internal sealed class ParsedApiNamespaceTest
{
    [Test]
    public void ShouldParseApiNamespaceWithoutPrefix()
    {
        var parsedNamespace = new ParsedApiNamespace("TesterWebAppApi");
        Assert.That(parsedNamespace.Prefix, Is.EqualTo(""));
        Assert.That(parsedNamespace.AppName, Is.EqualTo("Tester"));
        Assert.That(parsedNamespace.AppType, Is.EqualTo("WebApp"));
    }

    [Test]
    public void ShouldParseApiNamespaceWithPrefix()
    {
        var parsedNamespace = new ParsedApiNamespace("XTI_TesterWebAppApi");
        Assert.That(parsedNamespace.Prefix, Is.EqualTo("XTI_"));
        Assert.That(parsedNamespace.AppName, Is.EqualTo("Tester"));
        Assert.That(parsedNamespace.AppType, Is.EqualTo("WebApp"));
    }
}
