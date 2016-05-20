// Copyright 2012, 2013, 2014 Derek J. Bailey
// Modified work copyright 2016 Stefan Solntsev
// 
// This file (ChemicalFormula.cs) is part of Chemistry Library.
// 
// Chemistry Library is free software: you can redistribute it and/or modify it
// under the terms of the GNU Lesser General Public License as published
// by the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// Chemistry Library is distributed in the hope that it will be useful, but WITHOUT
// ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or
// FITNESS FOR A PARTICULAR PURPOSE. See the GNU Lesser General Public
// License for more details.
// 
// You should have received a copy of the GNU Lesser General Public
// License along with Chemistry Library. If not, see <http://www.gnu.org/licenses/>

namespace Test
{
    using Chemistry;
    using NUnit.Framework;
    using System.Collections.Generic;

    [TestFixture]
    public class ChemicalFormulaTestFixture
    {
        private static readonly ChemicalFormula NullChemicalFormula = null;
        private static readonly Element NullElement = null;
        private static readonly Isotope NullIsotope = null;
        private static readonly IChemicalFormula NullIChemicalFormula = null;

        [OneTimeSetUp]
        public void SetUp()
        {
            var elementH = new Element("H", 1, 1.007975);
            PeriodicTable.Add(elementH.AtomicSymbol, elementH);
            elementH.AddIsotope(1, 1.00782503223, 0.999885);
            elementH.AddIsotope(2, 2.01410177812, 0.000115);

            var elementC = new Element("C", 6, 12.0106);
            PeriodicTable.Add(elementC.AtomicSymbol, elementC);
            elementC.AddIsotope(12, 12, 0.9893);
            elementC.AddIsotope(13, 13.00335483507, 0.0107);


            var elementN = new Element("N", 7, 14.006855);
            PeriodicTable.Add(elementN.AtomicSymbol, elementN);
            elementN.AddIsotope(14, 14.00307400443, 0.99636);
            elementN.AddIsotope(15, 15.00010889888, 0.00364);


            var elementO = new Element("O", 8, 15.9994);
            PeriodicTable.Add(elementO.AtomicSymbol, elementO);
            elementO.AddIsotope(16, 15.99491461957, 0.99757);
            elementO.AddIsotope(17, 16.99913175650, 0.00038);
            elementO.AddIsotope(18, 17.99915961286, 0.00205);

            var elementFe = new Element("Fe", 26, 55.845);
            PeriodicTable.Add(elementFe.AtomicSymbol, elementFe);
            elementFe.AddIsotope(56, 55.93493633, 0.91754);

            var elementBr = new Element("Br", 35, 79.904);
            PeriodicTable.Add(elementBr.AtomicSymbol, elementBr);
            elementBr.AddIsotope(79, 78.9183376, 0.5069);

            var elementCa = new Element("Ca", 20, 40.078);
            PeriodicTable.Add(elementCa.AtomicSymbol, elementCa);
            elementCa.AddIsotope(40, 39.962590863, 0.96941);

            var elementS = new Element("S", 16, 32.0675);
            PeriodicTable.Add(elementS.AtomicSymbol, elementS);
            elementS.AddIsotope(32, 31.9720711744, 0.9499);
            elementS.AddIsotope(33, 32.9714589098, 0.0075);
            elementS.AddIsotope(34, 33.967867004, 0.0425);
            elementS.AddIsotope(36, 35.96708071, 0.0001);
            
            var elementSe = new Element("Se", 34, 78.971);
            PeriodicTable.Add(elementSe.AtomicSymbol, elementSe);
            elementSe.AddIsotope(74, 73.922475934, 0.0089);
            
            var elementAl = new Element("Al", 13, 26.9815385);
            PeriodicTable.Add(elementAl.AtomicSymbol, elementAl);
            elementAl.AddIsotope(27, 26.98153853, 1);
        }

