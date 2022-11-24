using CoolProp;
using FluentAssertions;
using Xunit;

namespace SharpProp.Tests
{
    public static class PhasesTests
    {
        [Theory]
        [InlineData(Phases.Liquid, phases.iphase_liquid)]
        [InlineData(Phases.Supercritical, phases.iphase_supercritical)]
        [InlineData(Phases.SupercriticalGas, phases.iphase_supercritical_gas)]
        [InlineData(Phases.SupercriticalLiquid, phases.iphase_supercritical_liquid)]
        [InlineData(Phases.CriticalPoint, phases.iphase_critical_point)]
        [InlineData(Phases.Gas, phases.iphase_gas)]
        [InlineData(Phases.TwoPhase, phases.iphase_twophase)]
        [InlineData(Phases.Unknown, phases.iphase_unknown)]
        [InlineData(Phases.NotImposed, phases.iphase_not_imposed)]
        public static void Phases_AllValues_AreEquivalentToCoolPropPhases(
            Phases sharpPropPhase, phases coolPropPhase) =>
            sharpPropPhase.Should().HaveSameValueAs(coolPropPhase);
    }
}