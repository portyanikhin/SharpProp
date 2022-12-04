using UnitsNet.NumberExtensions.NumberToRatio;

namespace SharpProp.Tests;

[Collection("Fluids")]
public class TestHumidAirProcesses
{
    private static readonly TemperatureDelta TemperatureDelta = TemperatureDelta.FromKelvins(5);
    private static readonly SpecificEnergy EnthalpyDelta = 5.KilojoulesPerKilogram();
    private static readonly Pressure PressureDrop = 200.Pascals();
    private static readonly Ratio LowHumidity = 5.PartsPerThousand();
    private static readonly Ratio HighHumidity = 9.PartsPerThousand();

    private static readonly RelativeHumidity LowRelativeHumidity =
        RelativeHumidity.FromPercent(45);

    private static readonly RelativeHumidity HighRelativeHumidity =
        RelativeHumidity.FromPercent(90);

    public TestHumidAirProcesses() =>
        HumidAir = new HumidAir().WithState(
            InputHumidAir.Pressure(1.Atmospheres()),
            InputHumidAir.Temperature(20.DegreesCelsius()),
            InputHumidAir.RelativeHumidity(RelativeHumidity.FromPercent(50)));

    private HumidAir HumidAir { get; }

    [Theory]
    [InlineData(50, 0, "During the cooling process, the temperature should decrease!")]
    [InlineData(0, 0,
        "The outlet temperature after dry heat transfer should be greater than the dew point temperature!")]
    [InlineData(15, -100, "Invalid pressure drop in the heat exchanger!")]
    public void DryCoolingTo_WrongTemperatureOrPressureDrop_ThrowsArgumentException(
        double temperature, double pressureDrop, string message)
    {
        Action action = () =>
            _ = HumidAir.DryCoolingTo(temperature.DegreesCelsius(), pressureDrop.Pascals());
        action.Should().Throw<ArgumentException>().WithMessage($"{message}");
    }

    [Fact]
    public void DryCoolingTo_TemperatureWithPressureDrop_ReturnsAirAtTemperatureSameHumidityAndLowerPressure() =>
        HumidAir.DryCoolingTo(HumidAir.Temperature - TemperatureDelta, PressureDrop)
            .Should().Be(HumidAir.WithState(
                InputHumidAir.Pressure(HumidAir.Pressure - PressureDrop),
                InputHumidAir.Temperature(HumidAir.Temperature - TemperatureDelta),
                InputHumidAir.Humidity(HumidAir.Humidity)));

    [Theory]
    [InlineData(50, 0, "During the cooling process, the enthalpy should decrease!")]
    [InlineData(0, 0,
        "The outlet enthalpy after dry heat transfer should be greater than the dew point enthalpy!")]
    [InlineData(30, -100, "Invalid pressure drop in the heat exchanger!")]
    public void DryCoolingTo_WrongEnthalpyOrPressureDrop_ThrowsArgumentException(
        double enthalpy, double pressureDrop, string message)
    {
        Action action = () =>
            _ = HumidAir.DryCoolingTo(enthalpy.KilojoulesPerKilogram(), pressureDrop.Pascals());
        action.Should().Throw<ArgumentException>().WithMessage($"{message}");
    }

    [Fact]
    public void DryCoolingTo_EnthalpyWithPressureDrop_ReturnsAirAtEnthalpySameHumidityAndLowerPressure() =>
        HumidAir.DryCoolingTo(HumidAir.Enthalpy - EnthalpyDelta, PressureDrop)
            .Should().Be(HumidAir.WithState(
                InputHumidAir.Pressure(HumidAir.Pressure - PressureDrop),
                InputHumidAir.Enthalpy(HumidAir.Enthalpy - EnthalpyDelta),
                InputHumidAir.Humidity(HumidAir.Humidity)));

