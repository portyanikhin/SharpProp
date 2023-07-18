using UnitsNet.NumberExtensions.NumberToRatio;

namespace SharpProp.Tests;

/// <summary>
///     An example of how to
///     extend the <see cref="InputHumidAir"/> record.
/// </summary>
public record InputHumidAirExtended : InputHumidAir
{
    private InputHumidAirExtended(
        string coolPropKey,
        double value
    ) : base(coolPropKey, value)
    {
    }

    /// <summary>
    ///     Water mole fraction.
    /// </summary>
    /// <param name="value">
    ///     The value of the input.
    /// </param>
    /// <returns>
    ///     Water mole fraction for the input.
    /// </returns>
    public static InputHumidAirExtended WaterMoleFraction(Ratio value) =>
        new("psi_w", value.DecimalFractions);
}

public class InputHumidAirExtendedTests
{
    private readonly InputHumidAirExtended _input;

    public InputHumidAirExtendedTests() =>
        _input = InputHumidAirExtended
            .WaterMoleFraction(5.PartsPerThousand());

    [Fact]
    public void CoolPropKey_NewInput_MatchesWithCoolProp() =>
        _input.CoolPropKey.Should().Be("psi_w");

    [Fact]
    public void Value_NewInput_ShouldBeInSIUnits() =>
        _input.Value.Should().Be(5e-3);
}