using System.Text;

namespace XTI_WebApp.ClientGenerator.Typescript;

public sealed class TypeScriptFile
{
    private readonly string fileName;
    private readonly Func<string, Stream> createStream;
    private readonly StringBuilder str = new();

    private string indentation = "";

    public TypeScriptFile(string fileName, Func<string, Stream> createStream)
    {
        this.fileName = fileName;
        if (!this.fileName.EndsWith(".ts"))
        {
            this.fileName += ".ts";
        }
        AddLine("// Generated code");
        this.createStream = createStream;
    }

    public void Indent()
    {
        indentation += "\t";
    }

    public void Outdent()
    {
        if (indentation != "")
        {
            indentation = indentation.Remove(indentation.Length - 1);
        }
    }

    public void AddLine(string line = "")
    {
        if (str.Length > 0)
        {
            str.Append("\r\n");
        }
        str.Append($"{indentation}{line}");
    }

    public void Append(string text) => str.Append(text);

    public async Task Output()
    {
        using var stream = createStream(fileName);
        var bytes = Encoding.UTF8.GetBytes(str.ToString());
        await stream.WriteAsync(bytes, 0, bytes.Length);
    }
}