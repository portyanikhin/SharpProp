using NUnit.Framework;

namespace SharpProp.Tests
{
    public class TestInputHumidAirExtended
    {
        private InputHumidAirExtended _input = null!;

        [SetUp]
        public void SetUp() => _input = InputHumidAirExtended.WaterMoleFraction(5e-3);

        [Test(ExpectedResult = "psi_w")]
        public string TestCoolPropKey() => _input.CoolPropKey;

        [Test(ExpectedResult = 5e-3)]
        public double TestValue() => _input.Value;

        /// <summary>
        ///     An example of how to extend <see cref="InputHumidAir" />
        /// </summary>
        public record InputHumidAirExtended : InputHumidAir
        {
            private InputHumidAirExtended(string coolPropKey, double value) : base(coolPropKey, value)
            {
            }

            /// <summary>
            ///     Water mole fraction
            /// </summary>
            /// <param name="value">The value [mol/mol h.a.]</param>
            /// <returns>Molar density for the input [mol/mol h.a.]</returns>
            public static InputHumidAirExtended WaterMoleFraction(double value) =>
                new("psi_w", value);
        }
    }
}