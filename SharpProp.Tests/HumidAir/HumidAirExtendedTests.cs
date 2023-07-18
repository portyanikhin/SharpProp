using UnitsNet.NumberExtensions.NumberToRatio;

namespace SharpProp.Tests;

/// <summary>
///     An example of how to
///     add new properties to the
///     <see cref="HumidAir"/> class.
/// </summary>
public class HumidAirExtended : HumidAir
{
    private SpecificEntropy? _specificHeatConstVolume;

    /// <summary>
    ///     Mass specific constant
    ///     volume specific heat per humid air
    ///     (by default, kJ/kg/K).
    /// </summary>
    public SpecificEntropy SpecificHeatConstVolume =>
        _specificHeatConstVolume ??=
            SpecificEntropy.FromJoulesPerKilogramKelvin(
                KeyedOutput("CVha")
            ).ToUnit(SpecificEntropyUnit.KilojoulePerKilogramKelvin);

    protected override void Reset()
    {
        base.Reset();
        _specificHeatConstVolume = null;
    }

    protected override HumidAir CreateInstance() =>
        new HumidAirExtended();

    public new HumidAirExtended Clone() =>
        (HumidAirExtended) base.Clone();

    public new HumidAirExtended Factory() =>
        (HumidAirExtended) base.Factory();

    public new HumidAirExtended WithState(
        IKeyedInput<string> fistInput,
        IKeyedInput<string> secondInput,
        IKeyedInput<string> thirdInput
    ) => (HumidAirExtended) base.WithState(
        fistInput,
        secondInput,
        thirdInput
    );

    public new HumidAirExtended DryCoolingTo(
        Temperature temperature,
        Pressure? pressureDrop = null
    ) => (HumidAirExtended) base.DryCoolingTo(temperature, pressureDrop);

    public new HumidAirExtended DryCoolingTo(
        SpecificEnergy enthalpy,
        Pressure? pressureDrop = null
    ) => (HumidAirExtended) base.DryCoolingTo(enthalpy, pressureDrop);

    public new HumidAirExtended WetCoolingTo(
        Temperature temperature,
        RelativeHumidity relativeHumidity,
        Pressure? pressureDrop = null
    ) => (HumidAirExtended) base.WetCoolingTo(
        temperature,
        relativeHumidity,
        pressureDrop
    );

    public new HumidAirExtended WetCoolingTo(
        Temperature temperature,
        Ratio humidity,
        Pressure? pressureDrop = null
    ) => (HumidAirExtended) base.WetCoolingTo(
        temperature,
        humidity,
        pressureDrop
    );

    public new HumidAirExtended WetCoolingTo(
        SpecificEnergy enthalpy,
        RelativeHumidity relativeHumidity,
        Pressure? pressureDrop = null
    ) => (HumidAirExtended) base.WetCoolingTo(
        enthalpy,
        relativeHumidity,
        pressureDrop
    );

    public new HumidAirExtended WetCoolingTo(
        SpecificEnergy enthalpy,
        Ratio humidity,
        Pressure? pressureDrop = null
    ) => (HumidAirExtended) base.WetCoolingTo(
        enthalpy,
        humidity,
        pressureDrop
    );

    public new HumidAirExtended HeatingTo(
        Temperature temperature,
        Pressure? pressureDrop = null
    ) => (HumidAirExtended) base.HeatingTo(temperature, pressureDrop);

    public new HumidAirExtended HeatingTo(
        SpecificEnergy enthalpy,
        Pressure? pressureDrop = null
    ) => (HumidAirExtended) base.HeatingTo(enthalpy, pressureDrop);

    public new HumidAirExtended HumidificationByWaterTo(
        RelativeHumidity relativeHumidity
    ) => (HumidAirExtended) base.HumidificationByWaterTo(relativeHumidity);

    public new HumidAirExtended HumidificationByWaterTo(Ratio humidity) =>
        (HumidAirExtended) base.HumidificationByWaterTo(humidity);

    public new HumidAirExtended HumidificationBySteamTo(
        RelativeHumidity relativeHumidity
    ) => (HumidAirExtended) base.HumidificationBySteamTo(relativeHumidity);

