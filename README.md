A library for basic chemistry objects. It includes the periodic table, elements and their isotopes, and chemical formulas.

[![Build Status](https://travis-ci.org/stefanks/Chemistry.svg?branch=master)](https://travis-ci.org/stefanks/Chemistry) [![Build status](https://ci.appveyor.com/api/projects/status/d8dxfnj8lv7p4bhu/branch/master?svg=true)](https://ci.appveyor.com/project/stefanks/chemistry/branch/master)[![Coverage Status](https://coveralls.io/repos/github/stefanks/Chemistry/badge.svg?branch=master)](https://coveralls.io/github/stefanks/Chemistry?branch=master)[![Coverity Scan Build Status](https://scan.coverity.com/projects/9146/badge.svg)](https://scan.coverity.com/projects/stefanks-chemistry)

Releases are here: https://www.nuget.org/packages/Chemistry/

## Usage

### Populate the periodic table

The static periodic table class acts as a globally accessible storage of elements. To populate it, create an element and add it to the periodic table, specifying the atomic symbol, atomic number and the standard atomic weight. 
```csharp
            PeriodicTable.Add(new Element("C", 6, 12.0106));
            Element carbon = PeriodicTable.GetElement("C");
```

If needed, isotopes can be specified by providing the mass number, relative atomic mass and relative abundance. 
```csharp
            carbon.AddIsotope(12, 12, 0.9893);
            carbon.AddIsotope(13, 13.00335483507, 0.0107);
```
Ideally, the standard atomic weight should be equal to the average of the relative atomic masses of all isotopes weighted by their relative abundance. Since there is no requirement to specify all (or any) isotopes, we do not explicitly enforce this constraint, but provide two useful methods for validating the relative abundances and average masses of loaded elements. 
```csharp
            PeriodicTableValidationResult res1 = PeriodicTable.ValidateAbundances(1e-3);
            PeriodicTableValidationResult res2 = PeriodicTable.ValidateAverageMasses(1e-3);
```
To automatically populate the periodic table with current NIST values, use the NuGet package [UsefulProteomicsDatabases](https://www.nuget.org/packages/UsefulProteomicsDatabases). 
```csharp
            Loaders.LoadElements("elements.dat");
```

### Chemical Formulas

Elements (and isotopes) can be combined to create chemical formulas.
```csharp
            ChemicalFormula formula = new ChemicalFormula("C2H3NO");
```

### Isotopic Distribution

Isotopic distribution of a chemical compound can be calculated from its chemical formula.
```csharp
            IsotopicDistribution dist = new IsotopicDistribution(formula);
```

## License
Code heavily borrowed from https://github.com/dbaileychess/CSMSL and distrubuted under the appropriate license, LGPL.
