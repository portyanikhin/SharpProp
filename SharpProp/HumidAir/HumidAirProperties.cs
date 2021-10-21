using System.Collections.Generic;
using UnitsNet;
using UnitsNet.Units;

namespace SharpProp
{
    public partial class HumidAir
    {
        private double? _compressibility;
        private ThermalConductivity? _conductivity;
        private Density? _density;
        private Temperature? _dewTemperature;
        private DynamicViscosity? _dynamicViscosity;
        private SpecificEnergy? _enthalpy;
        private SpecificEntropy? _entropy;
        private Ratio? _humidity;
        private Pressure? _partialPressure;
        private Pressure? _pressure;
        private RelativeHumidity? _relativeHumidity;
        private SpecificEntropy? _specificHeat;
        private Temperature? _temperature;
        private Temperature? _wetBulbTemperature;

        /// <summary>
        ///     Compressibility factor
        /// </summary>
        public double Compressibility => _compressibility ??= KeyedOutput("Z");

        /// <summary>
        ///     Thermal conductivity
        /// </summary>
        public ThermalConductivity Conductivity => _conductivity ??=
            ThermalConductivity.FromWattsPerMeterKelvin(KeyedOutput("K"));

        /// <summary>
        ///     Mass density per humid air unit
        /// </summary>
        public Density Density => _density ??=
            Density.FromKilogramsPerCubicMeter(1.0 / KeyedOutput("Vha"));

        /// <summary>
        ///     Dew-point temperature
        /// </summary>
        public Temperature DewTemperature => _dewTemperature ??=
            Temperature.FromKelvins(KeyedOutput("D"))
                .ToUnit(TemperatureUnit.DegreeCelsius);

        /// <summary>
        ///     Dynamic viscosity
        /// </summary>
        public DynamicViscosity DynamicViscosity => _dynamicViscosity ??=
            DynamicViscosity.FromPascalSeconds(KeyedOutput("M"))
                .ToUnit(DynamicViscosityUnit.MillipascalSecond);

        /// <summary>
        ///     Mass specific enthalpy per humid air
        /// </summary>
        public SpecificEnergy Enthalpy => _enthalpy ??=
            SpecificEnergy.FromJoulesPerKilogram(KeyedOutput("Hha"))
                .ToUnit(SpecificEnergyUnit.KilojoulePerKilogram);

        /// <summary>
        ///     Mass specific entropy per humid air
        /// </summary>
        public SpecificEntropy Entropy => _entropy ??=
            SpecificEntropy.FromJoulesPerKilogramKelvin(KeyedOutput("Sha"))
                .ToUnit(SpecificEntropyUnit.KilojoulePerKilogramKelvin);

        /// <summary>
        ///     Absolute humidity ratio
        /// </summary>
        public Ratio Humidity => _humidity ??= 
            Ratio.FromDecimalFractions(KeyedOutput("W"))
                .ToUnit(RatioUnit.PartPerThousand);

        private List<IKeyedInput<string>> Inputs { get; set; } = new(3);

        /// <summary>
        ///     Partial pressure of water vapor
        /// </summary>
        public Pressure PartialPressure => _partialPressure ??=
            Pressure.FromPascals(KeyedOutput("P_w"))
                .ToUnit(PressureUnit.Kilopascal);

        /// <summary>
        ///     Absolute pressure
        /// </summary>
        public Pressure Pressure => _pressure ??=
            Pressure.FromPascals(KeyedOutput("P"))
                .ToUnit(PressureUnit.Kilopascal);

        /// <summary>
        ///     Relative humidity ratio
        /// </summary>
        public RelativeHumidity RelativeHumidity => _relativeHumidity ??=
            RelativeHumidity.FromPercent(Ratio.FromDecimalFractions(KeyedOutput("R")).Percent);

        /// <summary>
        ///     Mass specific constant pressure specific heat per humid air
        /// </summary>
        public SpecificEntropy SpecificHeat => _specificHeat ??=
            SpecificEntropy.FromJoulesPerKilogramKelvin(KeyedOutput("Cha"))
                .ToUnit(SpecificEntropyUnit.KilojoulePerKilogramKelvin);

        /// <summary>
        ///     Dry-bulb temperature
        /// </summary>
        public Temperature Temperature => _temperature ??=
            Temperature.FromKelvins(KeyedOutput("T"))
                .ToUnit(TemperatureUnit.DegreeCelsius);

        /// <summary>
        ///     Wet-bulb temperature
        /// </summary>
        public Temperature WetBulbTemperature => _wetBulbTemperature ??=
            Temperature.FromKelvins(KeyedOutput("B"))
                .ToUnit(TemperatureUnit.DegreeCelsius);
    }
}