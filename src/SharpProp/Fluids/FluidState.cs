namespace SharpProp;

public abstract partial class AbstractFluid
{
    protected const double Tolerance = 1e-6;
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

    public double? Compressibility =>
        _compressibility ??= NullableKeyedOutput(Parameters.iZ);

    public ThermalConductivity? Conductivity =>
        _conductivity ??= KeyedOutputIsNotNull(
            Parameters.iconductivity,
            out var output
        )
            ? ThermalConductivity.FromWattsPerMeterKelvin(output!.Value)
            : null;

    public Pressure? CriticalPressure =>
        _criticalPressure ??= KeyedOutputIsNotNull(
            Parameters.iP_critical,
            out var output
        )
            ? Pressure
                .FromPascals(output!.Value)
                .ToUnit(PressureUnit.Kilopascal)
            : null;

    public Temperature? CriticalTemperature =>
        _criticalTemperature ??= KeyedOutputIsNotNull(
            Parameters.iT_critical,
            out var output
        )
            ? Temperature
                .FromKelvins(output!.Value)
                .ToUnit(TemperatureUnit.DegreeCelsius)
            : null;

    public Density Density =>
        _density ??= Density.FromKilogramsPerCubicMeter(
            KeyedOutput(Parameters.iDmass)
        );

    public DynamicViscosity? DynamicViscosity =>
        _dynamicViscosity ??= KeyedOutputIsNotNull(
            Parameters.iviscosity,
            out var output
        )
            ? UnitsNet.DynamicViscosity
                .FromPascalSeconds(output!.Value)
                .ToUnit(DynamicViscosityUnit.MillipascalSecond)
            : null;

    public SpecificEnergy Enthalpy =>
        _enthalpy ??= SpecificEnergy
            .FromJoulesPerKilogram(KeyedOutput(Parameters.iHmass))
            .ToUnit(SpecificEnergyUnit.KilojoulePerKilogram);

    public SpecificEntropy Entropy =>
        _entropy ??= SpecificEntropy
            .FromJoulesPerKilogramKelvin(KeyedOutput(Parameters.iSmass))
            .ToUnit(SpecificEntropyUnit.KilojoulePerKilogramKelvin);

    public Temperature? FreezingTemperature =>
        _freezingTemperature ??= KeyedOutputIsNotNull(
            Parameters.iT_freeze,
            out var output
        )
            ? Temperature
                .FromKelvins(output!.Value)
                .ToUnit(TemperatureUnit.DegreeCelsius)
            : null;

    public SpecificEnergy InternalEnergy =>
        _internalEnergy ??= SpecificEnergy
            .FromJoulesPerKilogram(KeyedOutput(Parameters.iUmass))
            .ToUnit(SpecificEnergyUnit.KilojoulePerKilogram);

    public KinematicViscosity? KinematicViscosity =>
        (DynamicViscosity / Density)?.ToUnit(
            KinematicViscosityUnit.Centistokes
        );

    public Pressure? MaxPressure =>
        _maxPressure ??= KeyedOutputIsNotNull(Parameters.iP_max, out var output)
            ? Pressure
                .FromPascals(output!.Value)
                .ToUnit(PressureUnit.Kilopascal)
            : null;

    public Temperature MaxTemperature =>
        _maxTemperature ??= Temperature
            .FromKelvins(KeyedOutput(Parameters.iT_max))
            .ToUnit(TemperatureUnit.DegreeCelsius);

    public Pressure? MinPressure =>
        _minPressure ??= KeyedOutputIsNotNull(Parameters.iP_min, out var output)
            ? Pressure
                .FromPascals(output!.Value)
                .ToUnit(PressureUnit.Kilopascal)
            : null;

    public Temperature MinTemperature =>
        _minTemperature ??= Temperature
            .FromKelvins(KeyedOutput(Parameters.iT_min))
            .ToUnit(TemperatureUnit.DegreeCelsius);

    public MolarMass? MolarMass =>
        _molarMass ??= KeyedOutputIsNotNull(
            Parameters.imolar_mass,
            out var output
        )
            ? UnitsNet.MolarMass
                .FromKilogramsPerMole(output!.Value)
                .ToUnit(MolarMassUnit.GramPerMole)
            : null;

    public Phases Phase => _phase ??= (Phases)KeyedOutput(Parameters.iPhase);

    public double? Prandtl =>
        _prandtl ??= NullableKeyedOutput(Parameters.iPrandtl);

    public Pressure Pressure =>
        _pressure ??= Pressure
            .FromPascals(KeyedOutput(Parameters.iP))
            .ToUnit(PressureUnit.Kilopascal);

    public Ratio? Quality =>
        _quality ??= KeyedOutputIsNotNull(Parameters.iQ, out var output)
            ? Ratio
                .FromDecimalFractions(output!.Value)
                .ToUnit(RatioUnit.Percent)
            : null;

    public Speed? SoundSpeed =>
        _soundSpeed ??= KeyedOutputIsNotNull(
            Parameters.ispeed_sound,
            out var output
        )
            ? Speed.FromMetersPerSecond(output!.Value)
            : null;

    public SpecificEntropy SpecificHeat =>
        _specificHeat ??= SpecificEntropy
            .FromJoulesPerKilogramKelvin(KeyedOutput(Parameters.iCpmass))
            .ToUnit(SpecificEntropyUnit.KilojoulePerKilogramKelvin);

    public SpecificVolume SpecificVolume =>
        SpecificVolume.FromCubicMetersPerKilogram(
            1.0 / Density.KilogramsPerCubicMeter
        );

    public ForcePerLength? SurfaceTension =>
        _surfaceTension ??= KeyedOutputIsNotNull(
            Parameters.isurface_tension,
            out var output
        )
            ? ForcePerLength.FromNewtonsPerMeter(output!.Value)
            : null;

    public Temperature Temperature =>
        _temperature ??= Temperature
            .FromKelvins(KeyedOutput(Parameters.iT))
            .ToUnit(TemperatureUnit.DegreeCelsius);

    public Pressure? TriplePressure =>
        _triplePressure ??= KeyedOutputIsNotNull(
            Parameters.iP_triple,
            out var output
        )
            ? Pressure
                .FromPascals(output!.Value)
                .ToUnit(PressureUnit.Kilopascal)
            : null;

    public Temperature? TripleTemperature =>
        _tripleTemperature ??= KeyedOutputIsNotNull(
            Parameters.iT_triple,
            out var output
        )
            ? Temperature
                .FromKelvins(output!.Value)
                .ToUnit(TemperatureUnit.DegreeCelsius)
            : null;
}
