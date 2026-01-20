using UnitsNet.NumberExtensions.NumberToRatio;

namespace SharpProp;

public abstract partial class AbstractFluid
{
    protected AbstractFluid IsentropicCompressionTo(Pressure pressure) =>
        pressure > Pressure
            ? WithState(Input.Pressure(pressure), Input.Entropy(Entropy))
            : throw new ArgumentException(
                "Compressor outlet pressure should be higher than inlet pressure!"
            );

    protected AbstractFluid CompressionTo(Pressure pressure, Ratio isentropicEfficiency) =>
        isentropicEfficiency.Percent is > 0 and < 100
            ? WithState(
                Input.Pressure(pressure),
                Input.Enthalpy(
                    Enthalpy
                        + (IsentropicCompressionTo(pressure).Enthalpy - Enthalpy)
                            / isentropicEfficiency.DecimalFractions
                )
            )
            : throw new ArgumentException("Invalid compressor isentropic efficiency!");

    protected AbstractFluid IsenthalpicExpansionTo(Pressure pressure) =>
        pressure < Pressure
            ? WithState(Input.Pressure(pressure), Input.Enthalpy(Enthalpy))
            : throw new ArgumentException(
                "Expansion valve outlet pressure should be lower than inlet pressure!"
            );

    protected AbstractFluid IsentropicExpansionTo(Pressure pressure) =>
        pressure < Pressure
            ? WithState(Input.Pressure(pressure), Input.Entropy(Entropy))
            : throw new ArgumentException(
                "Expander outlet pressure should be lower than inlet pressure!"
            );

    protected AbstractFluid ExpansionTo(Pressure pressure, Ratio isentropicEfficiency) =>
        isentropicEfficiency.Percent is > 0 and < 100
            ? WithState(
                Input.Pressure(pressure),
                Input.Enthalpy(
                    Enthalpy
                        - (Enthalpy - IsentropicExpansionTo(pressure).Enthalpy)
                            * isentropicEfficiency.DecimalFractions
                )
            )
            : throw new ArgumentException("Invalid expander isentropic efficiency!");

    protected AbstractFluid CoolingTo(Temperature temperature, Pressure? pressureDrop = null) =>
        temperature < Temperature
            ? HeatTransferTo(temperature, pressureDrop)
            : throw new ArgumentException(
                "During the cooling process, the temperature should decrease!"
            );

    protected AbstractFluid CoolingTo(SpecificEnergy enthalpy, Pressure? pressureDrop = null) =>
        enthalpy < Enthalpy
            ? HeatTransferTo(enthalpy, pressureDrop)
            : throw new ArgumentException(
                "During the cooling process, the enthalpy should decrease!"
            );

    protected AbstractFluid HeatingTo(Temperature temperature, Pressure? pressureDrop = null) =>
        temperature > Temperature
            ? HeatTransferTo(temperature, pressureDrop)
            : throw new ArgumentException(
                "During the heating process, the temperature should increase!"
            );

    protected AbstractFluid HeatingTo(SpecificEnergy enthalpy, Pressure? pressureDrop = null) =>
        enthalpy > Enthalpy
            ? HeatTransferTo(enthalpy, pressureDrop)
            : throw new ArgumentException(
                "During the heating process, the enthalpy should increase!"
            );

    protected AbstractFluid BubblePointAt(Pressure pressure) =>
        WithState(Input.Pressure(pressure), Input.Quality(0.Percent()));

    protected AbstractFluid BubblePointAt(Temperature temperature) =>
        WithState(Input.Temperature(temperature), Input.Quality(0.Percent()));

    protected AbstractFluid DewPointAt(Pressure pressure) =>
        WithState(Input.Pressure(pressure), Input.Quality(100.Percent()));

    protected AbstractFluid DewPointAt(Temperature temperature) =>
        WithState(Input.Temperature(temperature), Input.Quality(100.Percent()));

    protected AbstractFluid TwoPhasePointAt(Pressure pressure, Ratio quality) =>
        WithState(Input.Pressure(pressure), Input.Quality(quality));

    protected AbstractFluid Mixing(
        Ratio firstSpecificMassFlow,
        IFluidState first,
        Ratio secondSpecificMassFlow,
        IFluidState second
    ) =>
        first.Pressure.Equals(second.Pressure, Tolerance.Pascals())
            ? WithState(
                Input.Pressure(first.Pressure),
                Input.Enthalpy(
                    (
                        firstSpecificMassFlow.DecimalFractions * first.Enthalpy
                        + secondSpecificMassFlow.DecimalFractions * second.Enthalpy
                    ) / (firstSpecificMassFlow + secondSpecificMassFlow).DecimalFractions
                )
            )
            : throw new ArgumentException(
                "The mixing process is possible only for flows with the same pressure!"
            );

    private AbstractFluid HeatTransferTo(Temperature temperature, Pressure? pressureDrop = null) =>
        pressureDrop.HasValue && pressureDrop.Value < Pressure.Zero
            ? throw new ArgumentException("Invalid pressure drop in the heat exchanger!")
            : WithState(
                Input.Pressure(Pressure - (pressureDrop ?? Pressure.Zero)),
                Input.Temperature(temperature)
            );

    private AbstractFluid HeatTransferTo(SpecificEnergy enthalpy, Pressure? pressureDrop = null) =>
        pressureDrop.HasValue && pressureDrop.Value < Pressure.Zero
            ? throw new ArgumentException("Invalid pressure drop in the heat exchanger!")
            : WithState(
                Input.Pressure(Pressure - (pressureDrop ?? Pressure.Zero)),
                Input.Enthalpy(enthalpy)
            );
}
