using CoolProp;
using FluentAssertions;
using UnitsNet;
using UnitsNet.NumberExtensions.NumberToMolarMass;
using Xunit;

namespace SharpProp.Tests
{
    /// <summary>
    ///     An example of how to extend the <see cref="Input" /> record.
    /// </summary>
    public record InputExtended : Input
    {
        private InputExtended(Parameters coolPropKey, double value) :
            base(coolPropKey, value)
        {
        }

        /// <summary>
        ///     Molar density.
        /// </summary>
        /// <param name="value">The value of the input.</param>
        /// <returns>Molar density for the input.</returns>
        public static InputExtended MolarDensity(MolarMass value) =>
            new(Parameters.iDmolar, value.KilogramsPerMole);
    }

    public class InputExtendedTests
    {
        public InputExtendedTests() =>
            Input = InputExtended.MolarDensity(9e5.GramsPerMole());

        private InputExtended Input { get; }

        [Fact]
        public void CoolPropKey_NewInput_MatchesWithCoolProp() =>
            Input.CoolPropKey.Should().Be(Parameters.iDmolar);

        [Fact]
        public void Value_NewInput_ShouldBeInSIUnits() =>
            Input.Value.Should().Be(900);
    }
}