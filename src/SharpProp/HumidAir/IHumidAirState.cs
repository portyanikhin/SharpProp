namespace SharpProp;

/// <summary>
///     Humid air state.
/// </summary>
public interface IHumidAirState
{
    /// <summary>
    ///     Compressibility factor (dimensionless).
    /// </summary>
    double Compressibility { get; }

    /// <summary>
    ///     Thermal conductivity (by default, W/m/K).
    /// </summary>
    ThermalConductivity Conductivity { get; }

    /// <summary>
    ///     Mass density per humid air unit (by default, kg/m3).
    /// </summary>
    Density Density { get; }

    /// <summary>
    ///     Dew-point temperature (by default, °C).
    /// </summary>
    Temperature DewTemperature { get; }

    /// <summary>
    ///     Dynamic viscosity (by default, mPa*s).
    /// </summary>
    DynamicViscosity DynamicViscosity { get; }

    /// <summary>
    ///     Mass specific enthalpy per humid air (by default, kJ/kg).
    /// </summary>
    SpecificEnergy Enthalpy { get; }

    /// <summary>
    ///     Mass specific entropy per humid air (by default, kJ/kg/K).
    /// </summary>
    SpecificEntropy Entropy { get; }

    /// <summary>
    ///     Absolute humidity ratio (by default, g/kg d.a.).
    /// </summary>
    Ratio Humidity { get; }

    /// <summary>
    ///     Kinematic viscosity (by default, cSt).
    /// </summary>
    KinematicViscosity KinematicViscosity { get; }

    /// <summary>
    ///     Partial pressure of water vapor (by default, kPa).
    /// </summary>
    Pressure PartialPressure { get; }

    /// <summary>
    ///     Prandtl number (dimensionless).
    /// </summary>
    double Prandtl { get; }

    /// <summary>
    ///     Absolute pressure (by default, kPa).
    /// </summary>
    Pressure Pressure { get; }

    /// <summary>
    ///     Relative humidity ratio (by default, %).
    /// </summary>
    RelativeHumidity RelativeHumidity { get; }

    /// <summary>
    ///     Mass specific constant pressure
    ///     specific heat per humid air (by default, kJ/kg/K).
    /// </summary>
    SpecificEntropy SpecificHeat { get; }

    /// <summary>
    ///     Mass specific volume per humid air unit (by default, m3/kg).
    /// </summary>
    SpecificVolume SpecificVolume { get; }

    /// <summary>
    ///     Dry-bulb temperature (by default, °C).
    /// </summary>
    Temperature Temperature { get; }

    /// <summary>
    ///     Wet-bulb temperature (by default, °C).
    /// </summary>
    Temperature WetBulbTemperature { get; }
}
