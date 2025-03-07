﻿// ReSharper disable All

namespace SharpProp;

/// <summary>
///     List of CoolProp fluids.
///     See more info about CoolProp
///     <a href="http://www.coolprop.org/fluid_properties/PurePseudoPure.html"> pure and pseudo-pure fluids</a>,
///     <a href="http://www.coolprop.org/fluid_properties/Incompressibles.html">incompressible fluids</a> and
///     <a href="http://www.coolprop.org/coolprop/HighLevelAPI.html#predefined-mixtures">predefined mixtures</a>.
/// </summary>
public enum FluidsList
{
    // Pure and pseudo-pure fluids
    [FluidInfo("Acetone")]
    Acetone,

    [FluidInfo("Air")]
    Air,

    [FluidInfo("Air")]
    R729,

    [FluidInfo("Ammonia")]
    Ammonia,

    [FluidInfo("Ammonia")]
    R717,

    [FluidInfo("Argon")]
    Argon,

    [FluidInfo("Argon")]
    R740,

    [FluidInfo("Benzene")]
    Benzene,

    [FluidInfo("1-Butene")]
    Butene,

    [FluidInfo("CarbonDioxide")]
    CarbonDioxide,

    [FluidInfo("CarbonDioxide")]
    R744,

    [FluidInfo("CarbonMonoxide")]
    CarbonMonoxide,

    [FluidInfo("CarbonylSulfide")]
    CarbonylSulfide,

    [FluidInfo("cis-2-Butene")]
    cis2Butene,

    [FluidInfo("CycloHexane")]
    CycloHexane,

    [FluidInfo("CycloPentane")]
    CycloPentane,

    [FluidInfo("CycloPropane")]
    CycloPropane,

    [FluidInfo("D4")]
    D4,

    [FluidInfo("D5")]
    D5,

    [FluidInfo("D6")]
    D6,

    [FluidInfo("Deuterium")]
    Deuterium,

    [FluidInfo("Dichloroethane")]
    Dichloroethane,

    [FluidInfo("DiethylEther")]
    DiethylEther,

    [FluidInfo("DimethylCarbonate")]
    DimethylCarbonate,

    [FluidInfo("DimethylEther")]
    DimethylEther,

    [FluidInfo("Ethane")]
    Ethane,

    [FluidInfo("Ethane")]
    R170,

    [FluidInfo("Ethanol")]
    Ethanol,

    [FluidInfo("EthylBenzene")]
    EthylBenzene,

    [FluidInfo("Ethylene")]
    Ethylene,

    [FluidInfo("Ethylene")]
    R1150,

    [FluidInfo("EthyleneOxide")]
    EthyleneOxide,

    [FluidInfo("Fluorine")]
    Fluorine,

    [FluidInfo("HeavyWater")]
    HeavyWater,

    [FluidInfo("Helium")]
    Helium,

    [FluidInfo("Helium")]
    R704,

    [FluidInfo("HFE143m")]
    HFE143m,

    [FluidInfo("HFE143m")]
    RE143a,

    [FluidInfo("Hydrogen")]
    Hydrogen,

    [FluidInfo("Hydrogen")]
    R702,

    [FluidInfo("HydrogenChloride")]
    HydrogenChloride,

    [FluidInfo("HydrogenSulfide")]
    HydrogenSulfide,

    [FluidInfo("IsoButane")]
    IsoButane,

    [FluidInfo("IsoButane")]
    R600a,

    [FluidInfo("IsoButene")]
    IsoButene,

    [FluidInfo("Isohexane")]
    Isohexane,

    [FluidInfo("Isopentane")]
    Isopentane,

    [FluidInfo("Isopentane")]
    R601a,

    [FluidInfo("Krypton")]
    Krypton,

    [FluidInfo("MD2M")]
    MD2M,

    [FluidInfo("MD3M")]
    MD3M,

    [FluidInfo("MD4M")]
    MD4M,

    [FluidInfo("MDM")]
    MDM,

    [FluidInfo("Methane")]
    Methane,

    [FluidInfo("Methane")]
    R50,

    [FluidInfo("Methanol")]
    Methanol,

    [FluidInfo("MethylLinoleate")]
    MethylLinoleate,

    [FluidInfo("MethylLinolenate")]
    MethylLinolenate,

    [FluidInfo("MethylOleate")]
    MethylOleate,

