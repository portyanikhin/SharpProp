using System;
using FluentAssertions;
using NUnit.Framework;
using UnitsNet;
using UnitsNet.NumberExtensions.NumberToPressure;
using UnitsNet.NumberExtensions.NumberToRatio;
using UnitsNet.NumberExtensions.NumberToSpecificEnergy;
using UnitsNet.NumberExtensions.NumberToTemperature;

namespace SharpProp.Tests
{
    public static class TestHumidAirProcesses
    {
        private static readonly HumidAir HumidAir =
            new HumidAir().WithState(
                InputHumidAir.Pressure(1.Atmospheres()),
                InputHumidAir.Temperature(20.DegreesCelsius()),
                InputHumidAir.RelativeHumidity(RelativeHumidity.FromPercent(50)));

        private static readonly TemperatureDelta TemperatureDelta = TemperatureDelta.FromKelvins(5);
        private static readonly SpecificEnergy EnthalpyDelta = 5.KilojoulesPerKilogram();
        private static readonly Pressure PressureDrop = 200.Pascals();
        private static readonly Ratio LowHumidity = 5.PartsPerThousand();
        private static readonly Ratio HighHumidity = 9.PartsPerThousand();

        private static readonly RelativeHumidity LowRelativeHumidity =
            RelativeHumidity.FromPercent(45);

        private static readonly RelativeHumidity HighRelativeHumidity =
            RelativeHumidity.FromPercent(90);

        [Test]
        public static void TestDryCoolingToTemperature() =>
            HumidAir.DryCoolingTo(HumidAir.Temperature - TemperatureDelta, PressureDrop)
                .Should().Be(HumidAir.WithState(
                    InputHumidAir.Pressure(HumidAir.Pressure - PressureDrop),
                    InputHumidAir.Temperature(HumidAir.Temperature - TemperatureDelta),
                    InputHumidAir.Humidity(HumidAir.Humidity)));

        [TestCase(50, 0, "During the cooling process, the temperature should decrease!")]
        [TestCase(0, 0,
            "The outlet temperature after dry heat transfer should be greater than the dew point temperature!")]
        [TestCase(15, -100, "Invalid pressure drop in the heat exchanger!")]
        public static void TestDryCoolingToTemperature_WrongInput(
            double temperature, double pressureDrop, string message)
        {
            Action action = () =>
                _ = HumidAir.DryCoolingTo(temperature.DegreesCelsius(), pressureDrop.Pascals());
            action.Should().Throw<ArgumentException>().WithMessage($"{message}");
        }

        [Test]
        public static void TestDryCoolingToEnthalpy() =>
            HumidAir.DryCoolingTo(HumidAir.Enthalpy - EnthalpyDelta, PressureDrop)
                .Should().Be(HumidAir.WithState(
                    InputHumidAir.Pressure(HumidAir.Pressure - PressureDrop),
                    InputHumidAir.Enthalpy(HumidAir.Enthalpy - EnthalpyDelta),
                    InputHumidAir.Humidity(HumidAir.Humidity)));

        [TestCase(50, 0, "During the cooling process, the enthalpy should decrease!")]
        [TestCase(0, 0,
            "The outlet enthalpy after dry heat transfer should be greater than the dew point enthalpy!")]
        [TestCase(30, -100, "Invalid pressure drop in the heat exchanger!")]
        public static void TestDryCoolingToEnthalpy_WrongInput(
            double enthalpy, double pressureDrop, string message)
        {
            Action action = () =>
                _ = HumidAir.DryCoolingTo(enthalpy.KilojoulesPerKilogram(), pressureDrop.Pascals());
            action.Should().Throw<ArgumentException>().WithMessage($"{message}");
        }

        [Test]
        public static void TestWetCoolingToTemperature_RelativeHumidity() =>
            HumidAir.WetCoolingTo(HumidAir.Temperature - TemperatureDelta, LowRelativeHumidity,
                    PressureDrop)
                .Should().Be(HumidAir.WithState(
                    InputHumidAir.Pressure(HumidAir.Pressure - PressureDrop),
                    InputHumidAir.Temperature(HumidAir.Temperature - TemperatureDelta),
                    InputHumidAir.RelativeHumidity(LowRelativeHumidity)));

        [TestCase(50, 60, 0, "During the cooling process, the temperature should decrease!")]
        [TestCase(15, 100, 0, "During the wet cooling process, the absolute humidity ratio should decrease!")]
        [TestCase(15, 60, -100, "Invalid pressure drop in the heat exchanger!")]
        public static void TestWetCoolingToTemperature_RelativeHumidity_WrongInput(
            double temperature, double relativeHumidity, double pressureDrop, string message)
        {
            Action action = () =>
                _ = HumidAir.WetCoolingTo(temperature.DegreesCelsius(),
                    RelativeHumidity.FromPercent(relativeHumidity), pressureDrop.Pascals());
            action.Should().Throw<ArgumentException>().WithMessage($"{message}");
        }

