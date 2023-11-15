namespace SharpProp.Tests;

/// <summary>
///     An example of how to add new properties to
///     the <see cref="IHumidAir"/> interface.
/// </summary>
public interface IHumidAirExtended : IHumidAir
{
    /// <summary>
    ///     Mass specific constant volume
    ///     specific heat per humid air (by default, kJ/kg/K).
    /// </summary>
    public SpecificEntropy SpecificHeatConstVolume { get; }

    /// <inheritdoc cref="IHumidAir.WithState"/>
    public new IHumidAirExtended WithState(
        IKeyedInput<string> firstInput,
        IKeyedInput<string> secondInput,
        IKeyedInput<string> thirdInput
    );

    /// <inheritdoc cref="IHumidAir.DryCoolingTo(Temperature, Pressure?)"/>
    public new IHumidAirExtended DryCoolingTo(
        Temperature temperature,
        Pressure? pressureDrop = null
    );

    /// <inheritdoc cref="IHumidAir.DryCoolingTo(SpecificEnergy, Pressure?)"/>
    public new IHumidAirExtended DryCoolingTo(
        SpecificEnergy enthalpy,
        Pressure? pressureDrop = null
    );

    /// <inheritdoc cref="IHumidAir.WetCoolingTo(Temperature, RelativeHumidity, Pressure?)"/>
    public new IHumidAirExtended WetCoolingTo(
        Temperature temperature,
        RelativeHumidity relativeHumidity,
        Pressure? pressureDrop = null
    );

    /// <inheritdoc cref="IHumidAir.WetCoolingTo(Temperature, Ratio, Pressure?)"/>
    public new IHumidAirExtended WetCoolingTo(
        Temperature temperature,
        Ratio humidity,
        Pressure? pressureDrop = null
    );

    /// <inheritdoc cref="IHumidAir.WetCoolingTo(SpecificEnergy, RelativeHumidity, Pressure?)"/>
    public new IHumidAirExtended WetCoolingTo(
        SpecificEnergy enthalpy,
        RelativeHumidity relativeHumidity,
        Pressure? pressureDrop = null
    );

    /// <inheritdoc cref="IHumidAir.WetCoolingTo(SpecificEnergy, Ratio, Pressure?)"/>
    public new IHumidAirExtended WetCoolingTo(
        SpecificEnergy enthalpy,
        Ratio humidity,
        Pressure? pressureDrop = null
    );

    /// <inheritdoc cref="IHumidAir.HeatingTo(Temperature, Pressure?)"/>
    public new IHumidAirExtended HeatingTo(
        Temperature temperature,
        Pressure? pressureDrop = null
    );

    /// <inheritdoc cref="IHumidAir.HeatingTo(SpecificEnergy, Pressure?)"/>
    public new IHumidAirExtended HeatingTo(
        SpecificEnergy enthalpy,
        Pressure? pressureDrop = null
    );

    /// <inheritdoc cref="IHumidAir.HumidificationByWaterTo(RelativeHumidity)"/>
    public new IHumidAirExtended HumidificationByWaterTo(
        RelativeHumidity relativeHumidity
    );

    /// <inheritdoc cref="IHumidAir.HumidificationByWaterTo(Ratio)"/>
    public new IHumidAirExtended HumidificationByWaterTo(Ratio humidity);

    /// <inheritdoc cref="IHumidAir.HumidificationBySteamTo(RelativeHumidity)"/>
    public new IHumidAirExtended HumidificationBySteamTo(
        RelativeHumidity relativeHumidity
    );

    /// <inheritdoc cref="IHumidAir.HumidificationBySteamTo(Ratio)"/>
    public new IHumidAirExtended HumidificationBySteamTo(Ratio humidity);

    /// <inheritdoc cref="IHumidAir.Mixing"/>
    public IHumidAirExtended Mixing(
        Ratio firstSpecificMassFlow,
        IHumidAirExtended first,
        Ratio secondSpecificMassFlow,
        IHumidAirExtended second
    );

    /// <inheritdoc cref="IClonable{T}.Clone"/>
    public new IHumidAirExtended Clone();

    /// <inheritdoc cref="IFactory{T}.Factory"/>
    public new IHumidAirExtended Factory();
}
