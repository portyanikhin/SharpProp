namespace SharpProp;

public partial class HumidAir
{
    private const double Tolerance = 1e-6;
    private double? _compressibility;
    private ThermalConductivity? _conductivity;
    private Temperature? _dewTemperature;
    private DynamicViscosity? _dynamicViscosity;
    private SpecificEnergy? _enthalpy;
    private SpecificEntropy? _entropy;
    private Ratio? _humidity;
    private Pressure? _partialPressure;
    private Pressure? _pressure;
    private RelativeHumidity? _relativeHumidity;
    private SpecificEntropy? _specificHeat;
    private SpecificVolume? _specificVolume;
    private Temperature? _temperature;
    private Temperature? _wetBulbTemperature;

    public double Compressibility => _compressibility ??= KeyedOutput("Z");

    public ThermalConductivity Conductivity =>
        _conductivity ??= ThermalConductivity.FromWattsPerMeterKelvin(
            KeyedOutput("K")
        );

    public Density Density =>
        Density.FromKilogramsPerCubicMeter(
            1.0 / SpecificVolume.CubicMetersPerKilogram
        );

    public Temperature DewTemperature =>
        _dewTemperature ??= Temperature
            .FromKelvins(KeyedOutput("D"))
            .ToUnit(TemperatureUnit.DegreeCelsius);

    public DynamicViscosity DynamicViscosity =>
        _dynamicViscosity ??= DynamicViscosity
            .FromPascalSeconds(KeyedOutput("M"))
            .ToUnit(DynamicViscosityUnit.MillipascalSecond);

    public SpecificEnergy Enthalpy =>
        _enthalpy ??= SpecificEnergy
            .FromJoulesPerKilogram(KeyedOutput("Hha"))
            .ToUnit(SpecificEnergyUnit.KilojoulePerKilogram);

    public SpecificEntropy Entropy =>
        _entropy ??= SpecificEntropy
            .FromJoulesPerKilogramKelvin(KeyedOutput("Sha"))
            .ToUnit(SpecificEntropyUnit.KilojoulePerKilogramKelvin);

    public Ratio Humidity =>
        _humidity ??= Ratio
            .FromDecimalFractions(KeyedOutput("W"))
            .ToUnit(RatioUnit.PartPerThousand);

    public KinematicViscosity KinematicViscosity =>
        (DynamicViscosity / Density).ToUnit(KinematicViscosityUnit.Centistokes);

    public Pressure PartialPressure =>
        _partialPressure ??= Pressure
            .FromPascals(KeyedOutput("P_w"))
            .ToUnit(PressureUnit.Kilopascal);

    public double Prandtl =>
        DynamicViscosity.PascalSeconds
        * SpecificHeat.JoulesPerKilogramKelvin
        / Conductivity.WattsPerMeterKelvin;

    public Pressure Pressure =>
        _pressure ??= Pressure
            .FromPascals(KeyedOutput("P"))
            .ToUnit(PressureUnit.Kilopascal);

    public RelativeHumidity RelativeHumidity =>
        _relativeHumidity ??= RelativeHumidity.FromPercent(
            Ratio.FromDecimalFractions(KeyedOutput("R")).Percent
        );

    public SpecificEntropy SpecificHeat =>
        _specificHeat ??= SpecificEntropy
            .FromJoulesPerKilogramKelvin(KeyedOutput("Cha"))
            .ToUnit(SpecificEntropyUnit.KilojoulePerKilogramKelvin);

    public SpecificVolume SpecificVolume =>
        _specificVolume ??= SpecificVolume.FromCubicMetersPerKilogram(
            KeyedOutput("Vha")
        );

    public Temperature Temperature =>
        _temperature ??= Temperature
            .FromKelvins(KeyedOutput("T"))
            .ToUnit(TemperatureUnit.DegreeCelsius);

    public Temperature WetBulbTemperature =>
        _wetBulbTemperature ??= Temperature
            .FromKelvins(KeyedOutput("B"))
            .ToUnit(TemperatureUnit.DegreeCelsius);
}
