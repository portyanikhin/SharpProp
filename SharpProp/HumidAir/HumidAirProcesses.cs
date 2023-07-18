using UnitsNet.NumberExtensions.NumberToRelativeHumidity;

namespace SharpProp;

public partial class HumidAir
{
    private HumidAir DewPoint =>
        WithState(
            InputHumidAir.Pressure(Pressure),
            InputHumidAir.Temperature(DewTemperature),
            InputHumidAir.RelativeHumidity(100.Percent())
        );

    /// <summary>
    ///     The process of cooling without
    ///     dehumidification to a given temperature.
    /// </summary>
    /// <param name="temperature">Temperature.</param>
    /// <param name="pressureDrop">
    ///     Pressure drop in
    ///     the heat exchanger (optional).
    /// </param>
    /// <returns>
    ///     The state of the humid air
    ///     at the end of the process.
    /// </returns>
    /// <exception cref="ArgumentException">
    ///     During the cooling process,
    ///     the temperature should decrease!
    /// </exception>
    /// <exception cref="ArgumentException">
    ///     The outlet temperature
    ///     after dry heat transfer should be
    ///     greater than the dew point temperature!
    /// </exception>
    /// <exception cref="ArgumentException">
    ///     Invalid pressure drop
    ///     in the heat exchanger!
    /// </exception>
    public HumidAir DryCoolingTo(
        Temperature temperature,
        Pressure? pressureDrop = null
    ) => DryHeatTransferTo(temperature, true, pressureDrop);

    /// <summary>
    ///     The process of cooling without
    ///     dehumidification to a given enthalpy.
    /// </summary>
    /// <param name="enthalpy">Enthalpy.</param>
    /// <param name="pressureDrop">
    ///     Pressure drop in
    ///     the heat exchanger (optional).
    /// </param>
    /// <returns>
    ///     The state of the humid air
    ///     at the end of the process.
    /// </returns>
    /// <exception cref="ArgumentException">
    ///     During the cooling process,
    ///     the enthalpy should decrease!
    /// </exception>
    /// <exception cref="ArgumentException">
    ///     The outlet enthalpy
    ///     after dry heat transfer should be
    ///     greater than the dew point enthalpy!
    /// </exception>
    /// <exception cref="ArgumentException">
    ///     Invalid pressure drop
    ///     in the heat exchanger!
    /// </exception>
    public HumidAir DryCoolingTo(
        SpecificEnergy enthalpy,
        Pressure? pressureDrop = null
    ) => DryHeatTransferTo(enthalpy, true, pressureDrop);

    /// <summary>
    ///     The process of cooling with
    ///     dehumidification to a given temperature
    ///     and relative humidity ratio.
    /// </summary>
    /// <param name="temperature">Temperature.</param>
    /// <param name="relativeHumidity">
    ///     Relative humidity ratio.
    /// </param>
    /// <param name="pressureDrop">
    ///     Pressure drop in
    ///     the heat exchanger (optional).
    /// </param>
    /// <returns>
    ///     The state of the humid air
    ///     at the end of the process.
    /// </returns>
    /// <exception cref="ArgumentException">
    ///     During the cooling process,
    ///     the temperature should decrease!
    /// </exception>
    /// <exception cref="ArgumentException">
    ///     During the wet cooling process,
    ///     the absolute humidity ratio should decrease!
    /// </exception>
    /// <exception cref="ArgumentException">
    ///     Invalid pressure drop
    ///     in the heat exchanger!
    /// </exception>
    public HumidAir WetCoolingTo(
        Temperature temperature,
        RelativeHumidity relativeHumidity,
        Pressure? pressureDrop = null
    ) => WetCoolingTo(
        InputHumidAir.Temperature(temperature),
        InputHumidAir.RelativeHumidity(relativeHumidity),
        pressureDrop
    );

    /// <summary>
    ///     The process of cooling with
    ///     dehumidification to a given temperature
    ///     and absolute humidity ratio.
    /// </summary>
    /// <param name="temperature">Temperature.</param>
    /// <param name="humidity">
    ///     Absolute humidity ratio.
    /// </param>
    /// <param name="pressureDrop">
    ///     Pressure drop in
    ///     the heat exchanger (optional).
    /// </param>
    /// <returns>
    ///     The state of the humid air
    ///     at the end of the process.
    /// </returns>
    /// <exception cref="ArgumentException">
    ///     During the cooling process,
    ///     the temperature should decrease!
    /// </exception>
    /// <exception cref="ArgumentException">
    ///     During the wet cooling process,
    ///     the absolute humidity ratio should decrease!
    /// </exception>
    /// <exception cref="ArgumentException">
    ///     Invalid pressure drop
    ///     in the heat exchanger!
    /// </exception>
    public HumidAir WetCoolingTo(
        Temperature temperature,
        Ratio humidity,
        Pressure? pressureDrop = null
    ) => WetCoolingTo(
        InputHumidAir.Temperature(temperature),
        InputHumidAir.Humidity(humidity),
        pressureDrop
    );

