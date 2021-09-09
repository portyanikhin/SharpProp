using CoolProp;
using NUnit.Framework;

namespace SharpProp.Tests
{
    public class TestFluidExtended
    {
        private FluidExtended _fluid = null!;

        [SetUp]
        public void SetUp()
        {
            _fluid = new FluidExtended(FluidsList.Water);
            _fluid.Update(Input.Pressure(101325), Input.Temperature(293.15));
        }

        [Test(ExpectedResult = 55408.953697937126)]
        public double TestMolarDensity() => _fluid.MolarDensity;

        [Test(ExpectedResult = null)]
        public double? TestOzoneDepletionPotential() => _fluid.OzoneDepletionPotential;

        /// <summary>
        ///     An example of how to add new properties to a <see cref="Fluid" />
        /// </summary>
        private class FluidExtended : Fluid
        {
            private double? _molarDensity;
            private double? _ozoneDepletionPotential;

            public FluidExtended(FluidsList name, double? fraction = null) : base(name, fraction)
            {
            }

            /// <summary>
            ///     Molar density [kg/mol]
            /// </summary>
            public double MolarDensity => _molarDensity ??= KeyedOutput(parameters.iDmolar);

            /// <summary>
            ///     Ozone depletion potential (ODP) [-]
            /// </summary>
            public double? OzoneDepletionPotential => _ozoneDepletionPotential ??= NullableKeyedOutput(parameters.iODP);

            protected override void Reset()
            {
                base.Reset();
                _molarDensity = _ozoneDepletionPotential = null;
            }
        }
    }
}