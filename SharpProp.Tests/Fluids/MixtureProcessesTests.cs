using UnitsNet.NumberExtensions.NumberToRatio;

namespace SharpProp.Tests;

[Collection("Fluids")]
public class MixtureProcessesTests : IDisposable
{
    private static readonly TemperatureDelta TemperatureDelta =
        TemperatureDelta.FromKelvins(10);

    private static readonly Pressure PressureDrop = 50.Kilopascals();

    private readonly Mixture _mixture;

    public MixtureProcessesTests() =>
        _mixture = new Mixture(
            new List<FluidsList> { FluidsList.Argon, FluidsList.IsoButane },
            new List<Ratio> { 50.Percent(), 50.Percent() }
        ).WithState(
            Input.Pressure(1.Atmospheres()),
            Input.Temperature(20.DegreesCelsius())
        );

    public void Dispose()
    {
        _mixture.Dispose();
        GC.SuppressFinalize(this);
    }

    [Theory]
    [InlineData(
        5,
        0,
        "During the cooling process, the temperature should decrease!"
    )]
    [InlineData(-5, -10, "Invalid pressure drop in the heat exchanger!")]
    public void CoolingTo_WrongTemperatureOrPressureDrop_ThrowsArgumentException(
        double temperatureDelta,
        double pressureDrop,
        string message
    )
    {
        Action action = () =>
            _ = _mixture.CoolingTo(
                _mixture.Temperature
                    + TemperatureDelta.FromKelvins(temperatureDelta),
                pressureDrop.Kilopascals()
            );
        action.Should().Throw<ArgumentException>().WithMessage($"*{message}*");
    }

    [Fact]
    public void CoolingTo_TemperatureWithPressureDrop_ReturnsMixtureAtTemperatureAndLowerPressure() =>
        _mixture
            .CoolingTo(_mixture.Temperature - TemperatureDelta, PressureDrop)
            .Should()
            .Be(
                _mixture.WithState(
                    Input.Pressure(_mixture.Pressure - PressureDrop),
                    Input.Temperature(_mixture.Temperature - TemperatureDelta)
                )
            );

    [Theory]
    [InlineData(
        5,
        0,
        "During the heating process, the temperature should increase!"
    )]
    [InlineData(-5, -10, "Invalid pressure drop in the heat exchanger!")]
    public void HeatingTo_WrongTemperatureOrPressureDrop_ThrowsArgumentException(
        double temperatureDelta,
        double pressureDrop,
        string message
    )
    {
        Action action = () =>
            _ = _mixture.HeatingTo(
                _mixture.Temperature
                    - TemperatureDelta.FromKelvins(temperatureDelta),
                pressureDrop.Kilopascals()
            );
        action.Should().Throw<ArgumentException>().WithMessage($"*{message}*");
    }

    [Fact]
    public void HeatingTo_TemperatureWithPressureDrop_ReturnsMixtureAtTemperatureAndLowerPressure() =>
        _mixture
            .HeatingTo(_mixture.Temperature + TemperatureDelta, PressureDrop)
            .Should()
            .Be(
                _mixture.WithState(
                    Input.Pressure(_mixture.Pressure - PressureDrop),
                    Input.Temperature(_mixture.Temperature + TemperatureDelta)
                )
            );
}
