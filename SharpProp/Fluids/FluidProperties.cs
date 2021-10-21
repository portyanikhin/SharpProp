using CoolProp;
using UnitsNet;
using UnitsNet.Units;

namespace SharpProp
{
    public abstract partial class AbstractFluid
    {
        private double? _compressibility;
        private ThermalConductivity? _conductivity;
        private Pressure? _criticalPressure;
        private Temperature? _criticalTemperature;
        private Density? _density;
        private DynamicViscosity? _dynamicViscosity;
        private SpecificEnergy? _enthalpy;
        private SpecificEntropy? _entropy;
        private Temperature? _freezingTemperature;
        private SpecificEnergy? _internalEnergy;
        private Pressure? _maxPressure;
        private Temperature? _maxTemperature;
        private Pressure? _minPressure;
        private Temperature? _minTemperature;
        private MolarMass? _molarMass;
        private Phases? _phase;
        private double? _prandtl;
        private Pressure? _pressure;
        private Ratio? _quality;
        private Speed? _soundSpeed;
        private SpecificEntropy? _specificHeat;
        private ForcePerLength? _surfaceTension;
        private Temperature? _temperature;
        private Pressure? _triplePressure;
        private Temperature? _tripleTemperature;

        /// <summary>
        ///     CoolProp backend
        /// </summary>
        protected AbstractState Backend { get; init; } = null!;

        /// <summary>
        ///     Compressibility factor
        /// </summary>
        public double? Compressibility => _compressibility ??= NullableKeyedOutput(Parameters.iZ);

        /// <summary>
        ///     Thermal conductivity
        /// </summary>
        public ThermalConductivity? Conductivity => _conductivity ??=
            KeyedOutputIsNotNull(Parameters.iconductivity, out var output)
                ? ThermalConductivity.FromWattsPerMeterKelvin(output!.Value)
                : null;

        /// <summary>
        ///     Absolute pressure at the critical point
        /// </summary>
        public Pressure? CriticalPressure => _criticalPressure ??=
            KeyedOutputIsNotNull(Parameters.iP_critical, out var output)
                ? Pressure.FromPascals(output!.Value).ToUnit(PressureUnit.Kilopascal)
                : null;

        /// <summary>
        ///     Temperature at the critical point
        /// </summary>
        public Temperature? CriticalTemperature => _criticalTemperature ??=
            KeyedOutputIsNotNull(Parameters.iT_critical, out var output)
                ? Temperature.FromKelvins(output!.Value).ToUnit(TemperatureUnit.DegreeCelsius)
                : null;

        /// <summary>
        ///     Mass density
        /// </summary>
        public Density Density => _density ??= Density
            .FromKilogramsPerCubicMeter(KeyedOutput(Parameters.iDmass));

        /// <summary>
        ///     Dynamic viscosity
        /// </summary>
        public DynamicViscosity? DynamicViscosity => _dynamicViscosity ??=
            KeyedOutputIsNotNull(Parameters.iviscosity, out var output)
                ? UnitsNet.DynamicViscosity.FromPascalSeconds(output!.Value)
                    .ToUnit(DynamicViscosityUnit.MillipascalSecond)
                : null;

        /// <summary>
        ///     Mass specific enthalpy
        /// </summary>
        public SpecificEnergy Enthalpy => _enthalpy ??= SpecificEnergy
            .FromJoulesPerKilogram(KeyedOutput(Parameters.iHmass))
            .ToUnit(SpecificEnergyUnit.KilojoulePerKilogram);

        /// <summary>
        ///     Mass specific entropy
        /// </summary>
        public SpecificEntropy Entropy => _entropy ??= SpecificEntropy
            .FromJoulesPerKilogramKelvin(KeyedOutput(Parameters.iSmass))
            .ToUnit(SpecificEntropyUnit.KilojoulePerKilogramKelvin);

        /// <summary>
        ///     Temperature at freezing point (for incompressible fluids)
        /// </summary>
        public Temperature? FreezingTemperature => _freezingTemperature ??=
            KeyedOutputIsNotNull(Parameters.iT_freeze, out var output)
                ? Temperature.FromKelvins(output!.Value).ToUnit(TemperatureUnit.DegreeCelsius)
                : null;

        /// <summary>
        ///     Mass specific internal energy
        /// </summary>
        public SpecificEnergy InternalEnergy => _internalEnergy ??= SpecificEnergy
            .FromJoulesPerKilogram(KeyedOutput(Parameters.iUmass))
            .ToUnit(SpecificEnergyUnit.KilojoulePerKilogram);

