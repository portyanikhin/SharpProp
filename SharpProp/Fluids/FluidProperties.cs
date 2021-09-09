using CoolProp;

namespace SharpProp
{
    public abstract partial class AbstractFluid
    {
        private double? _compressibility;
        private double? _conductivity;
        private double? _criticalPressure;
        private double? _criticalTemperature;
        private double? _density;
        private double? _dynamicViscosity;
        private double? _enthalpy;
        private double? _entropy;
        private double? _freezingTemperature;
        private double? _internalEnergy;
        private double? _maxPressure;
        private double? _maxTemperature;
        private double? _minPressure;
        private double? _minTemperature;
        private double? _molarMass;
        private Phases? _phase;
        private double? _prandtl;
        private double? _pressure;
        private double? _quality;
        private double? _soundSpeed;
        private double? _specificHeat;
        private double? _surfaceTension;
        private double? _temperature;
        private double? _triplePressure;
        private double? _tripleTemperature;

        /// <summary>
        ///     CoolProp backend
        /// </summary>
        protected AbstractState Backend { get; init; } = null!;

        /// <summary>
        ///     Compressibility factor [-]
        /// </summary>
        public double? Compressibility => _compressibility ??= NullableKeyedOutput(parameters.iZ);

        /// <summary>
        ///     Thermal conductivity [W/m/K]
        /// </summary>
        public double? Conductivity => _conductivity ??= NullableKeyedOutput(parameters.iconductivity);

        /// <summary>
        ///     Absolute pressure at the critical point [Pa]
        /// </summary>
        public double? CriticalPressure => _criticalPressure ??= NullableKeyedOutput(parameters.iP_critical);

        /// <summary>
        ///     Absolute temperature at the critical point [K]
        /// </summary>
        public double? CriticalTemperature => _criticalTemperature ??= NullableKeyedOutput(parameters.iT_critical);

        /// <summary>
        ///     Mass density [kg/m3]
        /// </summary>
        public double Density => _density ??= KeyedOutput(parameters.iDmass);

        /// <summary>
        ///     Dynamic viscosity [Pa*s]
        /// </summary>
        public double? DynamicViscosity => _dynamicViscosity ??= NullableKeyedOutput(parameters.iviscosity);

        /// <summary>
        ///     Mass specific enthalpy [J/kg]
        /// </summary>
        public double Enthalpy => _enthalpy ??= KeyedOutput(parameters.iHmass);

        /// <summary>
        ///     Mass specific entropy [J/kg/K]
        /// </summary>
        public double Entropy => _entropy ??= KeyedOutput(parameters.iSmass);

        /// <summary>
        ///     Temperature at freezing point (for incompressible fluids) [K]
        /// </summary>
        public double? FreezingTemperature => _freezingTemperature ??= NullableKeyedOutput(parameters.iT_freeze);

        /// <summary>
        ///     Mass specific internal energy [J/kg]
        /// </summary>
        public double InternalEnergy => _internalEnergy ??= KeyedOutput(parameters.iUmass);

        /// <summary>
        ///     Maximum pressure limit [Pa]
        /// </summary>
        public double? MaxPressure => _maxPressure ??= NullableKeyedOutput(parameters.iP_max);

        /// <summary>
        ///     Maximum temperature limit [K]
        /// </summary>
        public double MaxTemperature => _maxTemperature ??= KeyedOutput(parameters.iT_max);

        /// <summary>
        ///     Minimum pressure limit [Pa]
        /// </summary>
        public double? MinPressure => _minPressure ??= NullableKeyedOutput(parameters.iP_min);

        /// <summary>
        ///     Minimum temperature limit [K]
        /// </summary>
        public double MinTemperature => _minTemperature ??= KeyedOutput(parameters.iT_min);

        /// <summary>
        ///     Molar mass [kg/mol]
        /// </summary>
        public double? MolarMass => _molarMass ??= NullableKeyedOutput(parameters.imolar_mass);

        /// <summary>
        ///     Phase
        /// </summary>
        public Phases Phase => _phase ??= (Phases) KeyedOutput(parameters.iPhase);

        /// <summary>
        ///     Prandtl number [-]
        /// </summary>
        public double? Prandtl => _prandtl ??= NullableKeyedOutput(parameters.iPrandtl);

        /// <summary>
        ///     Absolute pressure [Pa]
        /// </summary>
        public double Pressure => _pressure ??= KeyedOutput(parameters.iP);

        /// <summary>
        ///     Mass vapor quality [-]
        /// </summary>
        public double Quality => _quality ??= KeyedOutput(parameters.iQ);

        /// <summary>
        ///     Sound speed [m/s]
        /// </summary>
        public double? SoundSpeed => _soundSpeed ??= NullableKeyedOutput(parameters.ispeed_sound);

        /// <summary>
        ///     Mass specific constant pressure specific heat [J/kg/K]
        /// </summary>
        public double SpecificHeat => _specificHeat ??= KeyedOutput(parameters.iCpmass);

        /// <summary>
        ///     Surface tension [N/m]
        /// </summary>
        public double? SurfaceTension => _surfaceTension ??= NullableKeyedOutput(parameters.isurface_tension);

        /// <summary>
        ///     Absolute temperature [K]
        /// </summary>
        public double Temperature => _temperature ??= KeyedOutput(parameters.iT);

        /// <summary>
        ///     Absolute pressure at the triple point [Pa]
        /// </summary>
        public double? TriplePressure => _triplePressure ??= NullableKeyedOutput(parameters.iP_triple);

        /// <summary>
        ///     Absolute temperature at the triple point [K]
        /// </summary>
        public double? TripleTemperature => _tripleTemperature ??= NullableKeyedOutput(parameters.iT_triple);
    }
}