    [FluidInfo("MethylPalmitate")]
    MethylPalmitate,

    [FluidInfo("MethylStearate")]
    MethylStearate,

    [FluidInfo("MM")]
    MM,

    [FluidInfo("m-Xylene")]
    mXylene,

    [FluidInfo("n-Butane")]
    nButane,

    [FluidInfo("n-Butane")]
    R600,

    [FluidInfo("n-Decane")]
    nDecane,

    [FluidInfo("n-Dodecane")]
    nDodecane,

    [FluidInfo("Neon")]
    Neon,

    [FluidInfo("Neon")]
    R720,

    [FluidInfo("Neopentane")]
    Neopentane,

    [FluidInfo("n-Heptane")]
    nHeptane,

    [FluidInfo("n-Hexane")]
    nHexane,

    [FluidInfo("Nitrogen")]
    Nitrogen,

    [FluidInfo("Nitrogen")]
    R728,

    [FluidInfo("NitrousOxide")]
    NitrousOxide,

    [FluidInfo("n-Nonane")]
    nNonane,

    [FluidInfo("n-Octane")]
    nOctane,

    [FluidInfo("Novec649")]
    Novec649,

    [FluidInfo("n-Pentane")]
    nPentane,

    [FluidInfo("n-Pentane")]
    R601,

    [FluidInfo("n-Propane")]
    nPropane,

    [FluidInfo("n-Propane")]
    R290,

    [FluidInfo("n-Undecane")]
    nUndecane,

    [FluidInfo("OrthoDeuterium")]
    OrthoDeuterium,

    [FluidInfo("OrthoHydrogen")]
    OrthoHydrogen,

    [FluidInfo("Oxygen")]
    Oxygen,

    [FluidInfo("Oxygen")]
    R732,

    [FluidInfo("o-Xylene")]
    oXylene,

    [FluidInfo("ParaDeuterium")]
    ParaDeuterium,

    [FluidInfo("ParaHydrogen")]
    ParaHydrogen,

    [FluidInfo("Propylene")]
    Propylene,

    [FluidInfo("Propylene")]
    R1270,

    [FluidInfo("Propyne")]
    Propyne,

    [FluidInfo("p-Xylene")]
    pXylene,

    [FluidInfo("R11")]
    R11,

    [FluidInfo("R113")]
    R113,

    [FluidInfo("R114")]
    R114,

    [FluidInfo("R115")]
    R115,

    [FluidInfo("R116")]
    R116,

    [FluidInfo("R12")]
    R12,

    [FluidInfo("R123")]
    R123,

    [FluidInfo("R1233zd(E)")]
    R1233zdE,

    [FluidInfo("R1234yf")]
    R1234yf,

    [FluidInfo("R1234ze(E)")]
    R1234zeE,

    [FluidInfo("R1234ze(Z)")]
    R1234zeZ,

    [FluidInfo("R124")]
    R124,

    [FluidInfo("R1243zf")]
    R1243zf,

    [FluidInfo("R125")]
    R125,

    [FluidInfo("R13")]
    R13,

    [FluidInfo("R1336mzz(E)")]
    R1336mzzE,

    [FluidInfo("R134a")]
    R134a,

    [FluidInfo("R13I1")]
    R13I1,

    [FluidInfo("R14")]
    R14,

    [FluidInfo("R141b")]
    R141b,

    [FluidInfo("R142b")]
    R142b,

    [FluidInfo("R143a")]
    R143a,

    [FluidInfo("R152A")]
    R152a,

    [FluidInfo("R161")]
    R161,

    [FluidInfo("R21")]
    R21,

    [FluidInfo("R218")]
    R218,

    [FluidInfo("R22")]
    R22,

    [FluidInfo("R227ea")]
    R227ea,

    [FluidInfo("R23")]
    R23,

    [FluidInfo("R236ea")]
    R236ea,

    [FluidInfo("R236fa")]
    R236fa,

    [FluidInfo("R245ca")]
    R245ca,

    [FluidInfo("R245fa")]
    R245fa,

    [FluidInfo("R32")]
    R32,

    [FluidInfo("R365mfc")]
    R365mfc,

    [FluidInfo("R40")]
    R40,

    [FluidInfo("R404A")]
    R404A,

    [FluidInfo("R407C")]
    R407C,

    [FluidInfo("R41")]
    R41,

    [FluidInfo("R410A")]
    R410A,

