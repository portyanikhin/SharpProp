using System;
using CoolProp;
using SharpProp.Validators;

namespace SharpProp
{
    public abstract partial class AbstractFluid
    {
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
        public virtual AbstractFluid WithState(IKeyedInput<parameters> firstInput, IKeyedInput<parameters> secondInput)
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
        public void Update(IKeyedInput<parameters> firstInput, IKeyedInput<parameters> secondInput)
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
            _compressibility = _conductivity = _density = _dynamicViscosity = _enthalpy = null;
            _entropy = _internalEnergy = _prandtl = _pressure = _quality = null;
            _soundSpeed = _specificHeat = _surfaceTension = _temperature = null;
            _phase = null;
        }

        /// <summary>
        ///     Returns nullable keyed output
        /// </summary>
        /// <param name="key">Key of output</param>
        /// <returns>A nullable keyed output</returns>
        protected double? NullableKeyedOutput(parameters key)
        {
            try
            {
                var value = KeyedOutput(key);
                if (key is parameters.iQ && value is < 0 or > 1)
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
        protected double KeyedOutput(parameters key)
        {
            var value = Backend.keyed_output(key);
            OutputsValidator.Validate(value);
            return value;
        }

        private static (input_pairs inputPair, double firstValue, double secondValue) GenerateUpdatePair(
            IKeyedInput<parameters> firstInput, IKeyedInput<parameters> secondInput)
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

        private static input_pairs? GetInputPair(IKeyedInput<parameters> firstInput,
            IKeyedInput<parameters> secondInput)
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
            GetInputPairName(IKeyedInput<parameters> firstInput, IKeyedInput<parameters> secondInput) =>
            $"{firstInput.CoolPropHighLevelKey}{secondInput.CoolPropHighLevelKey}_INPUTS";
    }
}