        [Test]
        public static void TestWetCoolingToTemperature_Humidity() =>
            HumidAir.WetCoolingTo(HumidAir.Temperature - TemperatureDelta, LowHumidity,
                    PressureDrop)
                .Should().Be(HumidAir.WithState(
                    InputHumidAir.Pressure(HumidAir.Pressure - PressureDrop),
                    InputHumidAir.Temperature(HumidAir.Temperature - TemperatureDelta),
                    InputHumidAir.Humidity(LowHumidity)));

        [TestCase(50, 5, 0, "During the cooling process, the temperature should decrease!")]
        [TestCase(15, 9, 0, "During the wet cooling process, the absolute humidity ratio should decrease!")]
        [TestCase(15, 5, -100, "Invalid pressure drop in the heat exchanger!")]
        public static void TestWetCoolingToTemperature_Humidity_WrongInput(
            double temperature, double humidity, double pressureDrop, string message)
        {
            Action action = () =>
                _ = HumidAir.WetCoolingTo(temperature.DegreesCelsius(),
                    humidity.PartsPerThousand(), pressureDrop.Pascals());
            action.Should().Throw<ArgumentException>().WithMessage($"{message}");
        }

        [Test]
        public static void TestWetCoolingToEnthalpy_RelativeHumidity() =>
            HumidAir.WetCoolingTo(HumidAir.Enthalpy - EnthalpyDelta, LowRelativeHumidity,
                    PressureDrop)
                .Should().Be(HumidAir.WithState(
                    InputHumidAir.Pressure(HumidAir.Pressure - PressureDrop),
                    InputHumidAir.Enthalpy(HumidAir.Enthalpy - EnthalpyDelta),
                    InputHumidAir.RelativeHumidity(LowRelativeHumidity)));

        [TestCase(50, 60, 0, "During the cooling process, the enthalpy should decrease!")]
        [TestCase(35, 100, 0, "During the wet cooling process, the absolute humidity ratio should decrease!")]
        [TestCase(15, 60, -100, "Invalid pressure drop in the heat exchanger!")]
        public static void TestWetCoolingToEnthalpy_RelativeHumidity_WrongInput(
            double enthalpy, double relativeHumidity, double pressureDrop, string message)
        {
            Action action = () =>
                _ = HumidAir.WetCoolingTo(enthalpy.KilojoulesPerKilogram(),
                    RelativeHumidity.FromPercent(relativeHumidity), pressureDrop.Pascals());
            action.Should().Throw<ArgumentException>().WithMessage($"{message}");
        }

        [Test]
        public static void TestWetCoolingToEnthalpy_Humidity() =>
            HumidAir.WetCoolingTo(HumidAir.Enthalpy - EnthalpyDelta, LowHumidity,
                    PressureDrop)
                .Should().Be(HumidAir.WithState(
                    InputHumidAir.Pressure(HumidAir.Pressure - PressureDrop),
                    InputHumidAir.Enthalpy(HumidAir.Enthalpy - EnthalpyDelta),
                    InputHumidAir.Humidity(LowHumidity)));

        [TestCase(50, 5, 0, "During the cooling process, the enthalpy should decrease!")]
        [TestCase(15, 9, 0, "During the wet cooling process, the absolute humidity ratio should decrease!")]
        [TestCase(15, 5, -100, "Invalid pressure drop in the heat exchanger!")]
        public static void TestWetCoolingToEnthalpy_Humidity_WrongInput(
            double enthalpy, double humidity, double pressureDrop, string message)
        {
            Action action = () =>
                _ = HumidAir.WetCoolingTo(enthalpy.KilojoulesPerKilogram(),
                    humidity.PartsPerThousand(), pressureDrop.Pascals());
            action.Should().Throw<ArgumentException>().WithMessage($"{message}");
        }

        [Test]
        public static void TestHeatingToTemperature() =>
            HumidAir.HeatingTo(HumidAir.Temperature + TemperatureDelta, PressureDrop)
                .Should().Be(HumidAir.WithState(
                    InputHumidAir.Pressure(HumidAir.Pressure - PressureDrop),
                    InputHumidAir.Temperature(HumidAir.Temperature + TemperatureDelta),
                    InputHumidAir.Humidity(HumidAir.Humidity)));

        [TestCase(15, 0, "During the heating process, the temperature should increase!")]
        [TestCase(50, -100, "Invalid pressure drop in the heat exchanger!")]
        public static void TestHeatingToTemperature_WrongInput(
            double temperature, double pressureDrop, string message)
        {
            Action action = () =>
                _ = HumidAir.HeatingTo(temperature.DegreesCelsius(), pressureDrop.Pascals());
            action.Should().Throw<ArgumentException>().WithMessage($"{message}");
        }

        [Test]
        public static void TestHeatingToEnthalpy() =>
            HumidAir.HeatingTo(HumidAir.Enthalpy + EnthalpyDelta, PressureDrop)
                .Should().Be(HumidAir.WithState(
                    InputHumidAir.Pressure(HumidAir.Pressure - PressureDrop),
                    InputHumidAir.Enthalpy(HumidAir.Enthalpy + EnthalpyDelta),
                    InputHumidAir.Humidity(HumidAir.Humidity)));

