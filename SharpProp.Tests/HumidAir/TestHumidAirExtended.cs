using FluentAssertions;
using NUnit.Framework;
using UnitsNet;
using UnitsNet.NumberExtensions.NumberToPressure;
using UnitsNet.NumberExtensions.NumberToRatio;
using UnitsNet.NumberExtensions.NumberToSpecificEnergy;
using UnitsNet.NumberExtensions.NumberToTemperature;
using UnitsNet.Units;

namespace SharpProp.Tests
{
    /// <summary>
    ///     An example of how to add new properties to a <see cref="HumidAir" />.
    /// </summary>
    public class HumidAirExtended : HumidAir
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

        public override HumidAirExtended Factory() => new();

        public override HumidAirExtended WithState(IKeyedInput<string> fistInput,
            IKeyedInput<string> secondInput, IKeyedInput<string> thirdInput) =>
            (HumidAirExtended) base.WithState(fistInput, secondInput, thirdInput);

        public override HumidAirExtended DryCoolingTo(Temperature temperature,
            Pressure? pressureDrop = null) =>
            (HumidAirExtended) base.DryCoolingTo(temperature, pressureDrop);

        public override HumidAirExtended DryCoolingTo(SpecificEnergy enthalpy,
            Pressure? pressureDrop = null) =>
            (HumidAirExtended) base.DryCoolingTo(enthalpy, pressureDrop);

        public override HumidAirExtended WetCoolingTo(Temperature temperature,
            RelativeHumidity relativeHumidity, Pressure? pressureDrop = null) =>
            (HumidAirExtended) base.WetCoolingTo(temperature, relativeHumidity, pressureDrop);

        public override HumidAirExtended WetCoolingTo(Temperature temperature,
            Ratio humidity, Pressure? pressureDrop = null) =>
            (HumidAirExtended) base.WetCoolingTo(temperature, humidity, pressureDrop);

        public override HumidAirExtended WetCoolingTo(SpecificEnergy enthalpy,
            RelativeHumidity relativeHumidity, Pressure? pressureDrop = null) =>
            (HumidAirExtended) base.WetCoolingTo(enthalpy, relativeHumidity, pressureDrop);

        public override HumidAirExtended WetCoolingTo(SpecificEnergy enthalpy,
            Ratio humidity, Pressure? pressureDrop = null) =>
            (HumidAirExtended) base.WetCoolingTo(enthalpy, humidity, pressureDrop);

        public override HumidAirExtended HeatingTo(Temperature temperature,
            Pressure? pressureDrop = null) =>
            (HumidAirExtended) base.HeatingTo(temperature, pressureDrop);

        public override HumidAirExtended HeatingTo(SpecificEnergy enthalpy,
            Pressure? pressureDrop = null) =>
            (HumidAirExtended) base.HeatingTo(enthalpy, pressureDrop);

        public override HumidAirExtended HumidificationByWaterTo(
            RelativeHumidity relativeHumidity) =>
            (HumidAirExtended) base.HumidificationByWaterTo(relativeHumidity);

        public override HumidAirExtended HumidificationByWaterTo(Ratio humidity) =>
            (HumidAirExtended) base.HumidificationByWaterTo(humidity);

        public override HumidAirExtended HumidificationBySteamTo(
            RelativeHumidity relativeHumidity) =>
            (HumidAirExtended) base.HumidificationBySteamTo(relativeHumidity);

        public override HumidAirExtended HumidificationBySteamTo(Ratio humidity) =>
            (HumidAirExtended) base.HumidificationBySteamTo(humidity);

        public override HumidAirExtended Mixing(Ratio firstSpecificMassFlow, HumidAir first,
            Ratio secondSpecificMassFlow, HumidAir second) =>
            (HumidAirExtended) base.Mixing(firstSpecificMassFlow, first,
                secondSpecificMassFlow, second);
    }

    public static class TestHumidAirExtended
    {
        private static readonly HumidAirExtended HumidAir =
            new HumidAirExtended().WithState(
                InputHumidAir.Pressure(1.Atmospheres()),
                InputHumidAir.Temperature(20.DegreesCelsius()),
                InputHumidAir.RelativeHumidity(RelativeHumidity.FromPercent(50)));

        private static readonly TemperatureDelta TemperatureDelta = TemperatureDelta.FromKelvins(5);
        private static readonly SpecificEnergy EnthalpyDelta = 5.KilojoulesPerKilogram();
        private static readonly Ratio LowHumidity = 5.PartsPerThousand();
        private static readonly Ratio HighHumidity = 9.PartsPerThousand();

        private static readonly RelativeHumidity LowRelativeHumidity =
            RelativeHumidity.FromPercent(45);

        private static readonly RelativeHumidity HighRelativeHumidity =
            RelativeHumidity.FromPercent(90);

        [Test(ExpectedResult = 722.68718970632506)]
        public static double TestSpecificHeatConstVolume() =>
            HumidAir.SpecificHeatConstVolume.JoulesPerKilogramKelvin;

        [Test]
        public static void TestProcesses()
        {
            HumidAir.DryCoolingTo(HumidAir.Temperature - TemperatureDelta)
                .Should().BeOfType<HumidAirExtended>();
            HumidAir.DryCoolingTo(HumidAir.Enthalpy - EnthalpyDelta)
                .Should().BeOfType<HumidAirExtended>();
            HumidAir.WetCoolingTo(HumidAir.Temperature - TemperatureDelta,
                    LowRelativeHumidity)
                .Should().BeOfType<HumidAirExtended>();
            HumidAir.WetCoolingTo(HumidAir.Temperature - TemperatureDelta,
                    LowHumidity)
                .Should().BeOfType<HumidAirExtended>();
            HumidAir.WetCoolingTo(HumidAir.Enthalpy - EnthalpyDelta,
                    LowRelativeHumidity)
                .Should().BeOfType<HumidAirExtended>();
            HumidAir.WetCoolingTo(HumidAir.Enthalpy - EnthalpyDelta,
                    LowHumidity)
                .Should().BeOfType<HumidAirExtended>();
            HumidAir.HeatingTo(HumidAir.Temperature + TemperatureDelta)
                .Should().BeOfType<HumidAirExtended>();
            HumidAir.HeatingTo(HumidAir.Enthalpy + EnthalpyDelta)
                .Should().BeOfType<HumidAirExtended>();
            HumidAir.HumidificationByWaterTo(HighRelativeHumidity)
                .Should().BeOfType<HumidAirExtended>();
            HumidAir.HumidificationByWaterTo(HighHumidity)
                .Should().BeOfType<HumidAirExtended>();
            HumidAir.HumidificationBySteamTo(HighRelativeHumidity)
                .Should().BeOfType<HumidAirExtended>();
            HumidAir.HumidificationBySteamTo(HighHumidity)
                .Should().BeOfType<HumidAirExtended>();
            HumidAir.Mixing(
                    100.Percent(),
                    HumidAir.WetCoolingTo(HumidAir.Temperature - TemperatureDelta,
                        LowHumidity),
                    200.Percent(),
                    HumidAir.HumidificationBySteamTo(HighHumidity))
                .Should().BeOfType<HumidAirExtended>();
        }
    }
}