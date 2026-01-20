using UnitsNet.NumberExtensions.NumberToRelativeHumidity;

namespace SharpProp;

public partial class HumidAir
{
    private IHumidAir DewPoint =>
        WithState(
            InputHumidAir.Pressure(Pressure),
            InputHumidAir.Temperature(DewTemperature),
            InputHumidAir.RelativeHumidity(100.Percent())
        );

    public IHumidAir DryCoolingTo(Temperature temperature, Pressure? pressureDrop = null) =>
        DryHeatTransferTo(temperature, true, pressureDrop);

    public IHumidAir DryCoolingTo(SpecificEnergy enthalpy, Pressure? pressureDrop = null) =>
        DryHeatTransferTo(enthalpy, true, pressureDrop);

    public IHumidAir WetCoolingTo(
        Temperature temperature,
        RelativeHumidity relativeHumidity,
        Pressure? pressureDrop = null
    ) =>
        WetCoolingTo(
            InputHumidAir.Temperature(temperature),
            InputHumidAir.RelativeHumidity(relativeHumidity),
            pressureDrop
        );

    public IHumidAir WetCoolingTo(
        Temperature temperature,
        Ratio humidity,
        Pressure? pressureDrop = null
    ) =>
        WetCoolingTo(
            InputHumidAir.Temperature(temperature),
            InputHumidAir.Humidity(humidity),
            pressureDrop
        );

    public IHumidAir WetCoolingTo(
        SpecificEnergy enthalpy,
        RelativeHumidity relativeHumidity,
        Pressure? pressureDrop = null
    ) =>
        WetCoolingTo(
            InputHumidAir.Enthalpy(enthalpy),
            InputHumidAir.RelativeHumidity(relativeHumidity),
            pressureDrop
        );

    public IHumidAir WetCoolingTo(
        SpecificEnergy enthalpy,
        Ratio humidity,
        Pressure? pressureDrop = null
    ) =>
        WetCoolingTo(
            InputHumidAir.Enthalpy(enthalpy),
            InputHumidAir.Humidity(humidity),
            pressureDrop
        );

    public IHumidAir HeatingTo(Temperature temperature, Pressure? pressureDrop = null) =>
        DryHeatTransferTo(temperature, false, pressureDrop);

    public IHumidAir HeatingTo(SpecificEnergy enthalpy, Pressure? pressureDrop = null) =>
        DryHeatTransferTo(enthalpy, false, pressureDrop);

    public IHumidAir HumidificationByWaterTo(RelativeHumidity relativeHumidity) =>
        HumidificationTo(
            InputHumidAir.Enthalpy(Enthalpy),
            InputHumidAir.RelativeHumidity(relativeHumidity)
        );

    public IHumidAir HumidificationByWaterTo(Ratio humidity) =>
        HumidificationTo(InputHumidAir.Enthalpy(Enthalpy), InputHumidAir.Humidity(humidity));

    public IHumidAir HumidificationBySteamTo(RelativeHumidity relativeHumidity) =>
        HumidificationTo(
            InputHumidAir.Temperature(Temperature),
            InputHumidAir.RelativeHumidity(relativeHumidity)
        );

    public IHumidAir HumidificationBySteamTo(Ratio humidity) =>
        HumidificationTo(InputHumidAir.Temperature(Temperature), InputHumidAir.Humidity(humidity));

    public IHumidAir Mixing(
        Ratio firstSpecificMassFlow,
        IHumidAir first,
        Ratio secondSpecificMassFlow,
        IHumidAir second
    ) =>
        first.Pressure.Equals(second.Pressure, Tolerance.Pascals())
            ? WithState(
                InputHumidAir.Pressure(first.Pressure),
                InputHumidAir.Enthalpy(
                    (
                        firstSpecificMassFlow.DecimalFractions * first.Enthalpy
                        + secondSpecificMassFlow.DecimalFractions * second.Enthalpy
                    ) / (firstSpecificMassFlow + secondSpecificMassFlow).DecimalFractions
                ),
                InputHumidAir.Humidity(
                    Ratio.FromDecimalFractions(
                        (
                            firstSpecificMassFlow.DecimalFractions
                                * first.Humidity.DecimalFractions
                                * (1 + second.Humidity.DecimalFractions)
                            + secondSpecificMassFlow.DecimalFractions
                                * second.Humidity.DecimalFractions
                                * (1 + first.Humidity.DecimalFractions)
                        )
                            / (
                                firstSpecificMassFlow.DecimalFractions
                                    * (1 + second.Humidity.DecimalFractions)
                                + secondSpecificMassFlow.DecimalFractions
                                    * (1 + first.Humidity.DecimalFractions)
                            )
                    )
                )
            )
            : throw new ArgumentException(
                "The mixing process is possible only for flows with the same pressure!"
            );

