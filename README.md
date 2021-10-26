# ![SharpProp](https://raw.githubusercontent.com/portyanikhin/SharpProp/f7f79cdc0fedbca3e4c816ef2205cfb8300e193f/SharpProp/pictures/header.svg)

[![Build & Tests](https://github.com/portyanikhin/SharpProp/actions/workflows/build-tests.yml/badge.svg)](https://github.com/portyanikhin/SharpProp/actions/workflows/build-tests.yml)
[![CodeQL](https://github.com/portyanikhin/SharpProp/actions/workflows/codeql-analysis.yml/badge.svg)](https://github.com/portyanikhin/SharpProp/actions/workflows/codeql-analysis.yml)
[![NuGet](https://img.shields.io/nuget/v/SharpProp)](https://www.nuget.org/packages/SharpProp/)
![Platform](https://img.shields.io/badge/platform-win--64%20%7C%20linux--64-lightgrey)
[![License](https://img.shields.io/github/license/portyanikhin/SharpProp)](https://github.com/portyanikhin/SharpProp/blob/master/LICENSE)
[![codecov](https://codecov.io/gh/portyanikhin/SharpProp/branch/master/graph/badge.svg?token=P3JH3D1L0Q)](https://codecov.io/gh/portyanikhin/SharpProp)

A simple, full-featured, lightweight, cross-platform [CoolProp](http://www.coolprop.org/) wrapper for C#

## Quick start

* `Fluid` class - for pure fluids and binary mixtures
* `Mixture` class - for mixtures with pure fluids components
* `FluidsList` enum - list of all available fluids
* `Input` record - inputs for the `Fluid` and `Mixture` classes
* `HumidAir` class - for humid air
* `InputHumidAir` record - inputs for the `HumidAir` class

### Unit safety

All calculations of thermophysical properties are **_unit safe_** (thanks to [UnitsNet](https://github.com/angularsen/UnitsNet)).
This allows you to avoid errors associated with incorrect dimensions of quantities,
and will help you save a lot of time on their search and elimination.
In addition, you will be able to convert all values to many other dimensions without the slightest difficulty.

### List of properties

For the `Fluid` and `Mixture` instances:

* `Compressibility` - compressibility factor _(dimensionless)_
* `Conductivity` - thermal conductivity _(by default, W/m/K)_
* `CriticalPressure` - absolute pressure at the critical point _(by default, kPa)_
* `CriticalTemperature` - temperature at the critical point _(by default, °C)_
* `Density` - mass density _(by default, kg/m3)_
* `DynamicViscosity` - dynamic viscosity _(by default, mPa*s)_
* `Enthalpy` - mass specific enthalpy _(by default, kJ/kg)_
* `Entropy` - mass specific entropy _(by default, kJ/kg/K)_
* `FreezingTemperature` - temperature at freezing point (for incompressible fluids) _(by default, °C)_
* `InternalEnergy` - mass specific internal energy _(by default, kJ/kg)_
* `MaxPressure` - maximum pressure limit _(by default, kPa)_
* `MaxTemperature` - maximum temperature limit _(by default, °C)_
* `MinPressure` - minimum pressure limit _(by default, kPa)_
* `MinTemperature` - minimum temperature limit _(by default, °C)_
* `MolarMass` - molar mass _(by default, g/mol)_
* `Phase` - phase _(enum)_
* `Prandtl` - Prandtl number _(dimensionless)_
* `Pressure` - absolute pressure _(by default, kPa)_
* `Quality` - mass vapor quality _(by default, %)_
* `SoundSpeed` - sound speed _(by default, m/s)_
* `SpecificHeat` - mass specific constant pressure specific heat _(by default, kJ/kg/K)_
* `SurfaceTension` - surface tension _(by default, N/m)_
* `Temperature` - temperature _(by default, °C)_
* `TriplePressure` - absolute pressure at the triple point _(by default, kPa)_
* `TripleTemperature` - temperature at the triple point _(by default, °C)_

For the `HumidAir` instances:

* `Compressibility` - compressibility factor _(dimensionless)_
* `Conductivity` - thermal conductivity _(by default, W/m/K)_
* `Density` - mass density per humid air unit _(by default, kg/m3)_
* `DewTemperature` - dew-point temperature _(by default, °C)_
* `DynamicViscosity` - dynamic viscosity _(by default, mPa*s)_
* `Enthalpy` - mass specific enthalpy per humid air _(by default, kJ/kg)_
* `Entropy` - mass specific entropy per humid air _(by default, kJ/kg/K)_
* `Humidity` - absolute humidity ratio _(by default, g/kg d.a.)_
* `PartialPressure` - partial pressure of water vapor _(by default, kPa)_
* `Pressure` - absolute pressure _(by default, kPa)_
* `RelativeHumidity` - relative humidity ratio _(by default, %)_
* `SpecificHeat` - mass specific constant pressure specific heat per humid air _(by default, kJ/kg/K)_
* `Temperature` - dry-bulb temperature _(by default, °C)_
* `WetBulbTemperature` - wet-bulb temperature _(by default, °C)_

**NB.** If the required property is not present in the instance of the fluid, then you can add it by extending
the `Fluid`, `Mixture` or `HumidAir` classes.

### Examples

#### Pure fluids

To calculate the specific heat of saturated water vapour at _1 atm_:

```c#
using System;
using SharpProp;
using UnitsNet.NumberExtensions.NumberToPressure;
using UnitsNet.NumberExtensions.NumberToRatio;
using UnitsNet.Units;
```

```c#
var waterVapour = new Fluid(FluidsList.Water);
waterVapour.Update(Input.Pressure(1.Atmospheres()), Input.Quality(100.Percent()));
Console.WriteLine(waterVapour.SpecificHeat.JoulesPerKilogramKelvin); // 2079.937085633241
Console.WriteLine(waterVapour.SpecificHeat);                         // 2.08 kJ/kg.K
Console.WriteLine(waterVapour.SpecificHeat
    .ToUnit(SpecificEntropyUnit.CaloriePerGramKelvin));              // 0.5 cal/g.K
```

#### Incompressible binary mixtures

To calculate the dynamic viscosity of propylene glycol aqueous solution 
with _60 %_ mass fraction at _100 kPa_ and _-20 °C_:

```c#
using System;
using SharpProp;
using UnitsNet.NumberExtensions.NumberToPressure;
using UnitsNet.NumberExtensions.NumberToRatio;
using UnitsNet.NumberExtensions.NumberToTemperature;
using UnitsNet.Units;
```

```c#
var propyleneGlycol = new Fluid(FluidsList.MPG, 60.Percent());
propyleneGlycol.Update(Input.Pressure(100.Kilopascals()), Input.Temperature((-20).DegreesCelsius()));
Console.WriteLine(propyleneGlycol.DynamicViscosity?.PascalSeconds); // 0.13907391053938878
Console.WriteLine(propyleneGlycol.DynamicViscosity);                // 139.07 mPa·s
Console.WriteLine(propyleneGlycol.DynamicViscosity?
    .ToUnit(DynamicViscosityUnit.Poise));                           // 1.39 P
```

#### Mixtures

To calculate the density of ethanol aqueous solution(with ethanol _40 %_ mass fraction) 
at _200 kPa_ and _277.15 K_:

```c#
using System;
using System.Collections.Generic;
using SharpProp;
using UnitsNet;
using UnitsNet.NumberExtensions.NumberToPressure;
using UnitsNet.NumberExtensions.NumberToRatio;
using UnitsNet.NumberExtensions.NumberToTemperature;
using UnitsNet.Units;
```

```c#
var mixture = new Mixture(new List<FluidsList> {FluidsList.Water, FluidsList.Ethanol}, 
    new List<Ratio> {60.Percent(), 40.Percent()});
mixture.Update(Input.Pressure(200.Kilopascals()), Input.Temperature(277.15.Kelvins()));
Console.WriteLine(mixture.Density.KilogramsPerCubicMeter);               // 883.3922771627759
Console.WriteLine(mixture.Density);                                      // 883.39 kg/m3
Console.WriteLine(mixture.Density.ToUnit(DensityUnit.GramPerDeciliter)); // 88.34 g/dl
```

#### Humid air

To calculate the wet bulb temperature of humid air 
at _99 kPa_, _30 °C_ and _50 %_ relative humidity:

```c#
using System;
using SharpProp;
using UnitsNet.NumberExtensions.NumberToPressure;
using UnitsNet.NumberExtensions.NumberToRelativeHumidity;
using UnitsNet.NumberExtensions.NumberToTemperature;
using UnitsNet.Units;
```

```c#
var humidAir = new HumidAir();
humidAir.Update(InputHumidAir.Pressure(99.Kilopascals()), InputHumidAir.Temperature(30.DegreesCelsius()),
    InputHumidAir.RelativeHumidity(50.Percent()));
// or use:
// var humidAir1 = 
//     HumidAir.WithState(InputHumidAir.Pressure(99.Kilopascals()), 
//         InputHumidAir.Temperature(30.DegreesCelsius()), InputHumidAir.RelativeHumidity(50.Percent()));
Console.WriteLine(humidAir.WetBulbTemperature.Kelvins); // 295.0965785590792
Console.WriteLine(humidAir.WetBulbTemperature);         // 21.95 °C
Console.WriteLine(humidAir.WetBulbTemperature
    .ToUnit(TemperatureUnit.DegreeFahrenheit));         // 71.5 °F
```

#### Converting to JSON string

For example, converting the `Fluid` instance to _indented_ JSON string:

```c#
using System;
using SharpProp;
using UnitsNet.NumberExtensions.NumberToRatio;
using UnitsNet.NumberExtensions.NumberToTemperature;
```

```c#
var refrigerant = new Fluid(FluidsList.R32);
refrigerant.Update(Input.Temperature(5.DegreesCelsius()), Input.Quality(100.Percent()));
Console.WriteLine(refrigerant.AsJson());
```

As a result:

```json
{
  "Name": "R32",
  "Fraction": {
    "Unit": "RatioUnit.Percent",
    "Value": 100.0
  },
  "Compressibility": 0.8266625877210833,
  "Conductivity": {
    "Unit": "ThermalConductivityUnit.WattPerMeterKelvin",
    "Value": 0.013435453854396475
  },
  "CriticalPressure": {
    "Unit": "PressureUnit.Kilopascal",
    "Value": 5782.0
  },
  "CriticalTemperature": {
    "Unit": "TemperatureUnit.DegreeCelsius",
    "Value": 78.10500000000002
  },
  "Density": {
    "Unit": "DensityUnit.KilogramPerCubicMeter",
    "Value": 25.89088151061046
  },
  "DynamicViscosity": {
    "Unit": "DynamicViscosityUnit.MillipascalSecond",
    "Value": 0.012606543144761657
  },
  "Enthalpy": {
    "Unit": "SpecificEnergyUnit.KilojoulePerKilogram",
    "Value": 516.1057800378023
  },
  "Entropy": {
    "Unit": "SpecificEntropyUnit.KilojoulePerKilogramKelvin",
    "Value": 2.1362654412978777
  },
  "FreezingTemperature": null,
  "InternalEnergy": {
    "Unit": "SpecificEnergyUnit.KilojoulePerKilogram",
    "Value": 479.35739743435374
  },
  "MaxPressure": {
    "Unit": "PressureUnit.Kilopascal",
    "Value": 70000.0
  },
  "MaxTemperature": {
    "Unit": "TemperatureUnit.DegreeCelsius",
    "Value": 161.85000000000002
  },
  "MinPressure": {
    "Unit": "PressureUnit.Kilopascal",
    "Value": 0.04799989387605937
  },
  "MinTemperature": {
    "Unit": "TemperatureUnit.DegreeCelsius",
    "Value": -136.80999999999997
  },
  "MolarMass": {
    "Unit": "MolarMassUnit.GramPerMole",
    "Value": 52.024
  },
  "Phase": "TwoPhase",
  "Prandtl": 1.2252282243443504,
  "Pressure": {
    "Unit": "PressureUnit.Kilopascal",
    "Value": 951.448019691762
  },
  "Quality": {
    "Unit": "RatioUnit.Percent",
    "Value": 100.0
  },
  "SoundSpeed": {
    "Unit": "SpeedUnit.MeterPerSecond",
    "Value": 209.6337575990297
  },
  "SpecificHeat": {
    "Unit": "SpecificEntropyUnit.KilojoulePerKilogramKelvin",
    "Value": 1.3057899441785379
  },
  "SurfaceTension": {
    "Unit": "ForcePerLengthUnit.NewtonPerMeter",
    "Value": 0.010110117241546162
  },
  "Temperature": {
    "Unit": "TemperatureUnit.DegreeCelsius",
    "Value": 5.0
  },
  "TriplePressure": {
    "Unit": "PressureUnit.Kilopascal",
    "Value": 0.04799989387605937
  },
  "TripleTemperature": {
    "Unit": "TemperatureUnit.DegreeCelsius",
    "Value": -136.80999999999997
  }
}
```

#### Equality of instances

You can simply determine the equality of `Fluid`, `Mixture` and `HumidAir` instances by its state. 
Just use the `Equals`method or the equality operators (`==` or `!=`). 
Exactly the same way you can compare inputs (`Input`, `InputHumidAir` or any `IKeyedInput` record).

For example:

```c#
using System;
using SharpProp;
using UnitsNet.NumberExtensions.NumberToPressure;
using UnitsNet.NumberExtensions.NumberToRelativeHumidity;
using UnitsNet.NumberExtensions.NumberToTemperature;
```

```c#
var humidAir = HumidAir.WithState(InputHumidAir.Pressure(1.Atmospheres()),
    InputHumidAir.Temperature(20.DegreesCelsius()), InputHumidAir.RelativeHumidity(50.Percent()));
var humidAirWithSameState = HumidAir.WithState(InputHumidAir.Pressure(101325.Pascals()),
    InputHumidAir.Temperature(293.15.Kelvins()), InputHumidAir.RelativeHumidity(50.Percent()));
Console.WriteLine(humidAir == humidAirWithSameState);             // true
Console.WriteLine(InputHumidAir.Pressure(1.Atmospheres()) == 
                  InputHumidAir.Pressure(101.325.Kilopascals())); // true
```

#### Adding other properties

* [Example for the `Fluid` and `Mixture`](https://github.com/portyanikhin/SharpProp/blob/master/SharpProp.Tests/Fluids/TestFluidExtended.cs)
* [Example for the `HumidAir`](https://github.com/portyanikhin/SharpProp/blob/master/SharpProp.Tests/HumidAir/TestHumidAirExtended.cs)

#### Adding other inputs

* [Example for the `Fluid` and `Mixture`](https://github.com/portyanikhin/SharpProp/blob/master/SharpProp.Tests/Fluids/TestInputExtended.cs)
* [Example for the `HumidAir`](https://github.com/portyanikhin/SharpProp/blob/master/SharpProp.Tests/HumidAir/TestInputHumidAirExtended.cs)