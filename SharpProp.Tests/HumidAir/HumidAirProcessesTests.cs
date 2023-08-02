using UnitsNet.NumberExtensions.NumberToRatio;

namespace SharpProp.Tests;

[Collection("Fluids")]
public class TestHumidAirProcesses
{
    private static readonly TemperatureDelta TemperatureDelta =
        TemperatureDelta.FromKelvins(5);

    private static readonly SpecificEnergy EnthalpyDelta =
        5.KilojoulesPerKilogram();

    private static readonly Pressure PressureDrop = 200.Pascals();

    private static readonly Ratio LowHumidity = 5.PartsPerThousand();

    private static readonly Ratio HighHumidity = 9.PartsPerThousand();

    private static readonly RelativeHumidity LowRelativeHumidity =
        RelativeHumidity.FromPercent(45);

    private static readonly RelativeHumidity HighRelativeHumidity =
        RelativeHumidity.FromPercent(90);

    private readonly IHumidAir _humidAir = new HumidAir().WithState(
        InputHumidAir.Pressure(1.Atmospheres()),
        InputHumidAir.Temperature(20.DegreesCelsius()),
        InputHumidAir.RelativeHumidity(RelativeHumidity.FromPercent(50))
    );

    [Theory]
    [InlineData(
        50,
        0,
        "During the cooling process, the temperature should decrease!"
    )]
    [InlineData(
        0,
        0,
        "The outlet temperature after dry heat transfer "
            + "should be greater than the dew point temperature!"
    )]
    [InlineData(15, -100, "Invalid pressure drop in the heat exchanger!")]
    public void DryCoolingTo_WrongTemperatureOrPressureDrop_ThrowsArgumentException(
        double temperature,
        double pressureDrop,
        string message
    )
    {
        Action action = () =>
            _ = _humidAir.DryCoolingTo(
                temperature.DegreesCelsius(),
                pressureDrop.Pascals()
            );
        action.Should().Throw<ArgumentException>().WithMessage($"{message}");
    }

    [Fact]
    public void DryCoolingTo_TemperatureWithPressureDrop_ReturnsAirAtTemperatureSameHumidityAndLowerPressure() =>
        _humidAir
            .DryCoolingTo(
                _humidAir.Temperature - TemperatureDelta,
                PressureDrop
            )
            .Should()
            .Be(
                _humidAir.WithState(
                    InputHumidAir.Pressure(_humidAir.Pressure - PressureDrop),
                    InputHumidAir.Temperature(
                        _humidAir.Temperature - TemperatureDelta
                    ),
                    InputHumidAir.Humidity(_humidAir.Humidity)
                )
            );

    [Theory]
    [InlineData(
        50,
        0,
        "During the cooling process, the enthalpy should decrease!"
    )]
    [InlineData(
        0,
        0,
        "The outlet enthalpy after dry heat transfer "
            + "should be greater than the dew point enthalpy!"
    )]
    [InlineData(30, -100, "Invalid pressure drop in the heat exchanger!")]
    public void DryCoolingTo_WrongEnthalpyOrPressureDrop_ThrowsArgumentException(
        double enthalpy,
        double pressureDrop,
        string message
    )
    {
        Action action = () =>
            _ = _humidAir.DryCoolingTo(
                enthalpy.KilojoulesPerKilogram(),
                pressureDrop.Pascals()
            );
        action.Should().Throw<ArgumentException>().WithMessage($"{message}");
    }

    [Fact]
    public void DryCoolingTo_EnthalpyWithPressureDrop_ReturnsAirAtEnthalpySameHumidityAndLowerPressure() =>
        _humidAir
            .DryCoolingTo(_humidAir.Enthalpy - EnthalpyDelta, PressureDrop)
            .Should()
            .Be(
                _humidAir.WithState(
                    InputHumidAir.Pressure(_humidAir.Pressure - PressureDrop),
                    InputHumidAir.Enthalpy(_humidAir.Enthalpy - EnthalpyDelta),
                    InputHumidAir.Humidity(_humidAir.Humidity)
                )
            );