        [Test]
        public void AddElementToFormula()
        {
            ChemicalFormula formulaA = new ChemicalFormula("C2H3NO");
            ChemicalFormula formulaB = new ChemicalFormula("C2H3N2O");

            Element n = PeriodicTable.GetElement("N");

            formulaA.Add(n, 1);

            Assert.AreEqual(formulaA, formulaB);
        }

        [Test]
        public void AddFormulaToFormula()
        {
            ChemicalFormula formulaA = new ChemicalFormula("C2H3NO");
            ChemicalFormula formulaB = new ChemicalFormula("H2O");
            ChemicalFormula formulaC = new ChemicalFormula("C2H5NO2");

            formulaA.Add(formulaB);

            Assert.AreEqual(formulaA, formulaC);
        }

        [Test]
        public void AddFormulaToItself()
        {
            ChemicalFormula formulaA = new ChemicalFormula("C2H3NO");
            ChemicalFormula formulaB = new ChemicalFormula("C4H6N2O2");

            formulaA.Add(new ChemicalFormula(formulaA));

            Assert.AreEqual(formulaA, formulaB);
        }

        [Test]
        public void AddIChemicalFormulaToFormula()
        {
            ChemicalFormula formulaA = new ChemicalFormula("C2H3NO");
            IChemicalFormula formulaB = new ChemicalFormula("H2O");
            ChemicalFormula formulaC = new ChemicalFormula("C2H5NO2");

            formulaA.Add(formulaB);

            Assert.AreEqual(formulaA, formulaC);
        }

        [Test]
        public void AddIsotopeToFormula()
        {
            ChemicalFormula formulaA = new ChemicalFormula("C2H3NO");
            ChemicalFormula formulaB = new ChemicalFormula("C2H3H{1}NO");

            Isotope h1 = PeriodicTable.GetElement("H")[1];

            formulaA.Add(h1, 1);

            Assert.AreEqual(formulaA, formulaB);
        }

        /// <summary>
        /// This tests that the array for chemical formula properly expands
        /// </summary>
        [Test]
        public void AddLargeElementToFormula()
        {
            ChemicalFormula formulaA = new ChemicalFormula("C2H3NO");
            ChemicalFormula formulaB = new ChemicalFormula("C2H3NOFe");

            Element fe = PeriodicTable.GetElement("Fe");

            formulaA.Add(fe, 1);

            Assert.AreEqual(formulaA, formulaB);
        }

        [Test]
        public void AddNegativeFormulaToFormula()
        {
            ChemicalFormula formulaA = new ChemicalFormula("C2H3NO");
            ChemicalFormula formulaB = new ChemicalFormula("C-1H-2");
            ChemicalFormula formulaC = new ChemicalFormula("CHNO");

            formulaA.Add(formulaB);

            Assert.AreEqual(formulaA, formulaC);
        }

        [Test]
        public void AddNegativeIsotopeToFormula()
        {
            ChemicalFormula formulaA = new ChemicalFormula("C2HH{1}2NO");
            ChemicalFormula formulaB = new ChemicalFormula("C2HNO");

            Isotope h1 = PeriodicTable.GetElement("H")[1];

            formulaA.Add(h1, -2);

            Assert.AreEqual(formulaA, formulaB);
        }

        [Test]
        public void AddNonExistentSymbolToFormula()
        {
            ChemicalFormula formulaA = new ChemicalFormula("C2H3NO");


            Assert.That(() => formulaA.Add("Faa", 1),
                        Throws.TypeOf<KeyNotFoundException>());
        }

        [Test]
        public void AddNullElementToFormula()
        {
            ChemicalFormula formulaA = new ChemicalFormula("C2H3NO");
            ChemicalFormula formulaB = new ChemicalFormula("C2H3NO");

            formulaA.Add(NullElement, 1);

            Assert.AreEqual(formulaA, formulaB);
        }

