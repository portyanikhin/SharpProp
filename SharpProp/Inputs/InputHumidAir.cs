using UnitsNet;

namespace SharpProp
{
    /// <summary>
    ///     CoolProp keyed input for humid air
    /// </summary>
    public record InputHumidAir : IKeyedInput<string>
    {
        /// <summary>
        ///     CoolProp keyed input for humid air
        /// </summary>
        /// <param name="coolPropKey">CoolProp internal key</param>
        /// <param name="value">Input value</param>
        protected InputHumidAir(string coolPropKey, double value) => (CoolPropKey, Value) = (coolPropKey, value);

        /// <summary>
        ///     CoolProp internal key
        /// </summary>
        public string CoolPropKey { get; }

        /// <summary>
        ///     Input value
        /// </summary>
        public double Value { get; }

        /// <summary>
        ///     Mass density per humid air unit
        /// </summary>
        /// <param name="value">The value of the input</param>
        /// <returns>Mass density per humid air unit for the input</returns>
        public static InputHumidAir Density(Density value) => new("Vha", 1.0 / value.KilogramsPerCubicMeter);

        /// <summary>
        ///     Dew-point temperature
        /// </summary>
        /// <param name="value">The value of the input</param>
        /// <returns>Dew-point temperature for the input</returns>
        public static InputHumidAir DewTemperature(Temperature value) => new("D", value.Kelvins);

        /// <summary>
        ///     Mass specific enthalpy per humid air
        /// </summary>
        /// <param name="value">The value of the input</param>
        /// <returns>Mass specific enthalpy per humid air for the input</returns>
        public static InputHumidAir Enthalpy(SpecificEnergy value) => new("Hha", value.JoulesPerKilogram);

        /// <summary>
        ///     Mass specific entropy per humid air
        /// </summary>
        /// <param name="value">The value of the input</param>
        /// <returns>Mass specific entropy per humid air for the input</returns>
        public static InputHumidAir Entropy(SpecificEntropy value) => new("Sha", value.JoulesPerKilogramKelvin);

        /// <summary>
        ///     Absolute humidity ratio
        /// </summary>
        /// <param name="value">The value of the input</param>
        /// <returns>Absolute humidity ratio for the input</returns>
        public static InputHumidAir Humidity(Ratio value) => new("W", value.DecimalFractions);

        /// <summary>
        ///     Partial pressure of water vapor
        /// </summary>
        /// <param name="value">The value of the input</param>
        /// <returns>Partial pressure of water vapor for the input</returns>
        public static InputHumidAir PartialPressure(Pressure value) => new("P_w", value.Pascals);

        /// <summary>
        ///     Absolute pressure
        /// </summary>
        /// <param name="value">The value of the input</param>
        /// <returns>Absolute pressure for the input</returns>
        public static InputHumidAir Pressure(Pressure value) => new("P", value.Pascals);

        /// <summary>
        ///     Relative humidity ratio
        /// </summary>
        /// <param name="value">The value of the input</param>
        /// <returns>Relative humidity ratio for the input</returns>
        public static InputHumidAir RelativeHumidity(RelativeHumidity value) => 
            new("R", Ratio.FromPercent(value.Percent).DecimalFractions);

        /// <summary>
        ///     Dry-bulb temperature
        /// </summary>
        /// <param name="value">The value of the input</param>
        /// <returns>Dry-bulb temperature for the input</returns>
        public static InputHumidAir Temperature(Temperature value) => new("T", value.Kelvins);

        /// <summary>
        ///     Wet-bulb temperature
        /// </summary>
        /// <param name="value">The value of the input</param>
        /// <returns>Wet-bulb temperature for the input</returns>
        public static InputHumidAir WetBulbTemperature(Temperature value) => new("B", value.Kelvins);
    }
}