    [Theory]
    [InlineData(
        50,
        60,
        0,
        "During the cooling process, the temperature should decrease!"
    )]
    [InlineData(
        15,
        100,
        0,
        "During the wet cooling process, the absolute humidity ratio should decrease!"
    )]
    [InlineData(15, 60, -100, "Invalid pressure drop in the heat exchanger!")]
    public void WetCoolingTo_WrongTemperatureRelHumidityOrPressureDrop_ThrowsArgumentException(
        double temperature,
        double relativeHumidity,
        double pressureDrop,
        string message
    )
    {
        Action action = () =>
            _ = _humidAir.WetCoolingTo(
                temperature.DegreesCelsius(),
                RelativeHumidity.FromPercent(relativeHumidity),
                pressureDrop.Pascals()
            );
        action.Should().Throw<ArgumentException>().WithMessage($"{message}");
    }

    [Fact]
    public void WetCoolingTo_TemperatureRelHumidityWithPressureDrop_ReturnsAirAtTemperatureRelHumidityAndLowerPressure() =>
        _humidAir
            .WetCoolingTo(
                _humidAir.Temperature - TemperatureDelta,
                LowRelativeHumidity,
                PressureDrop
            )
            .Should()
            .Be(
                _humidAir.WithState(
                    InputHumidAir.Pressure(_humidAir.Pressure - PressureDrop),
                    InputHumidAir.Temperature(
                        _humidAir.Temperature - TemperatureDelta
                    ),
                    InputHumidAir.RelativeHumidity(LowRelativeHumidity)
                )
            );

    [Theory]
    [InlineData(
        50,
        5,
        0,
        "During the cooling process, the temperature should decrease!"
    )]
    [InlineData(
        15,
        9,
        0,
        "During the wet cooling process, the absolute humidity ratio should decrease!"
    )]
    [InlineData(15, 5, -100, "Invalid pressure drop in the heat exchanger!")]
    public void WetCoolingTo_WrongTemperatureHumidityOrPressureDrop_ThrowsArgumentException(
        double temperature,
        double humidity,
        double pressureDrop,
        string message
    )
    {
        Action action = () =>
            _ = _humidAir.WetCoolingTo(
                temperature.DegreesCelsius(),
                humidity.PartsPerThousand(),
                pressureDrop.Pascals()
            );
        action.Should().Throw<ArgumentException>().WithMessage($"{message}");
    }

    [Fact]
    public void WetCoolingTo_TemperatureHumidityWithPressureDrop_ReturnsAirAtTemperatureHumidityAndLowerPressure() =>
        _humidAir
            .WetCoolingTo(
                _humidAir.Temperature - TemperatureDelta,
                LowHumidity,
                PressureDrop
            )
            .Should()
            .Be(
                _humidAir.WithState(
                    InputHumidAir.Pressure(_humidAir.Pressure - PressureDrop),
                    InputHumidAir.Temperature(
                        _humidAir.Temperature - TemperatureDelta
                    ),
                    InputHumidAir.Humidity(LowHumidity)
                )
            );

    [Theory]
    [InlineData(
        50,
        60,
        0,
        "During the cooling process, the enthalpy should decrease!"
    )]
    [InlineData(
        35,
        100,
        0,
        "During the wet cooling process, the absolute humidity ratio should decrease!"
    )]
    [InlineData(15, 60, -100, "Invalid pressure drop in the heat exchanger!")]
    public void WetCoolingTo_WrongEnthalpyRelHumidityOrPressureDrop_ThrowsArgumentException(
        double enthalpy,
        double relativeHumidity,
        double pressureDrop,
        string message
    )
    {
        Action action = () =>
            _ = _humidAir.WetCoolingTo(
                enthalpy.KilojoulesPerKilogram(),
                RelativeHumidity.FromPercent(relativeHumidity),
                pressureDrop.Pascals()
            );
        action.Should().Throw<ArgumentException>().WithMessage($"{message}");
    }

    [Fact]
    public void WetCoolingTo_EnthalpyRelHumidityWithPressureDrop_ReturnsAirAtEnthalpyRelHumidityAndLowerPressure() =>
        _humidAir
            .WetCoolingTo(
                _humidAir.Enthalpy - EnthalpyDelta,
                LowRelativeHumidity,
                PressureDrop
            )
            .Should()
            .Be(
                _humidAir.WithState(
                    InputHumidAir.Pressure(_humidAir.Pressure - PressureDrop),
                    InputHumidAir.Enthalpy(_humidAir.Enthalpy - EnthalpyDelta),
                    InputHumidAir.RelativeHumidity(LowRelativeHumidity)
                )
            );

