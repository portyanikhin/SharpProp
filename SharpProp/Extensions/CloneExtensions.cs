namespace SharpProp
{
    public static class CloneExtensions
    {
        /// <summary>
        ///     Performs deep (full) copy of the fluid instance.
        /// </summary>
        /// <param name="instance">The fluid instance.</param>
        /// <returns>Deep copy of the fluid instance.</returns>
        public static Fluid Clone(this Fluid instance) =>
            instance.Factory()
                .WithState(instance.Inputs[0], instance.Inputs[1]);

        /// <summary>
        ///     Performs deep (full) copy of the mixture instance.
        /// </summary>
        /// <param name="instance">The mixture instance.</param>
        /// <returns>Deep copy of the mixture instance.</returns>
        public static Mixture Clone(this Mixture instance) =>
            instance.Factory()
                .WithState(instance.Inputs[0], instance.Inputs[1]);

        /// <summary>
        ///     Performs deep (full) copy of the humid air instance.
        /// </summary>
        /// <param name="instance">The humid air instance.</param>
        /// <returns>Deep copy of the humid air instance.</returns>
        public static HumidAir Clone(this HumidAir instance) =>
            instance.Factory()
                .WithState(instance.Inputs[0], instance.Inputs[1], instance.Inputs[2]);
    }
}