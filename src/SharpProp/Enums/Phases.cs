// ReSharper disable All

namespace SharpProp;

/// <summary>
/// Phase states of fluids and mixtures.
/// </summary>
public enum Phases
{
    Liquid,
    Supercritical,
    SupercriticalGas,
    SupercriticalLiquid,
    CriticalPoint,
    Gas,
    TwoPhase,
    Unknown,
    NotImposed,
}

internal static class PhasesExtensions
{
    public static phases ToCoolPropEnum(this Phases phase) =>
        phase switch
        {
            Phases.Liquid => phases.iphase_liquid,
            Phases.Supercritical => phases.iphase_supercritical,
            Phases.SupercriticalGas => phases.iphase_supercritical_gas,
            Phases.SupercriticalLiquid => phases.iphase_supercritical_liquid,
            Phases.CriticalPoint => phases.iphase_critical_point,
            Phases.Gas => phases.iphase_gas,
            Phases.TwoPhase => phases.iphase_twophase,
            Phases.Unknown => phases.iphase_unknown,
            _ => phases.iphase_not_imposed,
        };
}
