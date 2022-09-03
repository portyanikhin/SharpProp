using System;

namespace SharpProp
{
    /// <summary>
    ///     CoolProp outputs validator.
    /// </summary>
    internal class OutputsValidator
    {
        /// <summary>
        ///     CoolProp outputs validator.
        /// </summary>
        /// <param name="output">CoolProp output.</param>
        public OutputsValidator(double output) => Output = output;

        private double Output { get; }

        /// <summary>
        ///     Validates the CoolProp output.
        /// </summary>
        /// <exception cref="ArgumentException">Invalid or not defined state!</exception>
        public void Validate()
        {
            if (double.IsInfinity(Output) || double.IsNaN(Output))
                throw new ArgumentException("Invalid or not defined state!");
        }
    }
}