    [FluidInfo("R507A")]
    R507A,

    [FluidInfo("RC318")]
    RC318,

    [FluidInfo("SES36")]
    SES36,

    [FluidInfo("SulfurDioxide")]
    SulfurDioxide,

    [FluidInfo("SulfurDioxide")]
    R764,

    [FluidInfo("SulfurHexafluoride")]
    SulfurHexafluoride,

    [FluidInfo("SulfurHexafluoride")]
    R846,

    [FluidInfo("Toluene")]
    Toluene,

    [FluidInfo("trans-2-Butene")]
    trans2Butene,

    [FluidInfo("Water")]
    Water,

    [FluidInfo("Water")]
    R718,

    [FluidInfo("Xenon")]
    Xenon,

    // Incompressible pure fluids
    [FluidInfo("AS10", "INCOMP")]
    AS10,

    [FluidInfo("AS20", "INCOMP")]
    AS20,

    [FluidInfo("AS30", "INCOMP")]
    AS30,

    [FluidInfo("AS40", "INCOMP")]
    AS40,

    [FluidInfo("AS55", "INCOMP")]
    AS55,

    [FluidInfo("DEB", "INCOMP")]
    DEB,

    [FluidInfo("DowJ", "INCOMP")]
    DowJ,

    [FluidInfo("DowJ2", "INCOMP")]
    DowJ2,

    [FluidInfo("DowQ", "INCOMP")]
    DowQ,

    [FluidInfo("DowQ2", "INCOMP")]
    DowQ2,

    [FluidInfo("DSF", "INCOMP")]
    DSF,

    [FluidInfo("HC10", "INCOMP")]
    HC10,

    [FluidInfo("HC20", "INCOMP")]
    HC20,

    [FluidInfo("HC30", "INCOMP")]
    HC30,

    [FluidInfo("HC40", "INCOMP")]
    HC40,

    [FluidInfo("HC50", "INCOMP")]
    HC50,

    [FluidInfo("HCB", "INCOMP")]
    HCB,

    [FluidInfo("HCM", "INCOMP")]
    HCM,

    [FluidInfo("HFE", "INCOMP")]
    HFE,

    [FluidInfo("HFE2", "INCOMP")]
    HFE2,

    [FluidInfo("HY20", "INCOMP")]
    HY20,

    [FluidInfo("HY30", "INCOMP")]
    HY30,

    [FluidInfo("HY40", "INCOMP")]
    HY40,

    [FluidInfo("HY45", "INCOMP")]
    HY45,

    [FluidInfo("HY50", "INCOMP")]
    HY50,

    [FluidInfo("NaK", "INCOMP")]
    NaK,

    [FluidInfo("NBS", "INCOMP")]
    NBS,

    [FluidInfo("PBB", "INCOMP")]
    PBB,

    [FluidInfo("PCL", "INCOMP")]
    PCL,

    [FluidInfo("PCR", "INCOMP")]
    PCR,

    [FluidInfo("PGLT", "INCOMP")]
    PGLT,

    [FluidInfo("PHE", "INCOMP")]
    PHE,

    [FluidInfo("PHR", "INCOMP")]
    PHR,

    [FluidInfo("PLR", "INCOMP")]
    PLR,

    [FluidInfo("PMR", "INCOMP")]
    PMR,

    [FluidInfo("PMS1", "INCOMP")]
    PMS1,

    [FluidInfo("PMS2", "INCOMP")]
    PMS2,

    [FluidInfo("PNF", "INCOMP")]
    PNF,

    [FluidInfo("PNF2", "INCOMP")]
    PNF2,

    [FluidInfo("S800", "INCOMP")]
    S800,

    [FluidInfo("SAB", "INCOMP")]
    SAB,

    [FluidInfo("T66", "INCOMP")]
    T66,

    [FluidInfo("T72", "INCOMP")]
    T72,

    [FluidInfo("TCO", "INCOMP")]
    TCO,

    [FluidInfo("TD12", "INCOMP")]
    TD12,

    [FluidInfo("TVP1", "INCOMP")]
    TVP1,

    [FluidInfo("TVP1869", "INCOMP")]
    TVP1869,

    [FluidInfo("TX22", "INCOMP")]
    TX22,

    [FluidInfo("TY10", "INCOMP")]
    TY10,

    [FluidInfo("TY15", "INCOMP")]
    TY15,

