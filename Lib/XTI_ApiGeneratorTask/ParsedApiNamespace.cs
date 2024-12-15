using System.Text.RegularExpressions;

namespace XTI_ApiGeneratorTask
{
    public sealed class ParsedApiNamespace
    {
        private static readonly Regex regex = new Regex("^(?<Prefix>([a-z]|\\d)+_)?(?<AppName>([a-z]|\\d)+)(?<AppType>((ConsoleApp)|(ServiceApp)|(WebApp)))Api$", RegexOptions.IgnoreCase);

        public ParsedApiNamespace(string ns)
        {
            var match = regex.Match(ns);
            if (match.Success)
            {
                Prefix = match.Groups["Prefix"].Value;
                AppName = match.Groups["AppName"].Value;
                AppType = match.Groups["AppType"].Value;
            }
            else
            {
                Prefix = "";
                AppName = "";
                AppType = "";
            }
        }

        public string Prefix { get; }
        public string AppName { get; }
        public string AppType { get; }
    }
}
