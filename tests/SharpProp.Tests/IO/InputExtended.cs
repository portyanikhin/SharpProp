namespace SharpProp.Tests;

/// <summary>
///     An example of how to extend the <see cref="Input"/> record.
/// </summary>
public record InputExtended : Input
{
    private InputExtended(Parameters coolPropKey, double value)
        : base(coolPropKey, value)
    {
    }

    /// <summary>
    ///     Molar density.
    /// </summary>
    /// <param name="value">The value of the input.</param>
    /// <returns>Molar density for the input.</returns>
    public static InputExtended MolarDensity(MolarMass value) =>
        new(Parameters.iDmolar, value.KilogramsPerMole);
}