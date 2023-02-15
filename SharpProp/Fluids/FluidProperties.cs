namespace SharpProp;

public abstract partial class AbstractFluid
{
    protected const double ComparisonTolerance = 1e-6;
    protected const ComparisonType ComparisonType = UnitsNet.ComparisonType.Relative;
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
    ///     CoolProp backend.
    /// </summary>
    protected AbstractState Backend { get; init; } = null!;

    /// <summary>
    ///     Compressibility factor (dimensionless).
    /// </summary>
    public double? Compressibility =>
        _compressibility ??= NullableKeyedOutput(Parameters.iZ);

    /// <summary>
    ///     Thermal conductivity (by default, W/m/K).
    /// </summary>
    public ThermalConductivity? Conductivity => _conductivity ??=
        KeyedOutputIsNotNull(Parameters.iconductivity, out var output)
            ? ThermalConductivity.FromWattsPerMeterKelvin(output!.Value)
            : null;

    /// <summary>
    ///     Absolute pressure at the critical point (by default, kPa).
    /// </summary>
    public Pressure? CriticalPressure => _criticalPressure ??=
        KeyedOutputIsNotNull(Parameters.iP_critical, out var output)
            ? Pressure.FromPascals(output!.Value).ToUnit(PressureUnit.Kilopascal)
            : null;

    /// <summary>
    ///     Temperature at the critical point (by default, °C).
    /// </summary>
    public Temperature? CriticalTemperature => _criticalTemperature ??=
        KeyedOutputIsNotNull(Parameters.iT_critical, out var output)
            ? Temperature.FromKelvins(output!.Value).ToUnit(TemperatureUnit.DegreeCelsius)
            : null;

    /// <summary>
    ///     Mass density (by default, kg/m3).
    /// </summary>
    public Density Density => _density ??=
        Density.FromKilogramsPerCubicMeter(KeyedOutput(Parameters.iDmass));

    /// <summary>
    ///     Dynamic viscosity (by default, mPa*s).
    /// </summary>
    public DynamicViscosity? DynamicViscosity => _dynamicViscosity ??=
        KeyedOutputIsNotNull(Parameters.iviscosity, out var output)
            ? UnitsNet.DynamicViscosity.FromPascalSeconds(output!.Value)
                .ToUnit(DynamicViscosityUnit.MillipascalSecond)
            : null;

    /// <summary>
    ///     Mass specific enthalpy (by default, kJ/kg).
    /// </summary>
    public SpecificEnergy Enthalpy => _enthalpy ??=
        SpecificEnergy.FromJoulesPerKilogram(KeyedOutput(Parameters.iHmass))
            .ToUnit(SpecificEnergyUnit.KilojoulePerKilogram);

    /// <summary>
    ///     Mass specific entropy (by default, kJ/kg/K).
    /// </summary>
    public SpecificEntropy Entropy => _entropy ??=
        SpecificEntropy.FromJoulesPerKilogramKelvin(KeyedOutput(Parameters.iSmass))
            .ToUnit(SpecificEntropyUnit.KilojoulePerKilogramKelvin);

    /// <summary>
    ///     Temperature at the freezing point (for incompressible fluids) (by default, °C).
    /// </summary>
    public Temperature? FreezingTemperature => _freezingTemperature ??=
        KeyedOutputIsNotNull(Parameters.iT_freeze, out var output)
            ? Temperature.FromKelvins(output!.Value).ToUnit(TemperatureUnit.DegreeCelsius)
            : null;

    internal List<IKeyedInput<Parameters>> Inputs { get; private set; } = new(2);

    /// <summary>
    ///     Mass specific internal energy (by default, kJ/kg).
    /// </summary>
    public SpecificEnergy InternalEnergy => _internalEnergy ??=
        SpecificEnergy.FromJoulesPerKilogram(KeyedOutput(Parameters.iUmass))
            .ToUnit(SpecificEnergyUnit.KilojoulePerKilogram);

    /// <summary>
    ///     Kinematic viscosity (by default, cSt).
    /// </summary>
    public KinematicViscosity? KinematicViscosity =>
        (DynamicViscosity / Density)?.ToUnit(KinematicViscosityUnit.Centistokes);

    /// <summary>
    ///     Maximum pressure limit (by default, kPa).
    /// </summary>
    public Pressure? MaxPressure => _maxPressure ??=
        KeyedOutputIsNotNull(Parameters.iP_max, out var output)
            ? Pressure.FromPascals(output!.Value).ToUnit(PressureUnit.Kilopascal)
            : null;

