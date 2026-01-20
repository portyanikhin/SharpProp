namespace SharpProp;

/// <summary>
/// Real humid air (see ASHRAE RP-1485).
/// </summary>
public interface IHumidAir
    : IHumidAirState,
        IClonable<IHumidAir>,
        IEquatable<IHumidAir>,
        IFactory<IHumidAir>,
        IJsonable
{
    /// <summary>
    /// Updates the state of the humid air.
    /// </summary>
    /// <param name="firstInput">First input property.</param>
    /// <param name="secondInput">Second input property.</param>
    /// <param name="thirdInput">Third input property.</param>
    /// <exception cref="ArgumentException">Need to define 3 unique inputs!</exception>
    void Update(
        IKeyedInput<string> firstInput,
        IKeyedInput<string> secondInput,
        IKeyedInput<string> thirdInput
    );

    /// <summary>
    /// Resets all properties.
    /// </summary>
    void Reset();

    /// <summary>
    /// Returns a new humid air instance with defined state.
    /// </summary>
    /// <param name="firstInput">First input property.</param>
    /// <param name="secondInput">Second input property.</param>
    /// <param name="thirdInput">Third input property.</param>
    /// <returns>A new humid air instance with a defined state.</returns>
    /// <exception cref="ArgumentException">Need to define 3 unique inputs!</exception>
    IHumidAir WithState(
        IKeyedInput<string> firstInput,
        IKeyedInput<string> secondInput,
        IKeyedInput<string> thirdInput
    );

    /// <summary>
    /// The process of cooling without dehumidification to given temperature.
    /// </summary>
    /// <param name="temperature">Temperature.</param>
    /// <param name="pressureDrop">Pressure drop in the heat exchanger (optional).</param>
    /// <returns>The state of the humid air at the end of the process.</returns>
    /// <exception cref="ArgumentException">
    /// During the cooling process, the temperature should decrease!
    /// </exception>
    /// <exception cref="ArgumentException">
    /// The outlet temperature after dry heat transfer
    /// should be greater than the dew point temperature!
    /// </exception>
    /// <exception cref="ArgumentException">Invalid pressure drop in the heat exchanger!</exception>
    IHumidAir DryCoolingTo(Temperature temperature, Pressure? pressureDrop = null);

    /// <summary>
    /// The process of cooling without dehumidification to given enthalpy.
    /// </summary>
    /// <param name="enthalpy">Enthalpy.</param>
    /// <param name="pressureDrop">Pressure drop in the heat exchanger (optional).</param>
    /// <returns>The state of the humid air at the end of the process.</returns>
    /// <exception cref="ArgumentException">
    /// During the cooling process, the enthalpy should decrease!
    /// </exception>
    /// <exception cref="ArgumentException">
    /// The outlet enthalpy after dry heat transfer
    /// should be greater than the dew point enthalpy!
    /// </exception>
    /// <exception cref="ArgumentException">Invalid pressure drop in the heat exchanger!</exception>
    IHumidAir DryCoolingTo(SpecificEnergy enthalpy, Pressure? pressureDrop = null);

    /// <summary>
    /// The process of cooling with dehumidification
    /// to given temperature and relative humidity ratio.
    /// </summary>
    /// <param name="temperature">Temperature.</param>
    /// <param name="relativeHumidity">Relative humidity ratio.</param>
    /// <param name="pressureDrop">Pressure drop in the heat exchanger (optional).</param>
    /// <returns>The state of the humid air at the end of the process.</returns>
    /// <exception cref="ArgumentException">
    /// During the cooling process, the temperature should decrease!
    /// </exception>
    /// <exception cref="ArgumentException">
    /// During the wet cooling process, the absolute humidity ratio should decrease!
    /// </exception>
    /// <exception cref="ArgumentException">Invalid pressure drop in the heat exchanger!</exception>
    IHumidAir WetCoolingTo(
        Temperature temperature,
        RelativeHumidity relativeHumidity,
        Pressure? pressureDrop = null
    );

    /// <summary>
    /// The process of cooling with dehumidification
    /// to given temperature and absolute humidity ratio.
    /// </summary>
    /// <param name="temperature">Temperature.</param>
    /// <param name="humidity">Absolute humidity ratio.</param>
    /// <param name="pressureDrop">Pressure drop in the heat exchanger (optional).</param>
    /// <returns>The state of the humid air at the end of the process.</returns>
    /// <exception cref="ArgumentException">
    /// During the cooling process, the temperature should decrease!
    /// </exception>
    /// <exception cref="ArgumentException">
    /// During the wet cooling process, the absolute humidity ratio should decrease!
    /// </exception>
    /// <exception cref="ArgumentException">Invalid pressure drop in the heat exchanger!</exception>
    IHumidAir WetCoolingTo(Temperature temperature, Ratio humidity, Pressure? pressureDrop = null);

    /// <summary>
    /// The process of cooling with dehumidification
    /// to given enthalpy and relative humidity ratio.
    /// </summary>
    /// <param name="enthalpy">Enthalpy.</param>
    /// <param name="relativeHumidity">Relative humidity ratio.</param>
    /// <param name="pressureDrop">Pressure drop in the heat exchanger (optional).</param>
    /// <returns>The state of the humid air at the end of the process.</returns>
    /// <exception cref="ArgumentException">
    /// During the cooling process, the enthalpy should decrease!
    /// </exception>
    /// <exception cref="ArgumentException">
    /// During the wet cooling process, the absolute humidity ratio should decrease!
    /// </exception>
    /// <exception cref="ArgumentException">Invalid pressure drop in the heat exchanger!</exception>
    IHumidAir WetCoolingTo(
        SpecificEnergy enthalpy,
        RelativeHumidity relativeHumidity,
        Pressure? pressureDrop = null
    );

    /// <summary>
    /// The process of cooling with dehumidification
    /// to given enthalpy and absolute humidity ratio.
    /// </summary>
    /// <param name="enthalpy">Enthalpy.</param>
    /// <param name="humidity">Absolute humidity ratio.</param>
    /// <param name="pressureDrop">Pressure drop in the heat exchanger (optional).</param>
    /// <returns>The state of the humid air at the end of the process.</returns>
    /// <exception cref="ArgumentException">
    /// During the cooling process, the enthalpy should decrease!
    /// </exception>
    /// <exception cref="ArgumentException">
    /// During the wet cooling process, the absolute humidity ratio should decrease!
    /// </exception>
    /// <exception cref="ArgumentException">Invalid pressure drop in the heat exchanger!</exception>
    IHumidAir WetCoolingTo(SpecificEnergy enthalpy, Ratio humidity, Pressure? pressureDrop = null);

    /// <summary>
    /// The process of heating to given temperature.
    /// </summary>
    /// <param name="temperature">Temperature.</param>
    /// <param name="pressureDrop">Pressure drop in the heat exchanger (optional).</param>
    /// <returns>The state of the humid air at the end of the process.</returns>
    /// <exception cref="ArgumentException">
    /// During the heating process, the temperature should increase!
    /// </exception>
    /// <exception cref="ArgumentException">Invalid pressure drop in the heat exchanger!</exception>
    IHumidAir HeatingTo(Temperature temperature, Pressure? pressureDrop = null);

    /// <summary>
    /// The process of heating to given enthalpy.
    /// </summary>
    /// <param name="enthalpy">Enthalpy.</param>
    /// <param name="pressureDrop">Pressure drop in the heat exchanger (optional).</param>
    /// <returns>The state of the humid air at the end of the process.</returns>
    /// <exception cref="ArgumentException">
    /// During the heating process, the enthalpy should increase!
    /// </exception>
    /// <exception cref="ArgumentException">Invalid pressure drop in the heat exchanger!</exception>
    IHumidAir HeatingTo(SpecificEnergy enthalpy, Pressure? pressureDrop = null);

    /// <summary>
    /// The process of humidification by water (isenthalpic) to given relative humidity ratio.
    /// </summary>
    /// <param name="relativeHumidity">Relative humidity ratio.</param>
    /// <returns>The state of the humid air at the end of the process.</returns>
    /// <exception cref="ArgumentException">
    /// During the humidification process, the absolute humidity ratio should increase!
    /// </exception>
    IHumidAir HumidificationByWaterTo(RelativeHumidity relativeHumidity);

    /// <summary>
    /// The process of humidification by water (isenthalpic) to given absolute humidity ratio.
    /// </summary>
    /// <param name="humidity">Absolute humidity ratio.</param>
    /// <returns>The state of the humid air at the end of the process.</returns>
    /// <exception cref="ArgumentException">
    /// During the humidification process, the absolute humidity ratio should increase!
    /// </exception>
    IHumidAir HumidificationByWaterTo(Ratio humidity);

    /// <summary>
    /// The process of humidification by steam (isothermal) to given relative humidity ratio.
    /// </summary>
    /// <param name="relativeHumidity">Relative humidity ratio.</param>
    /// <returns>The state of the humid air at the end of the process.</returns>
    /// <exception cref="ArgumentException">
    /// During the humidification process, the absolute humidity ratio should increase!
    /// </exception>
    IHumidAir HumidificationBySteamTo(RelativeHumidity relativeHumidity);

    /// <summary>
    /// The process of humidification by steam (isothermal) to given absolute humidity ratio.
    /// </summary>
    /// <param name="humidity">Absolute humidity ratio.</param>
    /// <returns>The state of the humid air at the end of the process.</returns>
    /// <exception cref="ArgumentException">
    /// During the humidification process, the absolute humidity ratio should increase!
    /// </exception>
    IHumidAir HumidificationBySteamTo(Ratio humidity);

    /// <summary>
    /// The mixing process.
    /// </summary>
    /// <param name="firstSpecificMassFlow">
    /// Specific mass flow rate of the humid air at the first state.
    /// </param>
    /// <param name="first">Humid air at the first state.</param>
    /// <param name="secondSpecificMassFlow">
    /// Specific mass flow rate of the humid air the second state.
    /// </param>
    /// <param name="second">Humid air at the second state.</param>
    /// <returns>The state of the humid air at the end of the process.</returns>
    /// <exception cref="ArgumentException">
    /// The mixing process is possible only for flows with the same pressure!
    /// </exception>
    IHumidAir Mixing(
        Ratio firstSpecificMassFlow,
        IHumidAir first,
        Ratio secondSpecificMassFlow,
        IHumidAir second
    );
}
