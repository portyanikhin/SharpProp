using NUnit.Framework;
using UnitsNet;
using UnitsNet.NumberExtensions.NumberToPressure;
using UnitsNet.NumberExtensions.NumberToRelativeHumidity;
using UnitsNet.NumberExtensions.NumberToTemperature;
using UnitsNet.Units;

namespace SharpProp.Tests
{
    public class TestHumidAirExtended
    {
        private HumidAirExtended _humidAir = null!;

        [SetUp]
        public void SetUp()
        {
            _humidAir = new HumidAirExtended();
            _humidAir.Update(InputHumidAir.Pressure(1.Atmospheres()), InputHumidAir.Temperature(20.DegreesCelsius()),
                InputHumidAir.RelativeHumidity(50.Percent()));
        }

        [Test(ExpectedResult = 722.68718970632506)]
        public double TestSpecificHeatConstVolume() =>
            _humidAir.SpecificHeatConstVolume.JoulesPerKilogramKelvin;

        /// <summary>
        ///     An example of how to add new properties to a <see cref="HumidAir" />.
        /// </summary>
        private class HumidAirExtended : HumidAir
        {
            private SpecificEntropy? _specificHeatConstVolume;

            /// <summary>
            ///     Mixture specific heat at constant volume per unit humid air.
            /// </summary>
            public SpecificEntropy SpecificHeatConstVolume => _specificHeatConstVolume ??=
                SpecificEntropy.FromJoulesPerKilogramKelvin(KeyedOutput("CVha"))
                    .ToUnit(SpecificEntropyUnit.KilojoulePerKilogramKelvin);

            protected override void Reset()
            {
                base.Reset();
                _specificHeatConstVolume = null;
            }
        }
    }
}