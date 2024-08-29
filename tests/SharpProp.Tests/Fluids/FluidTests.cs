using UnitsNet.NumberExtensions.NumberToRatio;

namespace SharpProp.Tests;

[Collection("Fluids")]
public class FluidTests : IDisposable
{
    private const double Tolerance = 1e-9;
    private IFluid _fluid = new Fluid(FluidsList.Water);

    public void Dispose()
    {
        _fluid.Dispose();
        GC.SuppressFinalize(this);
    }

    [Theory]
    [InlineData(null, "Need to define the fraction!")]
    [InlineData(
        -200.0,
        "Invalid fraction value! It should be in [0;60] %. Entered value = -200 %."
    )]
    [InlineData(
        200.0,
        "Invalid fraction value! It should be in [0;60] %. Entered value = 200 %."
    )]
    public static void Fluid_InvalidFraction_ThrowsArgumentException(
        double? fraction,
        string message
    )
    {
        Action action = () =>
            _ = new Fluid(FluidsList.MPG, fraction?.Percent());
        action.Should().Throw<ArgumentException>().WithMessage(message);
    }

    [Fact]
    public async Task Fluid_MultiThreading_IsThreadSafe()
    {
        var tasks = new List<Task<Temperature>>();
        for (var i = 0; i < 100; i++)
        {
            tasks.Add(
                Task.Run(() => _fluid.DewPointAt(1.Atmospheres()).Temperature)
            );
        }

        var result = await Task.WhenAll(tasks);
        result.Distinct().Count().Should().Be(1);
    }

    [Fact]
    public void Update_SameInputs_ThrowsArgumentException()
    {
        IAbstractFluid fluid = _fluid;
        var action = () =>
            fluid.Update(
                Input.Pressure(1.Atmospheres()),
                Input.Pressure(101325.Pascals())
            );
        action
            .Should()
            .Throw<ArgumentException>()
            .WithMessage("Need to define 2 unique inputs!");
    }

    [Fact]
    public void Update_Always_InputsAreCached()
    {
        IAbstractFluid fluid = _fluid;
        fluid.Update(
            Input.Pressure(101325.Pascals()),
            Input.Temperature(293.15.Kelvins())
        );
        fluid.Pressure.Pascals.Should().Be(101325);
        fluid.Temperature.Kelvins.Should().Be(293.15);
    }

    [Theory]
    [MemberData(nameof(FluidNames))]
    public void Update_VariousFluids_MatchesWithCoolProp(FluidsList name)
    {
        SetUpFluid(name);
        IAbstractFluid fluid = _fluid;
        var actual = new[]
        {
            fluid.Compressibility,
            fluid.Conductivity?.WattsPerMeterKelvin,
            fluid.CriticalPressure?.Pascals,
            fluid.CriticalTemperature?.Kelvins,
            fluid.Density.KilogramsPerCubicMeter,
            fluid.DynamicViscosity?.PascalSeconds,
            fluid.Enthalpy.JoulesPerKilogram,
            fluid.Entropy.JoulesPerKilogramKelvin,
            fluid.FreezingTemperature?.Kelvins,
            fluid.InternalEnergy.JoulesPerKilogram,
            fluid.MaxPressure?.Pascals,
            fluid.MaxTemperature.Kelvins,
            fluid.MinPressure?.Pascals,
            fluid.MinTemperature.Kelvins,
            fluid.MolarMass?.KilogramsPerMole,
            fluid.Prandtl,
            fluid.Pressure.Pascals,
            fluid.Quality?.DecimalFractions,
            fluid.SoundSpeed?.MetersPerSecond,
            fluid.SpecificHeat.JoulesPerKilogramKelvin,
            fluid.SurfaceTension?.NewtonsPerMeter,
            fluid.Temperature.Kelvins,
            fluid.TriplePressure?.Pascals,
            fluid.TripleTemperature?.Kelvins,
        };
        var expected = new[]
        {
            "Z",
            "L",
            "p_critical",
            "T_critical",
            "D",
            "V",
            "H",
            "S",
            "T_freeze",
            "U",
            "P_max",
            "T_max",
            "P_min",
            "T_min",
            "M",
            "Prandtl",
            "P",
            "Q",
            "A",
            "C",
            "I",
            "T",
            "p_triple",
            "T_triple",
        }
            .Select(CoolPropInterface)
            .ToList();
        for (var i = 0; i < actual.Length; i++)
        {
            actual[i].Should().BeApproximately(expected[i], Tolerance);
        }

        fluid
            .KinematicViscosity?.Equals(
                (fluid.DynamicViscosity / fluid.Density)!.Value,
                Tolerance.Centistokes()
            )
            .Should()
            .BeTrue();
        fluid
            .SpecificVolume.Equals(
                SpecificVolume.FromCubicMetersPerKilogram(
                    1.0 / fluid.Density.KilogramsPerCubicMeter
                ),
                Tolerance.CubicMetersPerKilogram()
            )
            .Should()
            .BeTrue();
    }

