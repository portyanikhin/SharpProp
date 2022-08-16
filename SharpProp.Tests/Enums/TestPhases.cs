using CoolProp;
using FluentAssertions;
using NUnit.Framework;

namespace SharpProp.Tests;

public static class TestPhases
{
    [Test]
    [Sequential]
    public static void CompareWithCoolPropPhases([Values] phases coolPropPhase,
        [Values] Phases sharpPropPhase) =>
        sharpPropPhase.Should().HaveSameValueAs(coolPropPhase);
}