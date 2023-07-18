namespace SharpProp;

/// <summary>
///     Supports cloning, which creates
///     a new instance of a class with
///     the same value as an existing instance.
/// </summary>
/// <typeparam name="T">Type of the instance.</typeparam>
public interface IClonable<out T> where T : class
{
    /// <summary>
    ///     Performs deep (full) copy of the instance.
    /// </summary>
    /// <returns>Deep copy of the instance.</returns>
    public T Clone();
}