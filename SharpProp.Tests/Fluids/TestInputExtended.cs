using CoolProp;
using NUnit.Framework;
using UnitsNet;
using UnitsNet.NumberExtensions.NumberToMolarMass;

namespace SharpProp.Tests;

/// <summary>
///     An example of how to extend <see cref="Input" />.
/// </summary>
public record InputExtended : Input
{
    private InputExtended(Parameters coolPropKey, double value) :
        base(coolPropKey, value)
    {
    }

    /// <summary>
    ///     Molar density.
    /// </summary>
    /// <param name="value">The value of the input.</param>
    /// <returns>Molar density for the input.</returns>
    public static InputExtended MolarDensity(MolarMass value) =>
        new(Parameters.iDmolar, value.KilogramsPerMole);
}

public static class TestInputExtended
{
    private static readonly InputExtended Input =
        InputExtended.MolarDensity(900.KilogramsPerMole());

    [Test(ExpectedResult = Parameters.iDmolar)]
    public static Parameters TestCoolPropKey() => Input.CoolPropKey;

    [Test(ExpectedResult = 900)]
    public static double TestValue() => Input.Value;
}