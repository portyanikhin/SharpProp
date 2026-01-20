using UnitsNet.NumberExtensions.NumberToRatio;

namespace SharpProp.Tests;

public class InputHumidAirExtendedTests
{
    private readonly InputHumidAirExtended _input = InputHumidAirExtended.WaterMoleFraction(
        5.PartsPerThousand()
    );

    [Fact]
    public void CoolPropKey_NewInput_MatchesWithCoolProp() =>
        _input.CoolPropKey.Should().Be("psi_w");

    [Fact]
    public void Value_NewInput_ShouldBeInSIUnits() => _input.Value.Should().Be(5e-3);
}