    private IHumidAir DryHeatTransferTo(
        Temperature temperature,
        bool cooling,
        Pressure? pressureDrop = null
    )
    {
        CheckTemperature(temperature, cooling);
        CheckDewTemperature(temperature);
        CheckPressureDrop(pressureDrop);
        return WithState(
            InputHumidAir.Pressure(Pressure - (pressureDrop ?? Pressure.Zero)),
            InputHumidAir.Temperature(temperature),
            InputHumidAir.Humidity(Humidity)
        );
    }

    private IHumidAir DryHeatTransferTo(
        SpecificEnergy enthalpy,
        bool cooling,
        Pressure? pressureDrop = null
    )
    {
        CheckEnthalpy(enthalpy, cooling);
        CheckDewEnthalpy(enthalpy);
        CheckPressureDrop(pressureDrop);
        return WithState(
            InputHumidAir.Pressure(Pressure - (pressureDrop ?? Pressure.Zero)),
            InputHumidAir.Enthalpy(enthalpy),
            InputHumidAir.Humidity(Humidity)
        );
    }

    private IHumidAir WetCoolingTo(
        IKeyedInput<string> fistInput,
        IKeyedInput<string> secondInput,
        Pressure? pressureDrop = null
    )
    {
        if (fistInput.CoolPropKey == "T")
        {
            CheckTemperature(fistInput.Value.Kelvins(), true);
        }

        if (fistInput.CoolPropKey == "Hha")
        {
            CheckEnthalpy(fistInput.Value.JoulesPerKilogram(), true);
        }

        CheckPressureDrop(pressureDrop);
        var result = WithState(
            InputHumidAir.Pressure(Pressure - (pressureDrop ?? Pressure.Zero)),
            fistInput,
            secondInput
        );
        return result.Humidity < Humidity
            ? result
            : throw new ArgumentException(
                "During the wet cooling process, the absolute humidity ratio should decrease!"
            );
    }

    private IHumidAir HumidificationTo(
        IKeyedInput<string> fistInput,
        IKeyedInput<string> secondInput
    )
    {
        var result = WithState(InputHumidAir.Pressure(Pressure), fistInput, secondInput);
        return result.Humidity > Humidity
            ? result
            : throw new ArgumentException(
                "During the humidification process, the absolute humidity ratio should increase!"
            );
    }

    private void CheckTemperature(Temperature temperature, bool cooling)
    {
        switch (cooling)
        {
            case true when temperature >= Temperature:
                throw new ArgumentException(
                    "During the cooling process, the temperature should decrease!"
                );
            case false when temperature <= Temperature:
                throw new ArgumentException(
                    "During the heating process, the temperature should increase!"
                );
        }
    }

    private void CheckEnthalpy(SpecificEnergy enthalpy, bool cooling)
    {
        switch (cooling)
        {
            case true when enthalpy >= Enthalpy:
                throw new ArgumentException(
                    "During the cooling process, the enthalpy should decrease!"
                );
            case false when enthalpy <= Enthalpy:
                throw new ArgumentException(
                    "During the heating process, the enthalpy should increase!"
                );
        }
    }

    private void CheckDewTemperature(Temperature temperature)
    {
        if (temperature < DewTemperature)
        {
            throw new ArgumentException(
                "The outlet temperature after dry heat transfer should be "
                    + "greater than the dew point temperature!"
            );
        }
    }

    private void CheckDewEnthalpy(SpecificEnergy enthalpy)
    {
        if (enthalpy < DewPoint.Enthalpy)
        {
            throw new ArgumentException(
                "The outlet enthalpy after dry heat transfer should be "
                    + "greater than the dew point enthalpy!"
            );
        }
    }

    private static void CheckPressureDrop(Pressure? pressureDrop)
    {
        if (pressureDrop.HasValue && pressureDrop.Value < Pressure.Zero)
        {
            throw new ArgumentException("Invalid pressure drop in the heat exchanger!");
        }
    }
}
