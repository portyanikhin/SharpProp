using FluentAssertions;
using UnitsNet;
using UnitsNet.NumberExtensions.NumberToRatio;
using Xunit;

namespace SharpProp.Tests
{
    /// <summary>
    ///     An example of how to extend <see cref="InputHumidAir" />.
    /// </summary>
    public record InputHumidAirExtended : InputHumidAir
    {
        private InputHumidAirExtended(string coolPropKey, double value) :
            base(coolPropKey, value)
        {
        }

        /// <summary>
        ///     Water mole fraction.
        /// </summary>
        /// <param name="value">The value of the input.</param>
        /// <returns>Water mole fraction for the input.</returns>
        public static InputHumidAirExtended WaterMoleFraction(Ratio value) =>
            new("psi_w", value.DecimalFractions);
    }

    public class InputHumidAirExtendedTests
    {
        public InputHumidAirExtendedTests() =>
            Input = InputHumidAirExtended.WaterMoleFraction(5.PartsPerThousand());

        private InputHumidAirExtended Input { get; }

        [Fact]
        public void CoolPropKey_NewInput_MatchesWithCoolProp() =>
            Input.CoolPropKey.Should().Be("psi_w");

        [Fact]
        public void Value_NewInput_ShouldBeInSIUnits() =>
            Input.Value.Should().Be(5e-3);
    }
}