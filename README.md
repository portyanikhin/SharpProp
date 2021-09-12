# SharpProp

[![License](https://img.shields.io/badge/license-MIT-green)](https://github.com/portyanikhin/SharpProp/blob/master/LICENSE)
![Coverage](https://img.shields.io/badge/coverage-100%25-brightgreen)

A simple, full-featured, lightweight [CoolProp] wrapper for C#

[CoolProp]: http://www.coolprop.org/

## NuGet

The project gets published on [NuGet].

[NuGet]: https://www.nuget.org/packages/SharpProp/

## Quick start

All calculations of thermophysical properties are performed in _SI units_.

The `Fluid` class is responsible for pure fluids and binary mixtures,
the `Mixture` class - for mixtures with pure fluids components,
the `HumidAir` class - for humid air.

The `FluidsList` is an enum of all available fluids.

### List of properties

For the `Fluid` and `Mixture` instances:
* `Compressibility` - compressibility factor (-)
* `Conductivity` - thermal conductivity (W/m/K)
* `CriticalPressure` - absolute pressure at the critical point (Pa)
* `CriticalTemperature` - absolute temperature at the critical point (K)
* `Density` - mass density (kg/m3)
* `DynamicViscosity` - dynamic viscosity (Pa*s)
* `Enthalpy` - mass specific enthalpy (J/kg)
* `Entropy` - mass specific entropy (J/kg/K)
* `FreezingTemperature` - temperature at freezing point (for incompressible fluids) (K)
* `InternalEnergy` - mass specific internal energy (J/kg)
* `MaxPressure` - maximum pressure limit (Pa)
* `MaxTemperature` - maximum temperature limit (K)
* `MinPressure` - minimum pressure limit (Pa)
* `MinTemperature` - minimum temperature limit (K)
* `MolarMass` - molar mass (kg/mol)
* `Phase` - phase
* `Prandtl` - Prandtl number (-)
* `Pressure` - absolute pressure (Pa)
* `Quality` - mass vapor quality (-)
* `SoundSpeed` - sound speed (m/s)
* `SpecificHeat` - mass specific constant pressure specific heat (J/kg/K)
* `SurfaceTension` - surface tension (N/m)
* `Temperature` - absolute temperature (K)
* `TriplePressure` - absolute pressure at the triple point (Pa)
* `TripleTemperature` - absolute temperature at the triple point (K)

For the `HumidAir` instances:
* `Compressibility` - compressibility factor (-)
* `Conductivity` - thermal conductivity (W/m/K)
* `Density` - mass density per humid air unit (kg/m3)
* `DewTemperature` - dew-point absolute temperature (K)
* `DynamicViscosity` - dynamic viscosity (Pa*s)
* `Enthalpy` - mass specific enthalpy per humid air (J/kg)
* `Entropy` - mass specific entropy per humid air (J/kg/K)
* `Humidity` - absolute humidity ratio (kg/kg d.a.)
* `PartialPressure` - partial pressure of water vapor (Pa)
* `Pressure` - absolute pressure (Pa)
* `RelativeHumidity` - relative humidity ratio (from 0 to 1) (-)
* `SpecificHeat` - mass specific constant pressure specific heat per humid air (J/kg/K)
* `Temperature` - absolute dry-bulb temperature (K)
* `WetBulbTemperature` - absolute wet-bulb temperature (K)

**NB.** If the required property is not present in the instance of the fluid,
then you can add it by extending the `Fluid`, `Mixture` or `HumidAir` classes - _examples below_.

### Examples

#### Pure fluids

To calculate the specific heat of saturated water vapour at _101325 Pa_:

```c#
using System;
using SharpProp;

namespace TestProject
{
    internal static class Program
    {
        private static void Main()
        {
            var waterVapour = new Fluid(FluidsList.Water);
            waterVapour.Update(Input.Pressure(101325), Input.Quality(1));
            Console.WriteLine(waterVapour.SpecificHeat); // 2079.937085633241
        }
    }
}
```

#### Incompressible binary mixtures

To calculate the dynamic viscosity of propylene glycol aqueous solution
with _60 %_ mass fraction at _101325 Pa_ and _253.15 K_:

```c#
using System;
using SharpProp;

namespace TestProject
{
    internal static class Program
    {
        private static void Main()
        {
            var propyleneGlycol = new Fluid(FluidsList.MPG, 0.6);
            propyleneGlycol.Update(Input.Pressure(101325), Input.Temperature(253.15));
            Console.WriteLine(propyleneGlycol.DynamicViscosity); // 0.13907391053938847
        }
    }
}
```

#### Mixtures

To calculate the density of ethanol aqueous solution (with ethanol _40 %_ mass fraction)
at _200 kPa_ and _277.15 K_:

```c#
using System;
using System.Collections.Generic;
using SharpProp;

namespace TestProject
{
    internal static class Program
    {
        private static void Main()
        {
            var mixture = new Mixture(new List<FluidsList> {FluidsList.Water, FluidsList.Ethanol},
                new List<double> {0.6, 0.4});
            mixture.Update(Input.Pressure(200e3), Input.Temperature(277.15));
            Console.WriteLine(mixture.Density); // 883.3922771627759
        }
    }
}
```

#### Humid air

To calculate the wet bulb temperature of humid air at _99 kPa_, _303.15 K_ and _50 %_
relative humidity:

```c#
using System;
using SharpProp;

namespace TestProject
{
    internal static class Program
    {
        private static void Main()
        {
            var humidAir = new HumidAir();
            humidAir.Update(InputHumidAir.Pressure(99e3), InputHumidAir.Temperature(303.15),
                InputHumidAir.RelativeHumidity(0.5));
            // or use:
            // var humidAir = HumidAir.WithState(InputHumidAir.Pressure(99e3), InputHumidAir.Temperature(303.15),
            //     InputHumidAir.RelativeHumidity(0.5));
            Console.WriteLine(humidAir.WetBulbTemperature); // 295.0965785590792
        }
    }
}
```

#### Adding other properties or inputs

See an examples in [SharpProp.Tests/Fluids] and [SharpProp.Tests/HumidAir].

[SharpProp.Tests/Fluids]: https://github.com/portyanikhin/SharpProp/tree/master/SharpProp.Tests/Fluids
[SharpProp.Tests/HumidAir]: https://github.com/portyanikhin/SharpProp/tree/master/SharpProp.Tests/HumidAir