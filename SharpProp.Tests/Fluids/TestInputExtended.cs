using CoolProp;
using NUnit.Framework;
using UnitsNet;
using UnitsNet.NumberExtensions.NumberToMolarMass;

namespace SharpProp.Tests
{
    public class TestInputExtended
    {
        private InputExtended _input = null!;

        [SetUp]
        public void SetUp() => _input = InputExtended.MolarDensity(900.KilogramsPerMole());

        [Test(ExpectedResult = Parameters.iDmolar)]
        public Parameters TestCoolPropKey() => _input.CoolPropKey;

        [Test(ExpectedResult = 900)]
        public double TestValue() => _input.Value;

        /// <summary>
        ///     An example of how to extend <see cref="Input" />.
        /// </summary>
        private record InputExtended : Input
        {
            private InputExtended(Parameters coolPropKey, double value) : base(coolPropKey, value)
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
    }
}