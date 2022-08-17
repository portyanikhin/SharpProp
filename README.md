# ![SharpProp](https://raw.githubusercontent.com/portyanikhin/SharpProp/master/SharpProp/pictures/header.svg)

[![Build & Tests](https://github.com/portyanikhin/SharpProp/actions/workflows/build-tests.yml/badge.svg)](https://github.com/portyanikhin/SharpProp/actions/workflows/build-tests.yml)
[![CodeQL](https://github.com/portyanikhin/SharpProp/actions/workflows/codeql-analysis.yml/badge.svg)](https://github.com/portyanikhin/SharpProp/actions/workflows/codeql-analysis.yml)
[![NuGet](https://img.shields.io/nuget/v/SharpProp)](https://www.nuget.org/packages/SharpProp)
![Platform](https://img.shields.io/badge/platform-win--64%20%7C%20linux--64-lightgrey)
[![License](https://img.shields.io/github/license/portyanikhin/SharpProp)](https://github.com/portyanikhin/SharpProp/blob/master/LICENSE)
[![Codecov](https://codecov.io/gh/portyanikhin/SharpProp/branch/master/graph/badge.svg?token=P3JH3D1L0Q)](https://codecov.io/gh/portyanikhin/SharpProp)

A simple, full-featured, lightweight, cross-platform [CoolProp](http://www.coolprop.org) wrapper for C#.

## Navigation

- [How to install](#how-to-install)
- [Unit safety](#unit-safety)
- [Project structure](#project-structure)
- [List of properties](#list-of-properties)
    - [Properties of `Fluid` and `Mixture` instances](#properties-of-fluid-and-mixture-instances)
    - [Properties of `HumidAir` instances](#properties-of-humidair-instances)
- [List of methods](#list-of-methods)
    - [Methods of `Fluid` instances](#methods-of-fluid-instances)
    - [Methods of `Mixture` instances](#methods-of-mixture-instances)
    - [Methods of `HumidAir` instances](#methods-of-humidair-instances)
- [Examples](#examples)
    - [Pure fluids](#pure-fluids)
    - [Incompressible binary mixtures](#incompressible-binary-mixtures)
    - [Mixtures](#mixtures)
    - [Humid air](#humid-air)
    - [Equality of instances](#equality-of-instances)
    - [Converting to a JSON string](#converting-to-a-json-string)
    - [Deep cloning](#deep-cloning)
    - [Adding other properties](#adding-other-properties)
    - [Adding other inputs](#adding-other-inputs)

## How to install

Run the following command in the
[Package Manager Console](http://docs.nuget.org/docs/start-here/using-the-package-manager-console):

```shell
PM> Install-Package SharpProp -Version 4.2.4
```

Or add this to the `.csproj` file:

```xml
<ItemGroup>
    <PackageReference Include="SharpProp" Version="4.2.4"/>
</ItemGroup>
```

## Unit safety

All calculations of thermophysical properties are **_unit safe_** 
(thanks to [UnitsNet](https://github.com/angularsen/UnitsNet)).
This allows you to avoid errors associated with incorrect dimensions of quantities,
and will help you save a lot of time on their search and elimination.
In addition, you will be able to convert all values to many other dimensions without the slightest difficulty.

## Project structure

* `Fluid` class - for pure fluids and binary mixtures.
* `Mixture` class - for mixtures with pure fluids components.
* `FluidsList` enum - list of all available fluids.
* `Input` record - inputs for the `Fluid` and `Mixture` classes.
* `HumidAir` class - for humid air.
* `InputHumidAir` record - inputs for the `HumidAir` class.

## List of properties

If the required property is not present in the instance of the fluid, then you can add it by extending
the `Fluid`, `Mixture` or `HumidAir` classes (see [how to add other properties](#adding-other-properties)).

### Properties of `Fluid` and `Mixture` instances

* `Compressibility` - compressibility factor _(dimensionless)_.
* `Conductivity` - thermal conductivity _(by default, W/m/K)_.
* `CriticalPressure` - absolute pressure at the critical point _(by default, kPa)_.
* `CriticalTemperature` - temperature at the critical point _(by default, °C)_.
* `Density` - mass density _(by default, kg/m3)_.
* `DynamicViscosity` - dynamic viscosity _(by default, mPa*s)_.
* `Enthalpy` - mass specific enthalpy _(by default, kJ/kg)_.
* `Entropy` - mass specific entropy _(by default, kJ/kg/K)_.
* `FreezingTemperature` - temperature at freezing point (for incompressible fluids) _(by default, °C)_.
* `InternalEnergy` - mass specific internal energy _(by default, kJ/kg)_.
* `KinematicViscosity` - kinematic viscosity _(by default, cSt)_.
* `MaxPressure` - maximum pressure limit _(by default, kPa)_.
* `MaxTemperature` - maximum temperature limit _(by default, °C)_.
* `MinPressure` - minimum pressure limit _(by default, kPa)_.
* `MinTemperature` - minimum temperature limit _(by default, °C)_.
* `MolarMass` - molar mass _(by default, g/mol)_.
* `Phase` - phase state _(enum)_.
* `Prandtl` - Prandtl number _(dimensionless)_.
* `Pressure` - absolute pressure _(by default, kPa)_.
* `Quality` - mass vapor quality _(by default, %)_.
* `SoundSpeed` - sound speed _(by default, m/s)_.
* `SpecificHeat` - mass specific constant pressure specific heat _(by default, kJ/kg/K)_.
* `SurfaceTension` - surface tension _(by default, N/m)_.
* `Temperature` - temperature _(by default, °C)_.
* `TriplePressure` - absolute pressure at the triple point _(by default, kPa)_.
* `TripleTemperature` - temperature at the triple point _(by default, °C)_.

### Properties of `HumidAir` instances

* `Compressibility` - compressibility factor _(dimensionless)_.
* `Conductivity` - thermal conductivity _(by default, W/m/K)_.
* `Density` - mass density per humid air unit _(by default, kg/m3)_.
* `DewTemperature` - dew-point temperature _(by default, °C)_.
* `DynamicViscosity` - dynamic viscosity _(by default, mPa*s)_.
* `Enthalpy` - mass specific enthalpy per humid air _(by default, kJ/kg)_.
* `Entropy` - mass specific entropy per humid air _(by default, kJ/kg/K)_.
* `Humidity` - absolute humidity ratio _(by default, g/kg d.a.)_.
* `KinematicViscosity` - kinematic viscosity _(by default, cSt)_.
* `PartialPressure` - partial pressure of water vapor _(by default, kPa)_.
* `Prandtl` - Prandtl number _(dimensionless)_.
* `Pressure` - absolute pressure _(by default, kPa)_.
* `RelativeHumidity` - relative humidity ratio _(by default, %)_.
* `SpecificHeat` - mass specific constant pressure specific heat per humid air _(by default, kJ/kg/K)_.
* `Temperature` - dry-bulb temperature _(by default, °C)_.
* `WetBulbTemperature` - wet-bulb temperature _(by default, °C)_.

## List of methods

For more information, see the XML documentation.

### Methods of `Fluid` instances

* `Factory` - returns a new fluid object with no defined state.
* `WithState` - returns a new fluid object with a defined state.
* `Update` - update fluid state.
* `IsentropicCompressionTo` - the process of isentropic compression to a given pressure.
* `CompressionTo` - the process of compression to a given pressure.
* `IsenthalpicExpansionTo` - the process of isenthalpic expansion to a given pressure.
* `IsentropicExpansionTo` - the process of isentropic expansion to a given pressure.
* `ExpansionTo` - the process of expansion to a given pressure.
* `CoolingTo` - the process of cooling to a given temperature or enthalpy.
* `HeatingTo` - the process of heating to a given temperature or enthalpy.
* `BubblePointAt` - bubble point at a given pressure or temperature.
* `DewPointAt` - dew point at a given pressure or temperature.
* `TwoPhasePointAt` - two-phase point at a given pressure.
* `Mixing` - the mixing process.

### Methods of `Mixture` instances

* `Factory` - returns a new fluid object with no defined state.
* `WithState` - returns a new fluid object with a defined state.
* `Update` - update fluid state.
* `CoolingTo` - the process of cooling to a given temperature.
* `HeatingTo` - the process of heating to a given temperature.

### Methods of `HumidAir` instances

* `Factory` - returns a new humid air object with no defined state.
* `WithState` - returns a new humid air object with a defined state.
* `Update` - update humid air state.
* `DryCoolingTo` - the process of cooling without dehumidification to a given temperature or enthalpy.
* `WetCoolingTo` - the process of cooling with dehumidification to a given temperature or enthalpy and relative or absolute humidity ratio.
* `HeatingTo` - the process of heating to a given temperature or enthalpy.
* `HumidificationByWaterTo` - the process of humidification by water (isenthalpic) to a given relative or absolute humidity ratio.
* `HumidificationBySteamTo` - the process of humidification by steam (isothermal) to a given relative or absolute humidity ratio.
* `Mixing` - the mixing process.

## Examples

### Pure fluids

To calculate the specific heat of saturated water vapor at _1 atm_:

```c#
var waterVapour = new Fluid(FluidsList.Water)
    .DewPointAt((1).Atmospheres());
Console.WriteLine(waterVapour.SpecificHeat.JoulesPerKilogramKelvin); // 2079.937085633241
Console.WriteLine(waterVapour.SpecificHeat);                         // 2.08 kJ/kg·K
Console.WriteLine(waterVapour.SpecificHeat
    .ToUnit(SpecificEntropyUnit.CaloriePerGramKelvin));              // 0.5 cal/g·K
```

### Incompressible binary mixtures

To calculate the dynamic viscosity of propylene glycol aqueous solution 
with _60 %_ mass fraction at _100 kPa_ and _-20 °C_:

```c#
var propyleneGlycol = new Fluid(FluidsList.MPG, (60).Percent())
    .WithState(Input.Pressure((100).Kilopascals()),
        Input.Temperature((-20).DegreesCelsius()));
Console.WriteLine(propyleneGlycol.DynamicViscosity?.PascalSeconds); // 0.13907391053938878
Console.WriteLine(propyleneGlycol.DynamicViscosity);                // 139.07 mPa·s
Console.WriteLine(propyleneGlycol.DynamicViscosity?
    .ToUnit(DynamicViscosityUnit.Poise));                           // 1.39 P
```

### Mixtures

To calculate the density of ethanol aqueous solution (with ethanol _40 %_ mass fraction) 
at _200 kPa_ and _277.15 K_:

```c#
var mixture = new Mixture(
    new List<FluidsList> {FluidsList.Water, FluidsList.Ethanol},
    new List<Ratio> {(60).Percent(), (40).Percent()})
    .WithState(Input.Pressure((200).Kilopascals()),
        Input.Temperature((277.15).Kelvins()));
Console.WriteLine(mixture.Density.KilogramsPerCubicMeter);               // 883.3922771627759
Console.WriteLine(mixture.Density);                                      // 883.39 kg/m3
Console.WriteLine(mixture.Density.ToUnit(DensityUnit.GramPerDeciliter)); // 88.34 g/dl
```

### Humid air

To calculate the wet bulb temperature of humid air 
at _99 kPa_, _30 °C_ and _50 %_ relative humidity:

```c#
var humidAir = new HumidAir().WithState(
    InputHumidAir.Pressure((99).Kilopascals()),
    InputHumidAir.Temperature((30).DegreesCelsius()),
    InputHumidAir.RelativeHumidity((50).Percent()));
Console.WriteLine(humidAir.WetBulbTemperature.Kelvins); // 295.0965785590792
Console.WriteLine(humidAir.WetBulbTemperature);         // 21.95 °C
Console.WriteLine(humidAir.WetBulbTemperature
    .ToUnit(TemperatureUnit.DegreeFahrenheit));         // 71.5 °F
```

### Equality of instances

You can simply determine the equality of `Fluid`, `Mixture` and `HumidAir` instances by its state.
Just use the `Equals` method or the equality operators (`==` or `!=`).
Exactly the same way you can compare inputs (`Input`, `InputHumidAir` or any `IKeyedInput` record).

For example:

```c#
var humidAir = new HumidAir().WithState(
    InputHumidAir.Pressure((1).Atmospheres()),
    InputHumidAir.Temperature((20).DegreesCelsius()),
    InputHumidAir.RelativeHumidity((50).Percent()));
var sameHumidAir = new HumidAir().WithState(
    InputHumidAir.Pressure((101325).Pascals()),
    InputHumidAir.Temperature((293.15).Kelvins()),
    InputHumidAir.RelativeHumidity((50).Percent()));
Console.WriteLine(humidAir == sameHumidAir);          // true
Console.WriteLine(
    InputHumidAir.Pressure((1).Atmospheres()) ==
    InputHumidAir.Pressure((101.325).Kilopascals())); // true
```

### Converting to a JSON string

The `Fluid`, `Mixture` and `HumidAir` classes have an extension method `AsJson`,
which performs converting of instance to a JSON string.
For example, converting a `Fluid` instance to an _indented_ JSON string:

```c#
var refrigerant = new Fluid(FluidsList.R32)
    .DewPointAt((5).DegreesCelsius());
Console.WriteLine(refrigerant.AsJson());
```

As a result:

```json5
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
  "KinematicViscosity": {
    "Unit": "KinematicViscosityUnit.Centistokes",
    "Value": 0.48691054182899535
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

### Deep cloning

The `Fluid`, `Mixture` and `HumidAir` classes have an extension method `Clone`, 
which performs deep (full) copy of instance:

```c#
var origin = new Fluid(FluidsList.Water)
    .WithState(Input.Pressure((1).Atmospheres()),
        Input.Temperature((20).DegreesCelsius()));
var clone = origin.Clone();
Console.WriteLine(origin == clone); // true
clone.Update(Input.Pressure((1).Atmospheres()),
    Input.Temperature((30).DegreesCelsius()));
Console.WriteLine(origin == clone); // false
```

### Adding other properties

* [Example for the `Fluid` and `Mixture`](https://github.com/portyanikhin/SharpProp/blob/master/SharpProp.Tests/Fluids/TestFluidExtended.cs).
* [Example for the `HumidAir`](https://github.com/portyanikhin/SharpProp/blob/master/SharpProp.Tests/HumidAir/TestHumidAirExtended.cs).

### Adding other inputs

* [Example for the `Fluid` and `Mixture`](https://github.com/portyanikhin/SharpProp/blob/master/SharpProp.Tests/Fluids/TestInputExtended.cs).
* [Example for the `HumidAir`](https://github.com/portyanikhin/SharpProp/blob/master/SharpProp.Tests/HumidAir/TestInputHumidAirExtended.cs).