        [Test]
        public void AddNullIsotopeToFormula()
        {
            ChemicalFormula formulaA = new ChemicalFormula("C2H3NO");
            ChemicalFormula formulaB = new ChemicalFormula("C2H3NO");

            formulaA.Add(NullIsotope, 1);

            Assert.AreEqual(formulaA, formulaB);
        }

        [Test]
        public void AddNullFormulaToFormula()
        {
            ChemicalFormula formulaA = new ChemicalFormula("C2H3NO");
            ChemicalFormula formulaB = new ChemicalFormula("C2H3NO");

            formulaA.Add(NullChemicalFormula);

            Assert.AreEqual(formulaA, formulaB);
        }

        [Test]
        public void AddNullIChemicalFormulaToFormula()
        {
            ChemicalFormula formulaA = new ChemicalFormula("C2H3NO");
            ChemicalFormula formulaB = new ChemicalFormula("C2H3NO");

            formulaA.Add(NullIChemicalFormula);

            Assert.AreEqual(formulaA, formulaB);
        }

        [Test]
        public void AddZeroElementToFormula()
        {
            ChemicalFormula formulaA = new ChemicalFormula("C2H3NO");
            ChemicalFormula formulaB = new ChemicalFormula("C2H3NO");

            Element n = PeriodicTable.GetElement("N");

            formulaA.Add(n, 0);

            Assert.AreEqual(formulaA, formulaB);
        }

        [Test]
        public void AddZeroIsotopeToFormula()
        {
            ChemicalFormula formulaA = new ChemicalFormula("C2H3NO");
            ChemicalFormula formulaB = new ChemicalFormula("C2H3NO");

            Isotope h1 = PeriodicTable.GetElement("H")[1];

            formulaA.Add(h1, 0);

            Assert.AreEqual(formulaA, formulaB);
        }

        [Test]
        public void AddZeroSymbolToFormula()
        {
            ChemicalFormula formulaA = new ChemicalFormula("C2H3NO");
            ChemicalFormula formulaB = new ChemicalFormula("C2H3NO");

            formulaA.Add("H", 0);

            Assert.AreEqual(formulaA, formulaB);
        }

        [Test]
        public void ClearFormula()
        {
            ChemicalFormula formulaA = new ChemicalFormula("C2H3NO");
            formulaA.Clear();
            Assert.AreEqual(formulaA, ChemicalFormula.Empty);
        }

        [Test]
        public void ConstructorBlankStringEqualsEmptyFormula()
        {
            ChemicalFormula formulaA = new ChemicalFormula("");

            Assert.AreEqual(formulaA, ChemicalFormula.Empty);
        }

        [Test]
        public void ConstructorEmptyStringEqualsEmptyFormula()
        {
            ChemicalFormula formulaA = new ChemicalFormula(string.Empty);

            Assert.AreEqual(formulaA, ChemicalFormula.Empty);
        }

        [Test]
        public void ConstructorDefaultEqualsEmptyFormula()
        {
            ChemicalFormula formulaA = new ChemicalFormula();

            Assert.AreEqual(formulaA, ChemicalFormula.Empty);
        }

        [Test]
        public void CopyConstructorValueEquality()
        {
            ChemicalFormula formulaA = new ChemicalFormula("C2H3NO");
            ChemicalFormula formulaB = new ChemicalFormula(formulaA);

            Assert.AreEqual(formulaA, formulaB);
        }

        [Test]
        public void CopyConstructorNullEqualsEmptyFormula()
        {
            ChemicalFormula formulaA = new ChemicalFormula(NullChemicalFormula);

            Assert.AreEqual(formulaA, ChemicalFormula.Empty);
        }

        [Test]
        public void CopyConstructorReferenceInequality()
        {
            ChemicalFormula formulaA = new ChemicalFormula("C2H3NO");
            ChemicalFormula formulaB = new ChemicalFormula(formulaA);

            Assert.AreNotSame(formulaA, formulaB);
        }

