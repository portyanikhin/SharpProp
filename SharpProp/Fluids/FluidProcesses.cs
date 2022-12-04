using System;
using UnitsNet;
using UnitsNet.NumberExtensions.NumberToRatio;

namespace SharpProp
{
    public abstract partial class AbstractFluid
    {
        /// <summary>
        ///     The process of isentropic compression to a given pressure.
        /// </summary>
        /// <param name="pressure">Pressure.</param>
        /// <returns>The state of the fluid at the end of the process.</returns>
        /// <exception cref="ArgumentException">
        ///     Compressor outlet pressure should be higher than inlet pressure!
        /// </exception>
        public virtual AbstractFluid IsentropicCompressionTo(Pressure pressure) =>
            pressure > Pressure
                ? WithState(Input.Pressure(pressure), Input.Entropy(Entropy))
                : throw new ArgumentException(
                    "Compressor outlet pressure should be higher than inlet pressure!");

        /// <summary>
        ///     The process of compression to a given pressure.
        /// </summary>
        /// <param name="pressure">Pressure.</param>
        /// <param name="isentropicEfficiency">Compressor isentropic efficiency.</param>
        /// <returns>The state of the fluid at the end of the process.</returns>
        /// <exception cref="ArgumentException">
        ///     Compressor outlet pressure should be higher than inlet pressure!
        /// </exception>
        /// <exception cref="ArgumentException">Invalid compressor isentropic efficiency!</exception>
        public virtual AbstractFluid CompressionTo(Pressure pressure, Ratio isentropicEfficiency) =>
            isentropicEfficiency.Percent is > 0 and < 100
                ? WithState(Input.Pressure(pressure),
                    Input.Enthalpy(Enthalpy + (IsentropicCompressionTo(pressure).Enthalpy - Enthalpy) /
                        isentropicEfficiency.DecimalFractions))
                : throw new ArgumentException("Invalid compressor isentropic efficiency!");

        /// <summary>
        ///     The process of isenthalpic expansion to a given pressure.
        /// </summary>
        /// <param name="pressure">Pressure.</param>
        /// <returns>The state of the fluid at the end of the process.</returns>
        /// <exception cref="ArgumentException">
        ///     Expansion valve outlet pressure should be lower than inlet pressure!
        /// </exception>
        public virtual AbstractFluid IsenthalpicExpansionTo(Pressure pressure) =>
            pressure < Pressure
                ? WithState(Input.Pressure(pressure), Input.Enthalpy(Enthalpy))
                : throw new ArgumentException(
                    "Expansion valve outlet pressure should be lower than inlet pressure!");

        /// <summary>
        ///     The process of isentropic expansion to a given pressure.
        /// </summary>
        /// <param name="pressure">Pressure.</param>
        /// <returns>The state of the fluid at the end of the process.</returns>
        /// <exception cref="ArgumentException">
        ///     Expander outlet pressure should be lower than inlet pressure!
        /// </exception>
        public virtual AbstractFluid IsentropicExpansionTo(Pressure pressure) =>
            pressure < Pressure
                ? WithState(Input.Pressure(pressure), Input.Entropy(Entropy))
                : throw new ArgumentException(
                    "Expander outlet pressure should be lower than inlet pressure!");

        /// <summary>
        ///     The process of expansion to a given pressure.
        /// </summary>
        /// <param name="pressure">Pressure.</param>
        /// <param name="isentropicEfficiency">Expander isentropic efficiency.</param>
        /// <returns>The state of the fluid at the end of the process.</returns>
        /// <exception cref="ArgumentException">
        ///     Expander outlet pressure should be lower than inlet pressure!
        /// </exception>
        /// <exception cref="ArgumentException">Invalid expander isentropic efficiency!</exception>
        public virtual AbstractFluid ExpansionTo(Pressure pressure, Ratio isentropicEfficiency) =>
            isentropicEfficiency.Percent is > 0 and < 100
                ? WithState(Input.Pressure(pressure),
                    Input.Enthalpy(Enthalpy - (Enthalpy - IsentropicExpansionTo(pressure).Enthalpy) *
                        isentropicEfficiency.DecimalFractions))
                : throw new ArgumentException("Invalid expander isentropic efficiency!");

        /// <summary>
        ///     The process of cooling to a given temperature.
        /// </summary>
        /// <param name="temperature">Temperature.</param>
        /// <param name="pressureDrop">Pressure drop in the heat exchanger (optional).</param>
        /// <returns>The state of the fluid at the end of the process.</returns>
        /// <exception cref="ArgumentException">
        ///     During the cooling process, the temperature should decrease!
        /// </exception>
        /// <exception cref="ArgumentException">Invalid pressure drop in the heat exchanger!</exception>
        public virtual AbstractFluid CoolingTo(Temperature temperature, Pressure? pressureDrop = null) =>
            temperature < Temperature
                ? HeatTransferTo(temperature, pressureDrop)
                : throw new ArgumentException(
                    "During the cooling process, the temperature should decrease!");

        /// <summary>
        ///     The process of cooling to a given enthalpy.
        /// </summary>
        /// <param name="enthalpy">Enthalpy.</param>
        /// <param name="pressureDrop">Pressure drop in the heat exchanger (optional).</param>
        /// <returns>The state of the fluid at the end of the process.</returns>
        /// <exception cref="ArgumentException">
        ///     During the cooling process, the enthalpy should decrease!
        /// </exception>
        /// <exception cref="ArgumentException">Invalid pressure drop in the heat exchanger!</exception>
        public virtual AbstractFluid CoolingTo(SpecificEnergy enthalpy, Pressure? pressureDrop = null) =>
            enthalpy < Enthalpy
                ? HeatTransferTo(enthalpy, pressureDrop)
                : throw new ArgumentException(
                    "During the cooling process, the enthalpy should decrease!");