    [Fact]
    public void Reset_Always_ResetsAllNonTrivialProperties()
    {
        IAbstractFluid fluid = _fluid;
        fluid.Update(
            Input.Pressure(1.Atmospheres()),
            Input.Temperature(20.DegreesCelsius())
        );
        Action action = () => _ = fluid.Pressure;
        action.Should().NotThrow();
        fluid.Reset();
        action
            .Should()
            .Throw<ArgumentException>()
            .WithMessage("Invalid or not defined state!");
    }

    [Fact]
    public void SpecifyPhase_Always_SpecifiesPhaseForAllFurtherCalculations()
    {
        IAbstractFluid fluid = _fluid;
        fluid.SpecifyPhase(Phases.Gas);
        var action = () =>
            fluid.Update(
                Input.Pressure(1.Atmospheres()),
                Input.Temperature(20.DegreesCelsius())
            );
        action.Should().Throw<ApplicationException>();
        fluid.UnspecifyPhase();
        action.Should().NotThrow();
    }

    [Fact]
    public void WithState_WaterInStandardConditions_PhaseIsLiquid() =>
        _fluid
            .WithState(
                Input.Pressure(1.Atmospheres()),
                Input.Temperature(20.DegreesCelsius())
            )
            .Phase.Should()
            .Be(Phases.Liquid);

    [Fact]
    public void Clone_Always_ReturnsNewInstanceWithSameState()
    {
        IClonable<IFluid> origin = _fluid.WithState(
            Input.Pressure(1.Atmospheres()),
            Input.Temperature(20.DegreesCelsius())
        );
        var clone = origin.Clone();
        clone.Should().Be(origin);
        clone.Update(
            Input.Pressure(1.Atmospheres()),
            Input.Temperature(30.DegreesCelsius())
        );
        clone.Should().NotBe(origin);
    }

    [Fact]
    public void Factory_Always_NameIsConstant()
    {
        IFactory<IFluid> fluid = _fluid;
        fluid.Factory().Name.Should().Be(_fluid.Name);
    }

    [Fact]
    public void Factory_Always_FractionIsConstant()
    {
        IFactory<IFluid> fluid = _fluid;
        fluid.Factory().Fraction.Should().Be(_fluid.Fraction);
    }