        [TestCase(15, 0, "During the heating process, the enthalpy should increase!")]
        [TestCase(50, -100, "Invalid pressure drop in the heat exchanger!")]
        public static void TestHeatingToEnthalpy_WrongInput(
            double enthalpy, double pressureDrop, string message)
        {
            Action action = () =>
                _ = HumidAir.HeatingTo(enthalpy.KilojoulesPerKilogram(), pressureDrop.Pascals());
            action.Should().Throw<ArgumentException>().WithMessage($"{message}");
        }

        [Test]
        public static void TestHumidificationByWaterToRelativeHumidity() =>
            HumidAir.HumidificationByWaterTo(HighRelativeHumidity).Should().Be(
                HumidAir.WithState(
                    InputHumidAir.Pressure(HumidAir.Pressure),
                    InputHumidAir.Enthalpy(HumidAir.Enthalpy),
                    InputHumidAir.RelativeHumidity(HighRelativeHumidity)));

        [Test]
        public static void TestHumidificationByWaterToRelativeHumidity_WrongInput()
        {
            Action action = () =>
                _ = HumidAir.HumidificationByWaterTo(LowRelativeHumidity);
            action.Should().Throw<ArgumentException>().WithMessage(
                "During the humidification process, the absolute humidity ratio should increase!");
        }

        [Test]
        public static void TestHumidificationByWaterToHumidity() =>
            HumidAir.HumidificationByWaterTo(HighHumidity).Should().Be(
                HumidAir.WithState(
                    InputHumidAir.Pressure(HumidAir.Pressure),
                    InputHumidAir.Enthalpy(HumidAir.Enthalpy),
                    InputHumidAir.Humidity(HighHumidity)));

        [Test]
        public static void TestHumidificationByWaterToHumidity_WrongInput()
        {
            Action action = () =>
                _ = HumidAir.HumidificationByWaterTo(LowHumidity);
            action.Should().Throw<ArgumentException>().WithMessage(
                "During the humidification process, the absolute humidity ratio should increase!");
        }

        [Test]
        public static void TestHumidificationBySteamToRelativeHumidity() =>
            HumidAir.HumidificationBySteamTo(HighRelativeHumidity).Should().Be(
                HumidAir.WithState(
                    InputHumidAir.Pressure(HumidAir.Pressure),
                    InputHumidAir.Temperature(HumidAir.Temperature),
                    InputHumidAir.RelativeHumidity(HighRelativeHumidity)));

        [Test]
        public static void TestHumidificationBySteamToRelativeHumidity_WrongInput()
        {
            Action action = () =>
                _ = HumidAir.HumidificationBySteamTo(LowRelativeHumidity);
            action.Should().Throw<ArgumentException>().WithMessage(
                "During the humidification process, the absolute humidity ratio should increase!");
        }

        [Test]
        public static void TestHumidificationBySteamToHumidity() =>
            HumidAir.HumidificationBySteamTo(HighHumidity).Should().Be(
                HumidAir.WithState(
                    InputHumidAir.Pressure(HumidAir.Pressure),
                    InputHumidAir.Temperature(HumidAir.Temperature),
                    InputHumidAir.Humidity(HighHumidity)));

        [Test]
        public static void TestHumidificationBySteamToHumidity_WrongInput()
        {
            Action action = () =>
                _ = HumidAir.HumidificationBySteamTo(LowHumidity);
            action.Should().Throw<ArgumentException>().WithMessage(
                "During the humidification process, the absolute humidity ratio should increase!");
        }

        [Test]
        public static void TestMixing()
        {
            var first = HumidAir.HeatingTo(HumidAir.Temperature + TemperatureDelta);
            var second = HumidAir.HumidificationByWaterTo(HighRelativeHumidity);
            new HumidAir().Mixing(100.Percent(), first, 200.Percent(), second)
                .Should().Be(HumidAir.WithState(
                    InputHumidAir.Pressure(HumidAir.Pressure),
                    InputHumidAir.Enthalpy((1 * first.Enthalpy + 2 * second.Enthalpy) / 3.0),
                    InputHumidAir.Humidity((1 * first.Humidity + 2 * second.Humidity) / 3.0)));
        }

        [Test]
        public static void TestMixing_WrongInput()
        {
            var first = HumidAir.WithState(
                InputHumidAir.Pressure(HumidAir.Pressure - PressureDrop),
                InputHumidAir.Temperature(HumidAir.Temperature + TemperatureDelta),
                InputHumidAir.Humidity(HumidAir.Humidity));
            var second = HumidAir.HumidificationByWaterTo(HighRelativeHumidity);
            Action action = () =>
                _ = new HumidAir().Mixing(100.Percent(), first, 200.Percent(), second);
            action.Should().Throw<ArgumentException>().WithMessage(
                "The mixing process is possible only for flows with the same pressure!");
        }
    }
}