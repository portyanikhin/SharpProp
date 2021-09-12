using NUnit.Framework;

namespace SharpProp.Tests
{
    public class TestHumidAirExtended
    {
        private HumidAirExtended _humidAir = null!;

        [SetUp]
        public void SetUp()
        {
            _humidAir = new HumidAirExtended();
            _humidAir.Update(InputHumidAir.Pressure(101325), InputHumidAir.Temperature(293.15),
                InputHumidAir.RelativeHumidity(0.5));
        }

        [Test(ExpectedResult = 722.68718970632506)]
        public double TestSpecificHeatConstVolume() => _humidAir.SpecificHeatConstVolume;

        /// <summary>
        ///     An example of how to add new properties to a <see cref="HumidAir" />
        /// </summary>
        private class HumidAirExtended : HumidAir
        {
            private double? _specificHeatConstVolume;

            /// <summary>
            ///     Mixture specific heat at constant volume per unit humid air [J/kg/K]
            /// </summary>
            public double SpecificHeatConstVolume => _specificHeatConstVolume ??= KeyedOutput("CVha");

            protected override void Reset()
            {
                base.Reset();
                _specificHeatConstVolume = null;
            }
        }
    }
}