        [Test]
        public void EmptyMonoisotopicMassIsZero()
        {
            Assert.AreEqual(0.0, ChemicalFormula.Empty.MonoisotopicMass);
        }

        [Test]
        public void EmptyAverageMassIsZero()
        {
            Assert.AreEqual(0.0, ChemicalFormula.Empty.AverageMass);
        }

        [Test]
        public void EmptyStringIsBlank()
        {
            Assert.IsEmpty(ChemicalFormula.Empty.Formula);
        }

        [Test]
        public void EmptyAtomCountIsZero()
        {
            Assert.AreEqual(0, ChemicalFormula.Empty.AtomCount);
        }

        [Test]
        public void EmptyElementCountIsZero()
        {
            Assert.AreEqual(0, ChemicalFormula.Empty.NumberOfUniqueElementsByAtomicNumber);
        }

        [Test]
        public void EmptyIsotopeCountIsZero()
        {
            Assert.AreEqual(0, ChemicalFormula.Empty.NumberOfUniqueIsotopes);
        }

        [Test]
        public void FormulaValueInequalityNullFormula()
        {
            ChemicalFormula formulaA = new ChemicalFormula("C2H3NO");

            Assert.AreNotEqual(NullChemicalFormula, formulaA);
        }

        [Test]
        public void FormulaValueInequality()
        {
            ChemicalFormula formulaA = new ChemicalFormula("C2H3NO");
            ChemicalFormula formulaB = new ChemicalFormula("NC1OH3");

            Assert.AreNotEqual(formulaA, formulaB);
        }

        [Test]
        public void FormulaValueInequalityHeavyIsotope()
        {
            ChemicalFormula formulaA = new ChemicalFormula("C2H3NO");
            ChemicalFormula formulaB = new ChemicalFormula("CC{13}H3NO");

            Assert.AreNotEqual(formulaA, formulaB);
        }

        [Test]
        public void FormulaValueEqualityItself()
        {
            ChemicalFormula formulaA = new ChemicalFormula("C2H3NO");

            Assert.AreEqual(formulaA, formulaA);
        }

        [Test]
        public void FormulaValueEquality()
        {
            ChemicalFormula formulaA = new ChemicalFormula("C2H3NO");
            ChemicalFormula formulaB = new ChemicalFormula("NC2OH3");

            Assert.AreEqual(formulaA, formulaB);
        }

        [Test]
        public void HashCodeEquality()
        {
            ChemicalFormula formulaA = new ChemicalFormula("C2H3NO");
            ChemicalFormula formulaB = new ChemicalFormula("H3C2NO");

            Assert.AreEqual(formulaA.GetHashCode(), formulaB.GetHashCode());
        }

        [Test]
        public void HashCodeImmutableEquality()
        {
            ChemicalFormula formulaA = new ChemicalFormula("C2H3NO");

            Assert.AreEqual(formulaA.GetHashCode(), formulaA.GetHashCode());
        }

        [Test]
        public void HillNotation()
        {
            ChemicalFormula formulaA = new ChemicalFormula("H3NC2O");

            Assert.AreEqual("C2H3NO", formulaA.Formula);
        }

        [Test]
        public void HillNotationNoCarbon()
        {
            ChemicalFormula formulaA = new ChemicalFormula("BrH");

            Assert.AreEqual("HBr", formulaA.Formula);
        }

        [Test]
        public void HillNotationNoCarbonNoHydrogen()
        {
            ChemicalFormula formulaA = new ChemicalFormula("Ca5O14Br6");

            Assert.AreEqual("Br6Ca5O14", formulaA.Formula);
        }

        [Test]
        public void HillNotationNoHydrogen()
        {
            ChemicalFormula formulaA = new ChemicalFormula("NC2O");

            Assert.AreEqual("C2NO", formulaA.Formula);
        }