    /// <summary>
    ///     The process of cooling with
    ///     dehumidification to a given enthalpy
    ///     and relative humidity ratio.
    /// </summary>
    /// <param name="enthalpy">Enthalpy.</param>
    /// <param name="relativeHumidity">
    ///     Relative humidity ratio.
    /// </param>
    /// <param name="pressureDrop">
    ///     Pressure drop in
    ///     the heat exchanger (optional).
    /// </param>
    /// <returns>
    ///     The state of the humid air
    ///     at the end of the process.
    /// </returns>
    /// <exception cref="ArgumentException">
    ///     During the cooling process,
    ///     the enthalpy should decrease!
    /// </exception>
    /// <exception cref="ArgumentException">
    ///     During the wet cooling process,
    ///     the absolute humidity ratio should decrease!
    /// </exception>
    /// <exception cref="ArgumentException">
    ///     Invalid pressure drop
    ///     in the heat exchanger!
    /// </exception>
    public HumidAir WetCoolingTo(
        SpecificEnergy enthalpy,
        RelativeHumidity relativeHumidity,
        Pressure? pressureDrop = null
    ) => WetCoolingTo(
        InputHumidAir.Enthalpy(enthalpy),
        InputHumidAir.RelativeHumidity(relativeHumidity),
        pressureDrop
    );

    /// <summary>
    ///     The process of cooling with
    ///     dehumidification to a given enthalpy
    ///     and absolute humidity ratio.
    /// </summary>
    /// <param name="enthalpy">Enthalpy.</param>
    /// <param name="humidity">
    ///     Absolute humidity ratio.
    /// </param>
    /// <param name="pressureDrop">
    ///     Pressure drop in
    ///     the heat exchanger (optional).
    /// </param>
    /// <returns>
    ///     The state of the humid air
    ///     at the end of the process.
    /// </returns>
    /// <exception cref="ArgumentException">
    ///     During the cooling process,
    ///     the enthalpy should decrease!
    /// </exception>
    /// <exception cref="ArgumentException">
    ///     During the wet cooling process,
    ///     the absolute humidity ratio should decrease!
    /// </exception>
    /// <exception cref="ArgumentException">
    ///     Invalid pressure drop
    ///     in the heat exchanger!
    /// </exception>
    public HumidAir WetCoolingTo(
        SpecificEnergy enthalpy,
        Ratio humidity,
        Pressure? pressureDrop = null
    ) => WetCoolingTo(
        InputHumidAir.Enthalpy(enthalpy),
        InputHumidAir.Humidity(humidity),
        pressureDrop
    );

    /// <summary>
    ///     The process of heating
    ///     to a given temperature.
    /// </summary>
    /// <param name="temperature">Temperature.</param>
    /// <param name="pressureDrop">
    ///     Pressure drop in
    ///     the heat exchanger (optional).
    /// </param>
    /// <returns>
    ///     The state of the humid air
    ///     at the end of the process.
    /// </returns>
    /// <exception cref="ArgumentException">
    ///     During the heating process,
    ///     the temperature should increase!
    /// </exception>
    /// <exception cref="ArgumentException">
    ///     Invalid pressure drop
    ///     in the heat exchanger!
    /// </exception>
    public HumidAir HeatingTo(
        Temperature temperature,
        Pressure? pressureDrop = null
    ) => DryHeatTransferTo(temperature, false, pressureDrop);

    /// <summary>
    ///     The process of heating
    ///     to a given enthalpy.
    /// </summary>
    /// <param name="enthalpy">Enthalpy.</param>
    /// <param name="pressureDrop">
    ///     Pressure drop in
    ///     the heat exchanger (optional).
    /// </param>
    /// <returns>
    ///     The state of the humid air
    ///     at the end of the process.
    /// </returns>
    /// <exception cref="ArgumentException">
    ///     During the heating process,
    ///     the enthalpy should increase!
    /// </exception>
    /// <exception cref="ArgumentException">
    ///     Invalid pressure drop
    ///     in the heat exchanger!
    /// </exception>
    public HumidAir HeatingTo(
        SpecificEnergy enthalpy,
        Pressure? pressureDrop = null
    ) => DryHeatTransferTo(enthalpy, false, pressureDrop);