    [Theory]
    [InlineData(50, 60, 0, "During the cooling process, the temperature should decrease!")]
    [InlineData(15, 100, 0, "During the wet cooling process, the absolute humidity ratio should decrease!")]
    [InlineData(15, 60, -100, "Invalid pressure drop in the heat exchanger!")]
    public void WetCoolingTo_WrongTemperatureRelHumidityOrPressureDrop_ThrowsArgumentException(
        double temperature, double relativeHumidity, double pressureDrop, string message)
    {
        Action action = () =>
            _ = HumidAir.WetCoolingTo(temperature.DegreesCelsius(),
                RelativeHumidity.FromPercent(relativeHumidity), pressureDrop.Pascals());
        action.Should().Throw<ArgumentException>().WithMessage($"{message}");
    }

    [Fact]
    public void
        WetCoolingTo_TemperatureRelHumidityWithPressureDrop_ReturnsAirAtTemperatureRelHumidityAndLowerPressure() =>
        HumidAir.WetCoolingTo(HumidAir.Temperature - TemperatureDelta, LowRelativeHumidity, PressureDrop)
            .Should().Be(HumidAir.WithState(
                InputHumidAir.Pressure(HumidAir.Pressure - PressureDrop),
                InputHumidAir.Temperature(HumidAir.Temperature - TemperatureDelta),
                InputHumidAir.RelativeHumidity(LowRelativeHumidity)));

    [Theory]
    [InlineData(50, 5, 0, "During the cooling process, the temperature should decrease!")]
    [InlineData(15, 9, 0, "During the wet cooling process, the absolute humidity ratio should decrease!")]
    [InlineData(15, 5, -100, "Invalid pressure drop in the heat exchanger!")]
    public void WetCoolingTo_WrongTemperatureHumidityOrPressureDrop_ThrowsArgumentException(
        double temperature, double humidity, double pressureDrop, string message)
    {
        Action action = () =>
            _ = HumidAir.WetCoolingTo(temperature.DegreesCelsius(),
                humidity.PartsPerThousand(), pressureDrop.Pascals());
        action.Should().Throw<ArgumentException>().WithMessage($"{message}");
    }

    [Fact]
    public void
        WetCoolingTo_TemperatureHumidityWithPressureDrop_ReturnsAirAtTemperatureHumidityAndLowerPressure() =>
        HumidAir.WetCoolingTo(HumidAir.Temperature - TemperatureDelta, LowHumidity, PressureDrop)
            .Should().Be(HumidAir.WithState(
                InputHumidAir.Pressure(HumidAir.Pressure - PressureDrop),
                InputHumidAir.Temperature(HumidAir.Temperature - TemperatureDelta),
                InputHumidAir.Humidity(LowHumidity)));

    [Theory]
    [InlineData(50, 60, 0, "During the cooling process, the enthalpy should decrease!")]
    [InlineData(35, 100, 0, "During the wet cooling process, the absolute humidity ratio should decrease!")]
    [InlineData(15, 60, -100, "Invalid pressure drop in the heat exchanger!")]
    public void WetCoolingTo_WrongEnthalpyRelHumidityOrPressureDrop_ThrowsArgumentException(
        double enthalpy, double relativeHumidity, double pressureDrop, string message)
    {
        Action action = () =>
            _ = HumidAir.WetCoolingTo(enthalpy.KilojoulesPerKilogram(),
                RelativeHumidity.FromPercent(relativeHumidity), pressureDrop.Pascals());
        action.Should().Throw<ArgumentException>().WithMessage($"{message}");
    }

    [Fact]
    public void
        WetCoolingTo_EnthalpyRelHumidityWithPressureDrop_ReturnsAirAtEnthalpyRelHumidityAndLowerPressure() =>
        HumidAir.WetCoolingTo(HumidAir.Enthalpy - EnthalpyDelta, LowRelativeHumidity, PressureDrop)
            .Should().Be(HumidAir.WithState(
                InputHumidAir.Pressure(HumidAir.Pressure - PressureDrop),
                InputHumidAir.Enthalpy(HumidAir.Enthalpy - EnthalpyDelta),
                InputHumidAir.RelativeHumidity(LowRelativeHumidity)));

