namespace SharpProp;

/// <summary>
///     Fluid state.
/// </summary>
public interface IFluidState
{
    /// <summary>
    ///     Compressibility factor (dimensionless).
    /// </summary>
    public double? Compressibility { get; }

    /// <summary>
    ///     Thermal conductivity (by default, W/m/K).
    /// </summary>
    public ThermalConductivity? Conductivity { get; }

    /// <summary>
    ///     Absolute pressure at the critical point (by default, kPa).
    /// </summary>
    public Pressure? CriticalPressure { get; }

    /// <summary>
    ///     Temperature at the critical point (by default, °C).
    /// </summary>
    public Temperature? CriticalTemperature { get; }

    /// <summary>
    ///     Mass density (by default, kg/m3).
    /// </summary>
    public Density Density { get; }

    /// <summary>
    ///     Dynamic viscosity (by default, mPa*s).
    /// </summary>
    public DynamicViscosity? DynamicViscosity { get; }

    /// <summary>
    ///     Mass specific enthalpy (by default, kJ/kg).
    /// </summary>
    public SpecificEnergy Enthalpy { get; }

    /// <summary>
    ///     Mass specific entropy (by default, kJ/kg/K).
    /// </summary>
    public SpecificEntropy Entropy { get; }

    /// <summary>
    ///     Temperature at the freezing point
    ///     (for incompressible fluids) (by default, °C).
    /// </summary>
    public Temperature? FreezingTemperature { get; }

    /// <summary>
    ///     Mass specific internal energy (by default, kJ/kg).
    /// </summary>
    public SpecificEnergy InternalEnergy { get; }

    /// <summary>
    ///     Kinematic viscosity (by default, cSt).
    /// </summary>
    public KinematicViscosity? KinematicViscosity { get; }

    /// <summary>
    ///     Maximum pressure limit (by default, kPa).
    /// </summary>
    public Pressure? MaxPressure { get; }

    /// <summary>
    ///     Maximum temperature limit (by default, °C).
    /// </summary>
    public Temperature MaxTemperature { get; }

    /// <summary>
    ///     Minimum pressure limit (by default, kPa).
    /// </summary>
    public Pressure? MinPressure { get; }

    /// <summary>
    ///     Minimum temperature limit (by default, °C).
    /// </summary>
    public Temperature MinTemperature { get; }

    /// <summary>
    ///     Molar mass (by default, g/mol).
    /// </summary>
    public MolarMass? MolarMass { get; }

    /// <summary>
    ///     Phase state.
    /// </summary>
    public Phases Phase { get; }

    /// <summary>
    ///     Prandtl number (dimensionless).
    /// </summary>
    public double? Prandtl { get; }

    /// <summary>
    ///     Absolute pressure (by default, kPa).
    /// </summary>
    public Pressure Pressure { get; }

    /// <summary>
    ///     Mass vapor quality (by default, %).
    /// </summary>
    public Ratio? Quality { get; }

    /// <summary>
    ///     Sound speed (by default, m/s).
    /// </summary>
    public Speed? SoundSpeed { get; }

    /// <summary>
    ///     Mass specific constant pressure specific heat (by default, kJ/kg/K).
    /// </summary>
    public SpecificEntropy SpecificHeat { get; }

    /// <summary>
    ///     Mass specific volume (by default, m3/kg).
    /// </summary>
    public SpecificVolume SpecificVolume { get; }

    /// <summary>
    ///     Surface tension (by default, N/m).
    /// </summary>
    public ForcePerLength? SurfaceTension { get; }

    /// <summary>
    ///     Temperature (by default, °C).
    /// </summary>
    public Temperature Temperature { get; }

    /// <summary>
    ///     Absolute pressure at the triple point (by default, kPa).
    /// </summary>
    public Pressure? TriplePressure { get; }

    /// <summary>
    ///     Temperature at the triple point (by default, °C).
    /// </summary>
    public Temperature? TripleTemperature { get; }
}
