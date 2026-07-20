namespace SharpProp;

/// <summary>
/// Supports converting to a JSON string.
/// </summary>
public interface IJsonable
{
    /// <summary>
    /// Converts the instance to a JSON string.
    /// </summary>
    /// <param name="indented"><c>true</c> if indented.</param>
    /// <returns>A JSON string.</returns>
    string AsJson(bool indented = true);
}

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
