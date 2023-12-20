using UnitsNet.NumberExtensions.NumberToRelativeHumidity;

namespace SharpProp.Tests;

[Collection("Fluids")]
public class HumidAirTests
{
    private const double Tolerance = 1e-9;
    private readonly IHumidAir _humidAir = new HumidAir();

    [Fact]
    public async Task HumidAir_MultiThreading_IsThreadSafe()
    {
        var tasks = new List<Task<Temperature>>();
        for (var i = 0; i < 100; i++)
        {
            tasks.Add(
                Task.Run(
                    () =>
                        _humidAir
                            .WithState(
                                InputHumidAir.Pressure(1.Atmospheres()),
                                InputHumidAir.Temperature(20.DegreesCelsius()),
                                InputHumidAir.RelativeHumidity(50.Percent())
                            )
                            .DewTemperature
                )
            );
        }

        var result = await Task.WhenAll(tasks);
        result.Distinct().Count().Should().Be(1);
    }

    [Fact]
    public void Update_SameInputs_ThrowsArgumentException()
    {
        var action = () =>
            _humidAir.Update(
                InputHumidAir.Pressure(1.Atmospheres()),
                InputHumidAir.Temperature(20.DegreesCelsius()),
                InputHumidAir.Temperature(30.DegreesCelsius())
            );
        action
            .Should()
            .Throw<ArgumentException>()
            .WithMessage("Need to define 3 unique inputs!");
    }

    [Fact]
    public void Update_Always_InputsAreCached()
    {
        _humidAir.Update(
            InputHumidAir.Pressure(101325.Pascals()),
            InputHumidAir.Temperature(293.15.Kelvins()),
            InputHumidAir.RelativeHumidity(50.Percent())
        );
        _humidAir.Pressure.Pascals.Should().Be(101325);
        _humidAir.Temperature.Kelvins.Should().Be(293.15);
        _humidAir.RelativeHumidity.Percent.Should().Be(50);
    }

    [Theory]
    [CombinatorialData]
    public void Update_VariousConditions_MatchesWithCoolProp(
        [CombinatorialValues(1, 2, 5)] double pressure,
        [CombinatorialRange(-20, 50, 10)] double temperature,
        [CombinatorialRange(0, 100, 10)] double relativeHumidity
    )
    {
        _humidAir.Update(
            InputHumidAir.Pressure(pressure.Bars()),
            InputHumidAir.Temperature(temperature.DegreesCelsius()),
            InputHumidAir.RelativeHumidity(relativeHumidity.Percent())
        );
        var actual = new[]
        {
            _humidAir.Compressibility,
            _humidAir.Conductivity.WattsPerMeterKelvin,
            _humidAir.DewTemperature.Kelvins,
            _humidAir.DynamicViscosity.PascalSeconds,
            _humidAir.Enthalpy.JoulesPerKilogram,
            _humidAir.Entropy.JoulesPerKilogramKelvin,
            _humidAir.Humidity.DecimalFractions,
            _humidAir.PartialPressure.Pascals,
            _humidAir.Pressure.Pascals,
            Ratio
                .FromPercent(_humidAir.RelativeHumidity.Percent)
                .DecimalFractions,
            _humidAir.SpecificHeat.JoulesPerKilogramKelvin,
            _humidAir.SpecificVolume.CubicMetersPerKilogram,
            _humidAir.Temperature.Kelvins,
            _humidAir.WetBulbTemperature.Kelvins
        };
        var expected = new[]
        {
            "Z",
            "K",
            "D",
            "M",
            "Hha",
            "Sha",
            "W",
            "P_w",
            "P",
            "R",
            "Cha",
            "Vha",
            "T",
            "B"
        }
            .Select(CoolPropInterface)
            .ToList();
        for (var i = 0; i < actual.Length; i++)
        {
            actual[i].Should().BeApproximately(expected[i], Tolerance);
        }

        _humidAir
            .Density.Equals(
                Density.FromKilogramsPerCubicMeter(
                    1.0 / _humidAir.SpecificVolume.CubicMetersPerKilogram
                ),
                Tolerance.KilogramsPerCubicMeter()
            )
            .Should()
            .BeTrue();
        _humidAir
            .KinematicViscosity.Equals(
                _humidAir.DynamicViscosity / _humidAir.Density,
                Tolerance.Centistokes()
            )
            .Should()
            .BeTrue();
        _humidAir
            .Prandtl.Should()
            .Be(
                _humidAir.DynamicViscosity.PascalSeconds
                    * _humidAir.SpecificHeat.JoulesPerKilogramKelvin
                    / _humidAir.Conductivity.WattsPerMeterKelvin
            );
    }

    [Fact]
    public void Reset_Always_Resets1AllProperties()
    {
        _humidAir.Update(
            InputHumidAir.Pressure(1.Atmospheres()),
            InputHumidAir.Temperature(20.DegreesCelsius()),
            InputHumidAir.RelativeHumidity(50.Percent())
        );
        Action action = () => _ = _humidAir.Pressure;
        action.Should().NotThrow();
        _humidAir.Reset();
        action
            .Should()
            .Throw<ArgumentException>()
            .WithMessage("Need to define 3 unique inputs!");
    }

