﻿// Copyright 2012, 2013, 2014 Derek J. Bailey
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
// License along with Chemistry Library. If not, see <http://www.gnu.org/licenses/>.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Chemistry
{
    /// <summary>
    /// A chemical / molecule consisting of multiple atoms.
    /// <remarks>This class is mutable</remarks>
    /// </summary>
    public sealed class ChemicalFormula : IEquatable<ChemicalFormula>, IHasChemicalFormula
    {
        /// <summary>
        /// A regular expression for matching chemical formulas such as: C2C{13}3H5NO5
        /// \s* (at end as well) allows for optional spacing among the elements, i.e. C2 C{13}3 H5 N O5
        /// The first group is the only non-optional group and that handles the chemical symbol: H, He, etc..
        /// The second group is optional, which handles alternative isotopes of elements: C{13} means carbon-13, while C is the common carbon-12
        /// The third group is optional and indicates if we are adding or subtracting the elements form the formula, C-2C{13}5 would mean first subtract 2 carbon-12 and then add 5 carbon-13
        /// The fourth group is optional and represents the number of isotopes to add, if not present it assumes 1: H2O means 2 Hydrogen and 1 Oxygen
        /// Modified from: http://stackoverflow.com/questions/4116786/parsing-a-chemical-formula-from-a-string-in-c
        /// </summary>
        private static readonly Regex FormulaRegex = new Regex(@"\s*([A-Z][a-z]*)(?:\{([0-9]+)\})?(-)?([0-9]+)?\s*", RegexOptions.Compiled);

        /// <summary>
        /// A wrapper for the formula regex that validates if a string is in the correct chemical formula format or not
        /// </summary>
        private static readonly Regex ValidateFormulaRegex = new Regex("^(" + FormulaRegex + ")+$", RegexOptions.Compiled);

        /// <summary>
        /// Determines if the chemical formula hill notation string is stored or calculated each time it is called.
        /// True means the notation is stored as a string in the chemical formula (quicker, but more memory)
        /// False means the notation is not stored as a string in the chemical formula (slower, no used memory)
        /// Changing this value will not automatically delete the interned notations of formulas.
        /// The default value is true.
        /// </summary>
        public static bool InternChemicalFormulaStrings = true;

        // Only gets elements which are not known if they are isotopes or not!!!
        internal Dictionary<Element, int> GetElements()
        {
            return elements;
        }

        /// <summary>
        /// The default empty chemicalFormula
        /// </summary>
        public static readonly ChemicalFormula Empty = new ChemicalFormula();


        /// <summary>
        /// Main data store, the isotopes.
        /// <remarks>Acts as a dictionary where each isotope's UniqueID
        /// is the key (index) of this array. The array is front loaded to provide the most
        /// common elements first (C H N O P) to reduce memory footprint and provide quick
        /// addition/subtraction of formulas.</remarks>
        /// </summary>
        private Dictionary<Isotope, int> isotopes;
        private Dictionary<Element, int> elements;
        
        #region Constructors


        /// <summary>
        /// Create an chemical formula from the given string representation
        /// </summary>
        /// <param name="chemicalFormula">The string representation of the chemical formula</param>
        public ChemicalFormula(string chemicalFormula)
        {
            isotopes = new Dictionary<Isotope, int>();
            elements = new Dictionary<Element, int>();
            ParseFormulaAndAddElements(chemicalFormula);
        }

        /// <summary>
        /// Create an chemical formula from an item that contains a chemical formula
        /// </summary>
        /// <param name="item">The item of which a new chemical formula will be made from</param>
        public ChemicalFormula(IHasChemicalFormula item)
            : this(item.thisChemicalFormula)
        {
        }

        /// <summary>
        /// Create a copy of a chemical formula from another chemical formula
        /// </summary>
        /// <param name="other">The chemical formula to copy</param>
        public ChemicalFormula(ChemicalFormula other):this()
        {
            if (other != null)
            {
                isotopes = new Dictionary<Isotope, int>(other.isotopes);
                elements = new Dictionary<Element, int>(other.elements);
                MonoisotopicMass = other.MonoisotopicMass;
            }
        }

        public ChemicalFormula()
        {
            isotopes = new Dictionary<Isotope, int>();
            elements = new Dictionary<Element, int>();
            MonoisotopicMass = 0;
        }


        #endregion Constructors

        #region Properties

        /// <summary>
        /// Gets the average mass of this chemical formula
        /// </summary>
        public double AverageMass
        {
            get
            {
                return 0;
            }
        }

        /// <summary>
        /// Gets the monoisotopic mass of this chemical formula: for elements use the principle isotope mass, not average mass
        /// </summary>
        public double MonoisotopicMass { get; private set; }

        /// <summary>
        /// Gets the number of atoms in this chemical formula
        /// </summary>
        public int AtomCount
        {
            get
            {
                return isotopes.Sum(b => b.Value) + elements.Sum(b => b.Value);
            }
        }

        /// <summary>
        /// Gets the number of unique chemical elements in this chemical formula
        /// </summary>
        public int NumberOfUniqueElementsByAtomicNumber
        {
            get
            {
                HashSet<int> ok = new HashSet<int>();
                foreach (var i in isotopes)
                    ok.Add(i.Key.AtomicNumber);
                foreach (var i in elements)
                    ok.Add(i.Key.AtomicNumber);
                return ok.Count;
            }
        }

        /// <summary>
        /// Gets the number of unique chemical isotopes in this chemical formula
        /// </summary>
        public int NumberOfUniqueIsotopes
        {
            get
            {
                return isotopes.Count();
            }
        }

        /// <summary>
        /// Gets the string representation (Hill Notation) of this chemical formula
        /// </summary>
        public string Formula
        {
            get
            {
                return GetHillNotation();
            }
        }

        #endregion Properties

        #region Add/Remove

        /// <summary>
        /// Replaces one isotope with another.
        /// Replacement happens on a 1 to 1 basis, i.e., if you remove 5 you will add 5
        /// </summary>
        /// <param name="isotopeToRemove">The isotope to remove</param>
        /// <param name="isotopToAdd">The isotope to add</param>
        public void Replace(Isotope isotopeToRemove, Isotope isotopToAdd)
        {
            int numberRemoved = Remove(isotopeToRemove);
            Add(isotopToAdd, numberRemoved);
        }

        /// <summary>
        /// Add a chemical formula containing object to this chemical formula
        /// </summary>
        /// <param name="item">The object that contains a chemical formula</param>
        public void Add(IHasChemicalFormula item)
        {
            if (item == null)
                return;
            Add(item.thisChemicalFormula);
        }

        /// <summary>
        /// Add a chemical formula to this chemical formula.
        /// </summary>
        /// <param name="formula">The chemical formula to add to this</param>
        public void Add(ChemicalFormula formula)
        {
            if (formula == null)
                return;

            foreach (var e in formula.GetElements())
                Add(e.Key, e.Value);
                foreach (var i in formula.GetIsotopes())
                Add(i.Key, i.Value);

            MonoisotopicMass += formula.MonoisotopicMass;
        }

        /// <summary>
        /// Add the principal isotope of the element to this chemical formula
        /// given its chemical symbol
        /// </summary>
        /// <param name="symbol">The chemical symbol of the element to add</param>
        /// <param name="count">The number of the element to add</param>
        public void Add(string symbol, int count)
        {
            try
            {
                Isotope isotope = PeriodicTable.GetElement(symbol).PrincipalIsotope;
                Add(isotope, count);
            }
            catch (KeyNotFoundException e)
            {
                throw new KeyNotFoundException(string.Format("The element symbol '{0}' is not found in the periodic table", symbol), e);
            }
        }

        public void Add(Element element, int count)
        {
            if (count == 0 || element == null)
                return;
            MonoisotopicMass += element.PrincipalIsotope.AtomicMass * count;
            if (!elements.ContainsKey(element))
                elements.Add(element, count);
            else
            {
                elements[element] += count;
                if (elements[element] == 0)
                    elements.Remove(element);
            }
        }

        /// <summary>
        /// Add an isotope to this chemical formula
        /// </summary>
        /// <param name="isotope">The isotope to add</param>
        /// <param name="count">The number of the isotope to add</param>
        public void Add(Isotope isotope, int count)
        {
            if (count == 0 || isotope == null)
                return;
            MonoisotopicMass += isotope.AtomicMass * count;
            if (!isotopes.ContainsKey(isotope))
                isotopes.Add(isotope, count);
            else
            {
                isotopes[isotope] += count;
                if (isotopes[isotope] == 0)
                    isotopes.Remove(isotope);
            }
        }

        /// <summary>
        /// Remove a chemical formula containing object from this chemical formula
        /// </summary>
        /// <param name="item">The object that contains a chemical formula</param>
        public void Remove(IHasChemicalFormula item)
        {
            if (item == null)
                return;
            Remove(item.thisChemicalFormula);
        }

        /// <summary>
        /// Remove a chemical formula from this chemical formula
        /// </summary>
        /// <param name="formula">The chemical formula to remove</param>
        public void Remove(ChemicalFormula formula)
        {
            if (formula == null)
                return;
            MonoisotopicMass -= formula.MonoisotopicMass;
            foreach (var e in formula.GetElements())
                Remove(e.Key, e.Value);
            foreach (var i in formula.GetIsotopes())
                Remove(i.Key, i.Value);
        }

        public void Remove(Element key, int count)
        {
            Add(key, -count);
        }

        /// <summary>
        /// Remove the principal isotope of the element represented by the symbol
        /// from this chemical formula
        /// </summary>
        /// <param name="symbol">The symbol of the chemical element to remove</param>
        /// <param name="count">The number of isotopes to remove</param>
        public void Remove(string symbol, int count)
        {
            Add(PeriodicTable.GetElement(symbol).PrincipalIsotope, -count);
        }

        /// <summary>
        /// Remove a isotope from this chemical formula
        /// </summary>
        /// <param name="isotope">The isotope to remove</param>
        /// <param name="count">The number of isotopes to remove</param>
        public void Remove(Isotope isotope, int count)
        {
            Add(isotope, -count);
        }

        /// <summary>
        /// Completely removes a particular isotope from this chemical formula.
        /// </summary>
        /// <param name="isotope">The isotope to remove</param>
        /// <returns>Number of removed isotopes</returns>
        public int Remove(Isotope isotope)
        {
            if (isotope == null)
                return 0;
            int count= isotopes[isotope];
            Add(isotope, -count);
            return count;
        }

        /// <summary>
        /// Remove all the isotopes of an chemical element represented by the symbol
        /// from this chemical formula
        /// </summary>
        /// <param name="symbol">The symbol of the chemical element to remove</param>
        /// <returns>Number of removed isotopes</returns>
        public int RemoveIsotopesOf(string symbol)
        {
            return RemoveIsotopesOf(PeriodicTable.GetElement(symbol));
        }

        /// <summary>
        /// Remove all the isotopes of an chemical element from this
        /// chemical formula
        /// </summary>
        /// <param name="element">The chemical element to remove</param>
        /// <returns>Number of removed isotopes</returns>
        public int RemoveIsotopesOf(Element element)
        {
            if (element == null)
                return 0;
            int count = elements[element];
            Add(element, -count);
            
            foreach (var k in isotopes.Where(b => b.Key.Element == element).ToList())
            {
                isotopes.Remove(k.Key);
            }
            //foreach(var i in isotopes)
            //{
            //    if (i.Key.Element == element)
            //    {
            //        count += i.Value;
            //        Add(i.Key, -i.Value);
            //    }
            //}
            return count;
        }

        /// <summary>
        /// Remove all isotopes and elements
        /// </summary>
        public void Clear()
        {
            isotopes = new Dictionary<Isotope, int>();
            elements = new Dictionary<Element, int>();
            MonoisotopicMass = 0;
        }

        #endregion Add/Remove

        #region Count/Contains

        /// <summary>
        /// Checks if the isotope is present in this chemical formula
        /// </summary>
        /// <param name="isotope">The isotope to look for</param>
        /// <returns>True if there is a non-negative number of the isotope in this formula</returns>
        public bool Contains(Isotope isotope)
        {
            return Count(isotope) != 0;
        }

        /// <summary>
        /// Checks if any isotope of the specified element is present in this chemical formula
        /// </summary>
        /// <param name="element">The element to look for</param>
        /// <returns>True if there is a non-zero number of the element in this formula</returns>
        public bool Contains(Element element)
        {
            return Count(element) != 0;
        }

        public bool Contains(string symbol)
        {
            return Count(symbol) != 0;
        }

        public bool Contains(ChemicalFormula formula)
        {
            return IsSuperSetOf(formula);
        }

        public bool IsSubSetOf(ChemicalFormula formula)
        {
            return formula != null && formula.IsSuperSetOf(this);
        }

        /// <summary>
        /// Checks whether this formula contains all the isotopes of the specified formula
        /// MIGHT CONSIDER ELEMENTS TO BE SUPERSET OF ISOTOPES IF NEEDED!!!
        /// Right now they are distinct
        /// </summary>
        /// <param name="formula"></param>
        /// <returns></returns>
        public bool IsSuperSetOf(ChemicalFormula formula)
        {
            foreach(var aa in formula.elements)
            {
                if (!elements.ContainsKey(aa.Key) || aa.Value > elements[aa.Key])
                    return false;
            }
            foreach (var aa in formula.isotopes)
            {
                if (!isotopes.ContainsKey(aa.Key) || aa.Value > isotopes[aa.Key])
                    return false;
            }
            return true;
        }

        public bool Contains(string symbol, int atomicNumber)
        {
            return Count(symbol, atomicNumber) != 0;
        }

        /// <summary>
        /// Return the number of given isotopes in this chemical fomrula
        /// </summary>
        /// <param name="isotope"></param>
        /// <returns></returns>
        public int Count(Isotope isotope)
        {
            if (isotope == null)
                return 0;
            return isotopes[isotope];
        }

        /// <summary>
        /// Count the number of isotopes and elements from this element that are
        /// present in this chemical formula
        /// </summary>
        /// <param name="element">The element to search for</param>
        /// <returns>The total number of all the element isotopes in this chemical formula</returns>
        public int Count(Element element)
        {
            if (element == null)
                return 0;
            var isotopeCount = element.Isotopes.Values.Sum(isotope => Count(isotope));
            return isotopeCount + elements[element];
        }

        public int Count(string symbol)
        {
            Element element = PeriodicTable.GetElement(symbol);
            return Count(element);
        }

        public int Count(string symbol, int atomicNumber)
        {
            Isotope isotope = PeriodicTable.GetElement(symbol)[atomicNumber];
            return Count(isotope);
        }


        /// <summary>
        /// Gets the ratio of the number of Carbon to Hydrogen in this chemical formula
        /// </summary>
        /// <returns></returns>
        public double GetCarbonHydrogenRatio()
        {
            int carbonCount = Count("C");

            if (carbonCount == 0)
                return 0;

            int hydrogenCount = Count("H");

            return hydrogenCount / (double)carbonCount;
        }

        #endregion Count/Contains

        public override int GetHashCode()
        {
            return Convert.ToInt32(MonoisotopicMass);
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as ChemicalFormula);
        }

        public bool Equals(ChemicalFormula other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            foreach(var kk in other.isotopes)
            {
                if (!isotopes.ContainsKey(kk.Key) || isotopes[kk.Key] != kk.Value)
                    return false;
            }
            foreach (var kk in other.elements)
            {
                if (!elements.ContainsKey(kk.Key) || elements[kk.Key] != kk.Value)
                    return false;
            }
            return true;
        }
       
        public override string ToString()
        {
            return Formula;
        }


        #region Private Methods
        
        /// <summary>
        /// Parses a string representation of chemical formula and adds the elements
        /// to this chemical formula
        /// </summary>
        /// <param name="formula">the Chemical Formula to parse</param>
        private void ParseFormulaAndAddElements(string formula)
        {

            if (string.IsNullOrEmpty(formula))
                return;

            if (!IsValidChemicalFormula(formula))
            {
                throw new FormatException("Input string for chemical formula was in an incorrect format");
            }

            foreach (Match match in FormulaRegex.Matches(formula))
            {
                string chemsym = match.Groups[1].Value; // Group 1: Chemical Symbol

                Element element;
                if (PeriodicTable.TryGetElement(chemsym, out element))
                {
                    //Isotope isotope = element.PrincipalIsotope; // Start with the most abundant (principal) isotope

                    int sign = match.Groups[3].Success ? // Group 3 (optional): Negative Sign
                        -1 :
                        1;

                    int numofelem = match.Groups[4].Success ? // Group 4 (optional): Number of Elements
                        int.Parse(match.Groups[4].Value) :
                        1;

                    if (match.Groups[2].Success) // Group 2 (optional): Isotope Mass Number
                    {
                        Add(element[int.Parse(match.Groups[2].Value)], sign * numofelem);
                    }
                    else
                    {
                        Add(element, numofelem * sign);
                    }
                }
                else
                {
                    throw new ArgumentException(string.Format("The chemical Symbol '{0}' does not exist in the Periodic Table", chemsym));
                }
            }
        }

        #endregion Private Methods

        #region Internal

        /// <summary>
        /// Get the internal isotope array for this chemical formula as a deep copy.
        /// </summary>
        /// <returns>The isotopes that make up this chemical formula</returns>
        internal Dictionary<Isotope, int> GetIsotopes()
        {
            return isotopes;
        }

        #endregion Internal

        #region Statics

        public static implicit operator ChemicalFormula(string sequence)
        {
            return new ChemicalFormula(sequence);
        }

        public static implicit operator String(ChemicalFormula sequence)
        {
            return sequence.ToString();
        }

        public static bool IsValidChemicalFormula(string chemicalFormula)
        {
            return ValidateFormulaRegex.IsMatch(chemicalFormula);
        }

        public static ChemicalFormula operator -(ChemicalFormula left, IHasChemicalFormula right)
        {
            if (left == null)
            {
                if (right == null)
                    return null;
                return new ChemicalFormula(right);
            }

            ChemicalFormula newFormula = new ChemicalFormula(left);
            newFormula.Remove(right);
            return newFormula;
        }

        public static ChemicalFormula operator *(ChemicalFormula formula, int count)
        {
            if (count == 0)
                return new ChemicalFormula();
            if (formula == null)
                return null;
            
            ChemicalFormula newFormula = new ChemicalFormula();
            foreach (var kk in formula.isotopes)
                newFormula.Add(kk.Key, kk.Value * count);
            foreach (var kk in formula.elements)
                newFormula.Add(kk.Key, kk.Value * count);
            newFormula.MonoisotopicMass = formula.MonoisotopicMass * count;
            return newFormula;
        }

        public static ChemicalFormula operator *(int count, ChemicalFormula formula)
        {
            return formula * count;
        }

        public static ChemicalFormula operator +(ChemicalFormula left, IHasChemicalFormula right)
        {
            if (left == null)
            {
                if (right == null)
                    return null;
                return new ChemicalFormula(right);
            }
            if (right == null)
                return new ChemicalFormula(left);

            ChemicalFormula newFormula = new ChemicalFormula(left);
            newFormula.Add(right);
            newFormula.MonoisotopicMass = left.MonoisotopicMass + right.MonoisotopicMass;
            return newFormula;
        }

        public static ChemicalFormula Combine(IEnumerable<IHasChemicalFormula> formulas)
        {
            ChemicalFormula returnFormula = new ChemicalFormula();
            foreach (IHasChemicalFormula iformula in formulas)
                returnFormula.Add(iformula);
            return returnFormula;
        }

        public double GetProtonCount()
        {
            int count = 0;
            foreach (var kk in isotopes)
                count += kk.Key.AtomicNumber * kk.Value;
            foreach (var kk in elements)
                count += kk.Key.AtomicNumber * kk.Value;
            return count;
        }

        public double GetNeutronCount()
        {
            int count = 0;
            if (elements.Count > 0)
                throw new Exception("Cannot know for sure what the number of neutrons is!");
            foreach (var kk in isotopes)
                count += kk.Key.Neutrons * kk.Value;
            return count;
        }

        #endregion Statics

        #region IHasChemicalFormula

        ChemicalFormula IHasChemicalFormula.thisChemicalFormula
        {
            get { return this; }
        }

        #endregion IHasChemicalFormula

        /// <summary>
        /// Produces the Hill Notation of the chemical formula
        /// </summary>
        public string GetHillNotation(string delimiter = "")
        {
            string s= "";

            // Find carbon
            if (elements.ContainsKey(PeriodicTable.GetElement("C"))) {
                s += "C";
                s += (elements[PeriodicTable.GetElement("C")] == 1 ? "" : "" + elements[PeriodicTable.GetElement("C")]);
                s += delimiter;
            }

            // Find carbon isotopes
            foreach(var i in PeriodicTable.GetElement("C").Isotopes)
            {
                if (isotopes.ContainsKey(i.Value))
                {
                    s += "C{";
                    s += i.Key;
                    s += "}";
                    s += (isotopes[i.Value] == 1 ? "" : "" + isotopes[i.Value]);
                    s += delimiter;
                }
            }

            // Find hydrogen
            if (elements.ContainsKey(PeriodicTable.GetElement("H")))
            {
                s += "H";
                s += (elements[PeriodicTable.GetElement("H")] == 1 ? "" : "" + elements[PeriodicTable.GetElement("H")]);
                s += delimiter;
            }

            // Find hydrogen isotopes
            foreach (var i in PeriodicTable.GetElement("H").Isotopes)
            {
                if (isotopes.ContainsKey(i.Value))
                {
                    s += "H{";
                    s += i.Key;
                    s += "}";
                    s += (isotopes[i.Value] == 1 ? "" : "" + isotopes[i.Value]);
                    s += delimiter;
                }
            }
            
            List<string> otherParts = new List<string>();

            foreach(var i in elements)
            {
                if (i.Key != PeriodicTable.GetElement("C") && i.Key != PeriodicTable.GetElement("H"))
                    otherParts.Add(i.Key.AtomicSymbol + (i.Value==1 ? "": ""+i.Value));
            }

            foreach (var i in isotopes)
            {
                if (i.Key.Element != PeriodicTable.GetElement("C") && i.Key.Element != PeriodicTable.GetElement("H"))
                    otherParts.Add(i.Key.AtomicSymbol +"{"+i.Key.MassNumber+"}"+ (i.Value == 1 ? "" : "" + i.Value));
            }

            otherParts.Sort();
            return s+string.Join(delimiter, otherParts);
        }
        
    }
}