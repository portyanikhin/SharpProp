using UnitsNet.NumberExtensions.NumberToRatio;

namespace SharpProp.Tests;

[Collection("Fluids")]
public class FluidProcessesTests : IDisposable
{
    private static readonly Ratio IsentropicEfficiency = 80.Percent();

    private static readonly TemperatureDelta TemperatureDelta =
        TemperatureDelta.FromKelvins(10);

    private static readonly SpecificEnergy EnthalpyDelta =
        50.KilojoulesPerKilogram();

    private static readonly Pressure PressureDrop = 50.Kilopascals();

    private readonly Fluid _fluid;

    public FluidProcessesTests() =>
        _fluid = new Fluid(FluidsList.Water).WithState(
            Input.Pressure(1.Atmospheres()),
            Input.Temperature(150.DegreesCelsius())
        );

    private Pressure HighPressure => 2 * _fluid.Pressure;

    private Pressure LowPressure => 0.5 * _fluid.Pressure;

    public void Dispose()
    {
        _fluid.Dispose();
        GC.SuppressFinalize(this);
    }

    [Fact]
    public void IsentropicCompressionTo_WrongPressure_ThrowsArgumentException()
    {
        Action action = () => _ = _fluid.IsentropicCompressionTo(LowPressure);
        action
            .Should()
            .Throw<ArgumentException>()
            .WithMessage(
                "Compressor outlet pressure "
                    + "should be higher than inlet pressure!"
            );
    }

    [Fact]
    public void IsentropicCompressionTo_HighPressure_ReturnsFluidAtHighPressureAndSameEntropy() =>
        _fluid
            .IsentropicCompressionTo(HighPressure)
            .Should()
            .Be(
                _fluid.WithState(
                    Input.Pressure(HighPressure),
                    Input.Entropy(_fluid.Entropy)
                )
            );

    [Theory]
    [InlineData(
        0.5,
        80,
        "Compressor outlet pressure should be higher than inlet pressure!"
    )]
    [InlineData(2, 0, "Invalid compressor isentropic efficiency!")]
    [InlineData(2, 100, "Invalid compressor isentropic efficiency!")]
    public void CompressionTo_WrongPressureOrIsentropicEfficiency_ThrowsArgumentException(
        double pressureRatio,
        double isentropicEfficiency,
        string message
    )
    {
        Action action = () =>
            _ = _fluid.CompressionTo(
                pressureRatio * _fluid.Pressure,
                isentropicEfficiency.Percent()
            );
        action.Should().Throw<ArgumentException>().WithMessage($"*{message}*");
    }

    [Fact]
    public void CompressionTo_HighPressureWithIsentropicEfficiency_ReturnsFluidAtHighPressureAndHigherEntropy() =>
        _fluid
            .CompressionTo(HighPressure, IsentropicEfficiency)
            .Should()
            .Be(
                _fluid.WithState(
                    Input.Pressure(HighPressure),
                    Input.Enthalpy(
                        _fluid.Enthalpy
                            + (
                                _fluid
                                    .IsentropicCompressionTo(HighPressure)
                                    .Enthalpy - _fluid.Enthalpy
                            ) / IsentropicEfficiency.DecimalFractions
                    )
                )
            );

    [Fact]
    public void IsenthalpicExpansionTo_WrongPressure_ThrowsArgumentException()
    {
        Action action = () => _ = _fluid.IsenthalpicExpansionTo(HighPressure);
        action
            .Should()
            .Throw<ArgumentException>()
            .WithMessage(
                "Expansion valve outlet pressure "
                    + "should be lower than inlet pressure!"
            );
    }

    [Fact]
    public void IsenthalpicExpansionTo_LowPressure_ReturnsFluidAtLowPressureAndSameEnthalpy() =>
        _fluid
            .IsenthalpicExpansionTo(LowPressure)
            .Should()
            .Be(
                _fluid.WithState(
                    Input.Pressure(LowPressure),
                    Input.Enthalpy(_fluid.Enthalpy)
                )
            );

    [Fact]
    public void IsentropicExpansionTo_WrongPressure_ThrowsArgumentException()
    {
        Action action = () => _ = _fluid.IsentropicExpansionTo(HighPressure);
        action
            .Should()
            .Throw<ArgumentException>()
            .WithMessage(
                "Expander outlet pressure "
                    + "should be lower than inlet pressure!"
            );
    }