    [FluidInfo("TY20", "INCOMP")]
    TY20,

    [FluidInfo("TY24", "INCOMP")]
    TY24,

    [FluidInfo("Water", "INCOMP")]
    WaterIncomp,

    [FluidInfo("XLT", "INCOMP")]
    XLT,

    [FluidInfo("XLT2", "INCOMP")]
    XLT2,

    [FluidInfo("ZS10", "INCOMP")]
    ZS10,

    [FluidInfo("ZS25", "INCOMP")]
    ZS25,

    [FluidInfo("ZS40", "INCOMP")]
    ZS40,

    [FluidInfo("ZS45", "INCOMP")]
    ZS45,

    [FluidInfo("ZS55", "INCOMP")]
    ZS55,

    // Incompressible mass-based binary mixtures
    [FluidInfo("FRE", "INCOMP", false, Mix.Mass, 0.19, 0.5)]
    FRE,

    [FluidInfo("IceEA", "INCOMP", false, Mix.Mass, 0.05, 0.35)]
    IceEA,

    [FluidInfo("IceNA", "INCOMP", false, Mix.Mass, 0.05, 0.35)]
    IceNA,

    [FluidInfo("IcePG", "INCOMP", false, Mix.Mass, 0.05, 0.35)]
    IcePG,

    [FluidInfo("LiBr", "INCOMP", false, Mix.Mass, 0, 0.75)]
    LiBr,

    [FluidInfo("MAM", "INCOMP", false, Mix.Mass, 0, 0.3)]
    MAM,

    [FluidInfo("MAM2", "INCOMP", false, Mix.Mass, 0.078, 0.236)]
    MAM2,

    [FluidInfo("MCA", "INCOMP", false, Mix.Mass, 0, 0.3)]
    MCA,

    [FluidInfo("MCA2", "INCOMP", false, Mix.Mass, 0.09, 0.294)]
    MCA2,

    [FluidInfo("MEA", "INCOMP", false, Mix.Mass, 0, 0.6)]
    MEA,

    [FluidInfo("MEA2", "INCOMP", false, Mix.Mass, 0.11, 0.6)]
    MEA2,

    [FluidInfo("MEG", "INCOMP", false, Mix.Mass, 0, 0.6)]
    MEG,

    [FluidInfo("MEG2", "INCOMP", false, Mix.Mass, 0, 0.56)]
    MEG2,

    [FluidInfo("MGL", "INCOMP", false, Mix.Mass, 0, 0.6)]
    MGL,

    [FluidInfo("MGL2", "INCOMP", false, Mix.Mass, 0.195, 0.63)]
    MGL2,

    [FluidInfo("MITSW", "INCOMP", false, Mix.Mass, 0, 0.12)]
    MITSW,

    [FluidInfo("MKA", "INCOMP", false, Mix.Mass, 0, 0.45)]
    MKA,

    [FluidInfo("MKA2", "INCOMP", false, Mix.Mass, 0.11, 0.41)]
    MKA2,

    [FluidInfo("MKC", "INCOMP", false, Mix.Mass, 0, 0.4)]
    MKC,

    [FluidInfo("MKC2", "INCOMP", false, Mix.Mass, 0, 0.39)]
    MKC2,

    [FluidInfo("MKF", "INCOMP", false, Mix.Mass, 0, 0.48)]
    MKF,

    [FluidInfo("MLI", "INCOMP", false, Mix.Mass, 0, 0.24)]
    MLI,

    [FluidInfo("MMA", "INCOMP", false, Mix.Mass, 0, 0.6)]
    MMA,

    [FluidInfo("MMA2", "INCOMP", false, Mix.Mass, 0.078, 0.474)]
    MMA2,

    [FluidInfo("MMG", "INCOMP", false, Mix.Mass, 0, 0.3)]
    MMG,

    [FluidInfo("MMG2", "INCOMP", false, Mix.Mass, 0, 0.205)]
    MMG2,

    [FluidInfo("MNA", "INCOMP", false, Mix.Mass, 0, 0.23)]
    MNA,

    [FluidInfo("MNA2", "INCOMP", false, Mix.Mass, 0, 0.23)]
    MNA2,

    [FluidInfo("MPG", "INCOMP", false, Mix.Mass, 0, 0.6)]
    MPG,

    [FluidInfo("MPG2", "INCOMP", false, Mix.Mass, 0.15, 0.57)]
    MPG2,

