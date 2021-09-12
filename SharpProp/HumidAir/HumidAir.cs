using System;
using System.Collections.Generic;
using System.Linq;
using CoolProp;

namespace SharpProp
{
    public partial class HumidAir
    {
        /// <summary>
        ///     Returns a new <see cref="HumidAir" /> object with a defined state
        /// </summary>
        /// <param name="fistInput">First input property</param>
        /// <param name="secondInput">Second input property</param>
        /// <param name="thirdInput">Third input property</param>
        /// <returns>A new <see cref="HumidAir" /> object with a defined state</returns>
        public static HumidAir WithState(IKeyedInput<string> fistInput, IKeyedInput<string> secondInput,
            IKeyedInput<string> thirdInput)
        {
            var humidAir = new HumidAir();
            humidAir.Update(fistInput, secondInput, thirdInput);
            return humidAir;
        }

        /// <summary>
        ///     Update humid air state with three inputs
        /// </summary>
        /// <param name="fistInput">First input property</param>
        /// <param name="secondInput">Second input property</param>
        /// <param name="thirdInput">Third input property</param>
        /// <exception cref="ArgumentException">Need to define 3 unique inputs!</exception>
        public void Update(IKeyedInput<string> fistInput, IKeyedInput<string> secondInput,
            IKeyedInput<string> thirdInput)
        {
            Reset();
            _inputs = new List<IKeyedInput<string>> {fistInput, secondInput, thirdInput};
            CheckInputs();
        }

        /// <summary>
        ///     Reset all properties
        /// </summary>
        protected virtual void Reset()
        {
            _compressibility = _conductivity = _density = _dewTemperature = _dynamicViscosity = null;
            _enthalpy = _entropy = _humidity = _partialPressure = _pressure = _relativeHumidity = null;
            _specificHeat = _temperature = _wetBulbTemperature = null;
        }

        /// <summary>
        ///     Returns not nullable keyed output
        /// </summary>
        /// <param name="key">Key of output</param>
        /// <returns>A not nullable keyed output</returns>
        /// <exception cref="ArgumentException">Need to define 3 unique inputs!</exception>
        /// <exception cref="ArgumentException">Invalid humid air state!</exception>
        protected double KeyedOutput(string key)
        {
            CheckInputs();
            var value = CP.HAPropsSI(key, _inputs[0].CoolPropKey, _inputs[0].Value,
                _inputs[1].CoolPropKey, _inputs[1].Value, _inputs[2].CoolPropKey, _inputs[2].Value);
            if (double.IsPositiveInfinity(value) || double.IsNegativeInfinity(value) || double.IsNaN(value))
                throw new ArgumentException("Invalid humid air state!");
            return value;
        }

        private void CheckInputs()
        {
            var uniqueKeys = _inputs.Select(input => input.CoolPropKey).Distinct().ToList();
            if (_inputs.Count != 3 || uniqueKeys.Count != 3)
                throw new ArgumentException("Need to define 3 unique inputs!");
        }
    }
}