        /// <summary>
        ///     The process of heating to a given temperature.
        /// </summary>
        /// <param name="temperature">Temperature.</param>
        /// <param name="pressureDrop">Pressure drop in the heat exchanger (optional).</param>
        /// <returns>The state of the fluid at the end of the process.</returns>
        /// <exception cref="ArgumentException">
        ///     During the heating process, the temperature should increase!
        /// </exception>
        /// <exception cref="ArgumentException">Invalid pressure drop in the heat exchanger!</exception>
        public virtual AbstractFluid HeatingTo(Temperature temperature, Pressure? pressureDrop = null) =>
            temperature > Temperature
                ? HeatTransferTo(temperature, pressureDrop)
                : throw new ArgumentException(
                    "During the heating process, the temperature should increase!");

        /// <summary>
        ///     The process of heating to a given enthalpy.
        /// </summary>
        /// <param name="enthalpy">Enthalpy.</param>
        /// <param name="pressureDrop">Pressure drop in the heat exchanger (optional).</param>
        /// <returns>The state of the fluid at the end of the process.</returns>
        /// <exception cref="ArgumentException">
        ///     During the heating process, the enthalpy should increase!
        /// </exception>
        /// <exception cref="ArgumentException">Invalid pressure drop in the heat exchanger!</exception>
        public virtual AbstractFluid HeatingTo(SpecificEnergy enthalpy, Pressure? pressureDrop = null) =>
            enthalpy > Enthalpy
                ? HeatTransferTo(enthalpy, pressureDrop)
                : throw new ArgumentException(
                    "During the heating process, the enthalpy should increase!");

        /// <summary>
        ///     Returns a bubble point at a given pressure.
        /// </summary>
        /// <param name="pressure">Pressure.</param>
        /// <returns>A bubble point at a given pressure.</returns>
        public virtual AbstractFluid BubblePointAt(Pressure pressure) =>
            WithState(Input.Pressure(pressure), Input.Quality(0.Percent()));

        /// <summary>
        ///     Returns a bubble point at a given temperature.
        /// </summary>
        /// <param name="temperature">Temperature.</param>
        /// <returns>A bubble point at a given temperature.</returns>
        public virtual AbstractFluid BubblePointAt(Temperature temperature) =>
            WithState(Input.Temperature(temperature), Input.Quality(0.Percent()));

        /// <summary>
        ///     Returns a dew point at a given pressure.
        /// </summary>
        /// <param name="pressure">Pressure.</param>
        /// <returns>A dew point at a given pressure.</returns>
        public virtual AbstractFluid DewPointAt(Pressure pressure) =>
            WithState(Input.Pressure(pressure), Input.Quality(100.Percent()));

        /// <summary>
        ///     Returns a dew point at a given temperature.
        /// </summary>
        /// <param name="temperature">Temperature.</param>
        /// <returns>A dew point at a given temperature.</returns>
        public virtual AbstractFluid DewPointAt(Temperature temperature) =>
            WithState(Input.Temperature(temperature), Input.Quality(100.Percent()));

        /// <summary>
        ///     Returns a two-phase point at a given pressure.
        /// </summary>
        /// <param name="pressure">Pressure.</param>
        /// <param name="quality">Vapor quality.</param>
        /// <returns>Two-phase point at a given pressure.</returns>
        public virtual AbstractFluid TwoPhasePointAt(Pressure pressure, Ratio quality) =>
            WithState(Input.Pressure(pressure), Input.Quality(quality));

        /// <summary>
        ///     The mixing process.
        /// </summary>
        /// <param name="firstSpecificMassFlow">
        ///     Specific mass flow rate of the fluid at the first state.
        /// </param>
        /// <param name="first">Fluid at the first state.</param>
        /// <param name="secondSpecificMassFlow">
        ///     Specific mass flow rate of the fluid at the second state.
        /// </param>
        /// <param name="second">Fluid at the second state.</param>
        /// <returns>The state of the fluid at the end of the process.</returns>
        /// <exception cref="ArgumentException">
        ///     The mixing process is possible only for flows with the same pressure!
        /// </exception>
        public virtual AbstractFluid Mixing(Ratio firstSpecificMassFlow, AbstractFluid first,
            Ratio secondSpecificMassFlow, AbstractFluid second) =>
            first.Pressure == second.Pressure
                ? WithState(Input.Pressure(first.Pressure), Input.Enthalpy(
                    (firstSpecificMassFlow.DecimalFractions * first.Enthalpy +
                     secondSpecificMassFlow.DecimalFractions * second.Enthalpy) /
                    (firstSpecificMassFlow + secondSpecificMassFlow).DecimalFractions))
                : throw new ArgumentException(
                    "The mixing process is possible only for flows with the same pressure!");

        private AbstractFluid HeatTransferTo(Temperature temperature, Pressure? pressureDrop = null) =>
            pressureDrop.HasValue && pressureDrop.Value < Pressure.Zero
                ? throw new ArgumentException("Invalid pressure drop in the heat exchanger!")
                : WithState(Input.Pressure(Pressure - (pressureDrop ?? Pressure.Zero)),
                    Input.Temperature(temperature));

        private AbstractFluid HeatTransferTo(SpecificEnergy enthalpy, Pressure? pressureDrop = null) =>
            pressureDrop.HasValue && pressureDrop.Value < Pressure.Zero
                ? throw new ArgumentException("Invalid pressure drop in the heat exchanger!")
                : WithState(Input.Pressure(Pressure - (pressureDrop ?? Pressure.Zero)),
                    Input.Enthalpy(enthalpy));
    }
}