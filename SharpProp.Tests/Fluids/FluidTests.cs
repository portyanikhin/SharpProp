using UnitsNet.NumberExtensions.NumberToRatio;

namespace SharpProp.Tests;

[Collection("Fluids")]
public class FluidTests : IDisposable
{
    public FluidTests() =>
        Fluid = new Fluid(FluidsList.Water);

    private Fluid Fluid { get; set; }

    public void Dispose()
    {
        Fluid.Dispose();
        GC.SuppressFinalize(this);
    }

    [Theory]
    [InlineData(null, "Need to define the fraction!")]
    [InlineData(-200.0, "Invalid fraction value! It should be in [0;60] %. Entered value = -200 %.")]
    [InlineData(200.0, "Invalid fraction value! It should be in [0;60] %. Entered value = 200 %.")]
    public static void Fluid_InvalidFraction_ThrowsArgumentException(double? fraction, string message)
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
            tasks.Add(Task.Run(() => Fluid.DewPointAt(1.Atmospheres()).Temperature));
        var result = await Task.WhenAll(tasks);
        result.Distinct().Count().Should().Be(1);
    }

    [Fact]
    public void Factory_Always_NameIsConstant() =>
        Fluid.Factory().Name.Should().Be(Fluid.Name);

    [Fact]
    public void Factory_Always_FractionIsConstant() =>
        Fluid.Factory().Fraction.Should().Be(Fluid.Fraction);

    [Fact]
    public void Factory_Always_PhaseIsUnknown() =>
        Fluid.Factory().Phase.Should().Be(Phases.Unknown);

    [Fact]
    public void WithState_WaterInStandardConditions_PhaseIsLiquid() =>
        Fluid.WithState(Input.Pressure(1.Atmospheres()),
                Input.Temperature(20.DegreesCelsius()))
            .Phase.Should().Be(Phases.Liquid);

    [Fact]
    public void Update_SameInputs_ThrowsArgumentException()
    {
        var action = () =>
            Fluid.Update(Input.Pressure(1.Atmospheres()),
                Input.Pressure(101325.Pascals()));
        action.Should().Throw<ArgumentException>()
            .WithMessage("Need to define 2 unique inputs!");
    }

    [Fact]
    public void Update_Always_InputsAreCached()
    {
        Fluid.Update(Input.Pressure(101325.Pascals()),
            Input.Temperature(293.15.Kelvins()));
        Fluid.Pressure.Pascals.Should().Be(101325);
        Fluid.Temperature.Kelvins.Should().Be(293.15);
    }

    [Theory]
    [MemberData(nameof(FluidNames))]
    public void Update_VariousFluids_MatchesWithCoolProp(FluidsList name)
    {
        SetUpFluid(name);
        var actual = new List<double?>
        {
            Fluid.Compressibility,
            Fluid.Conductivity?.WattsPerMeterKelvin,
            Fluid.CriticalPressure?.Pascals,
            Fluid.CriticalTemperature?.Kelvins,
            Fluid.Density.KilogramsPerCubicMeter,
            Fluid.DynamicViscosity?.PascalSeconds,
            Fluid.Enthalpy.JoulesPerKilogram,
            Fluid.Entropy.JoulesPerKilogramKelvin,
            Fluid.FreezingTemperature?.Kelvins,
            Fluid.InternalEnergy.JoulesPerKilogram,
            Fluid.MaxPressure?.Pascals,
            Fluid.MaxTemperature.Kelvins,
            Fluid.MinPressure?.Pascals,
            Fluid.MinTemperature.Kelvins,
            Fluid.MolarMass?.KilogramsPerMole,
            Fluid.Prandtl,
            Fluid.Pressure.Pascals,
            Fluid.Quality?.DecimalFractions,
            Fluid.SoundSpeed?.MetersPerSecond,
            Fluid.SpecificHeat.JoulesPerKilogramKelvin,
            Fluid.SurfaceTension?.NewtonsPerMeter,
            Fluid.Temperature.Kelvins,
            Fluid.TriplePressure?.Pascals,
            Fluid.TripleTemperature?.Kelvins
        };
        var expected = new List<string>
        {
            "Z", "L", "p_critical", "T_critical", "D", "V", "H", "S", "T_freeze", "U", "P_max", "T_max",
            "P_min", "T_min", "M", "Prandtl", "P", "Q", "A", "C", "I", "T", "p_triple", "T_triple"
        }.Select(CoolPropInterface).ToList();
        for (var i = 0; i < actual.Count; i++)
            actual[i].Should().BeApproximately(expected[i], 1e-9);
        Fluid.KinematicViscosity.Should().Be(Fluid.DynamicViscosity / Fluid.Density);
    }

    [Fact]
    public void Equals_Same_ReturnsTrue()
    {
        var origin = Fluid.WithState(Input.Pressure(1.Atmospheres()),
            Input.Temperature(20.DegreesCelsius()));
        var same = Fluid.WithState(Input.Pressure(101325.Pascals()),
            Input.Temperature(293.15.Kelvins()));
        origin.Should().Be(origin);
        origin.Should().BeSameAs(origin);
        origin.Should().Be(same);
        origin.Should().NotBeSameAs(same);
        (origin == same).Should().Be(origin.Equals(same));
    }

    [Fact]
    public void Equals_Other_ReturnsFalse()
    {
        var origin = Fluid.WithState(Input.Pressure(1.Atmospheres()),
            Input.Temperature(20.DegreesCelsius()));
        var other = Fluid.WithState(Input.Pressure(1.Atmospheres()),
            Input.Temperature(30.DegreesCelsius()));
        origin.Should().NotBe(other);
        origin.Should().NotBeNull();
        origin.Equals(new object()).Should().BeFalse();
        (origin != other).Should().Be(!origin.Equals(other));
    }

    [Fact]
    public void GetHashCode_Same_ReturnsSameHashCode()
    {
        var origin = Fluid.WithState(Input.Pressure(1.Atmospheres()),
            Input.Temperature(20.DegreesCelsius()));
        var same = Fluid.WithState(Input.Pressure(101325.Pascals()),
            Input.Temperature(293.15.Kelvins()));
        origin.GetHashCode().Should().Be(same.GetHashCode());
    }

    [Fact]
    public void GetHashCode_Other_ReturnsOtherHashCode()
    {
        var origin = Fluid.WithState(Input.Pressure(1.Atmospheres()),
            Input.Temperature(20.DegreesCelsius()));
        var other = Fluid.WithState(Input.Pressure(1.Atmospheres()),
            Input.Temperature(30.DegreesCelsius()));
        origin.GetHashCode().Should().NotBe(other.GetHashCode());
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public void AsJson_IndentedOrNot_ReturnsProperlyFormattedJson(bool indented)
    {
        var fluid = Fluid.WithState(Input.Pressure(1.Atmospheres()),
            Input.Temperature(20.DegreesCelsius()));
        fluid.AsJson(indented).Should().Be(
            JsonConvert.SerializeObject(fluid, new JsonSerializerSettings
            {
                Converters = new List<JsonConverter>
                    {new StringEnumConverter(), new UnitsNetIQuantityJsonConverter()},
                Formatting = indented ? Formatting.Indented : Formatting.None
            }));
    }

    [Fact]
    public void Clone_Always_ReturnsNewInstanceWithSameState()
    {
        var origin = Fluid.WithState(Input.Pressure(1.Atmospheres()),
            Input.Temperature(20.DegreesCelsius()));
        var clone = origin.Clone();
        clone.Should().Be(origin);
        clone.Update(Input.Pressure(1.Atmospheres()),
            Input.Temperature(30.DegreesCelsius()));
        clone.Should().NotBe(origin);
    }

    public static IEnumerable<object[]> FluidNames() =>
        Enum.GetValues(typeof(FluidsList)).Cast<FluidsList>()
            .Where(name => !(name is FluidsList.AL or FluidsList.AN || name.CoolPropName().EndsWith(".mix")))
            .Cast<object>().Select(name => new[] {name});

    private void SetUpFluid(FluidsList name)
    {
        var fraction = name.Pure()
            ? null as Ratio?
            : Math.Round(0.5 * (name.FractionMin() + name.FractionMax()).Percent)
                .Percent();
        Fluid = new Fluid(name, fraction);
        Fluid.Update(Input.Pressure(Fluid.MaxPressure ?? 10.Megapascals()),
            Input.Temperature(Fluid.MaxTemperature));
    }

    private double? CoolPropInterface(string outputKey)
    {
        switch (outputKey)
        {
            case "P":
                return Fluid.Pressure.Pascals;
            case "T":
                return Fluid.Temperature.Kelvins;
            default:
                try
                {
                    var value = CoolProp.PropsSI(outputKey,
                        "P", Fluid.Pressure.Pascals, "T", Fluid.Temperature.Kelvins,
                        $"{Fluid.Name.CoolPropBackend()}::{Fluid.Name.CoolPropName()}"
                        + (Fluid.Name.Pure() ? string.Empty : $"-{Fluid.Fraction.Percent}%"));
                    return CheckedValue(value, outputKey);
                }
                catch (ApplicationException)
                {
                    return null;
                }
        }
    }

    private static double? CheckedValue(double value, string outputKey) =>
        double.IsInfinity(value) ||
        double.IsNaN(value) ||
        (outputKey == "Q" && value is < 0 or > 1)
            ? null
            : value;
}