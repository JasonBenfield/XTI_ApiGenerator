using System;
using System.Collections.Generic;
using System.Linq;

namespace XTI_ApiGeneratorTask
{
    public sealed class AppDefinition
    {
        public AppDefinition(string name, string type, GroupDefinition[] groups, QueryDefinition[] queries)
        {
            Name = name;
            Type = type;
            BuilderClassName = $"{name}AppApiBuilder";
            ClassName = $"{name}AppApi";
            Groups = groups;
            Queries = queries;
            IsConsoleApp = type.Replace(" ", "").Equals("ConsoleApp", StringComparison.OrdinalIgnoreCase);
            IsServiceApp = type.Replace(" ", "").Equals("ServiceApp", StringComparison.OrdinalIgnoreCase);
            IsWebApp = type.Replace(" ", "").Equals("WebApp", StringComparison.OrdinalIgnoreCase);
        }

        public string Name { get; }
        public string Type { get; }
        public string BuilderClassName { get; }
        public string ClassName { get; }
        public GroupDefinition[] Groups { get; }
        public QueryDefinition[] Queries { get; }
        public bool IsWebApp { get; }
        public bool IsServiceApp { get; }
        public bool IsConsoleApp { get; }

        public string[] GetTargetNamespaces(string ns)
        {
            var namespaces = new List<string>();
            namespaces.AddRange
            (
                Groups
                    .Select(g => $"{ns}.{g.Name}")
                    .ToArray()
            );
            if (Queries.Any())
            {
                namespaces.Add("XTI_ODataQuery.Api");
            }
            return namespaces.ToArray();
        }
    }
}
