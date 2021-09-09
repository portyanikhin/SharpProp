using CoolProp;
using NUnit.Framework;

namespace SharpProp.Tests
{
    public static class TestInput
    {
        private static object[] _inputCases =
        {
            new object[] {Input.Density(999), parameters.iDmass, 999},
            new object[] {Input.Enthalpy(1e3), parameters.iHmass, 1e3},
            new object[] {Input.Entropy(5e3), parameters.iSmass, 5e3},
            new object[] {Input.InternalEnergy(1e4), parameters.iUmass, 1e4},
            new object[] {Input.Pressure(101325), parameters.iP, 101325},
            new object[] {Input.Quality(0.5), parameters.iQ, 0.5},
            new object[] {Input.Temperature(293.15), parameters.iT, 293.15}
        };

        [TestCaseSource(nameof(_inputCases))]
        public static void TestCoolPropKey(Input input, parameters coolPropKey, double value) =>
            Assert.AreEqual(coolPropKey, input.CoolPropKey);

        [TestCaseSource(nameof(_inputCases))]
        public static void TestValue(Input input, parameters coolPropKey, double value) =>
            Assert.AreEqual(value, input.Value);
    }
}