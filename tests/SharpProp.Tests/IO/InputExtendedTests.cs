namespace SharpProp.Tests;

public class InputExtendedTests
{
    private readonly InputExtended _input = InputExtended.MolarDensity(9e5.GramsPerMole());

    [Fact]
    public void CoolPropKey_NewInput_MatchesWithCoolProp() =>
        _input.CoolPropKey.Should().Be(Parameters.iDmolar);

    [Fact]
    public void Value_NewInput_ShouldBeInSIUnits() => _input.Value.Should().Be(900);
}
