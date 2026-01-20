namespace SharpProp;

/// <summary>
///     Supports creating of new instances with no defined state.
/// </summary>
/// <typeparam name="T">Type of the instance.</typeparam>
public interface IFactory<out T>
    where T : class
{
    /// <summary>
    ///     Creates a new instance with no defined state.
    /// </summary>
    /// <returns>A new instance with no defined state.</returns>
    T Factory();
}
