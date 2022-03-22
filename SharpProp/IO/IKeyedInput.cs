using CoolProp;

namespace SharpProp
{
    /// <summary>
    ///     CoolProp keyed input interface.
    /// </summary>
    /// <typeparam name="T">
    ///     Type of Coolprop internal keys (<see cref="Parameters" /> for fluids and mixtures;
    ///     <see cref="string" /> for humid air).
    /// </typeparam>
    public interface IKeyedInput<out T>
    {
        /// <summary>
        ///     CoolProp internal key.
        /// </summary>
        public T CoolPropKey { get; }

        /// <summary>
        ///     CoolProp key in high-level interface.
        /// </summary>
        public string CoolPropHighLevelKey => 
            CoolPropKey!.ToString()!.TrimStart('i');

        /// <summary>
        ///     Input value in SI units.
        /// </summary>
        public double Value { get; }
    }
}