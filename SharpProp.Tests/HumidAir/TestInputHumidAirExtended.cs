using NUnit.Framework;
using UnitsNet;
using UnitsNet.NumberExtensions.NumberToRatio;

namespace SharpProp.Tests
{
    public class TestInputHumidAirExtended
    {
        private InputHumidAirExtended _input = null!;

        [SetUp]
        public void SetUp() =>
            _input = InputHumidAirExtended.WaterMoleFraction(5.PartsPerThousand());

        [Test(ExpectedResult = "psi_w")]
        public string TestCoolPropKey() => _input.CoolPropKey;

        [Test(ExpectedResult = 5e-3)]
        public double TestValue() => _input.Value;

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
            /// <returns>Molar density for the input.</returns>
            public static InputHumidAirExtended WaterMoleFraction(Ratio value) =>
                new("psi_w", value.DecimalFractions);
        }
    }
}