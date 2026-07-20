namespace SharpProp.Tests;

public static class PhasesTests
{
    [Theory]
    [MemberData(nameof(CoolPropPhases))]
    public static void ToCoolPropEnum_AllPhases_MatchesWithCoolProp(
        Phases phase,
        phases coolPropPhase
    ) => phase.ToCoolPropEnum().Should().Be(coolPropPhase);

    public static TheoryData<Phases, phases> CoolPropPhases() =>
        new()
        {
            { Phases.Liquid, phases.iphase_liquid },
            { Phases.Supercritical, phases.iphase_supercritical },
            { Phases.SupercriticalGas, phases.iphase_supercritical_gas },
            { Phases.SupercriticalLiquid, phases.iphase_supercritical_liquid },
            { Phases.CriticalPoint, phases.iphase_critical_point },
            { Phases.Gas, phases.iphase_gas },
            { Phases.TwoPhase, phases.iphase_twophase },
            { Phases.Unknown, phases.iphase_unknown },
            { Phases.NotImposed, phases.iphase_not_imposed },
        };
}