    public new HumidAirExtended HumidificationBySteamTo(Ratio humidity) =>
        (HumidAirExtended) base.HumidificationBySteamTo(humidity);

    public new HumidAirExtended Mixing(
        Ratio firstSpecificMassFlow,
        HumidAir first,
        Ratio secondSpecificMassFlow,
        HumidAir second
    ) => (HumidAirExtended) base.Mixing(
        firstSpecificMassFlow,
        first,
        secondSpecificMassFlow,
        second
    );
}

[Collection("Fluids")]
public class HumidAirExtendedTests
{
    private static readonly TemperatureDelta TemperatureDelta =
        TemperatureDelta.FromKelvins(5);

    private static readonly SpecificEnergy EnthalpyDelta =
        5.KilojoulesPerKilogram();

    private static readonly Ratio LowHumidity =
        5.PartsPerThousand();

    private static readonly Ratio HighHumidity =
        9.PartsPerThousand();

    private static readonly RelativeHumidity LowRelativeHumidity =
        RelativeHumidity.FromPercent(45);

    private static readonly RelativeHumidity HighRelativeHumidity =
        RelativeHumidity.FromPercent(90);

    private readonly HumidAirExtended _humidAir;

    public HumidAirExtendedTests() =>
        _humidAir = new HumidAirExtended().WithState(
            InputHumidAir.Pressure(1.Atmospheres()),
            InputHumidAir.Temperature(20.DegreesCelsius()),
            InputHumidAir.RelativeHumidity(RelativeHumidity.FromPercent(50))
        );

    [Fact]
    public void SpecificHeatConstVolume_HumidAirInStandardConditions_Returns722() =>
        _humidAir.SpecificHeatConstVolume.JoulesPerKilogramKelvin
            .Should().Be(722.68718970632506);

    [Fact]
    public void Methods_New_ReturnsInstancesOfInheritedType()
    {
        _humidAir.Clone()
            .Should().BeOfType<HumidAirExtended>();
        _humidAir.Factory()
            .Should().BeOfType<HumidAirExtended>();
        _humidAir.DryCoolingTo(_humidAir.Temperature - TemperatureDelta)
            .Should().BeOfType<HumidAirExtended>();
        _humidAir.DryCoolingTo(_humidAir.Enthalpy - EnthalpyDelta)
            .Should().BeOfType<HumidAirExtended>();
        _humidAir.WetCoolingTo(_humidAir.Temperature - TemperatureDelta,
            LowRelativeHumidity
        ).Should().BeOfType<HumidAirExtended>();
        _humidAir.WetCoolingTo(_humidAir.Temperature - TemperatureDelta,
            LowHumidity
        ).Should().BeOfType<HumidAirExtended>();
        _humidAir.WetCoolingTo(_humidAir.Enthalpy - EnthalpyDelta,
            LowRelativeHumidity
        ).Should().BeOfType<HumidAirExtended>();
        _humidAir.WetCoolingTo(_humidAir.Enthalpy - EnthalpyDelta,
            LowHumidity
        ).Should().BeOfType<HumidAirExtended>();
        _humidAir.HeatingTo(_humidAir.Temperature + TemperatureDelta)
            .Should().BeOfType<HumidAirExtended>();
        _humidAir.HeatingTo(_humidAir.Enthalpy + EnthalpyDelta)
            .Should().BeOfType<HumidAirExtended>();
        _humidAir.HumidificationByWaterTo(HighRelativeHumidity)
            .Should().BeOfType<HumidAirExtended>();
        _humidAir.HumidificationByWaterTo(HighHumidity)
            .Should().BeOfType<HumidAirExtended>();
        _humidAir.HumidificationBySteamTo(HighRelativeHumidity)
            .Should().BeOfType<HumidAirExtended>();
        _humidAir.HumidificationBySteamTo(HighHumidity)
            .Should().BeOfType<HumidAirExtended>();
        _humidAir.Mixing(
            100.Percent(),
            _humidAir.WetCoolingTo(
                _humidAir.Temperature - TemperatureDelta,
                LowHumidity
            ),
            200.Percent(),
            _humidAir.HumidificationBySteamTo(HighHumidity)
        ).Should().BeOfType<HumidAirExtended>();
    }
}