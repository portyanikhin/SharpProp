namespace SharpProp;

/// <summary>
///     Supports converting to a JSON string.
/// </summary>
public interface IJsonable
{
    /// <summary>
    ///     Converts the instance to a JSON string.
    /// </summary>
    /// <param name="indented">True if indented.</param>
    /// <returns>A JSON string.</returns>
    public string AsJson(bool indented = true);
}