    [Fact]
    public void WithState_InvalidState_ThrowsArgumentException()
    {
        Action action = () =>
            _ = _humidAir
                .WithState(
                    InputHumidAir.Pressure(1.Atmospheres()),
                    InputHumidAir.Temperature(20.DegreesCelsius()),
                    InputHumidAir.RelativeHumidity(200.Percent())
                )
                .Density;
        action
            .Should()
            .Throw<ArgumentException>()
            .WithMessage("Invalid or not defined state!");
    }

    [Fact]
    public void WithState_ValidState_ReturnsNewInstanceWithDefinedState() =>
        _humidAir
            .WithState(
                InputHumidAir.Pressure(1.Atmospheres()),
                InputHumidAir.Temperature(20.DegreesCelsius()),
                InputHumidAir.RelativeHumidity(50.Percent())
            )
            .Should()
            .NotBe(new HumidAir());

    [Fact]
    public void Clone_Always_ReturnsNewInstanceWithSameState()
    {
        IClonable<IHumidAir> origin = _humidAir.WithState(
            InputHumidAir.Altitude(0.Meters()),
            InputHumidAir.Temperature(20.DegreesCelsius()),
            InputHumidAir.RelativeHumidity(50.Percent())
        );
        var clone = origin.Clone();
        clone.Should().Be(origin);
        clone.Update(
            InputHumidAir.Altitude(0.Meters()),
            InputHumidAir.Temperature(20.DegreesCelsius()),
            InputHumidAir.RelativeHumidity(60.Percent())
        );
        clone.Should().NotBe(origin);
    }

    [Fact]
    public void Factory_Always_ReturnsNewInstanceWithNoDefinedState() =>
        _humidAir.Factory().Should().Be(new HumidAir());

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public void AsJson_IndentedOrNot_ReturnsProperlyFormattedJson(bool indented)
    {
        IJsonable humidAir = _humidAir.WithState(
            InputHumidAir.Altitude(0.Meters()),
            InputHumidAir.Temperature(20.DegreesCelsius()),
            InputHumidAir.RelativeHumidity(50.Percent())
        );
        humidAir
            .AsJson(indented)
            .Should()
            .Be(
                JsonConvert.SerializeObject(
                    humidAir,
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
        var origin = _humidAir.WithState(
            InputHumidAir.Altitude(0.Meters()),
            InputHumidAir.Temperature(20.DegreesCelsius()),
            InputHumidAir.RelativeHumidity(50.Percent())
        );
        var same = _humidAir.WithState(
            InputHumidAir.Pressure(1.Atmospheres()),
            InputHumidAir.Temperature(293.15.Kelvins()),
            InputHumidAir.RelativeHumidity(50.Percent())
        );
        origin.Should().Be(origin);
        origin.Should().BeSameAs(origin);
        origin.Should().Be(same);
        origin.Should().NotBeSameAs(same);
    }

    [Fact]
    public void Equals_Other_ReturnsFalse()
    {
        var origin = _humidAir.WithState(
            InputHumidAir.Altitude(0.Meters()),
            InputHumidAir.Temperature(20.DegreesCelsius()),
            InputHumidAir.RelativeHumidity(50.Percent())
        );
        var other = _humidAir.WithState(
            InputHumidAir.Altitude(0.Meters()),
            InputHumidAir.Temperature(20.DegreesCelsius()),
            InputHumidAir.RelativeHumidity(60.Percent())
        );
        origin.Should().NotBe(other);
        origin.Should().NotBeNull();
        origin.Equals(new object()).Should().BeFalse();
    }

    [Fact]
    public void GetHashCode_Same_ReturnsSameHashCode()
    {
        var origin = _humidAir.WithState(
            InputHumidAir.Altitude(0.Meters()),
            InputHumidAir.Temperature(20.DegreesCelsius()),
            InputHumidAir.RelativeHumidity(50.Percent())
        );
        var same = _humidAir.WithState(
            InputHumidAir.Pressure(1.Atmospheres()),
            InputHumidAir.Temperature(293.15.Kelvins()),
            InputHumidAir.RelativeHumidity(50.Percent())
        );
        origin.GetHashCode().Should().Be(same.GetHashCode());
    }

    [Fact]
    public void GetHashCode_Other_ReturnsOtherHashCode()
    {
        var origin = _humidAir.WithState(
            InputHumidAir.Altitude(0.Meters()),
            InputHumidAir.Temperature(20.DegreesCelsius()),
            InputHumidAir.RelativeHumidity(50.Percent())
        );
        var other = _humidAir.WithState(
            InputHumidAir.Altitude(0.Meters()),
            InputHumidAir.Temperature(20.DegreesCelsius()),
            InputHumidAir.RelativeHumidity(60.Percent())
        );
        origin.GetHashCode().Should().NotBe(other.GetHashCode());
    }

    private double CoolPropInterface(string key) =>
        CoolProp.HAPropsSI(
            key,
            "P",
            _humidAir.Pressure.Pascals,
            "T",
            _humidAir.Temperature.Kelvins,
            "R",
            Ratio
                .FromPercent(_humidAir.RelativeHumidity.Percent)
                .DecimalFractions
        );
}
