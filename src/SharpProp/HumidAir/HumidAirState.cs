namespace SharpProp;

/// <summary>
/// Humid air state.
/// </summary>
public interface IHumidAirState
{
    /// <summary>
    /// Compressibility factor (dimensionless).
    /// </summary>
    double Compressibility { get; }

    /// <summary>
    /// Thermal conductivity (by default, W/m/K).
    /// </summary>
    ThermalConductivity Conductivity { get; }

    /// <summary>
    /// Mass density per humid air unit (by default, kg/m3).
    /// </summary>
    Density Density { get; }

    /// <summary>
    /// Dew-point temperature (by default, °C).
    /// </summary>
    Temperature DewTemperature { get; }

    /// <summary>
    /// Dynamic viscosity (by default, mPa*s).
    /// </summary>
    DynamicViscosity DynamicViscosity { get; }

    /// <summary>
    /// Mass specific enthalpy per humid air (by default, kJ/kg).
    /// </summary>
    SpecificEnergy Enthalpy { get; }

    /// <summary>
    /// Mass specific entropy per humid air (by default, kJ/kg/K).
    /// </summary>
    SpecificEntropy Entropy { get; }

    /// <summary>
    /// Absolute humidity ratio (by default, g/kg d.a.).
    /// </summary>
    Ratio Humidity { get; }

    /// <summary>
    /// Kinematic viscosity (by default, cSt).
    /// </summary>
    KinematicViscosity KinematicViscosity { get; }

    /// <summary>
    /// Partial pressure of water vapor (by default, kPa).
    /// </summary>
    Pressure PartialPressure { get; }

    /// <summary>
    /// Prandtl number (dimensionless).
    /// </summary>
    double Prandtl { get; }

    /// <summary>
    /// Absolute pressure (by default, kPa).
    /// </summary>
    Pressure Pressure { get; }

    /// <summary>
    /// Relative humidity ratio (by default, %).
    /// </summary>
    RelativeHumidity RelativeHumidity { get; }

    /// <summary>
    /// Mass specific constant pressure specific heat per humid air (by default, kJ/kg/K).
    /// </summary>
    SpecificEntropy SpecificHeat { get; }

    /// <summary>
    /// Mass specific volume per humid air unit (by default, m3/kg).
    /// </summary>
    SpecificVolume SpecificVolume { get; }

    /// <summary>
    /// Dry-bulb temperature (by default, °C).
    /// </summary>
    Temperature Temperature { get; }

    /// <summary>
    /// Wet-bulb temperature (by default, °C).
    /// </summary>
    Temperature WetBulbTemperature { get; }
}

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
        _conductivity ??= ThermalConductivity.FromWattsPerMeterKelvin(KeyedOutput("K"));

    public Density Density =>
        Density.FromKilogramsPerCubicMeter(1.0 / SpecificVolume.CubicMetersPerKilogram);

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
        _pressure ??= Pressure.FromPascals(KeyedOutput("P")).ToUnit(PressureUnit.Kilopascal);

    public RelativeHumidity RelativeHumidity =>
        _relativeHumidity ??= RelativeHumidity.FromPercent(
            Ratio.FromDecimalFractions(KeyedOutput("R")).Percent
        );

    public SpecificEntropy SpecificHeat =>
        _specificHeat ??= SpecificEntropy
            .FromJoulesPerKilogramKelvin(KeyedOutput("Cha"))
            .ToUnit(SpecificEntropyUnit.KilojoulePerKilogramKelvin);

    public SpecificVolume SpecificVolume =>
        _specificVolume ??= SpecificVolume.FromCubicMetersPerKilogram(KeyedOutput("Vha"));

    public Temperature Temperature =>
        _temperature ??= Temperature
            .FromKelvins(KeyedOutput("T"))
            .ToUnit(TemperatureUnit.DegreeCelsius);

    public Temperature WetBulbTemperature =>
        _wetBulbTemperature ??= Temperature
            .FromKelvins(KeyedOutput("B"))
            .ToUnit(TemperatureUnit.DegreeCelsius);
}
