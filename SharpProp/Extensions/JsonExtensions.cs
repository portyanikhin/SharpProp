namespace SharpProp;

public static class JsonExtensions
{
    private static readonly JsonSerializerSettings Settings = new()
    {
        Converters = new List<JsonConverter>
            {new StringEnumConverter(), new UnitsNetIQuantityJsonConverter()}
    };

    internal static string AsJson<T>(this T instance, bool indented = true)
    {
        Settings.Formatting = indented ? Formatting.Indented : Formatting.None;
        return JsonConvert.SerializeObject(instance, Settings);
    }
}