    [Theory]
    [InlineData(50, 5, 0, "During the cooling process, the enthalpy should decrease!")]
    [InlineData(15, 9, 0, "During the wet cooling process, the absolute humidity ratio should decrease!")]
    [InlineData(15, 5, -100, "Invalid pressure drop in the heat exchanger!")]
    public void WetCoolingTo_WrongEnthalpyHumidityOrPressureDrop_ThrowsArgumentException(
        double enthalpy, double humidity, double pressureDrop, string message)
    {
        Action action = () =>
            _ = HumidAir.WetCoolingTo(enthalpy.KilojoulesPerKilogram(),
                humidity.PartsPerThousand(), pressureDrop.Pascals());
        action.Should().Throw<ArgumentException>().WithMessage($"{message}");
    }

    [Fact]
    public void WetCoolingTo_EnthalpyHumidityWithPressureDrop_ReturnsAirAtEnthalpyHumidityAndLowerPressure() =>
        HumidAir.WetCoolingTo(HumidAir.Enthalpy - EnthalpyDelta, LowHumidity, PressureDrop)
            .Should().Be(HumidAir.WithState(
                InputHumidAir.Pressure(HumidAir.Pressure - PressureDrop),
                InputHumidAir.Enthalpy(HumidAir.Enthalpy - EnthalpyDelta),
                InputHumidAir.Humidity(LowHumidity)));

    [Theory]
    [InlineData(15, 0, "During the heating process, the temperature should increase!")]
    [InlineData(50, -100, "Invalid pressure drop in the heat exchanger!")]
    public void HeatingTo_WrongTemperatureOrPressureDrop_ThrowsArgumentException(
        double temperature, double pressureDrop, string message)
    {
        Action action = () =>
            _ = HumidAir.HeatingTo(temperature.DegreesCelsius(), pressureDrop.Pascals());
        action.Should().Throw<ArgumentException>().WithMessage($"{message}");
    }

    [Fact]
    public void HeatingTo_TemperatureWithPressureDrop_ReturnsAirAtTemperatureSameHumidityAndLowerPressure() =>
        HumidAir.HeatingTo(HumidAir.Temperature + TemperatureDelta, PressureDrop)
            .Should().Be(HumidAir.WithState(
                InputHumidAir.Pressure(HumidAir.Pressure - PressureDrop),
                InputHumidAir.Temperature(HumidAir.Temperature + TemperatureDelta),
                InputHumidAir.Humidity(HumidAir.Humidity)));

    [Theory]
    [InlineData(15, 0, "During the heating process, the enthalpy should increase!")]
    [InlineData(50, -100, "Invalid pressure drop in the heat exchanger!")]
    public void HeatingTo_WrongEnthalpyOrPressureDrop_ThrowsArgumentException(
        double enthalpy, double pressureDrop, string message)
    {
        Action action = () =>
            _ = HumidAir.HeatingTo(enthalpy.KilojoulesPerKilogram(), pressureDrop.Pascals());
        action.Should().Throw<ArgumentException>().WithMessage($"{message}");
    }

    [Fact]
    public void HeatingTo_EnthalpyWithPressureDrop_ReturnsAirAtEnthalpySameHumidityAndLowerPressure() =>
        HumidAir.HeatingTo(HumidAir.Enthalpy + EnthalpyDelta, PressureDrop)
            .Should().Be(HumidAir.WithState(
                InputHumidAir.Pressure(HumidAir.Pressure - PressureDrop),
                InputHumidAir.Enthalpy(HumidAir.Enthalpy + EnthalpyDelta),
                InputHumidAir.Humidity(HumidAir.Humidity)));