    [Fact]
    public void Factory_Always_PhaseIsUnknown()
    {
        IFactory<IFluid> fluid = _fluid;
        fluid.Factory().Phase.Should().Be(Phases.Unknown);
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public void AsJson_IndentedOrNot_ReturnsProperlyFormattedJson(bool indented)
    {
        IJsonable fluid = _fluid.WithState(
            Input.Pressure(1.Atmospheres()),
            Input.Temperature(20.DegreesCelsius())
        );
        fluid
            .AsJson(indented)
            .Should()
            .Be(
                JsonConvert.SerializeObject(
                    fluid,
                    new JsonSerializerSettings
                    {
                        Converters = new List<JsonConverter>
                        {
                            new StringEnumConverter(),
                            new UnitsNetIQuantityJsonConverter(),
                        },
                        Formatting = indented
                            ? Formatting.Indented
                            : Formatting.None,
                    }
                )
            );
    }

    [Fact]
    public void Equals_Same_ReturnsTrue()
    {
        var origin = _fluid.WithState(
            Input.Pressure(1.Atmospheres()),
            Input.Temperature(20.DegreesCelsius())
        );
        var same = _fluid.WithState(
            Input.Pressure(101325.Pascals()),
            Input.Temperature(293.15.Kelvins())
        );
        origin.Should().Be(origin);
        origin.Should().BeSameAs(origin);
        origin.Should().Be(same);
        origin.Should().NotBeSameAs(same);
    }

    [Fact]
    public void Equals_Other_ReturnsFalse()
    {
        var origin = _fluid.WithState(
            Input.Pressure(1.Atmospheres()),
            Input.Temperature(20.DegreesCelsius())
        );
        var other = _fluid.WithState(
            Input.Pressure(1.Atmospheres()),
            Input.Temperature(30.DegreesCelsius())
        );
        origin.Should().NotBe(other);
        origin.Should().NotBeNull();
        origin.Equals(new object()).Should().BeFalse();
    }

    [Fact]
    public void GetHashCode_Same_ReturnsSameHashCode()
    {
        var origin = _fluid.WithState(
            Input.Pressure(1.Atmospheres()),
            Input.Temperature(20.DegreesCelsius())
        );
        var same = _fluid.WithState(
            Input.Pressure(101325.Pascals()),
            Input.Temperature(293.15.Kelvins())
        );
        origin.GetHashCode().Should().Be(same.GetHashCode());
    }

    [Fact]
    public void GetHashCode_Other_ReturnsOtherHashCode()
    {
        var origin = _fluid.WithState(
            Input.Pressure(1.Atmospheres()),
            Input.Temperature(20.DegreesCelsius())
        );
        var other = _fluid.WithState(
            Input.Pressure(1.Atmospheres()),
            Input.Temperature(30.DegreesCelsius())
        );
        origin.GetHashCode().Should().NotBe(other.GetHashCode());
    }

    public static IEnumerable<object[]> FluidNames() =>
        Enum.GetValues(typeof(FluidsList))
            .Cast<FluidsList>()
            .Where(name =>
                !(
                    name is FluidsList.AL or FluidsList.AN
                    || name.CoolPropName().EndsWith(".mix")
                )
            )
            .Cast<object>()
            .Select(name => new[] { name });

    private void SetUpFluid(FluidsList name)
    {
        var fraction = name.Pure()
            ? null as Ratio?
            : Math.Round(
                    0.5 * (name.FractionMin() + name.FractionMax()).Percent
                )
                .Percent();
        _fluid = new Fluid(name, fraction);
        _fluid.Update(
            Input.Pressure(_fluid.MaxPressure ?? 10.Megapascals()),
            Input.Temperature(_fluid.MaxTemperature)
        );
    }

    private double? CoolPropInterface(string outputKey)
    {
        switch (outputKey)
        {
            case "P":
                return _fluid.Pressure.Pascals;
            case "T":
                return _fluid.Temperature.Kelvins;
            default:
                try
                {
                    var value = CoolProp.PropsSI(
                        outputKey,
                        "P",
                        _fluid.Pressure.Pascals,
                        "T",
                        _fluid.Temperature.Kelvins,
                        $"{_fluid.Name.CoolPropBackend()}::"
                            + $"{_fluid.Name.CoolPropName()}"
                            + (
                                _fluid.Name.Pure()
                                    ? string.Empty
                                    : $"-{_fluid.Fraction.Percent}%"
                            )
                    );
                    return CheckedValue(value, outputKey);
                }
                catch (ApplicationException)
                {
                    return null;
                }
        }
    }

    private static double? CheckedValue(double value, string outputKey) =>
        double.IsInfinity(value)
        || double.IsNaN(value)
        || outputKey == "Q" && value is < 0 or > 1
            ? null
            : value;
}