    [Theory]
    [InlineData(
        50,
        5,
        0,
        "During the cooling process, the enthalpy should decrease!"
    )]
    [InlineData(
        15,
        9,
        0,
        "During the wet cooling process, the absolute humidity ratio should decrease!"
    )]
    [InlineData(15, 5, -100, "Invalid pressure drop in the heat exchanger!")]
    public void WetCoolingTo_WrongEnthalpyHumidityOrPressureDrop_ThrowsArgumentException(
        double enthalpy,
        double humidity,
        double pressureDrop,
        string message
    )
    {
        Action action = () =>
            _ = _humidAir.WetCoolingTo(
                enthalpy.KilojoulesPerKilogram(),
                humidity.PartsPerThousand(),
                pressureDrop.Pascals()
            );
        action.Should().Throw<ArgumentException>().WithMessage($"{message}");
    }

    [Fact]
    public void WetCoolingTo_EnthalpyHumidityWithPressureDrop_ReturnsAirAtEnthalpyHumidityAndLowerPressure() =>
        _humidAir
            .WetCoolingTo(
                _humidAir.Enthalpy - EnthalpyDelta,
                LowHumidity,
                PressureDrop
            )
            .Should()
            .Be(
                _humidAir.WithState(
                    InputHumidAir.Pressure(_humidAir.Pressure - PressureDrop),
                    InputHumidAir.Enthalpy(_humidAir.Enthalpy - EnthalpyDelta),
                    InputHumidAir.Humidity(LowHumidity)
                )
            );

    [Theory]
    [InlineData(
        15,
        0,
        "During the heating process, the temperature should increase!"
    )]
    [InlineData(50, -100, "Invalid pressure drop in the heat exchanger!")]
    public void HeatingTo_WrongTemperatureOrPressureDrop_ThrowsArgumentException(
        double temperature,
        double pressureDrop,
        string message
    )
    {
        Action action = () =>
            _ = _humidAir.HeatingTo(
                temperature.DegreesCelsius(),
                pressureDrop.Pascals()
            );
        action.Should().Throw<ArgumentException>().WithMessage($"{message}");
    }

    [Fact]
    public void HeatingTo_TemperatureWithPressureDrop_ReturnsAirAtTemperatureSameHumidityAndLowerPressure() =>
        _humidAir
            .HeatingTo(_humidAir.Temperature + TemperatureDelta, PressureDrop)
            .Should()
            .Be(
                _humidAir.WithState(
                    InputHumidAir.Pressure(_humidAir.Pressure - PressureDrop),
                    InputHumidAir.Temperature(
                        _humidAir.Temperature + TemperatureDelta
                    ),
                    InputHumidAir.Humidity(_humidAir.Humidity)
                )
            );

    [Theory]
    [InlineData(
        15,
        0,
        "During the heating process, the enthalpy should increase!"
    )]
    [InlineData(50, -100, "Invalid pressure drop in the heat exchanger!")]
    public void HeatingTo_WrongEnthalpyOrPressureDrop_ThrowsArgumentException(
        double enthalpy,
        double pressureDrop,
        string message
    )
    {
        Action action = () =>
            _ = _humidAir.HeatingTo(
                enthalpy.KilojoulesPerKilogram(),
                pressureDrop.Pascals()
            );
        action.Should().Throw<ArgumentException>().WithMessage($"{message}");
    }

    [Fact]
    public void HeatingTo_EnthalpyWithPressureDrop_ReturnsAirAtEnthalpySameHumidityAndLowerPressure() =>
        _humidAir
            .HeatingTo(_humidAir.Enthalpy + EnthalpyDelta, PressureDrop)
            .Should()
            .Be(
                _humidAir.WithState(
                    InputHumidAir.Pressure(_humidAir.Pressure - PressureDrop),
                    InputHumidAir.Enthalpy(_humidAir.Enthalpy + EnthalpyDelta),
                    InputHumidAir.Humidity(_humidAir.Humidity)
                )
            );

    [Fact]
    public void HumidificationByWaterTo_WrongRelHumidity_ThrowsArgumentException()
    {
        Action action = () =>
            _ = _humidAir.HumidificationByWaterTo(LowRelativeHumidity);
        action
            .Should()
            .Throw<ArgumentException>()
            .WithMessage(
                "During the humidification process, "
                    + "the absolute humidity ratio should increase!"
            );
    }

    [Fact]
    public void HumidificationByWaterTo_RelHumidity_ReturnsAirAtRelHumidityAndSameEnthalpy() =>
        _humidAir
            .HumidificationByWaterTo(HighRelativeHumidity)
            .Should()
            .Be(
                _humidAir.WithState(
                    InputHumidAir.Pressure(_humidAir.Pressure),
                    InputHumidAir.Enthalpy(_humidAir.Enthalpy),
                    InputHumidAir.RelativeHumidity(HighRelativeHumidity)
                )
            );