    [Fact]
    public void IsentropicExpansionTo_LowPressure_ReturnsFluidAtLowPressureAndSameEntropy() =>
        _fluid
            .IsentropicExpansionTo(LowPressure)
            .Should()
            .Be(
                _fluid.WithState(
                    Input.Pressure(LowPressure),
                    Input.Entropy(_fluid.Entropy)
                )
            );

    [Theory]
    [InlineData(
        2,
        80,
        "Expander outlet pressure should be lower than inlet pressure!"
    )]
    [InlineData(0.5, 0, "Invalid expander isentropic efficiency!")]
    [InlineData(0.5, 100, "Invalid expander isentropic efficiency!")]
    public void ExpansionTo_WrongPressureOrIsentropicEfficiency_ThrowsArgumentException(
        double pressureRatio,
        double isentropicEfficiency,
        string message
    )
    {
        Action action = () =>
            _ = _fluid.ExpansionTo(
                pressureRatio * _fluid.Pressure,
                isentropicEfficiency.Percent()
            );
        action.Should().Throw<ArgumentException>().WithMessage($"*{message}*");
    }

    [Fact]
    public void ExpansionTo_LowPressureWithIsentropicEfficiency_ReturnsFluidAtLowPressureAndHigherEntropy() =>
        _fluid
            .ExpansionTo(LowPressure, IsentropicEfficiency)
            .Should()
            .Be(
                _fluid.WithState(
                    Input.Pressure(LowPressure),
                    Input.Enthalpy(
                        _fluid.Enthalpy
                            - (
                                _fluid.Enthalpy
                                - _fluid
                                    .IsentropicExpansionTo(LowPressure)
                                    .Enthalpy
                            ) * IsentropicEfficiency.DecimalFractions
                    )
                )
            );

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
            _ = _fluid.CoolingTo(
                _fluid.Temperature
                    + TemperatureDelta.FromKelvins(temperatureDelta),
                pressureDrop.Kilopascals()
            );
        action.Should().Throw<ArgumentException>().WithMessage($"*{message}*");
    }

    [Fact]
    public void CoolingTo_TemperatureWithPressureDrop_ReturnsFluidAtTemperatureAndLowerPressure() =>
        _fluid
            .CoolingTo(_fluid.Temperature - TemperatureDelta, PressureDrop)
            .Should()
            .Be(
                _fluid.WithState(
                    Input.Pressure(_fluid.Pressure - PressureDrop),
                    Input.Temperature(_fluid.Temperature - TemperatureDelta)
                )
            );

    [Theory]
    [InlineData(
        5,
        0,
        "During the cooling process, the enthalpy should decrease!"
    )]
    [InlineData(-5, -10, "Invalid pressure drop in the heat exchanger!")]
    public void CoolingTo_WrongEnthalpyOrPressureDrop_ThrowsArgumentException(
        double enthalpyDelta,
        double pressureDrop,
        string message
    )
    {
        Action action = () =>
            _ = _fluid.CoolingTo(
                _fluid.Enthalpy + enthalpyDelta.KilojoulesPerKilogram(),
                pressureDrop.Kilopascals()
            );
        action.Should().Throw<ArgumentException>().WithMessage($"*{message}*");
    }

    [Fact]
    public void CoolingTo_EnthalpyWithPressureDrop_ReturnsFluidAtEnthalpyAndLowerPressure() =>
        _fluid
            .CoolingTo(_fluid.Enthalpy - EnthalpyDelta, PressureDrop)
            .Should()
            .Be(
                _fluid.WithState(
                    Input.Pressure(_fluid.Pressure - PressureDrop),
                    Input.Enthalpy(_fluid.Enthalpy - EnthalpyDelta)
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
            _ = _fluid.HeatingTo(
                _fluid.Temperature
                    - TemperatureDelta.FromKelvins(temperatureDelta),
                pressureDrop.Kilopascals()
            );
        action.Should().Throw<ArgumentException>().WithMessage($"*{message}*");
    }

    [Fact]
    public void HeatingTo_TemperatureWithPressureDrop_ReturnsFluidAtTemperatureAndLowerPressure() =>
        _fluid
            .HeatingTo(_fluid.Temperature + TemperatureDelta, PressureDrop)
            .Should()
            .Be(
                _fluid.WithState(
                    Input.Pressure(_fluid.Pressure - PressureDrop),
                    Input.Temperature(_fluid.Temperature + TemperatureDelta)
                )
            );

    [Theory]
    [InlineData(
        5,
        0,
        "During the heating process, the enthalpy should increase!"
    )]
    [InlineData(-5, -10, "Invalid pressure drop in the heat exchanger!")]
    public void HeatingTo_WrongEnthalpyOrPressureDrop_ThrowsArgumentException(
        double enthalpyDelta,
        double pressureDrop,
        string message
    )
    {
        Action action = () =>
            _ = _fluid.HeatingTo(
                _fluid.Enthalpy - enthalpyDelta.KilojoulesPerKilogram(),
                pressureDrop.Kilopascals()
            );
        action.Should().Throw<ArgumentException>().WithMessage($"*{message}*");
    }

    [Fact]
    public void HeatingTo_EnthalpyWithPressureDrop_ReturnsFluidAtEnthalpyAndLowerPressure() =>
        _fluid
            .HeatingTo(_fluid.Enthalpy + EnthalpyDelta, PressureDrop)
            .Should()
            .Be(
                _fluid.WithState(
                    Input.Pressure(_fluid.Pressure - PressureDrop),
                    Input.Enthalpy(_fluid.Enthalpy + EnthalpyDelta)
                )
            );

    [Fact]
    public void BubblePointAt_Pressure_ReturnsSaturatedLiquidAtPressure() =>
        _fluid
            .BubblePointAt(1.Atmospheres())
            .Should()
            .Be(
                _fluid.WithState(
                    Input.Pressure(1.Atmospheres()),
                    Input.Quality(0.Percent())
                )
            );

    [Fact]
    public void BubblePointAt_Temperature_ReturnsSaturatedLiquidAtTemperature() =>
        _fluid
            .BubblePointAt(100.DegreesCelsius())
            .Should()
            .Be(
                _fluid.WithState(
                    Input.Temperature(100.DegreesCelsius()),
                    Input.Quality(0.Percent())
                )
            );

    [Fact]
    public void DewPointAt_Pressure_ReturnsSaturatedVaporAtPressure() =>
        _fluid
            .DewPointAt(1.Atmospheres())
            .Should()
            .Be(
                _fluid.WithState(
                    Input.Pressure(1.Atmospheres()),
                    Input.Quality(100.Percent())
                )
            );

    [Fact]
    public void DewPointAt_Temperature_ReturnsSaturatedVaporAtTemperature() =>
        _fluid
            .DewPointAt(100.DegreesCelsius())
            .Should()
            .Be(
                _fluid.WithState(
                    Input.Temperature(100.DegreesCelsius()),
                    Input.Quality(100.Percent())
                )
            );

    [Fact]
    public void TwoPhasePointAt_PressureAndQuality_ReturnsFluidAtPressureAndQuality() =>
        _fluid
            .TwoPhasePointAt(1.Atmospheres(), 50.Percent())
            .Should()
            .Be(
                _fluid.WithState(
                    Input.Pressure(1.Atmospheres()),
                    Input.Quality(50.Percent())
                )
            );

    [Fact]
    public void Mixing_WrongFluids_ThrowsArgumentException()
    {
        var first = new Fluid(FluidsList.Ammonia).DewPointAt(1.Atmospheres());
        var second = _fluid.HeatingTo(_fluid.Temperature + TemperatureDelta);
        Action action = () =>
            _ = _fluid.Mixing(100.Percent(), first, 200.Percent(), second);
        action
            .Should()
            .Throw<ArgumentException>()
            .WithMessage(
                "The mixing process is possible only for the same fluids!"
            );
    }

    [Fact]
    public void Mixing_WrongPressures_ThrowsArgumentException()
    {
        var first = _fluid.Clone();
        var second = _fluid.IsentropicCompressionTo(HighPressure);
        Action action = () =>
            _ = _fluid.Mixing(100.Percent(), first, 200.Percent(), second);
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
        var first = _fluid.CoolingTo(_fluid.Temperature - TemperatureDelta);
        var second = _fluid.HeatingTo(_fluid.Temperature + TemperatureDelta);
        _fluid
            .Mixing(100.Percent(), first, 200.Percent(), second)
            .Should()
            .Be(
                _fluid.WithState(
                    Input.Pressure(_fluid.Pressure),
                    Input.Enthalpy(
                        (1 * first.Enthalpy + 2 * second.Enthalpy) / 3.0
                    )
                )
            );
    }
}