    [Fact]
    public void HumidificationByWaterTo_WrongRelHumidity_ThrowsArgumentException()
    {
        Action action = () =>
            _ = HumidAir.HumidificationByWaterTo(LowRelativeHumidity);
        action.Should().Throw<ArgumentException>().WithMessage(
            "During the humidification process, the absolute humidity ratio should increase!");
    }

    [Fact]
    public void HumidificationByWaterTo_RelHumidity_ReturnsAirAtRelHumidityAndSameEnthalpy() =>
        HumidAir.HumidificationByWaterTo(HighRelativeHumidity).Should().Be(
            HumidAir.WithState(
                InputHumidAir.Pressure(HumidAir.Pressure),
                InputHumidAir.Enthalpy(HumidAir.Enthalpy),
                InputHumidAir.RelativeHumidity(HighRelativeHumidity)));

    [Fact]
    public void HumidificationByWaterTo_WrongHumidity_ThrowsArgumentException()
    {
        Action action = () =>
            _ = HumidAir.HumidificationByWaterTo(LowHumidity);
        action.Should().Throw<ArgumentException>().WithMessage(
            "During the humidification process, the absolute humidity ratio should increase!");
    }

    [Fact]
    public void HumidificationByWaterTo_Humidity_ReturnsAirAtHumidityAndSameEnthalpy() =>
        HumidAir.HumidificationByWaterTo(HighHumidity).Should().Be(
            HumidAir.WithState(
                InputHumidAir.Pressure(HumidAir.Pressure),
                InputHumidAir.Enthalpy(HumidAir.Enthalpy),
                InputHumidAir.Humidity(HighHumidity)));

    [Fact]
    public void HumidificationBySteamTo_WrongRelHumidity_ThrowsArgumentException()
    {
        Action action = () =>
            _ = HumidAir.HumidificationBySteamTo(LowRelativeHumidity);
        action.Should().Throw<ArgumentException>().WithMessage(
            "During the humidification process, the absolute humidity ratio should increase!");
    }

    [Fact]
    public void HumidificationBySteamTo_RelHumidity_ReturnsAirAtRelHumidityAndSameTemperature() =>
        HumidAir.HumidificationBySteamTo(HighRelativeHumidity).Should().Be(
            HumidAir.WithState(
                InputHumidAir.Pressure(HumidAir.Pressure),
                InputHumidAir.Temperature(HumidAir.Temperature),
                InputHumidAir.RelativeHumidity(HighRelativeHumidity)));

    [Fact]
    public void HumidificationBySteamTo_WrongHumidity_ThrowsArgumentException()
    {
        Action action = () =>
            _ = HumidAir.HumidificationBySteamTo(LowHumidity);
        action.Should().Throw<ArgumentException>().WithMessage(
            "During the humidification process, the absolute humidity ratio should increase!");
    }

    [Fact]
    public void HumidificationBySteamTo_Humidity_ReturnsAirAtHumidityAndSameTemperature() =>
        HumidAir.HumidificationBySteamTo(HighHumidity).Should().Be(
            HumidAir.WithState(
                InputHumidAir.Pressure(HumidAir.Pressure),
                InputHumidAir.Temperature(HumidAir.Temperature),
                InputHumidAir.Humidity(HighHumidity)));

    [Fact]
    public void Mixing_WrongPressures_ThrowsArgumentException()
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

    [Fact]
    public void Mixing_SamePressures_ReturnsMixPoint()
    {
        var first = HumidAir.HeatingTo(HumidAir.Temperature + TemperatureDelta);
        var second = HumidAir.HumidificationByWaterTo(HighRelativeHumidity);
        new HumidAir().Mixing(100.Percent(), first, 200.Percent(), second)
            .Should().Be(HumidAir.WithState(
                InputHumidAir.Pressure(HumidAir.Pressure),
                InputHumidAir.Enthalpy((1 * first.Enthalpy + 2 * second.Enthalpy) / 3.0),
                InputHumidAir.Humidity((1 * first.Humidity + 2 * second.Humidity) / 3.0)));
    }
}