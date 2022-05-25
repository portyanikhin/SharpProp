using CoolProp;
using NUnit.Framework;
using UnitsNet;
using UnitsNet.NumberExtensions.NumberToPressure;
using UnitsNet.NumberExtensions.NumberToTemperature;
using UnitsNet.Units;

namespace SharpProp.Tests
{
    public static class TestFluidExtended
    {
        private static FluidExtended Fluid =>
            new FluidExtended(FluidsList.Water)
                .WithState(Input.Pressure(1.Atmospheres()),
                    Input.Temperature(20.DegreesCelsius()));

        [Test(ExpectedResult = 4156.6814728615545)]
        public static double TestSpecificHeatConstVolume() =>
            Fluid.SpecificHeatConstVolume.JoulesPerKilogramKelvin;

        [Test(ExpectedResult = 55408.953697937126)]
        public static double? TestMolarDensity() =>
            Fluid.MolarDensity?.KilogramsPerMole;

        [Test(ExpectedResult = null)]
        public static double? TestOzoneDepletionPotential() =>
            Fluid.OzoneDepletionPotential;

        /// <summary>
        ///     An example of how to add new properties to a <see cref="Fluid" />.
        /// </summary>
        private class FluidExtended : Fluid
        {
            private MolarMass? _molarDensity;
            private double? _ozoneDepletionPotential;
            private SpecificEntropy? _specificHeatConstVolume;

            public FluidExtended(FluidsList name, Ratio? fraction = null) :
                base(name, fraction)
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
            public double? OzoneDepletionPotential =>
                _ozoneDepletionPotential ??= NullableKeyedOutput(Parameters.iODP);

            protected override void Reset()
            {
                base.Reset();
                _specificHeatConstVolume = null;
                _molarDensity = null;
                _ozoneDepletionPotential = null;
            }

            public override FluidExtended Factory() => new(Name, Fraction);

            public override FluidExtended WithState(IKeyedInput<Parameters> firstInput,
                IKeyedInput<Parameters> secondInput) =>
                (FluidExtended) base.WithState(firstInput, secondInput);
        }
    }
}