    [FluidInfo("VCA", "INCOMP", false, Mix.Mass, 0.147, 0.299)]
    VCA,

    [FluidInfo("VKC", "INCOMP", false, Mix.Mass, 0.128, 0.389)]
    VKC,

    [FluidInfo("VMA", "INCOMP", false, Mix.Mass, 0.1, 0.9)]
    VMA,

    [FluidInfo("VMG", "INCOMP", false, Mix.Mass, 0.072, 0.206)]
    VMG,

    [FluidInfo("VNA", "INCOMP", false, Mix.Mass, 0.07, 0.231)]
    VNA,

    // Incompressible volume-based binary mixtures
    [FluidInfo("AEG", "INCOMP", false, Mix.Volume, 0.1, 0.6)]
    AEG,

    [FluidInfo("AKF", "INCOMP", false, Mix.Volume, 0.4)]
    AKF,

    [FluidInfo("AL", "INCOMP", false, Mix.Volume, 0.1, 0.6)]
    AL,

    [FluidInfo("AN", "INCOMP", false, Mix.Volume, 0.1, 0.6)]
    AN,

    [FluidInfo("APG", "INCOMP", false, Mix.Volume, 0.1, 0.6)]
    APG,

    [FluidInfo("GKN", "INCOMP", false, Mix.Volume, 0.1, 0.6)]
    GKN,

    [FluidInfo("PK2", "INCOMP", false, Mix.Volume, 0.3)]
    PK2,

    [FluidInfo("PKL", "INCOMP", false, Mix.Volume, 0.1, 0.6)]
    PKL,

    [FluidInfo("ZAC", "INCOMP", false, Mix.Volume, 0.06, 0.5)]
    ZAC,

    [FluidInfo("ZFC", "INCOMP", false, Mix.Volume, 0.3, 0.6)]
    ZFC,

    [FluidInfo("ZLC", "INCOMP", false, Mix.Volume, 0.3, 0.7)]
    ZLC,

    [FluidInfo("ZM", "INCOMP", false, Mix.Volume)]
    ZM,

    [FluidInfo("ZMC", "INCOMP", false, Mix.Volume, 0.3, 0.7)]
    ZMC,

    // Predefined mixtures
    [FluidInfo("Air.mix")]
    AirMix,

    [FluidInfo("Amarillo.mix")]
    Amarillo,

    [FluidInfo("Ekofisk.mix")]
    Ekofisk,

    [FluidInfo("GulfCoast.mix")]
    GulfCoast,

    [FluidInfo("GulfCoastGas(NIST1).mix")]
    GulfCoastGasNIST,

    [FluidInfo("HighCO2.mix")]
    HighCO2,

    [FluidInfo("HighN2.mix")]
    HighN2,

    [FluidInfo("NaturalGasSample.mix")]
    NaturalGasSample,

    [FluidInfo("R401A.mix")]
    R401A,

    [FluidInfo("R401B.mix")]
    R401B,

    [FluidInfo("R401C.mix")]
    R401C,

    [FluidInfo("R402A.mix")]
    R402A,

    [FluidInfo("R402B.mix")]
    R402B,

    [FluidInfo("R403A.mix")]
    R403A,

    [FluidInfo("R403B.mix")]
    R403B,

    [FluidInfo("R404A.mix")]
    R404AMix,

    [FluidInfo("R405A.mix")]
    R405A,

    [FluidInfo("R406A.mix")]
    R406A,

    [FluidInfo("R407A.mix")]
    R407A,

    [FluidInfo("R407B.mix")]
    R407B,

    [FluidInfo("R407C.mix")]
    R407CMix,

    [FluidInfo("R407D.mix")]
    R407D,

    [FluidInfo("R407E.mix")]
    R407E,

    [FluidInfo("R407F.mix")]
    R407F,

    [FluidInfo("R408A.mix")]
    R408A,

    [FluidInfo("R409A.mix")]
    R409A,

    [FluidInfo("R409B.mix")]
    R409B,

    [FluidInfo("R410A.mix")]
    R410AMix,

    [FluidInfo("R410B.mix")]
    R410B,

    [FluidInfo("R411A.mix")]
    R411A,

    [FluidInfo("R411B.mix")]
    R411B,

    [FluidInfo("R412A.mix")]
    R412A,

    [FluidInfo("R413A.mix")]
    R413A,

