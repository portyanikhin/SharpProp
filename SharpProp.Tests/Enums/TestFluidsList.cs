using System;
using FluentAssertions;
using NUnit.Framework;
using SharpProp.Extensions;

namespace SharpProp.Tests
{
    public static class TestFluidsList
    {
        [Test]
        public static void TestCoolPropName([Values] FluidsList coolPropFluid)
        {
            if (coolPropFluid.CoolPropName().EndsWith(".mix"))
                Assert.Pass();
            else
                // ReSharper disable once SwitchStatementHandlesSomeKnownEnumValuesWithDefault
                switch (coolPropFluid)
                {
                    case FluidsList.R50:
                        coolPropFluid.CoolPropName().Should().Be("Methane");
                        break;
                    case FluidsList.RE143a:
                        coolPropFluid.CoolPropName().Should().Be("HFE143m");
                        break;
                    case FluidsList.R170:
                        coolPropFluid.CoolPropName().Should().Be("Ethane");
                        break;
                    case FluidsList.R290:
                        coolPropFluid.CoolPropName().Should().Be("n-Propane");
                        break;
                    case FluidsList.R600:
                        coolPropFluid.CoolPropName().Should().Be("n-Butane");
                        break;
                    case FluidsList.R600a:
                        coolPropFluid.CoolPropName().Should().Be("IsoButane");
                        break;
                    case FluidsList.R601:
                        coolPropFluid.CoolPropName().Should().Be("n-Pentane");
                        break;
                    case FluidsList.R601a:
                        coolPropFluid.CoolPropName().Should().Be("Isopentane");
                        break;
                    case FluidsList.R702:
                        coolPropFluid.CoolPropName().Should().Be("Hydrogen");
                        break;
                    case FluidsList.R704:
                        coolPropFluid.CoolPropName().Should().Be("Helium");
                        break;
                    case FluidsList.R717:
                        coolPropFluid.CoolPropName().Should().Be("Ammonia");
                        break;
                    case FluidsList.R718:
                        coolPropFluid.CoolPropName().Should().Be("Water");
                        break;
                    case FluidsList.R720:
                        coolPropFluid.CoolPropName().Should().Be("Neon");
                        break;
                    case FluidsList.R728:
                        coolPropFluid.CoolPropName().Should().Be("Nitrogen");
                        break;
                    case FluidsList.R729:
                        coolPropFluid.CoolPropName().Should().Be("Air");
                        break;
                    case FluidsList.R732:
                        coolPropFluid.CoolPropName().Should().Be("Oxygen");
                        break;
                    case FluidsList.R740:
                        coolPropFluid.CoolPropName().Should().Be("Argon");
                        break;
                    case FluidsList.R744:
                        coolPropFluid.CoolPropName().Should().Be("CarbonDioxide");
                        break;
                    case FluidsList.R764:
                        coolPropFluid.CoolPropName().Should().Be("SulfurDioxide");
                        break;
                    case FluidsList.R846:
                        coolPropFluid.CoolPropName().Should().Be("SulfurHexafluoride");
                        break;
                    case FluidsList.R1150:
                        coolPropFluid.CoolPropName().Should().Be("Ethylene");
                        break;
                    case FluidsList.R1270:
                        coolPropFluid.CoolPropName().Should().Be("Propylene");
                        break;
                    case FluidsList.Butene:
                        coolPropFluid.CoolPropName().Should().Be("1-Butene");
                        break;
                    case FluidsList.WaterIncomp:
                        coolPropFluid.CoolPropName().Should().Be("Water");
                        break;
                    default:
                        coolPropFluid.ToString().Should().Be(coolPropFluid.CoolPropName().RemoveChars('-', '(', ')'));
                        break;
                }
        }

        private static string RemoveChars(this string s, params char[] charsToRemove) =>
            string.Join("", s.Split(charsToRemove, StringSplitOptions.RemoveEmptyEntries));
    }
}