    /// <summary>
    ///     The process of humidification
    ///     by water (isenthalpic)
    ///     to a given relative humidity ratio.
    /// </summary>
    /// <param name="relativeHumidity">
    ///     Relative humidity ratio.
    /// </param>
    /// <returns>
    ///     The state of the humid air
    ///     at the end of the process.
    /// </returns>
    /// <exception cref="ArgumentException">
    ///     During the humidification process,
    ///     the absolute humidity ratio should increase!
    /// </exception>
    public HumidAir HumidificationByWaterTo(
        RelativeHumidity relativeHumidity
    ) => HumidificationTo(
        InputHumidAir.Enthalpy(Enthalpy),
        InputHumidAir.RelativeHumidity(relativeHumidity)
    );

    /// <summary>
    ///     The process of humidification
    ///     by water (isenthalpic)
    ///     to a given absolute humidity ratio.
    /// </summary>
    /// <param name="humidity">
    ///     Absolute humidity ratio.
    /// </param>
    /// <returns>
    ///     The state of the humid air
    ///     at the end of the process.
    /// </returns>
    /// <exception cref="ArgumentException">
    ///     During the humidification process,
    ///     the absolute humidity ratio should increase!
    /// </exception>
    public HumidAir HumidificationByWaterTo(Ratio humidity) =>
        HumidificationTo(
            InputHumidAir.Enthalpy(Enthalpy),
            InputHumidAir.Humidity(humidity)
        );

    /// <summary>
    ///     The process of humidification
    ///     by steam (isothermal)
    ///     to a given relative humidity ratio.
    /// </summary>
    /// <param name="relativeHumidity">
    ///     Relative humidity ratio.
    /// </param>
    /// <returns>
    ///     The state of the humid air
    ///     at the end of the process.
    /// </returns>
    /// <exception cref="ArgumentException">
    ///     During the humidification process,
    ///     the absolute humidity ratio should increase!
    /// </exception>
    public HumidAir HumidificationBySteamTo(
        RelativeHumidity relativeHumidity
    ) => HumidificationTo(
        InputHumidAir.Temperature(Temperature),
        InputHumidAir.RelativeHumidity(relativeHumidity)
    );

    /// <summary>
    ///     The process of humidification
    ///     by steam (isothermal)
    ///     to a given absolute humidity ratio.
    /// </summary>
    /// <param name="humidity">
    ///     Absolute humidity ratio.
    /// </param>
    /// <returns>
    ///     The state of the humid air
    ///     at the end of the process.
    /// </returns>
    /// <exception cref="ArgumentException">
    ///     During the humidification process,
    ///     the absolute humidity ratio should increase!
    /// </exception>
    public HumidAir HumidificationBySteamTo(Ratio humidity) =>
        HumidificationTo(
            InputHumidAir.Temperature(Temperature),
            InputHumidAir.Humidity(humidity)
        );

    /// <summary>
    ///     The mixing process.
    /// </summary>
    /// <param name="firstSpecificMassFlow">
    ///     Specific mass flow rate
    ///     of the humid air at the first state.
    /// </param>
    /// <param name="first">
    ///     Humid air at the first state.
    /// </param>
    /// <param name="secondSpecificMassFlow">
    ///     Specific mass flow rate
    ///     of the humid air the second state.
    /// </param>
    /// <param name="second">
    ///     Humid air at the second state.
    /// </param>
    /// <returns>
    ///     The state of the humid air
    ///     at the end of the process.
    /// </returns>
    /// <exception cref="ArgumentException">
    ///     The mixing process is possible
    ///     only for flows with the same pressure!
    /// </exception>
    public HumidAir Mixing(
        Ratio firstSpecificMassFlow,
        HumidAir first,
        Ratio secondSpecificMassFlow,
        HumidAir second
    ) => first.Pressure.Equals(second.Pressure, Tolerance.Pascals())
        ? WithState(
            InputHumidAir.Pressure(first.Pressure),
            InputHumidAir.Enthalpy(
                (firstSpecificMassFlow.DecimalFractions *
                 first.Enthalpy +
                 secondSpecificMassFlow.DecimalFractions *
                 second.Enthalpy) /
                (firstSpecificMassFlow +
                 secondSpecificMassFlow)
                .DecimalFractions
            ),
            InputHumidAir.Humidity(
                (firstSpecificMassFlow.DecimalFractions *
                 first.Humidity +
                 secondSpecificMassFlow.DecimalFractions *
                 second.Humidity) /
                (firstSpecificMassFlow +
                 secondSpecificMassFlow)
                .DecimalFractions
            )
        )
        : throw new ArgumentException(
            "The mixing process is possible " +
            "only for flows with the same pressure!"
        );

