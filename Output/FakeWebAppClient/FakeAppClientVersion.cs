// Generated Code
namespace FakeWebAppClient;
public sealed class FakeAppClientVersion
{
    public static FakeAppClientVersion Version(string value) => new FakeAppClientVersion(value);
    public FakeAppClientVersion(IHostEnvironment hostEnv) : this(getValue(hostEnv))
    {
    }

    private static string getValue(IHostEnvironment hostEnv)
    {
        string value;
        if (hostEnv.IsProduction())
        {
            value = "V0";
        }
        else
        {
            value = "Current";
        }

        return value;
    }

    private FakeAppClientVersion(string value)
    {
        Value = value;
    }

    public string Value { get; }
}