namespace SharpProp
{
    public class InputHumidAir : IKeyedInput<string>
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
        /// <param name="value">The value [kg/m3]</param>
        /// <returns>Mass density per humid air unit for the input [kg/m3]</returns>
        public static InputHumidAir Density(double value) => new("Vha", value);

        /// <summary>
        ///     Dew-point absolute temperature
        /// </summary>
        /// <param name="value">The value [K]</param>
        /// <returns>Dew-point absolute temperature for the input [K]</returns>
        public static InputHumidAir DewTemperature(double value) => new("D", value);

        /// <summary>
        ///     Mass specific enthalpy per humid air
        /// </summary>
        /// <param name="value">The value [J/kg]</param>
        /// <returns>Mass specific enthalpy per humid air for the input [J/kg]</returns>
        public static InputHumidAir Enthalpy(double value) => new("Hha", value);

        /// <summary>
        ///     Mass specific entropy per humid air
        /// </summary>
        /// <param name="value">The value [J/kg/K]</param>
        /// <returns>Mass specific entropy per humid air for the input [J/kg/K]</returns>
        public static InputHumidAir Entropy(double value) => new("Sha", value);

        /// <summary>
        ///     Absolute humidity ratio
        /// </summary>
        /// <param name="value">The value [kg/kg d.a.]</param>
        /// <returns>Absolute humidity ratio for the input [kg/kg d.a.]</returns>
        public static InputHumidAir Humidity(double value) => new("W", value);

        /// <summary>
        ///     Partial pressure of water vapor
        /// </summary>
        /// <param name="value">The value [Pa]</param>
        /// <returns>Partial pressure of water vapor for the input [Pa]</returns>
        public static InputHumidAir PartialPressure(double value) => new("P_w", value);

        /// <summary>
        ///     Absolute pressure
        /// </summary>
        /// <param name="value">The value [Pa]</param>
        /// <returns>Absolute pressure for the input [Pa]</returns>
        public static InputHumidAir Pressure(double value) => new("P", value);

        /// <summary>
        ///     Relative humidity ratio
        /// </summary>
        /// <param name="value">The value (from 0 to 1) [-]</param>
        /// <returns>Relative humidity ratio (from 0 to 1) for the input [-]</returns>
        public static InputHumidAir RelativeHumidity(double value) => new("R", value);

        /// <summary>
        ///     Absolute dry-bulb temperature
        /// </summary>
        /// <param name="value">The value [K]</param>
        /// <returns>Absolute dry-bulb temperature for the input [K]</returns>
        public static InputHumidAir Temperature(double value) => new("T", value);

        /// <summary>
        ///     Absolute wet-bulb temperature
        /// </summary>
        /// <param name="value">The value [K]</param>
        /// <returns>Absolute wet-bulb temperature for the input [K]</returns>
        public static InputHumidAir WetBulbTemperature(double value) => new("B", value);
    }
}