    [FluidInfo("R414A.mix")]
    R414A,

    [FluidInfo("R414B.mix")]
    R414B,

    [FluidInfo("R415A.mix")]
    R415A,

    [FluidInfo("R415B.mix")]
    R415B,

    [FluidInfo("R416A.mix")]
    R416A,

    [FluidInfo("R417A.mix")]
    R417A,

    [FluidInfo("R417B.mix")]
    R417B,

    [FluidInfo("R417C.mix")]
    R417C,

    [FluidInfo("R418A.mix")]
    R418A,

    [FluidInfo("R419A.mix")]
    R419A,

    [FluidInfo("R419B.mix")]
    R419B,

    [FluidInfo("R420A.mix")]
    R420A,

    [FluidInfo("R421A.mix")]
    R421A,

    [FluidInfo("R421B.mix")]
    R421B,

    [FluidInfo("R422A.mix")]
    R422A,

    [FluidInfo("R422B.mix")]
    R422B,

    [FluidInfo("R422C.mix")]
    R422C,

    [FluidInfo("R422D.mix")]
    R422D,

    [FluidInfo("R422E.mix")]
    R422E,

    [FluidInfo("R423A.mix")]
    R423A,

    [FluidInfo("R424A.mix")]
    R424A,

    [FluidInfo("R425A.mix")]
    R425A,

    [FluidInfo("R426A.mix")]
    R426A,

    [FluidInfo("R427A.mix")]
    R427A,

    [FluidInfo("R428A.mix")]
    R428A,

    [FluidInfo("R429A.mix")]
    R429A,

    [FluidInfo("R430A.mix")]
    R430A,

    [FluidInfo("R431A.mix")]
    R431A,

    [FluidInfo("R432A.mix")]
    R432A,

    [FluidInfo("R433A.mix")]
    R433A,

    [FluidInfo("R433B.mix")]
    R433B,

    [FluidInfo("R433C.mix")]
    R433C,

    [FluidInfo("R434A.mix")]
    R434A,

    [FluidInfo("R435A.mix")]
    R435A,

    [FluidInfo("R436A.mix")]
    R436A,

    [FluidInfo("R436B.mix")]
    R436B,

    [FluidInfo("R437A.mix")]
    R437A,

    [FluidInfo("R438A.mix")]
    R438A,

    [FluidInfo("R439A.mix")]
    R439A,

    [FluidInfo("R440A.mix")]
    R440A,

    [FluidInfo("R441A.mix")]
    R441A,

    [FluidInfo("R442A.mix")]
    R442A,

    [FluidInfo("R443A.mix")]
    R443A,

    [FluidInfo("R444A.mix")]
    R444A,

    [FluidInfo("R444B.mix")]
    R444B,

    [FluidInfo("R445A.mix")]
    R445A,

    [FluidInfo("R446A.mix")]
    R446A,

    [FluidInfo("R447A.mix")]
    R447A,

    [FluidInfo("R448A.mix")]
    R448A,

    [FluidInfo("R449A.mix")]
    R449A,

    [FluidInfo("R449B.mix")]
    R449B,

    [FluidInfo("R450A.mix")]
    R450A,

    [FluidInfo("R451A.mix")]
    R451A,

    [FluidInfo("R451B.mix")]
    R451B,

    [FluidInfo("R452A.mix")]
    R452A,

    [FluidInfo("R453A.mix")]
    R453A,

    [FluidInfo("R454A.mix")]
    R454A,

    [FluidInfo("R454B.mix")]
    R454B,

    [FluidInfo("R500.mix")]
    R500,

    [FluidInfo("R501.mix")]
    R501,

    [FluidInfo("R502.mix")]
    R502,

    [FluidInfo("R503.mix")]
    R503,

    [FluidInfo("R504.mix")]
    R504,

    [FluidInfo("R507A.mix")]
    R507AMix,

    [FluidInfo("R508A.mix")]
    R508A,

    [FluidInfo("R508B.mix")]
    R508B,

    [FluidInfo("R509A.mix")]
    R509A,

    [FluidInfo("R510A.mix")]
    R510A,

    [FluidInfo("R511A.mix")]
    R511A,

    [FluidInfo("R512A.mix")]
    R512A,

    [FluidInfo("R513A.mix")]
    R513A,

    [FluidInfo("TypicalNaturalGas.mix")]
    TypicalNaturalGas,
}
