using System;
using NUnit.Framework;
using SharpProp.Extensions;

namespace SharpProp.Tests
{
    public static class TestFluidsList
    {
        [Test]
        public static void TestCoolPropName([Values] FluidsList coolPropFluid)
        {
            switch (coolPropFluid)
            {
                case FluidsList.Butene:
                    Assert.AreEqual("1-Butene", coolPropFluid.CoolPropName());
                    break;
                case FluidsList.WaterIncomp:
                    Assert.AreEqual("Water", coolPropFluid.CoolPropName());
                    break;
                default:
                    Assert.AreEqual(coolPropFluid.CoolPropName().RemoveChars('-', '(', ')'), coolPropFluid.ToString());
                    break;
            }
        }

        private static string RemoveChars(this string s, params char[] charsToRemove) =>
            string.Join("", s.Split(charsToRemove, StringSplitOptions.RemoveEmptyEntries));
    }
}