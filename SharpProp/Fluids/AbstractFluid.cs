using System;
using CoolProp;
using SharpProp.Outputs;

namespace SharpProp
{
    /// <summary>
    ///     Fluids base class
    /// </summary>
    public abstract partial class AbstractFluid : Jsonable, IEquatable<AbstractFluid>
    {
        public bool Equals(AbstractFluid? other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return _phase == other._phase &&
                   (_compressibility, _conductivity, _criticalPressure, _criticalTemperature,
                       _density, _dynamicViscosity, _enthalpy, _entropy,
                       _freezingTemperature, _internalEnergy, _maxPressure, _maxTemperature,
                       _minPressure, _minTemperature, _molarMass, _prandtl, _pressure,
                       _quality, _soundSpeed, _specificHeat, _surfaceTension,
                       _temperature, _triplePressure, _tripleTemperature) ==
                   (other._compressibility, other._conductivity, other._criticalPressure, other._criticalTemperature,
                       other._density, other._dynamicViscosity, other._enthalpy, other._entropy,
                       other._freezingTemperature, other._internalEnergy, other._maxPressure, other._maxTemperature,
                       other._minPressure, other._minTemperature, other._molarMass, other._prandtl, other._pressure,
                       other._quality, other._soundSpeed, other._specificHeat, other._surfaceTension,
                       other._temperature, other._triplePressure, other._tripleTemperature);
        }

        /// <summary>
        ///     Returns a new fluid object with no defined state
        /// </summary>
        /// <returns>A new fluid object with no defined state</returns>
        public abstract AbstractFluid Factory();

        /// <summary>
        ///     Returns a new fluid object with a defined state
        /// </summary>
        /// <param name="firstInput">First input property</param>
        /// <param name="secondInput">Second input property</param>
        /// <returns>A new fluid object with a defined state</returns>
        /// <exception cref="ArgumentException">Need to define 2 unique inputs!</exception>
        public virtual AbstractFluid WithState(IKeyedInput<Parameters> firstInput, IKeyedInput<Parameters> secondInput)
        {
            var fluid = Factory();
            fluid.Update(firstInput, secondInput);
            return fluid;
        }

        /// <summary>
        ///     Update fluid state with two inputs
        /// </summary>
        /// <param name="firstInput">First input property</param>
        /// <param name="secondInput">Second input property</param>
        /// <exception cref="ArgumentException">Need to define 2 unique inputs!</exception>
        public void Update(IKeyedInput<Parameters> firstInput, IKeyedInput<Parameters> secondInput)
        {
            Reset();
            var (inputPair, firstValue, secondValue) = GenerateUpdatePair(firstInput, secondInput);
            Backend.update(inputPair, firstValue, secondValue);
        }

        /// <summary>
        ///     Reset all non-trivial properties
        /// </summary>
        protected virtual void Reset()
        {
            _compressibility = null;
            _conductivity = null;
            _density = null;
            _dynamicViscosity = null;
            _enthalpy = null;
            _entropy = null;
            _internalEnergy = null;
            _prandtl = null;
            _pressure = null;
            _quality = null;
            _soundSpeed = null;
            _specificHeat = null;
            _surfaceTension = null;
            _temperature = null;
            _phase = null;
        }

        /// <summary>
        ///     Returns <c>true</c> if output is not <c>null</c>
        /// </summary>
        /// <param name="key">Key of output</param>
        /// <param name="value">Value of output</param>
        /// <returns><c>true</c> if output is not <c>null</c></returns>
        protected bool KeyedOutputIsNotNull(Parameters key, out double? value)
        {
            value = NullableKeyedOutput(key);
            return value is not null;
        }

        /// <summary>
        ///     Returns nullable keyed output
        /// </summary>
        /// <param name="key">Key of output</param>
        /// <returns>A nullable keyed output</returns>
        protected double? NullableKeyedOutput(Parameters key)
        {
            try
            {
                var value = KeyedOutput(key);
                if (key is Parameters.iQ && value is < 0 or > 1)
                    return null;
                return value;
            }
            catch (Exception e) when (e is ApplicationException or ArgumentException)
            {
                return null;
            }
        }

        /// <summary>
        ///     Returns not nullable keyed output
        /// </summary>
        /// <param name="key">Key of output</param>
        /// <returns>A not nullable keyed output</returns>
        /// <exception cref="ArgumentException">Invalid or not defined state!</exception>
        protected double KeyedOutput(Parameters key)
        {
            var value = Backend.keyed_output(key);
            OutputsValidator.Validate(value);
            return value;
        }

        private static (input_pairs inputPair, double firstValue, double secondValue) GenerateUpdatePair(
            IKeyedInput<Parameters> firstInput, IKeyedInput<Parameters> secondInput)
        {
            var inputPair = GetInputPair(firstInput, secondInput);
            var swap = !inputPair.HasValue;
            if (swap) inputPair = GetInputPair(secondInput, firstInput);
            if (!inputPair.HasValue)
                throw new ArgumentException("Need to define 2 unique inputs!");

            return !swap
                ? ((input_pairs) inputPair, firstValue: firstInput.Value, secondValue: secondInput.Value)
                : ((input_pairs) inputPair, firstValue: secondInput.Value, secondValue: firstInput.Value);
        }

        private static input_pairs? GetInputPair(IKeyedInput<Parameters> firstInput,
            IKeyedInput<Parameters> secondInput)
        {
            try
            {
                return CP.get_input_pair_index(GetInputPairName(firstInput, secondInput));
            }
            catch
            {
                return null;
            }
        }

        private static string
            GetInputPairName(IKeyedInput<Parameters> firstInput, IKeyedInput<Parameters> secondInput) =>
            $"{firstInput.CoolPropHighLevelKey}{secondInput.CoolPropHighLevelKey}_INPUTS";

        public override bool Equals(object? obj) => Equals(obj as AbstractFluid);

        public override int GetHashCode()
        {
            var hashCode = new HashCode();
            hashCode.Add(Phase);
            hashCode.Add(Compressibility);
            hashCode.Add(Conductivity);
            hashCode.Add(CriticalPressure);
            hashCode.Add(CriticalTemperature);
            hashCode.Add(DynamicViscosity);
            hashCode.Add(FreezingTemperature);
            hashCode.Add(MaxPressure);
            hashCode.Add(MinPressure);
            hashCode.Add(MolarMass);
            hashCode.Add(Prandtl);
            hashCode.Add(Quality);
            hashCode.Add(SoundSpeed);
            hashCode.Add(SurfaceTension);
            hashCode.Add(TriplePressure);
            hashCode.Add(TripleTemperature);
            return hashCode.ToHashCode();
        }

        public static bool operator ==(AbstractFluid? left, AbstractFluid? right) => Equals(left, right);

        public static bool operator !=(AbstractFluid? left, AbstractFluid? right) => !Equals(left, right);
    }
}