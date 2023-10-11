namespace SharpProp.Tests;

public static class FluidsListTests
{
    [Theory]
    [MemberData(nameof(FluidNames))]
    [SuppressMessage(
        "ReSharper",
        "SwitchStatementHandlesSomeKnownEnumValuesWithDefault"
    )]
    public static void CoolPropName_AllFluids_AreValidForCoolProp(
        FluidsList name
    )
    {
        if (name.CoolPropName().EndsWith(".mix"))
            return;
        switch (name)
        {
            case FluidsList.R50:
                name.CoolPropName().Should().Be("Methane");
                break;
            case FluidsList.RE143a:
                name.CoolPropName().Should().Be("HFE143m");
                break;
            case FluidsList.R152a:
                name.CoolPropName().Should().Be("R152A");
                break;
            case FluidsList.R170:
                name.CoolPropName().Should().Be("Ethane");
                break;
            case FluidsList.R290:
                name.CoolPropName().Should().Be("n-Propane");
                break;
            case FluidsList.R600:
                name.CoolPropName().Should().Be("n-Butane");
                break;
            case FluidsList.R600a:
                name.CoolPropName().Should().Be("IsoButane");
                break;
            case FluidsList.R601:
                name.CoolPropName().Should().Be("n-Pentane");
                break;
            case FluidsList.R601a:
                name.CoolPropName().Should().Be("Isopentane");
                break;
            case FluidsList.R702:
                name.CoolPropName().Should().Be("Hydrogen");
                break;
            case FluidsList.R704:
                name.CoolPropName().Should().Be("Helium");
                break;
            case FluidsList.R717:
                name.CoolPropName().Should().Be("Ammonia");
                break;
            case FluidsList.R718:
                name.CoolPropName().Should().Be("Water");
                break;
            case FluidsList.R720:
                name.CoolPropName().Should().Be("Neon");
                break;
            case FluidsList.R728:
                name.CoolPropName().Should().Be("Nitrogen");
                break;
            case FluidsList.R729:
                name.CoolPropName().Should().Be("Air");
                break;
            case FluidsList.R732:
                name.CoolPropName().Should().Be("Oxygen");
                break;
            case FluidsList.R740:
                name.CoolPropName().Should().Be("Argon");
                break;
            case FluidsList.R744:
                name.CoolPropName().Should().Be("CarbonDioxide");
                break;
            case FluidsList.R764:
                name.CoolPropName().Should().Be("SulfurDioxide");
                break;
            case FluidsList.R846:
                name.CoolPropName().Should().Be("SulfurHexafluoride");
                break;
            case FluidsList.R1150:
                name.CoolPropName().Should().Be("Ethylene");
                break;
            case FluidsList.R1270:
                name.CoolPropName().Should().Be("Propylene");
                break;
            case FluidsList.Butene:
                name.CoolPropName().Should().Be("1-Butene");
                break;
            case FluidsList.WaterIncomp:
                name.CoolPropName().Should().Be("Water");
                break;
            default:
                name.ToString()
                    .Should()
                    .Be(name.CoolPropName().RemoveChars('-', '(', ')'));
                break;
        }
    }

    public static IEnumerable<object[]> FluidNames() =>
        Enum.GetValues(typeof(FluidsList))
            .Cast<object>()
            .Select(name => new[] { name });

    private static string RemoveChars(
        this string s,
        params char[] charsToRemove
    ) =>
        string.Join(
            "",
            s.Split(charsToRemove, StringSplitOptions.RemoveEmptyEntries)
        );
}