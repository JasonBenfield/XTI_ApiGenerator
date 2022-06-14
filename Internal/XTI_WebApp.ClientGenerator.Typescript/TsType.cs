using XTI_Forms;

namespace XTI_WebApp.ClientGenerator.Typescript;

internal sealed class TsType
{
    public TsType(ValueTemplate valueTemplate)
    {
        Value = getTsType(valueTemplate);
    }

    public TsType(Type? type)
    {
        Value = getTsType(type);
    }

    private static string getTsType(ValueTemplate valueTemplate)
    {
        if (valueTemplate is ArrayValueTemplate arrTempl)
        {
            var elType = getTsType(arrTempl.ElementTemplate.DataType);
            return $"{elType}[]";
        }
        return getTsType(valueTemplate.DataType);
    }

    private static string getTsType(Type? type)
    {
        if(type == null) { throw new NullReferenceException(nameof(type)); }
        string tsType;
        if 
        (
            type == typeof(string) || 
            type == typeof(TimeSpan) || 
            type == typeof(TimeSpan?) ||
            type.Name.StartsWith("ODataQueryOptions") == true
        )
        {
            tsType = "string";
        }
        else if
        (
            type == typeof(short) || type == typeof(int) || type == typeof(long)
            || type == typeof(double) || type == typeof(decimal) || type == typeof(float)
            || type == typeof(short?) || type == typeof(int?) || type == typeof(long?)
            || type == typeof(double?) || type == typeof(decimal?) || type == typeof(float?)
        )
        {
            tsType = "number";
        }
        else if
        (
            type == typeof(DateTime) || type == typeof(DateTimeOffset)
            || type == typeof(DateTime?) || type == typeof(DateTimeOffset?)
        )
        {
            tsType = "Date";
        }
        else if (type == typeof(bool) || type == typeof(bool?))
        {
            tsType = "boolean";
        }
        else if (type == typeof(object))
        {
            tsType = "object";
        }
        else if
        (
            type.IsGenericType &&
            (
                type.GetGenericTypeDefinition() == typeof(IDictionary<,>) ||
                type.GetGenericTypeDefinition() == typeof(Dictionary<,>)
            )
        )
        {
            var dictArgs = type.GetGenericArguments();
            var keyType = getTsType(dictArgs[0]);
            var valueType = getTsType(dictArgs[1]);
            tsType = $"Record<{keyType},{valueType}>";
        }
        else if
        (
            type.IsGenericType &&
            type.GetGenericTypeDefinition() == typeof(IQueryable<>)
        )
        {
            var queryableArgs = type.GetGenericArguments();
            var entityType = queryableArgs[0].Name;
            tsType = $"IQueryable{entityType}[]";
        }
        else if (typeof(Form).IsAssignableFrom(type))
        {
            tsType = type.Name;
        }
        else if (type.IsEnum == true)
        {
            tsType = type.Name;
        }
        else
        {
            tsType = $"I{type.Name}";
        }
        return tsType;
    }

    public string Value { get; }
}