        [Test]
        public void HillNotationWithHeavyIsotope()
        {
            ChemicalFormula formulaA = new ChemicalFormula("H3NC2C{13}2O");

            Assert.AreEqual("C2C{13}2H3NO", formulaA.Formula);
        }

        [Test]
        public void HillNotationWithNegativeCount()
        {
            ChemicalFormula formulaA = new ChemicalFormula("H3NC-2O");

            Assert.AreEqual("C-2H3NO", formulaA.Formula);
        }

        [Test]
        public void HillNotationWithHeavyIsotopeNegativeCount()
        {
            ChemicalFormula formulaA = new ChemicalFormula("H3NC2C{13}-2O");

            Assert.AreEqual("C2C{13}-2H3NO", formulaA.Formula);
        }

        [Test]
        public void ImplicitAddFormula()
        {
            ChemicalFormula formulaA = new ChemicalFormula("C2H3NO");
            ChemicalFormula formulaB = new ChemicalFormula("H2O");
            ChemicalFormula formulaC = new ChemicalFormula("C2H5NO2");

            ChemicalFormula formulaD = formulaA + formulaB;

            Assert.AreEqual(formulaC, formulaD);
        }

        [Test]
        public void ImplicitAddNullFormulaRight()
        {
            ChemicalFormula formulaA = new ChemicalFormula("C2H3NO");
            ChemicalFormula formulaB = new ChemicalFormula("C2H3NO");

            ChemicalFormula formulaC = formulaA + NullChemicalFormula;

            Assert.AreEqual(formulaC, formulaB);
        }

        [Test]
        public void ImplicitAddNullFormulaLeft()
        {
            ChemicalFormula formulaA = new ChemicalFormula("C2H3NO");
            ChemicalFormula formulaB = new ChemicalFormula("C2H3NO");

            ChemicalFormula formulaC = NullChemicalFormula + formulaA;

            Assert.AreEqual(formulaC, formulaB);
        }

        [Test]
        public void ImplicitAddNullFormulaLeftRight()
        {
            ChemicalFormula formulaA = NullChemicalFormula + NullChemicalFormula;

            Assert.AreEqual(formulaA, NullChemicalFormula);
        }

        [Test]
        public void ImplicitConstructor()
        {
            ChemicalFormula formulaA = new ChemicalFormula("C2H3NO");
            ChemicalFormula formulaB = "C2H3NO";

            Assert.AreEqual(formulaA, formulaB);
        }

        [Test]
        public void ImplicitMultipleFormulaLeft()
        {
            ChemicalFormula formulaA = new ChemicalFormula("C2H3NO");
            ChemicalFormula formulaB = new ChemicalFormula("C4H6N2O2");

            ChemicalFormula formulaC = formulaA * 2;

            Assert.AreEqual(formulaB, formulaC);
        }

        [Test]
        public void ImplicitMultipleFormulaRight()
        {
            ChemicalFormula formulaA = new ChemicalFormula("C2H3NO");
            ChemicalFormula formulaB = new ChemicalFormula("C4H6N2O2");

            ChemicalFormula formulaC = 2 * formulaA;

            Assert.AreEqual(formulaC, formulaB);
        }

        [Test]
        public void ImplicitSubtractFormula()
        {
            ChemicalFormula formulaA = new ChemicalFormula("C2H5NO2");
            ChemicalFormula formulaB = new ChemicalFormula("H2O");
            ChemicalFormula formulaC = new ChemicalFormula("C2H3NO");

            ChemicalFormula formulaD = formulaA - formulaB;

            Assert.AreEqual(formulaC, formulaD);
        }

        [Test]
        public void InvalidChemicalElement()
        {
            Assert.Throws<KeyNotFoundException>(() => { Element e = PeriodicTable.GetElement("Faa"); });
        }

        [Test]
        public void InvalidElementIsotope()
        {
            Assert.Throws<KeyNotFoundException>(() => { Isotope i = PeriodicTable.GetElement("C")[100]; });
        }