    [Fact]
    public void HumidificationByWaterTo_WrongHumidity_ThrowsArgumentException()
    {
        Action action = () =>
            _ = _humidAir.HumidificationByWaterTo(LowHumidity);
        action
            .Should()
            .Throw<ArgumentException>()
            .WithMessage(
                "During the humidification process, "
                    + "the absolute humidity ratio should increase!"
            );
    }

    [Fact]
    public void HumidificationByWaterTo_Humidity_ReturnsAirAtHumidityAndSameEnthalpy() =>
        _humidAir
            .HumidificationByWaterTo(HighHumidity)
            .Should()
            .Be(
                _humidAir.WithState(
                    InputHumidAir.Pressure(_humidAir.Pressure),
                    InputHumidAir.Enthalpy(_humidAir.Enthalpy),
                    InputHumidAir.Humidity(HighHumidity)
                )
            );

    [Fact]
    public void HumidificationBySteamTo_WrongRelHumidity_ThrowsArgumentException()
    {
        Action action = () =>
            _ = _humidAir.HumidificationBySteamTo(LowRelativeHumidity);
        action
            .Should()
            .Throw<ArgumentException>()
            .WithMessage(
                "During the humidification process, "
                    + "the absolute humidity ratio should increase!"
            );
    }

    [Fact]
    public void HumidificationBySteamTo_RelHumidity_ReturnsAirAtRelHumidityAndSameTemperature() =>
        _humidAir
            .HumidificationBySteamTo(HighRelativeHumidity)
            .Should()
            .Be(
                _humidAir.WithState(
                    InputHumidAir.Pressure(_humidAir.Pressure),
                    InputHumidAir.Temperature(_humidAir.Temperature),
                    InputHumidAir.RelativeHumidity(HighRelativeHumidity)
                )
            );

    [Fact]
    public void HumidificationBySteamTo_WrongHumidity_ThrowsArgumentException()
    {
        Action action = () =>
            _ = _humidAir.HumidificationBySteamTo(LowHumidity);
        action
            .Should()
            .Throw<ArgumentException>()
            .WithMessage(
                "During the humidification process, "
                    + "the absolute humidity ratio should increase!"
            );
    }

    [Fact]
    public void HumidificationBySteamTo_Humidity_ReturnsAirAtHumidityAndSameTemperature() =>
        _humidAir
            .HumidificationBySteamTo(HighHumidity)
            .Should()
            .Be(
                _humidAir.WithState(
                    InputHumidAir.Pressure(_humidAir.Pressure),
                    InputHumidAir.Temperature(_humidAir.Temperature),
                    InputHumidAir.Humidity(HighHumidity)
                )
            );

    [Fact]
    public void Mixing_WrongPressures_ThrowsArgumentException()
    {
        var first = _humidAir.WithState(
            InputHumidAir.Pressure(_humidAir.Pressure - PressureDrop),
            InputHumidAir.Temperature(_humidAir.Temperature + TemperatureDelta),
            InputHumidAir.Humidity(_humidAir.Humidity)
        );
        var second = _humidAir.HumidificationByWaterTo(HighRelativeHumidity);
        Action action = () =>
            _ = new HumidAir().Mixing(
                100.Percent(),
                first,
                200.Percent(),
                second
            );
        action
            .Should()
            .Throw<ArgumentException>()
            .WithMessage(
                "The mixing process is possible "
                    + "only for flows with the same pressure!"
            );
    }

    [Fact]
    public void Mixing_SamePressures_ReturnsMixPoint()
    {
        var first = _humidAir.HeatingTo(
            _humidAir.Temperature + TemperatureDelta
        );
        var second = _humidAir.HumidificationByWaterTo(HighRelativeHumidity);
        first
            .Mixing(100.Percent(), first, 200.Percent(), second)
            .Should()
            .Be(
                _humidAir.WithState(
                    InputHumidAir.Pressure(_humidAir.Pressure),
                    InputHumidAir.Enthalpy(
                        (1 * first.Enthalpy + 2 * second.Enthalpy) / 3.0
                    ),
                    InputHumidAir.Humidity(
                        (1 * first.Humidity + 2 * second.Humidity) / 3.0
                    )
                )
            );
    }
}
