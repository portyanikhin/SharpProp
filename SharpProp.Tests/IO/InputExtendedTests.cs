namespace SharpProp.Tests;

/// <summary>
///     An example of how to extend the <see cref="Input" /> record.
/// </summary>
public record InputExtended : Input
{
    private InputExtended(Parameters coolPropKey, double value) :
        base(coolPropKey, value)
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

public class InputExtendedTests
{
    private readonly InputExtended _input;

    public InputExtendedTests() =>
        _input = InputExtended.MolarDensity(9e5.GramsPerMole());

    [Fact]
    public void CoolPropKey_NewInput_MatchesWithCoolProp() =>
        _input.CoolPropKey.Should().Be(Parameters.iDmolar);

    [Fact]
    public void Value_NewInput_ShouldBeInSIUnits() =>
        _input.Value.Should().Be(900);
}