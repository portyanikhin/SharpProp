namespace SharpProp.Tests;

/// <summary>
///     An example of how to add new properties to the <see cref="HumidAir"/> class.
/// </summary>
/// <seealso cref="IHumidAirExtended"/>
public class HumidAirExtended : HumidAir, IHumidAirExtended
{
    private SpecificEntropy? _specificHeatConstVolume;

    public SpecificEntropy SpecificHeatConstVolume =>
        _specificHeatConstVolume ??= SpecificEntropy
            .FromJoulesPerKilogramKelvin(KeyedOutput("CVha"))
            .ToUnit(SpecificEntropyUnit.KilojoulePerKilogramKelvin);

    public override void Reset()
    {
        base.Reset();
        _specificHeatConstVolume = null;
    }

    public new IHumidAirExtended WithState(
        IKeyedInput<string> firstInput,
        IKeyedInput<string> secondInput,
        IKeyedInput<string> thirdInput
    ) => (HumidAirExtended)base.WithState(firstInput, secondInput, thirdInput);

    public new IHumidAirExtended DryCoolingTo(
        Temperature temperature,
        Pressure? pressureDrop = null
    ) => (HumidAirExtended)base.DryCoolingTo(temperature, pressureDrop);

    public new IHumidAirExtended DryCoolingTo(
        SpecificEnergy enthalpy,
        Pressure? pressureDrop = null
    ) => (HumidAirExtended)base.DryCoolingTo(enthalpy, pressureDrop);

    public new IHumidAirExtended WetCoolingTo(
        Temperature temperature,
        RelativeHumidity relativeHumidity,
        Pressure? pressureDrop = null
    ) => (HumidAirExtended)base.WetCoolingTo(temperature, relativeHumidity, pressureDrop);

    public new IHumidAirExtended WetCoolingTo(
        Temperature temperature,
        Ratio humidity,
        Pressure? pressureDrop = null
    ) => (HumidAirExtended)base.WetCoolingTo(temperature, humidity, pressureDrop);

    public new IHumidAirExtended WetCoolingTo(
        SpecificEnergy enthalpy,
        RelativeHumidity relativeHumidity,
        Pressure? pressureDrop = null
    ) => (HumidAirExtended)base.WetCoolingTo(enthalpy, relativeHumidity, pressureDrop);

    public new IHumidAirExtended WetCoolingTo(
        SpecificEnergy enthalpy,
        Ratio humidity,
        Pressure? pressureDrop = null
    ) => (HumidAirExtended)base.WetCoolingTo(enthalpy, humidity, pressureDrop);

    public new IHumidAirExtended HeatingTo(
        Temperature temperature,
        Pressure? pressureDrop = null
    ) => (HumidAirExtended)base.HeatingTo(temperature, pressureDrop);

    public new IHumidAirExtended HeatingTo(
        SpecificEnergy enthalpy,
        Pressure? pressureDrop = null
    ) => (HumidAirExtended)base.HeatingTo(enthalpy, pressureDrop);

    public new IHumidAirExtended HumidificationByWaterTo(RelativeHumidity relativeHumidity) =>
        (HumidAirExtended)base.HumidificationByWaterTo(relativeHumidity);

    public new IHumidAirExtended HumidificationByWaterTo(Ratio humidity) =>
        (HumidAirExtended)base.HumidificationByWaterTo(humidity);

    public new IHumidAirExtended HumidificationBySteamTo(RelativeHumidity relativeHumidity) =>
        (HumidAirExtended)base.HumidificationBySteamTo(relativeHumidity);

    public new IHumidAirExtended HumidificationBySteamTo(Ratio humidity) =>
        (HumidAirExtended)base.HumidificationBySteamTo(humidity);

    public IHumidAirExtended Mixing(
        Ratio firstSpecificMassFlow,
        IHumidAirExtended first,
        Ratio secondSpecificMassFlow,
        IHumidAirExtended second
    ) =>
        (HumidAirExtended)base.Mixing(firstSpecificMassFlow, first, secondSpecificMassFlow, second);

    public new IHumidAirExtended Clone() => (HumidAirExtended)base.Clone();

    public new IHumidAirExtended Factory() => (HumidAirExtended)base.Factory();

    protected override HumidAir CreateInstance() => new HumidAirExtended();
}
