using System;

namespace SharpProp.Outputs
{
    internal static class OutputsValidator
    {
        /// <summary>
        ///     Validates the CoolProp output.
        /// </summary>
        /// <exception cref="ArgumentException">Invalid or not defined state!</exception>
        public static void Validate(double value)
        {
            if (double.IsInfinity(value) || double.IsNaN(value))
                throw new ArgumentException("Invalid or not defined state!");
        }
    }
}