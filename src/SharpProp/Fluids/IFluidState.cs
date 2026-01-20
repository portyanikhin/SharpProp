namespace SharpProp;

/// <summary>
/// Fluid state.
/// </summary>
public interface IFluidState
{
    /// <summary>
    /// Compressibility factor (dimensionless).
    /// </summary>
    double? Compressibility { get; }

    /// <summary>
    /// Thermal conductivity (by default, W/m/K).
    /// </summary>
    ThermalConductivity? Conductivity { get; }

    /// <summary>
    /// Absolute pressure at the critical point (by default, kPa).
    /// </summary>
    Pressure? CriticalPressure { get; }

    /// <summary>
    /// Temperature at the critical point (by default, °C).
    /// </summary>
    Temperature? CriticalTemperature { get; }

    /// <summary>
    /// Mass density (by default, kg/m3).
    /// </summary>
    Density Density { get; }

    /// <summary>
    /// Dynamic viscosity (by default, mPa*s).
    /// </summary>
    DynamicViscosity? DynamicViscosity { get; }

    /// <summary>
    /// Mass specific enthalpy (by default, kJ/kg).
    /// </summary>
    SpecificEnergy Enthalpy { get; }

    /// <summary>
    /// Mass specific entropy (by default, kJ/kg/K).
    /// </summary>
    SpecificEntropy Entropy { get; }

    /// <summary>
    /// Temperature at the freezing point
    /// (for incompressible fluids) (by default, °C).
    /// </summary>
    Temperature? FreezingTemperature { get; }

    /// <summary>
    /// Mass specific internal energy (by default, kJ/kg).
    /// </summary>
    SpecificEnergy InternalEnergy { get; }

    /// <summary>
    /// Kinematic viscosity (by default, cSt).
    /// </summary>
    KinematicViscosity? KinematicViscosity { get; }

    /// <summary>
    /// Maximum pressure limit (by default, kPa).
    /// </summary>
    Pressure? MaxPressure { get; }

    /// <summary>
    /// Maximum temperature limit (by default, °C).
    /// </summary>
    Temperature MaxTemperature { get; }

    /// <summary>
    /// Minimum pressure limit (by default, kPa).
    /// </summary>
    Pressure? MinPressure { get; }

    /// <summary>
    /// Minimum temperature limit (by default, °C).
    /// </summary>
    Temperature MinTemperature { get; }

    /// <summary>
    /// Molar mass (by default, g/mol).
    /// </summary>
    MolarMass? MolarMass { get; }

    /// <summary>
    /// Phase state.
    /// </summary>
    Phases Phase { get; }

    /// <summary>
    /// Prandtl number (dimensionless).
    /// </summary>
    double? Prandtl { get; }

    /// <summary>
    /// Absolute pressure (by default, kPa).
    /// </summary>
    Pressure Pressure { get; }

    /// <summary>
    /// Mass vapor quality (by default, %).
    /// </summary>
    Ratio? Quality { get; }

    /// <summary>
    /// Sound speed (by default, m/s).
    /// </summary>
    Speed? SoundSpeed { get; }

    /// <summary>
    /// Mass specific constant pressure specific heat (by default, kJ/kg/K).
    /// </summary>
    SpecificEntropy SpecificHeat { get; }

    /// <summary>
    /// Mass specific volume (by default, m3/kg).
    /// </summary>
    SpecificVolume SpecificVolume { get; }

    /// <summary>
    /// Surface tension (by default, N/m).
    /// </summary>
    ForcePerLength? SurfaceTension { get; }

    /// <summary>
    /// Temperature (by default, °C).
    /// </summary>
    Temperature Temperature { get; }

    /// <summary>
    /// Absolute pressure at the triple point (by default, kPa).
    /// </summary>
    Pressure? TriplePressure { get; }

    /// <summary>
    /// Temperature at the triple point (by default, °C).
    /// </summary>
    Temperature? TripleTemperature { get; }
}
