namespace SharpProp;

internal static class JsonExtensions
{
    private static readonly JsonSerializerSettings Settings = new()
    {
        Converters = [new StringEnumConverter(), new UnitsNetIQuantityJsonConverter()],
    };

    public static string ConvertToJson(this object? instance, bool indented)
    {
        Settings.Formatting = indented ? Formatting.Indented : Formatting.None;
        return JsonConvert.SerializeObject(instance, Settings);
    }
}
