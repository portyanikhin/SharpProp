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
    public static string ConvertToJson(this object? instance, bool indented) =>
        JsonConvert.SerializeObject(
            instance,
            CreateSettings(indented ? Formatting.Indented : Formatting.None)
        );

    private static JsonSerializerSettings CreateSettings(Formatting formatting) =>
        new()
        {
            Converters = [new StringEnumConverter(), new UnitsNetIQuantityJsonConverter()],
            Formatting = formatting,
        };
}