        [Test]
        public void NullFormulaDoesNotEqualFormula()
        {
            ChemicalFormula formulaA = new ChemicalFormula("C2H3NO");

            Assert.AreNotEqual(formulaA, NullChemicalFormula);
        }

        [Test]
        public void NumberOfAtoms()
        {
            ChemicalFormula formulaA = new ChemicalFormula("C2H3NO");

            Assert.AreEqual(7, formulaA.AtomCount);
        }

        [Test]
        public void NumberOfAtomsOfEmptyFormula()
        {
            Assert.AreEqual(0, ChemicalFormula.Empty.AtomCount);
        }

        [Test]
        public void NumberOfAtomsOfNegativeFormula()
        {
            ChemicalFormula formulaA = new ChemicalFormula("C-2H-3N-1O-1");

            Assert.AreEqual(-7, formulaA.AtomCount);
        }

        [Test]
        public void ParsingFormulaNoNumbers()
        {
            ChemicalFormula formulaA = new ChemicalFormula("CCHHHNO");
            ChemicalFormula formulaB = new ChemicalFormula("C2H3NO");

            Assert.AreEqual(formulaA, formulaB);
        }

        [Test]
        public void ParsingFormulaWithInternalSpaces()
        {
            ChemicalFormula formulaA = new ChemicalFormula("C2 H3 N O");
            ChemicalFormula formulaB = new ChemicalFormula("C2H3NO");

            Assert.AreEqual(formulaA, formulaB);
        }

        [Test]
        public void ParsingFormulaWithSpacesAtEnd()
        {
            ChemicalFormula formulaA = new ChemicalFormula("C2H3NO  ");
            ChemicalFormula formulaB = new ChemicalFormula("C2H3NO");

            Assert.AreEqual(formulaA, formulaB);
        }

        [Test]
        public void ParsingFormulaWithSpacesAtBeginning()
        {
            ChemicalFormula formulaA = new ChemicalFormula("    C2H3NO");
            ChemicalFormula formulaB = new ChemicalFormula("C2H3NO");

            Assert.AreEqual(formulaA, formulaB);
        }

        [Test]
        public void ParsingFormulaWithSpaces()
        {
            ChemicalFormula formulaA = new ChemicalFormula("  C2 H3 N O  ");
            ChemicalFormula formulaB = new ChemicalFormula("C2H3NO");

            Assert.AreEqual(formulaA, formulaB);
        }

        [Test]
        public void ParsingFormulaNoNumbersRandomOrder()
        {
            ChemicalFormula formulaA = new ChemicalFormula("OCHHCHN");
            ChemicalFormula formulaB = new ChemicalFormula("C2H3NO");

            Assert.AreEqual(formulaA, formulaB);
        }

        [Test]
        public void ParsingFormulaRepeatedElements()
        {
            ChemicalFormula formulaA = new ChemicalFormula("CH3NOC");
            ChemicalFormula formulaB = new ChemicalFormula("C2H3NO");

            Assert.AreEqual(formulaA, formulaB);
        }

        [Test]
        public void ParsingFormulaRepeatedElementsCancelEachOther()
        {
            ChemicalFormula formulaA = new ChemicalFormula("C2H3NOC-2");
            ChemicalFormula formulaB = new ChemicalFormula("H3NO");

            Assert.AreEqual(formulaA, formulaB);
        }

        [Test]
        public void RemoveElementCompletelyFromFromula()
        {
            ChemicalFormula formulaA = new ChemicalFormula("C2H3NO");
            ChemicalFormula formulaB = new ChemicalFormula("C2NO");

            formulaA.RemoveIsotopesOf(PeriodicTable.GetElement("H"));

            Assert.AreEqual(formulaA, formulaB);
        }

