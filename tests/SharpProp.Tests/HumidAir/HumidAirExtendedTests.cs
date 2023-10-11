using UnitsNet.NumberExtensions.NumberToRatio;

namespace SharpProp.Tests;

[Collection("Fluids")]
public class HumidAirExtendedTests
{
    private static readonly TemperatureDelta TemperatureDelta =
        TemperatureDelta.FromKelvins(5);

    private static readonly SpecificEnergy EnthalpyDelta =
        5.KilojoulesPerKilogram();

    private static readonly Ratio LowHumidity = 5.PartsPerThousand();

    private static readonly Ratio HighHumidity = 9.PartsPerThousand();

    private static readonly RelativeHumidity LowRelativeHumidity =
        RelativeHumidity.FromPercent(45);

    private static readonly RelativeHumidity HighRelativeHumidity =
        RelativeHumidity.FromPercent(90);

    private readonly IHumidAirExtended _humidAir =
        new HumidAirExtended().WithState(
            InputHumidAir.Pressure(1.Atmospheres()),
            InputHumidAir.Temperature(20.DegreesCelsius()),
            InputHumidAir.RelativeHumidity(RelativeHumidity.FromPercent(50))
        );

    [Fact]
    public void SpecificHeatConstVolume_HumidAirInStandardConditions_Returns722() =>
        _humidAir.SpecificHeatConstVolume.JoulesPerKilogramKelvin
            .Should()
            .Be(722.68718970632506);

    [Fact]
    public void Methods_New_ReturnsInstancesOfInheritedType()
    {
        _humidAir.Clone().Should().BeOfType<HumidAirExtended>();
        _humidAir.Factory().Should().BeOfType<HumidAirExtended>();
        _humidAir
            .WithState(
                InputHumidAir.Pressure(_humidAir.Pressure),
                InputHumidAir.Temperature(_humidAir.Temperature),
                InputHumidAir.RelativeHumidity(HighRelativeHumidity)
            )
            .Should()
            .BeOfType<HumidAirExtended>();
        _humidAir
            .DryCoolingTo(_humidAir.Temperature - TemperatureDelta)
            .Should()
            .BeOfType<HumidAirExtended>();
        _humidAir
            .DryCoolingTo(_humidAir.Enthalpy - EnthalpyDelta)
            .Should()
            .BeOfType<HumidAirExtended>();
        _humidAir
            .WetCoolingTo(
                _humidAir.Temperature - TemperatureDelta,
                LowRelativeHumidity
            )
            .Should()
            .BeOfType<HumidAirExtended>();
        _humidAir
            .WetCoolingTo(_humidAir.Temperature - TemperatureDelta, LowHumidity)
            .Should()
            .BeOfType<HumidAirExtended>();
        _humidAir
            .WetCoolingTo(
                _humidAir.Enthalpy - EnthalpyDelta,
                LowRelativeHumidity
            )
            .Should()
            .BeOfType<HumidAirExtended>();
        _humidAir
            .WetCoolingTo(_humidAir.Enthalpy - EnthalpyDelta, LowHumidity)
            .Should()
            .BeOfType<HumidAirExtended>();
        _humidAir
            .HeatingTo(_humidAir.Temperature + TemperatureDelta)
            .Should()
            .BeOfType<HumidAirExtended>();
        _humidAir
            .HeatingTo(_humidAir.Enthalpy + EnthalpyDelta)
            .Should()
            .BeOfType<HumidAirExtended>();
        _humidAir
            .HumidificationByWaterTo(HighRelativeHumidity)
            .Should()
            .BeOfType<HumidAirExtended>();
        _humidAir
            .HumidificationByWaterTo(HighHumidity)
            .Should()
            .BeOfType<HumidAirExtended>();
        _humidAir
            .HumidificationBySteamTo(HighRelativeHumidity)
            .Should()
            .BeOfType<HumidAirExtended>();
        _humidAir
            .HumidificationBySteamTo(HighHumidity)
            .Should()
            .BeOfType<HumidAirExtended>();
        _humidAir
            .Mixing(
                100.Percent(),
                _humidAir.WetCoolingTo(
                    _humidAir.Temperature - TemperatureDelta,
                    LowHumidity
                ),
                200.Percent(),
                _humidAir.HumidificationBySteamTo(HighHumidity)
            )
            .Should()
            .BeOfType<HumidAirExtended>();
    }
}