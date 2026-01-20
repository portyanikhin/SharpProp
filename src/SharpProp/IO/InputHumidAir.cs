namespace SharpProp;

/// <summary>
///     CoolProp keyed input for humid air.
/// </summary>
/// <param name="CoolPropKey">CoolProp internal key.</param>
/// <param name="Value">Input value in SI units.</param>
public record InputHumidAir(string CoolPropKey, double Value)
    : KeyedInput<string>(CoolPropKey, Value)
{
    public override string CoolPropHighLevelKey => CoolPropKey;

    /// <summary>
    ///     Altitude above sea level.
    /// </summary>
    /// <remarks>
    ///     The pressure will be calculated by altitude above sea level,
    ///     according to ASHRAE Fundamentals Handbook.
    /// </remarks>
    /// <param name="value">The value of the input.</param>
    /// <returns>Altitude above sea level for the input.</returns>
    /// <exception cref="ArgumentOutOfRangeException">
    ///     Altitude above sea level should be between -5 000 and 11 000 meters!
    /// </exception>
    public static InputHumidAir Altitude(Length value) =>
        value.Meters is < -5000 or > 11000
            ? throw new ArgumentOutOfRangeException(
                nameof(value),
                value,
                "Altitude above sea level should be between -5 000 and 11 000 meters!"
            )
            : new InputHumidAir("P", 101325 * Math.Pow(1 - 2.25577e-5 * value.Meters, 5.2559));

    /// <summary>
    ///     Mass density per humid air unit.
    /// </summary>
    /// <param name="value">The value of the input.</param>
    /// <returns>
    ///     Mass density per humid air unit for the input.
    /// </returns>
    public static InputHumidAir Density(Density value) =>
        new("Vha", 1.0 / value.KilogramsPerCubicMeter);

    /// <summary>
    ///     Dew-point temperature.
    /// </summary>
    /// <param name="value">The value of the input.</param>
    /// <returns>Dew-point temperature for the input.</returns>
    public static InputHumidAir DewTemperature(Temperature value) => new("D", value.Kelvins);

    /// <summary>
    ///     Mass specific enthalpy per humid air.
    /// </summary>
    /// <param name="value">The value of the input.</param>
    /// <returns>
    ///     Mass specific enthalpy per humid air for the input.
    /// </returns>
    public static InputHumidAir Enthalpy(SpecificEnergy value) =>
        new("Hha", value.JoulesPerKilogram);

    /// <summary>
    ///     Mass specific entropy per humid air.
    /// </summary>
    /// <param name="value">The value of the input.</param>
    /// <returns>
    ///     Mass specific entropy per humid air for the input.
    /// </returns>
    public static InputHumidAir Entropy(SpecificEntropy value) =>
        new("Sha", value.JoulesPerKilogramKelvin);

    /// <summary>
    ///     Absolute humidity ratio.
    /// </summary>
    /// <param name="value">The value of the input.</param>
    /// <returns>Absolute humidity ratio for the input.</returns>
    public static InputHumidAir Humidity(Ratio value) => new("W", value.DecimalFractions);

    /// <summary>
    ///     Partial pressure of water vapor.
    /// </summary>
    /// <param name="value">The value of the input.</param>
    /// <returns>
    ///     Partial pressure of water vapor for the input.
    /// </returns>
    public static InputHumidAir PartialPressure(Pressure value) => new("P_w", value.Pascals);

    /// <summary>
    ///     Absolute pressure.
    /// </summary>
    /// <param name="value">The value of the input.</param>
    /// <returns>Absolute pressure for the input.</returns>
    public static InputHumidAir Pressure(Pressure value) => new("P", value.Pascals);

    /// <summary>
    ///     Relative humidity ratio.
    /// </summary>
    /// <param name="value">The value of the input.</param>
    /// <returns>Relative humidity ratio for the input.</returns>
    public static InputHumidAir RelativeHumidity(RelativeHumidity value) =>
        new("R", Ratio.FromPercent(value.Percent).DecimalFractions);

    /// <summary>
    ///     Mass specific volume per humid air unit.
    /// </summary>
    /// <param name="value">The value of the input.</param>
    /// <returns>
    ///     Mass specific volume per humid air unit for the input.
    /// </returns>
    public static InputHumidAir SpecificVolume(SpecificVolume value) =>
        new("Vha", value.CubicMetersPerKilogram);

    /// <summary>
    ///     Dry-bulb temperature.
    /// </summary>
    /// <param name="value">The value of the input.</param>
    /// <returns>Dry-bulb temperature for the input.</returns>
    public static InputHumidAir Temperature(Temperature value) => new("T", value.Kelvins);

    /// <summary>
    ///     Wet-bulb temperature.
    /// </summary>
    /// <param name="value">The value of the input.</param>
    /// <returns>Wet-bulb temperature for the input.</returns>
    public static InputHumidAir WetBulbTemperature(Temperature value) => new("B", value.Kelvins);
}