        [Test]
        public void RemoveElementCompletelyFromFromulaBySymbol()
        {
            ChemicalFormula formulaA = new ChemicalFormula("C2H3NO");
            ChemicalFormula formulaB = new ChemicalFormula("C2NO");

            formulaA.Remove("H");

            Assert.AreEqual(formulaA, formulaB);
        }

        [Test]
        public void RemoveElementCompletelyFromFromulaWithHeavyIsotope()
        {
            ChemicalFormula formulaA = new ChemicalFormula("C2C{13}H3NO");
            ChemicalFormula formulaB = new ChemicalFormula("H3NO");

            formulaA.RemoveIsotopesOf(PeriodicTable.GetElement("C"));

            Assert.AreEqual(formulaA, formulaB);
        }

        [Test]
        public void RemoveEmptyFormulaFromFromula()
        {
            ChemicalFormula formulaA = new ChemicalFormula("C2H3NO");
            ChemicalFormula formulaB = new ChemicalFormula("C2H3NO");

            formulaA.Remove(ChemicalFormula.Empty);

            Assert.AreEqual(formulaA, formulaB);
        }

        [Test]
        public void RemoveFormulaFromFromula()
        {
            ChemicalFormula formulaA = new ChemicalFormula("C2H5NO2");
            ChemicalFormula formulaB = new ChemicalFormula("H2O");
            ChemicalFormula formulaC = new ChemicalFormula("C2H3NO");

            formulaA.Remove(formulaB);

            Assert.AreEqual(formulaA, formulaC);
        }

        [Test]
        public void RemoveIsotopeCompletelyFromFromula()
        {
            ChemicalFormula formulaA = new ChemicalFormula("C2H3NO");
            ChemicalFormula formulaB = new ChemicalFormula("C2NO");

            formulaA.RemoveIsotopesOf(PeriodicTable.GetElement("H"));

            Assert.AreEqual(formulaA, formulaB);
        }

        [Test]
        public void RemoveElementFromFromula()
        {
            ChemicalFormula formulaA = new ChemicalFormula("C2H3NO");
            ChemicalFormula formulaB = new ChemicalFormula("C2HNO");

            formulaA.Remove(PeriodicTable.GetElement("H"), 2);

            Assert.AreEqual(formulaA, formulaB);
        }

        [Test]
        public void RemoveIsotopeFromFromulaEquality()
        {
            ChemicalFormula formulaA = new ChemicalFormula("C2H3NO");
            ChemicalFormula formulaB = new ChemicalFormula("C2H3O");

            formulaA.Remove("N", 1);

            Assert.AreEqual(formulaA, formulaB);
        }

        [Test]
        public void RemoveNegativeElementFromFromula()
        {
            ChemicalFormula formulaA = new ChemicalFormula("C2H3NO");
            ChemicalFormula formulaB = new ChemicalFormula("C2H5NO");

            formulaA.Remove(PeriodicTable.GetElement("H"), -2);

            Assert.AreEqual(formulaA, formulaB);
        }

        [Test]
        public void RemoveNonExistantIsotopeFromFromula()
        {
            ChemicalFormula formulaA = new ChemicalFormula("C2H5NO2");
            ChemicalFormula formulaB = new ChemicalFormula("Fe");
            ChemicalFormula formulaC = new ChemicalFormula("C2H5Fe-1NO2");

            formulaA.Remove(formulaB);

            Assert.AreEqual(formulaA, formulaC);
        }

        [Test]
        public void RemoveNullElementFromFromula()
        {
            ChemicalFormula formulaA = new ChemicalFormula("C2H3NO");
            ChemicalFormula formulaB = new ChemicalFormula("C2H3NO");

            formulaA.RemoveIsotopesOf(NullElement);

            Assert.AreEqual(formulaA, formulaB);
        }

