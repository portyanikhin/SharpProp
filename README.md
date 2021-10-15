# ![SharpProp](https://raw.githubusercontent.com/portyanikhin/SharpProp/f7f79cdc0fedbca3e4c816ef2205cfb8300e193f/SharpProp/pictures/header.svg)

[![Build & Tests](https://github.com/portyanikhin/SharpProp/actions/workflows/build-tests.yml/badge.svg)](https://github.com/portyanikhin/SharpProp/actions/workflows/build-tests.yml)
[![NuGet](https://img.shields.io/nuget/v/SharpProp)](https://www.nuget.org/packages/SharpProp/)
![Platform](https://img.shields.io/badge/platform-win--64%20%7C%20linux--64-lightgrey)
[![License](https://img.shields.io/github/license/portyanikhin/SharpProp)](https://github.com/portyanikhin/SharpProp/blob/master/LICENSE)
![Coverage](https://img.shields.io/badge/coverage-100%25-brightgreen)

A simple, full-featured, lightweight, cross-platform [CoolProp](http://www.coolprop.org/) wrapper for C#

## Quick start

All calculations of thermophysical properties are performed in _SI units_.

The `Fluid` class is responsible for pure fluids and binary mixtures, the `Mixture` class - for mixtures with pure
fluids components, the `HumidAir` class - for humid air.

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

**NB.** If the required property is not present in the instance of the fluid, then you can add it by extending
the `Fluid`, `Mixture` or `HumidAir` classes.

### Examples

Don't forget to add `using SharpProp;` at the top of the code.

#### Pure fluids

To calculate the specific heat of saturated water vapour at _101325 Pa_:

```c#
var waterVapour = new Fluid(FluidsList.Water);
waterVapour.Update(Input.Pressure(101325), Input.Quality(1));
Console.WriteLine(waterVapour.SpecificHeat); // 2079.937085633241
```

#### Incompressible binary mixtures

To calculate the dynamic viscosity of propylene glycol aqueous solution with _60 %_ mass fraction at _101325 Pa_ and _253.15 K_:

```c#
var propyleneGlycol = new Fluid(FluidsList.MPG, 0.6);
propyleneGlycol.Update(Input.Pressure(101325), Input.Temperature(253.15));
Console.WriteLine(propyleneGlycol.DynamicViscosity); // 0.13907391053938847
```

#### Mixtures

To calculate the density of ethanol aqueous solution (with ethanol _40 %_ mass fraction) at _200 kPa_ and _277.15 K_:

```c#
var mixture = new Mixture(new List<FluidsList> {FluidsList.Water, FluidsList.Ethanol}, 
    new List<double> {0.6, 0.4});
mixture.Update(Input.Pressure(200e3), Input.Temperature(277.15));
Console.WriteLine(mixture.Density); // 883.3922771627759
```

#### Humid air

To calculate the wet bulb temperature of humid air at _99 kPa_, _303.15 K_ and _50 %_ relative humidity:

```c#
var humidAir = new HumidAir();
humidAir.Update(InputHumidAir.Pressure(99e3), InputHumidAir.Temperature(303.15),
    InputHumidAir.RelativeHumidity(0.5));
// or use:
// var humidAir = HumidAir.WithState(InputHumidAir.Pressure(99e3), InputHumidAir.Temperature(303.15),
//     InputHumidAir.RelativeHumidity(0.5));
Console.WriteLine(humidAir.WetBulbTemperature); // 295.0965785590792
```

#### Converting to JSON string

For example, converting the `Fluid` instance to _indented_ JSON string:

```c#
var refrigerant = new Fluid(FluidsList.R32);
refrigerant.Update(Input.Temperature(278.15), Input.Quality(1));
Console.WriteLine(refrigerant.AsJson(indented: true));
```

As a result:

```json
{
  "Name": "R32",
  "Fraction": 1.0,
  "Compressibility": 0.8266625877210833,
  "Conductivity": 0.013435453854396475,
  "CriticalPressure": 5782000.0,
  "CriticalTemperature": 351.255,
  "Density": 25.89088151061046,
  "DynamicViscosity": 1.2606543144761657E-05,
  "Enthalpy": 516105.7800378023,
  "Entropy": 2136.2654412978777,
  "FreezingTemperature": null,
  "InternalEnergy": 479357.39743435377,
  "MaxPressure": 70000000.0,
  "MaxTemperature": 435.0,
  "MinPressure": 47.999893876059375,
  "MinTemperature": 136.34,
  "MolarMass": 0.052024,
  "Phase": "TwoPhase",
  "Prandtl": 1.2252282243443504,
  "Pressure": 951448.019691762,
  "Quality": 1.0,
  "SoundSpeed": 209.6337575990297,
  "SpecificHeat": 1305.7899441785378,
  "SurfaceTension": 0.010110117241546162,
  "Temperature": 278.15,
  "TriplePressure": 47.999893876059375,
  "TripleTemperature": 136.34
}
```

#### Equality of instances

You can simply determine the equality of `Fluid`, `Mixture` and `HumidAir` instances by its state.
Just use the `Equals` method or the equality operators (`==` or `!=`).
Exactly the same way you can compare inputs (`Input`, `InputHumidAir` or any `IKeyedInput` record).

For example:

```c#
var humidAir = HumidAir.WithState(InputHumidAir.Pressure(101325),
    InputHumidAir.Temperature(293.15), InputHumidAir.RelativeHumidity(0.5));
var humidAirWithSameState = HumidAir.WithState(InputHumidAir.Pressure(101325),
    InputHumidAir.Temperature(293.15), InputHumidAir.RelativeHumidity(0.5));
Console.WriteLine(humidAir == humidAirWithSameState); // true
Console.WriteLine(InputHumidAir.Pressure(101325) == InputHumidAir.Pressure(101.325e3)); // true
```

#### Adding other properties

* [Example for the `Fluid` and `Mixture`](https://github.com/portyanikhin/SharpProp/blob/master/SharpProp.Tests/Fluids/TestFluidExtended.cs)
* [Example for the `HumidAir`](https://github.com/portyanikhin/SharpProp/blob/master/SharpProp.Tests/HumidAir/TestHumidAirExtended.cs)

#### Adding other inputs

* [Example for the `Fluid` and `Mixture`](https://github.com/portyanikhin/SharpProp/blob/master/SharpProp.Tests/Fluids/TestInputExtended.cs)
* [Example for the `HumidAir`](https://github.com/portyanikhin/SharpProp/blob/master/SharpProp.Tests/HumidAir/TestInputHumidAirExtended.cs)