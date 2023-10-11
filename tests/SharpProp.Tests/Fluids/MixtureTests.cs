using UnitsNet.NumberExtensions.NumberToRatio;

namespace SharpProp.Tests;

[Collection("Fluids")]
public class MixtureTests : IDisposable
{
    private readonly IMixture _mixture = new Mixture(
        new List<FluidsList> { FluidsList.Water, FluidsList.Ethanol },
        new List<Ratio> { 60.Percent(), 40.Percent() }
    );

    public void Dispose()
    {
        _mixture.Dispose();
        GC.SuppressFinalize(this);
    }

    [Theory]
    [MemberData(nameof(WrongFluidsOrFractions))]
    public static void Mixture_WrongFluidsOrFractions_ThrowsArgumentException(
        List<FluidsList> fluids,
        List<Ratio> fractions,
        string message
    )
    {
        Action action = () => _ = new Mixture(fluids, fractions);
        action.Should().Throw<ArgumentException>().WithMessage(message);
    }

    [Fact]
    public void Update_SameInputs_ThrowsArgumentException()
    {
        IAbstractFluid mixture = _mixture;
        var action = () =>
            mixture.Update(
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
        IAbstractFluid mixture = _mixture;
        mixture.Update(
            Input.Pressure(101325.Pascals()),
            Input.Temperature(293.15.Kelvins())
        );
        mixture.Pressure.Pascals.Should().Be(101325);
        mixture.Temperature.Kelvins.Should().Be(293.15);
    }

    [Fact]
    public void WithState_VodkaInStandardConditions_PhaseIsLiquid() =>
        _mixture
            .WithState(
                Input.Pressure(1.Atmospheres()),
                Input.Temperature(20.DegreesCelsius())
            )
            .Phase.Should()
            .Be(Phases.Liquid);

    [Fact]
    public void Clone_Always_ReturnsNewInstanceWithSameState()
    {
        IClonable<IMixture> origin = _mixture.WithState(
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
    public void Factory_Always_FluidsAreConstant()
    {
        IFactory<IMixture> mixture = _mixture;
        mixture.Factory().Fluids.Should().BeEquivalentTo(_mixture.Fluids);
    }

    [Fact]
    public void Factory_Always_FractionsAreConstant()
    {
        IFactory<IMixture> mixture = _mixture;
        mixture.Factory().Fractions.Should().BeEquivalentTo(_mixture.Fractions);
    }

    [Fact]
    public void Factory_Always_PhaseIsUnknown()
    {
        IFactory<IMixture> mixture = _mixture;
        mixture.Factory().Phase.Should().Be(Phases.Unknown);
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public void AsJson_IndentedOrNot_ReturnsProperlyFormattedJson(bool indented)
    {
        IJsonable fluid = _mixture.WithState(
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
                            new UnitsNetIQuantityJsonConverter()
                        },
                        Formatting = indented
                            ? Formatting.Indented
                            : Formatting.None
                    }
                )
            );
    }

    [Fact]
    public void Equals_Same_ReturnsTrue()
    {
        var origin = _mixture.WithState(
            Input.Pressure(1.Atmospheres()),
            Input.Temperature(20.DegreesCelsius())
        );
        var same = _mixture.WithState(
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
        var origin = _mixture.WithState(
            Input.Pressure(1.Atmospheres()),
            Input.Temperature(20.DegreesCelsius())
        );
        var other = _mixture.WithState(
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
        var origin = _mixture.WithState(
            Input.Pressure(1.Atmospheres()),
            Input.Temperature(20.DegreesCelsius())
        );
        var same = _mixture.WithState(
            Input.Pressure(101325.Pascals()),
            Input.Temperature(293.15.Kelvins())
        );
        origin.GetHashCode().Should().Be(same.GetHashCode());
    }

    [Fact]
    public void GetHashCode_Other_ReturnsOtherHashCode()
    {
        var origin = _mixture.WithState(
            Input.Pressure(1.Atmospheres()),
            Input.Temperature(20.DegreesCelsius())
        );
        var other = _mixture.WithState(
            Input.Pressure(1.Atmospheres()),
            Input.Temperature(30.DegreesCelsius())
        );
        origin.GetHashCode().Should().NotBe(other.GetHashCode());
    }

    public static IEnumerable<object[]> WrongFluidsOrFractions() =>
        new[]
        {
            new object[]
            {
                new List<FluidsList> { FluidsList.Water },
                new List<Ratio> { 60.Percent(), 40.Percent() },
                "Invalid input! Fluids and Fractions "
                + "should be of the same length."
            },
            new object[]
            {
                new List<FluidsList> { FluidsList.MPG, FluidsList.MEG },
                new List<Ratio> { 60.Percent(), 40.Percent() },
                "Invalid components! All of them "
                + "should be a pure fluid with HEOS backend."
            },
            new object[]
            {
                new List<FluidsList> { FluidsList.Water, FluidsList.Ethanol },
                new List<Ratio> { -200.Percent(), 40.Percent() },
                "Invalid component mass fractions! "
                + "All of them should be in (0;100) %."
            },
            new object[]
            {
                new List<FluidsList> { FluidsList.Water, FluidsList.Ethanol },
                new List<Ratio> { 60.Percent(), 200.Percent() },
                "Invalid component mass fractions! "
                + "All of them should be in (0;100) %."
            },
            new object[]
            {
                new List<FluidsList> { FluidsList.Water, FluidsList.Ethanol },
                new List<Ratio> { 80.Percent(), 80.Percent() },
                "Invalid component mass fractions! "
                + "Their sum should be equal to 100 %."
            }
        };
}