    private HumidAir DryHeatTransferTo(
        Temperature temperature,
        bool cooling,
        Pressure? pressureDrop = null
    )
    {
        CheckTemperature(temperature, cooling);
        CheckDewTemperature(temperature);
        CheckPressureDrop(pressureDrop);
        return WithState(
            InputHumidAir.Pressure(
                Pressure - (pressureDrop ?? Pressure.Zero)
            ),
            InputHumidAir.Temperature(temperature),
            InputHumidAir.Humidity(Humidity)
        );
    }

    private HumidAir DryHeatTransferTo(
        SpecificEnergy enthalpy,
        bool cooling,
        Pressure? pressureDrop = null
    )
    {
        CheckEnthalpy(enthalpy, cooling);
        CheckDewEnthalpy(enthalpy);
        CheckPressureDrop(pressureDrop);
        return WithState(
            InputHumidAir.Pressure(
                Pressure - (pressureDrop ?? Pressure.Zero)
            ),
            InputHumidAir.Enthalpy(enthalpy),
            InputHumidAir.Humidity(Humidity)
        );
    }

    private HumidAir WetCoolingTo(
        IKeyedInput<string> fistInput,
        IKeyedInput<string> secondInput,
        Pressure? pressureDrop = null
    )
    {
        if (fistInput.CoolPropKey == "T")
            CheckTemperature(fistInput.Value.Kelvins(), true);
        if (fistInput.CoolPropKey == "Hha")
            CheckEnthalpy(fistInput.Value.JoulesPerKilogram(), true);
        CheckPressureDrop(pressureDrop);
        var result = WithState(
            InputHumidAir.Pressure(
                Pressure - (pressureDrop ?? Pressure.Zero)
            ),
            fistInput,
            secondInput
        );
        return result.Humidity < Humidity
            ? result
            : throw new ArgumentException(
                "During the wet cooling process, " +
                "the absolute humidity ratio should decrease!"
            );
    }

    private HumidAir HumidificationTo(
        IKeyedInput<string> fistInput,
        IKeyedInput<string> secondInput
    )
    {
        var result = WithState(
            InputHumidAir.Pressure(Pressure),
            fistInput,
            secondInput
        );
        return result.Humidity > Humidity
            ? result
            : throw new ArgumentException(
                "During the humidification process, " +
                "the absolute humidity ratio should increase!"
            );
    }

    private void CheckTemperature(Temperature temperature, bool cooling)
    {
        switch (cooling)
        {
            case true when temperature >= Temperature:
                throw new ArgumentException(
                    "During the cooling process, " +
                    "the temperature should decrease!"
                );
            case false when temperature <= Temperature:
                throw new ArgumentException(
                    "During the heating process, " +
                    "the temperature should increase!"
                );
        }
    }

    private void CheckEnthalpy(SpecificEnergy enthalpy, bool cooling)
    {
        switch (cooling)
        {
            case true when enthalpy >= Enthalpy:
                throw new ArgumentException(
                    "During the cooling process, " +
                    "the enthalpy should decrease!"
                );
            case false when enthalpy <= Enthalpy:
                throw new ArgumentException(
                    "During the heating process, " +
                    "the enthalpy should increase!"
                );
        }
    }

    private void CheckDewTemperature(Temperature temperature)
    {
        if (temperature < DewTemperature)
            throw new ArgumentException(
                "The outlet temperature after " +
                "dry heat transfer should be " +
                "greater than the dew point temperature!"
            );
    }

    private void CheckDewEnthalpy(SpecificEnergy enthalpy)
    {
        if (enthalpy < DewPoint.Enthalpy)
            throw new ArgumentException(
                "The outlet enthalpy after " +
                "dry heat transfer should be " +
                "greater than the dew point enthalpy!"
            );
    }

    private static void CheckPressureDrop(Pressure? pressureDrop)
    {
        if (pressureDrop.HasValue && pressureDrop.Value < Pressure.Zero)
            throw new ArgumentException(
                "Invalid pressure drop " +
                "in the heat exchanger!"
            );
    }
}