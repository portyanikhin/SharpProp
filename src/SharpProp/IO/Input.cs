﻿namespace SharpProp;

/// <summary>
///     CoolProp keyed input for fluids and mixtures.
/// </summary>
/// <param name="CoolPropKey">CoolProp internal key.</param>
/// <param name="Value">Input value in SI units.</param>
public record Input(Parameters CoolPropKey, double Value)
    : KeyedInput<Parameters>(CoolPropKey, Value)
{
    public override string CoolPropHighLevelKey => CoolPropKey.ToString().TrimStart('i');

    /// <summary>
    ///     Mass density.
    /// </summary>
    /// <param name="value">The value of the input.</param>
    /// <returns>Mass density for the input.</returns>
    public static Input Density(Density value) =>
        new(Parameters.iDmass, value.KilogramsPerCubicMeter);

    /// <summary>
    ///     Mass specific enthalpy.
    /// </summary>
    /// <param name="value">The value of the input.</param>
    /// <returns>Mass specific enthalpy for the input.</returns>
    public static Input Enthalpy(SpecificEnergy value) =>
        new(Parameters.iHmass, value.JoulesPerKilogram);

    /// <summary>
    ///     Mass specific entropy.
    /// </summary>
    /// <param name="value">The value of the input.</param>
    /// <returns>Mass specific entropy for the input.</returns>
    public static Input Entropy(SpecificEntropy value) =>
        new(Parameters.iSmass, value.JoulesPerKilogramKelvin);

    /// <summary>
    ///     Mass specific internal energy.
    /// </summary>
    /// <param name="value">The value of the input.</param>
    /// <returns>Mass specific internal energy for the input.</returns>
    public static Input InternalEnergy(SpecificEnergy value) =>
        new(Parameters.iUmass, value.JoulesPerKilogram);

    /// <summary>
    ///     Absolute pressure.
    /// </summary>
    /// <param name="value">The value of the input.</param>
    /// <returns>Absolute pressure for the input.</returns>
    public static Input Pressure(Pressure value) => new(Parameters.iP, value.Pascals);

    /// <summary>
    ///     Mass vapor quality.
    /// </summary>
    /// <param name="value">The value of the input.</param>
    /// <returns>Mass vapor quality for the input.</returns>
    public static Input Quality(Ratio value) => new(Parameters.iQ, value.DecimalFractions);

    /// <summary>
    ///     Mass specific volume.
    /// </summary>
    /// <param name="value">The value of the input.</param>
    /// <returns>Mass specific volume for the input.</returns>
    public static Input SpecificVolume(SpecificVolume value) =>
        new(Parameters.iDmass, 1.0 / value.CubicMetersPerKilogram);

    /// <summary>
    ///     Temperature.
    /// </summary>
    /// <param name="value">The value of the input.</param>
    /// <returns>Temperature for the input.</returns>
    public static Input Temperature(Temperature value) => new(Parameters.iT, value.Kelvins);
}
