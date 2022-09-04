using NUnit.Framework;
using UnitsNet;
using UnitsNet.NumberExtensions.NumberToRatio;

namespace SharpProp.Tests
{
    /// <summary>
    ///     An example of how to extend <see cref="InputHumidAir" />.
    /// </summary>
    public record InputHumidAirExtended : InputHumidAir
    {
        private InputHumidAirExtended(string coolPropKey, double value) :
            base(coolPropKey, value)
        {
        }

        /// <summary>
        ///     Water mole fraction.
        /// </summary>
        /// <param name="value">The value of the input.</param>
        /// <returns>Water mole fraction for the input.</returns>
        public static InputHumidAirExtended WaterMoleFraction(Ratio value) =>
            new("psi_w", value.DecimalFractions);
    }

    public static class TestInputHumidAirExtended
    {
        private static readonly InputHumidAirExtended Input =
            InputHumidAirExtended.WaterMoleFraction(5.PartsPerThousand());

        [Test(ExpectedResult = "psi_w")]
        public static string TestCoolPropKey() => Input.CoolPropKey;

        [Test(ExpectedResult = 5e-3)]
        public static double TestValue() => Input.Value;
    }
}