        /// <summary>
        ///     Maximum pressure limit
        /// </summary>
        public Pressure? MaxPressure => _maxPressure ??=
            KeyedOutputIsNotNull(Parameters.iP_max, out var output)
                ? Pressure.FromPascals(output!.Value).ToUnit(PressureUnit.Kilopascal)
                : null;

        /// <summary>
        ///     Maximum temperature limit
        /// </summary>
        public Temperature MaxTemperature => _maxTemperature ??= Temperature
            .FromKelvins(KeyedOutput(Parameters.iT_max))
            .ToUnit(TemperatureUnit.DegreeCelsius);

        /// <summary>
        ///     Minimum pressure limit
        /// </summary>
        public Pressure? MinPressure => _minPressure ??=
            KeyedOutputIsNotNull(Parameters.iP_min, out var output)
                ? Pressure.FromPascals(output!.Value).ToUnit(PressureUnit.Kilopascal)
                : null;

        /// <summary>
        ///     Minimum temperature limit
        /// </summary>
        public Temperature MinTemperature => _minTemperature ??= Temperature
            .FromKelvins(KeyedOutput(Parameters.iT_min))
            .ToUnit(TemperatureUnit.DegreeCelsius);

        /// <summary>
        ///     Molar mass
        /// </summary>
        public MolarMass? MolarMass => _molarMass ??=
            KeyedOutputIsNotNull(Parameters.imolar_mass, out var output)
                ? UnitsNet.MolarMass.FromKilogramsPerMole(output!.Value).ToUnit(MolarMassUnit.GramPerMole)
                : null;

        /// <summary>
        ///     Phase
        /// </summary>
        public Phases Phase => _phase ??= (Phases) KeyedOutput(Parameters.iPhase);

        /// <summary>
        ///     Prandtl number
        /// </summary>
        public double? Prandtl => _prandtl ??= NullableKeyedOutput(Parameters.iPrandtl);

        /// <summary>
        ///     Absolute pressure
        /// </summary>
        public Pressure Pressure => _pressure ??= Pressure
            .FromPascals(KeyedOutput(Parameters.iP))
            .ToUnit(PressureUnit.Kilopascal);

        /// <summary>
        ///     Mass vapor quality
        /// </summary>
        public Ratio? Quality => _quality ??=
            KeyedOutputIsNotNull(Parameters.iQ, out var output)
                ? Ratio.FromDecimalFractions(output!.Value).ToUnit(RatioUnit.Percent)
                : null;

        /// <summary>
        ///     Sound speed
        /// </summary>
        public Speed? SoundSpeed => _soundSpeed ??=
            KeyedOutputIsNotNull(Parameters.ispeed_sound, out var output)
                ? Speed.FromMetersPerSecond(output!.Value)
                : null;

        /// <summary>
        ///     Mass specific constant pressure specific heat
        /// </summary>
        public SpecificEntropy SpecificHeat => _specificHeat ??= SpecificEntropy
            .FromJoulesPerKilogramKelvin(KeyedOutput(Parameters.iCpmass))
            .ToUnit(SpecificEntropyUnit.KilojoulePerKilogramKelvin);

        /// <summary>
        ///     Surface tension
        /// </summary>
        public ForcePerLength? SurfaceTension => _surfaceTension ??=
            KeyedOutputIsNotNull(Parameters.isurface_tension, out var output)
                ? ForcePerLength.FromNewtonsPerMeter(output!.Value)
                : null;

        /// <summary>
        ///     Temperature
        /// </summary>
        public Temperature Temperature => _temperature ??= Temperature
            .FromKelvins(KeyedOutput(Parameters.iT))
            .ToUnit(TemperatureUnit.DegreeCelsius);

        /// <summary>
        ///     Absolute pressure at the triple point
        /// </summary>
        public Pressure? TriplePressure => _triplePressure ??=
            KeyedOutputIsNotNull(Parameters.iP_triple, out var output)
                ? Pressure.FromPascals(output!.Value).ToUnit(PressureUnit.Kilopascal)
                : null;

        /// <summary>
        ///     Temperature at the triple point
        /// </summary>
        public Temperature? TripleTemperature => _tripleTemperature ??=
            KeyedOutputIsNotNull(Parameters.iT_triple, out var output)
                ? Temperature.FromKelvins(output!.Value).ToUnit(TemperatureUnit.DegreeCelsius)
                : null;
    }
}