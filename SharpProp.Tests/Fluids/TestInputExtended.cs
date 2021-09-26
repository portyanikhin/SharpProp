using CoolProp;
using NUnit.Framework;

namespace SharpProp.Tests
{
    public class TestInputExtended
    {
        private InputExtended _input = null!;

        [SetUp]
        public void SetUp() => _input = InputExtended.MolarDensity(900);

        [Test(ExpectedResult = parameters.iDmolar)]
        public parameters TestCoolPropKey() => _input.CoolPropKey;

        [Test(ExpectedResult = 900)]
        public double TestValue() => _input.Value;

        /// <summary>
        ///     An example of how to extend <see cref="Input" />
        /// </summary>
        private record InputExtended : Input
        {
            private InputExtended(parameters coolPropKey, double value) : base(coolPropKey, value)
            {
            }

            /// <summary>
            ///     Molar density
            /// </summary>
            /// <param name="value">The value [kg/mol]</param>
            /// <returns>Molar density for the input [kg/mol]</returns>
            public static InputExtended MolarDensity(double value) => new(parameters.iDmolar, value);
        }
    }
}