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