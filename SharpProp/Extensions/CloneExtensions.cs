using Force.DeepCloner;

namespace SharpProp.Extensions
{
    public static class CloneExtensions
    {
        /// <summary>
        ///     Performs deep (full) copy of the <see cref="Fluid" /> instance.
        /// </summary>
        /// <param name="instance">The <see cref="Fluid" /> instance.</param>
        /// <returns>Deep copy of the <see cref="Fluid" /> instance.</returns>
        public static Fluid Clone(this Fluid instance) => 
            instance.DeepClone();
        
        /// <summary>
        ///     Performs deep (full) copy of the <see cref="Mixture" /> instance.
        /// </summary>
        /// <param name="instance">The <see cref="Mixture" /> instance.</param>
        /// <returns>Deep copy of the <see cref="Mixture" /> instance.</returns>
        public static Mixture Clone(this Mixture instance) => 
            instance.DeepClone();
        
        /// <summary>
        ///     Performs deep (full) copy of the <see cref="HumidAir" /> instance.
        /// </summary>
        /// <param name="instance">The <see cref="HumidAir" /> instance.</param>
        /// <returns>Deep copy of the <see cref="HumidAir" /> instance.</returns>
        public static HumidAir Clone(this HumidAir instance) => 
            instance.DeepClone();
    }
}