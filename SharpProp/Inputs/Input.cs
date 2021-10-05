using CoolProp;

namespace SharpProp
{
    /// <summary>
    ///     CoolProp keyed input for fluids and mixtures
    /// </summary>
    public record Input : IKeyedInput<parameters>
    {
        /// <summary>
        ///     CoolProp keyed input for fluids and mixtures
        /// </summary>
        /// <param name="coolPropKey">CoolProp internal key</param>
        /// <param name="value">Input value</param>
        protected Input(parameters coolPropKey, double value) => (CoolPropKey, Value) = (coolPropKey, value);

        /// <summary>
        ///     CoolProp internal key
        /// </summary>
        public parameters CoolPropKey { get; }

        /// <summary>
        ///     Input value
        /// </summary>
        public double Value { get; }

        /// <summary>
        ///     Mass density
        /// </summary>
        /// <param name="value">The value [kg/m3]</param>
        /// <returns>Mass density for the input [kg/m3]</returns>
        public static Input Density(double value) => new(parameters.iDmass, value);

        /// <summary>
        ///     Mass specific enthalpy
        /// </summary>
        /// <param name="value">The value [J/kg]</param>
        /// <returns>Mass specific enthalpy for the input [J/kg]</returns>
        public static Input Enthalpy(double value) => new(parameters.iHmass, value);

        /// <summary>
        ///     Mass specific entropy
        /// </summary>
        /// <param name="value">The value [J/kg/K]</param>
        /// <returns>Mass specific entropy for the input [J/kg/K]</returns>
        public static Input Entropy(double value) => new(parameters.iSmass, value);

        /// <summary>
        ///     Mass specific internal energy
        /// </summary>
        /// <param name="value">The value [J/kg]</param>
        /// <returns>Mass specific internal energy for the input [J/kg]</returns>
        public static Input InternalEnergy(double value) => new(parameters.iUmass, value);

        /// <summary>
        ///     Absolute pressure
        /// </summary>
        /// <param name="value">The value [Pa]</param>
        /// <returns>Absolute pressure for the input [Pa]</returns>
        public static Input Pressure(double value) => new(parameters.iP, value);

        /// <summary>
        ///     Mass vapor quality
        /// </summary>
        /// <param name="value">The value [-]</param>
        /// <returns>Mass vapor quality for the input [-]</returns>
        public static Input Quality(double value) => new(parameters.iQ, value);

        /// <summary>
        ///     Absolute temperature
        /// </summary>
        /// <param name="value">The value [K]</param>
        /// <returns>Absolute temperature for the input</returns>
        public static Input Temperature(double value) => new(parameters.iT, value);
    }
}