This is a library for basic chemistry objects. Includes the periodic table, elements and their isotopes, and chemical formulas.

[![Build Status](https://travis-ci.org/stefanks/Chemistry.svg?branch=master)](https://travis-ci.org/stefanks/Chemistry) [![Build status](https://ci.appveyor.com/api/projects/status/d8dxfnj8lv7p4bhu/branch/master?svg=true)](https://ci.appveyor.com/project/stefanks/chemistry/branch/master)[![Coverage Status](https://coveralls.io/repos/github/stefanks/Chemistry/badge.svg?branch=master)](https://coveralls.io/github/stefanks/Chemistry?branch=master)[![Coverity Scan Build Status](https://scan.coverity.com/projects/9146/badge.svg)](https://scan.coverity.com/projects/stefanks-chemistry)

Releases are here: https://www.nuget.org/packages/Chemistry/

## Usage

### Populate the periodic table

The static periodic table class acts as a globally accessible store of elements. To populate it, create an element and add it to the periodic table, specifying the atomic symbol, atomic number and the standard atomic weight. 
```csharp
            PeriodicTable.Add(new Element("C", 6, 12.0106));
            Element carbon = PeriodicTable.GetElement("C");
```

If needed, an isotope can be specified by providing it's mass number, relative atomic mass and relative abundance of the isotope.
```csharp
            carbon.AddIsotope(12, 12, 0.9893);
            carbon.AddIsotope(13, 13.00335483507, 0.0107);
```
In theory, the standard atomic weight should be equal to the average of the relative atomic masses of all isotopes weighted by their relative abundance. We do not enforce this constraint, since there is no requirement to specify all (or any) isotopes. 

To automatically populate the periodic table with current NIST estimates of all masses and abundances use the NuGet package [UsefulProteomicsDatabases](https://www.nuget.org/packages/UsefulProteomicsDatabases). 
```csharp
            Loaders.elementLocation = "elements.dat";
            Loaders.LoadElements();
```

### Chemical Formulas

Elements (and isotopes) can be combined to create chemical formulas.
```csharp
            ChemicalFormula formulaA = new ChemicalFormula("C2H3NO");
```

### Isotopic Distribution

Isotopic distribution of a chemical compound can be calculated from it's chemical formula.
```csharp
            ChemicalFormula formulaA = new ChemicalFormula("C2H3NO");
            double[] masses;
            double[] abundances;
            dist.CalculateDistribuition(formulaA, out masses, out abundances);
```

## License
Code heavily borrowed from https://github.com/dbaileychess/CSMSL and distrubuted under the appropriate license, LGPL.
