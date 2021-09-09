using System.Collections.Generic;

namespace SharpProp
{
    public partial class HumidAir
    {
        private double? _compressibility;
        private double? _conductivity;
        private double? _density;
        private double? _dewTemperature;
        private double? _dynamicViscosity;
        private double? _enthalpy;
        private double? _entropy;
        private double? _humidity;
        private List<IKeyedInput<string>> _inputs = new(3);
        private double? _partialPressure;
        private double? _pressure;
        private double? _relativeHumidity;
        private double? _specificHeat;
        private double? _temperature;
        private double? _wetBulbTemperature;

        /// <summary>
        ///     Compressibility factor [-]
        /// </summary>
        public double Compressibility => _compressibility ??= KeyedOutput("Z");

        /// <summary>
        ///     Thermal conductivity [W/m/K]
        /// </summary>
        public double Conductivity => _conductivity ??= KeyedOutput("K");

        /// <summary>
        ///     Mass density per humid air unit [kg/m3]
        /// </summary>
        public double Density => _density ??= 1.0 / KeyedOutput("Vha");

        /// <summary>
        ///     Dew-point absolute temperature [K]
        /// </summary>
        public double DewTemperature => _dewTemperature ??= KeyedOutput("D");

        /// <summary>
        ///     Dynamic viscosity [Pa*s]
        /// </summary>
        public double DynamicViscosity => _dynamicViscosity ??= KeyedOutput("M");

        /// <summary>
        ///     Mass specific enthalpy per humid air [J/kg]
        /// </summary>
        public double Enthalpy => _enthalpy ??= KeyedOutput("Hha");

        /// <summary>
        ///     Mass specific entropy per humid air [J/kg/K]
        /// </summary>
        public double Entropy => _entropy ??= KeyedOutput("Sha");

        /// <summary>
        ///     Absolute humidity ratio [kg/kg d.a.]
        /// </summary>
        public double Humidity => _humidity ??= KeyedOutput("W");

        /// <summary>
        ///     Partial pressure of water vapor [Pa]
        /// </summary>
        public double PartialPressure => _partialPressure ??= KeyedOutput("P_w");

        /// <summary>
        ///     Absolute pressure [Pa]
        /// </summary>
        public double Pressure => _pressure ??= KeyedOutput("P");

        /// <summary>
        ///     Relative humidity ratio (from 0 to 1) [-]
        /// </summary>
        public double RelativeHumidity => _relativeHumidity ??= KeyedOutput("R");

        /// <summary>
        ///     Mass specific constant pressure specific heat per humid air [J/kg/K]
        /// </summary>
        public double SpecificHeat => _specificHeat ??= KeyedOutput("Cha");

        /// <summary>
        ///     Absolute dry-bulb temperature [K]
        /// </summary>
        public double Temperature => _temperature ??= KeyedOutput("T");

        /// <summary>
        ///     Absolute wet-bulb temperature [K]
        /// </summary>
        public double WetBulbTemperature => _wetBulbTemperature ??= KeyedOutput("B");
    }
}