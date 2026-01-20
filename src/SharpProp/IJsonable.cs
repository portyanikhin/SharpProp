namespace SharpProp;

/// <summary>
///     Supports converting to a JSON string.
/// </summary>
public interface IJsonable
{
    /// <summary>
    ///     Converts the instance to a JSON string.
    /// </summary>
    /// <param name="indented"><c>true</c> if indented.</param>
    /// <returns>A JSON string.</returns>
    string AsJson(bool indented = true);
}