        [Test]
        public void RemoveNullFormulaFromFromula()
        {
            ChemicalFormula formulaA = new ChemicalFormula("C2H3NO");
            ChemicalFormula formulaB = new ChemicalFormula("C2H3NO");

            formulaA.Remove(NullChemicalFormula);

            Assert.AreEqual(formulaA, formulaB);
        }

        [Test]
        public void RemoveNullIsotopeFromFromula()
        {
            ChemicalFormula formulaA = new ChemicalFormula("C2H3NO");
            ChemicalFormula formulaB = new ChemicalFormula("C2H3NO");

            formulaA.Remove(NullIsotope);

            Assert.AreEqual(formulaA, formulaB);
        }

        [Test]
        public void RemoveZeroIsotopeFromFromula()
        {
            ChemicalFormula formulaA = new ChemicalFormula("C2H3NO");
            ChemicalFormula formulaB = new ChemicalFormula("C2H3NO");

            formulaA.Remove(PeriodicTable.GetIsotope("H", 1), 0);

            Assert.AreEqual(formulaA, formulaB);
        }


        [Test]
        public void TotalProtons()
        {
            ChemicalFormula formulaA = new ChemicalFormula("C2H3NO");

            Assert.AreEqual(30, formulaA.GetProtonCount());
        }

        [Test]
        public void UniqueElements()
        {
            ChemicalFormula formulaA = new ChemicalFormula("C2H3NO");

            Assert.AreEqual(4, formulaA.NumberOfUniqueElementsByAtomicNumber);
        }

        [Test]
        public void UniqueElementsOfEmptyFormula()
        {
            Assert.AreEqual(0, ChemicalFormula.Empty.NumberOfUniqueElementsByAtomicNumber);
        }

        [Test]
        public void UniqueElementsWithHeavyIsotope()
        {
            ChemicalFormula formulaA = new ChemicalFormula("CC{13}H3NO");

            Assert.AreEqual(4, formulaA.NumberOfUniqueElementsByAtomicNumber);
        }

        [Test]
        public void UniqueIsotopes()
        {
            ChemicalFormula formulaA = new ChemicalFormula("C2H3NO");

            Assert.AreEqual(0, formulaA.NumberOfUniqueIsotopes);
        }

        [Test]
        public void UniqueIsotopesOfEmptyFormula()
        {
            Assert.AreEqual(0, ChemicalFormula.Empty.NumberOfUniqueIsotopes);
        }

        [Test]
        public void UniqueIsotopesWithHeavyIsotope()
        {
            ChemicalFormula formulaA = new ChemicalFormula("CC{13}H3NO");

            Assert.AreEqual(1, formulaA.NumberOfUniqueIsotopes);
        }

        [Test]
        public void ChemicalForulaIsSubSet()
        {
            ChemicalFormula formulaA = new ChemicalFormula("C2H3NO");
            ChemicalFormula formulaB = new ChemicalFormula("C3H3NO");

            Assert.IsTrue(formulaA.IsSubSetOf(formulaB));
        }

        [Test]
        public void ChemicalForulaIsNotSubSet()
        {
            ChemicalFormula formulaA = new ChemicalFormula("C2H3NO");
            ChemicalFormula formulaB = new ChemicalFormula("C3H2NO");

            Assert.IsFalse(formulaA.IsSubSetOf(formulaB));
        }

        [Test]
        public void ChemicalForulaIsSuperSet()
        {
            ChemicalFormula formulaA = new ChemicalFormula("C2H3NO");
            ChemicalFormula formulaB = new ChemicalFormula("C3H3NO");

            Assert.IsTrue(formulaB.IsSuperSetOf(formulaA));
        }

        [Test]
        public void ChemicalForulaIsNotSuperSet()
        {
            ChemicalFormula formulaA = new ChemicalFormula("C2H3NO2");
            ChemicalFormula formulaB = new ChemicalFormula("C3H3NO");

            Assert.IsFalse(formulaB.IsSuperSetOf(formulaA));
        }
    }
}
