namespace SharpProp;

/// <summary>
///     Humid air state.
/// </summary>
public interface IHumidAirState
{
    /// <summary>
    ///     Compressibility factor (dimensionless).
    /// </summary>
    public double Compressibility { get; }

    /// <summary>
    ///     Thermal conductivity (by default, W/m/K).
    /// </summary>
    public ThermalConductivity Conductivity { get; }

    /// <summary>
    ///     Mass density per humid air unit (by default, kg/m3).
    /// </summary>
    public Density Density { get; }

    /// <summary>
    ///     Dew-point temperature (by default, °C).
    /// </summary>
    public Temperature DewTemperature { get; }

    /// <summary>
    ///     Dynamic viscosity (by default, mPa*s).
    /// </summary>
    public DynamicViscosity DynamicViscosity { get; }

    /// <summary>
    ///     Mass specific enthalpy per humid air (by default, kJ/kg).
    /// </summary>
    public SpecificEnergy Enthalpy { get; }

    /// <summary>
    ///     Mass specific entropy per humid air (by default, kJ/kg/K).
    /// </summary>
    public SpecificEntropy Entropy { get; }

    /// <summary>
    ///     Absolute humidity ratio (by default, g/kg d.a.).
    /// </summary>
    public Ratio Humidity { get; }

    /// <summary>
    ///     Kinematic viscosity (by default, cSt).
    /// </summary>
    public KinematicViscosity KinematicViscosity { get; }

    /// <summary>
    ///     Partial pressure of water vapor (by default, kPa).
    /// </summary>
    public Pressure PartialPressure { get; }

    /// <summary>
    ///     Prandtl number (dimensionless).
    /// </summary>
    public double Prandtl { get; }

    /// <summary>
    ///     Absolute pressure (by default, kPa).
    /// </summary>
    public Pressure Pressure { get; }

    /// <summary>
    ///     Relative humidity ratio (by default, %).
    /// </summary>
    public RelativeHumidity RelativeHumidity { get; }

    /// <summary>
    ///     Mass specific constant pressure
    ///     specific heat per humid air (by default, kJ/kg/K).
    /// </summary>
    public SpecificEntropy SpecificHeat { get; }

    /// <summary>
    ///     Dry-bulb temperature (by default, °C).
    /// </summary>
    public Temperature Temperature { get; }

    /// <summary>
    ///     Wet-bulb temperature (by default, °C).
    /// </summary>
    public Temperature WetBulbTemperature { get; }
}