    /// <summary>
    ///     Maximum temperature limit (by default, °C).
    /// </summary>
    public Temperature MaxTemperature => _maxTemperature ??=
        Temperature.FromKelvins(KeyedOutput(Parameters.iT_max))
            .ToUnit(TemperatureUnit.DegreeCelsius);

    /// <summary>
    ///     Minimum pressure limit (by default, kPa).
    /// </summary>
    public Pressure? MinPressure => _minPressure ??=
        KeyedOutputIsNotNull(Parameters.iP_min, out var output)
            ? Pressure.FromPascals(output!.Value).ToUnit(PressureUnit.Kilopascal)
            : null;

    /// <summary>
    ///     Minimum temperature limit (by default, °C).
    /// </summary>
    public Temperature MinTemperature => _minTemperature ??=
        Temperature.FromKelvins(KeyedOutput(Parameters.iT_min))
            .ToUnit(TemperatureUnit.DegreeCelsius);

    /// <summary>
    ///     Molar mass (by default, g/mol).
    /// </summary>
    public MolarMass? MolarMass => _molarMass ??=
        KeyedOutputIsNotNull(Parameters.imolar_mass, out var output)
            ? UnitsNet.MolarMass.FromKilogramsPerMole(output!.Value).ToUnit(MolarMassUnit.GramPerMole)
            : null;

    /// <summary>
    ///     Phase state.
    /// </summary>
    public Phases Phase =>
        _phase ??= (Phases) KeyedOutput(Parameters.iPhase);

    /// <summary>
    ///     Prandtl number (dimensionless).
    /// </summary>
    public double? Prandtl =>
        _prandtl ??= NullableKeyedOutput(Parameters.iPrandtl);

    /// <summary>
    ///     Absolute pressure (by default, kPa).
    /// </summary>
    public Pressure Pressure => _pressure ??=
        Pressure.FromPascals(KeyedOutput(Parameters.iP))
            .ToUnit(PressureUnit.Kilopascal);

    /// <summary>
    ///     Mass vapor quality (by default, %).
    /// </summary>
    public Ratio? Quality => _quality ??=
        KeyedOutputIsNotNull(Parameters.iQ, out var output)
            ? Ratio.FromDecimalFractions(output!.Value).ToUnit(RatioUnit.Percent)
            : null;

    /// <summary>
    ///     Sound speed (by default, m/s).
    /// </summary>
    public Speed? SoundSpeed => _soundSpeed ??=
        KeyedOutputIsNotNull(Parameters.ispeed_sound, out var output)
            ? Speed.FromMetersPerSecond(output!.Value)
            : null;

    /// <summary>
    ///     Mass specific constant pressure specific heat (by default, kJ/kg/K).
    /// </summary>
    public SpecificEntropy SpecificHeat => _specificHeat ??=
        SpecificEntropy.FromJoulesPerKilogramKelvin(KeyedOutput(Parameters.iCpmass))
            .ToUnit(SpecificEntropyUnit.KilojoulePerKilogramKelvin);

    /// <summary>
    ///     Surface tension (by default, N/m).
    /// </summary>
    public ForcePerLength? SurfaceTension => _surfaceTension ??=
        KeyedOutputIsNotNull(Parameters.isurface_tension, out var output)
            ? ForcePerLength.FromNewtonsPerMeter(output!.Value)
            : null;

    /// <summary>
    ///     Temperature (by default, °C).
    /// </summary>
    public Temperature Temperature => _temperature ??=
        Temperature.FromKelvins(KeyedOutput(Parameters.iT))
            .ToUnit(TemperatureUnit.DegreeCelsius);

    /// <summary>
    ///     Absolute pressure at the triple point (by default, kPa).
    /// </summary>
    public Pressure? TriplePressure => _triplePressure ??=
        KeyedOutputIsNotNull(Parameters.iP_triple, out var output)
            ? Pressure.FromPascals(output!.Value).ToUnit(PressureUnit.Kilopascal)
            : null;

    /// <summary>
    ///     Temperature at the triple point (by default, °C).
    /// </summary>
    public Temperature? TripleTemperature => _tripleTemperature ??=
        KeyedOutputIsNotNull(Parameters.iT_triple, out var output)
            ? Temperature.FromKelvins(output!.Value).ToUnit(TemperatureUnit.DegreeCelsius)
            : null;
}