using CoolProp;
using NUnit.Framework;
using UnitsNet;
using UnitsNet.NumberExtensions.NumberToPressure;
using UnitsNet.NumberExtensions.NumberToTemperature;
using UnitsNet.Units;

namespace SharpProp.Tests
{
    public class TestFluidExtended
    {
        private FluidExtended _fluid = null!;

        [SetUp]
        public void SetUp()
        {
            _fluid = new FluidExtended(FluidsList.Water);
            _fluid.Update(Input.Pressure(1.Atmospheres()), Input.Temperature(20.DegreesCelsius()));
        }

        [Test(ExpectedResult = 4156.6814728615545)]
        public double TestSpecificHeatConstVolume() => _fluid.SpecificHeatConstVolume.JoulesPerKilogramKelvin;

        [Test(ExpectedResult = 55408.953697937126)]
        public double? TestMolarDensity() => _fluid.MolarDensity?.KilogramsPerMole;

        [Test(ExpectedResult = null)]
        public double? TestOzoneDepletionPotential() => _fluid.OzoneDepletionPotential;

        /// <summary>
        ///     An example of how to add new properties to a <see cref="Fluid" />.
        /// </summary>
        private class FluidExtended : Fluid
        {
            private SpecificEntropy? _specificHeatConstVolume;
            private MolarMass? _molarDensity;
            private double? _ozoneDepletionPotential;

            public FluidExtended(FluidsList name, Ratio? fraction = null) : base(name, fraction)
            {
            }

            /// <summary>
            ///     Mass specific constant volume specific heat.
            /// </summary>
            public SpecificEntropy SpecificHeatConstVolume => _specificHeatConstVolume ??=
                SpecificEntropy.FromJoulesPerKilogramKelvin(KeyedOutput(Parameters.iCvmass))
                    .ToUnit(SpecificEntropyUnit.KilojoulePerKilogramKelvin);

            /// <summary>
            ///     Molar density.
            /// </summary>
            public MolarMass? MolarDensity => _molarDensity ??=
                KeyedOutputIsNotNull(Parameters.iDmolar, out var output)
                    ? UnitsNet.MolarMass.FromKilogramsPerMole(output!.Value)
                    : null;

            /// <summary>
            ///     Ozone depletion potential (ODP).
            /// </summary>
            public double? OzoneDepletionPotential => _ozoneDepletionPotential ??= NullableKeyedOutput(Parameters.iODP);

            protected override void Reset()
            {
                base.Reset();
                _specificHeatConstVolume = null;
                _molarDensity = null;
                _ozoneDepletionPotential = null;
            }
        }
    }
}