// Copyright 2012, 2013, 2014 Derek J. Bailey
// Modified work copyright 2016 Stefan Solntsev
// 
// This file (Element.cs) is part of Chemistry Library.
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

namespace Chemistry
{
    /// <summary>
    /// Represents a single chemical element. Elements comprises of multiple
    /// isotopes, with the element mass being a weighted average of all the
    /// isotopes atomic masses weighted by their natural relative abundance.
    /// </summary>
    public sealed class Element
    {
        /// <summary>
        /// The element's isotopes stored based on their atomic number
        /// </summary>
        public Dictionary<int, Isotope> Isotopes;

        /// <summary>
        /// Gets an isotope of this element based on its atomic number
        /// </summary>
        /// <param name="atomicNumber">The atomic number of the isotope to get</param>
        /// <returns>The isotope with the supplied atomic number</returns>
        public Isotope this[int atomicNumber]
        {
            get { return Isotopes[atomicNumber]; }
        }

        /// <summary>
        /// Create a new element
        /// </summary>
        /// <param name="name">The name of the element</param>
        /// <param name="symbol">The symbol of the element</param>
        /// <param name="atomicNumber">The atomic number of the element</param>
        public Element(string symbol, int atomicNumber, double averageMass)
        {
            AtomicSymbol = symbol;
            AtomicNumber = atomicNumber;
            AverageMass = averageMass;
            Isotopes = new Dictionary<int, Isotope>();
        }

        /// <summary>
        /// The atomic number of this element (also the number of protons)
        /// </summary>
        public int AtomicNumber { get; private set; }

        /// <summary>
        /// The atomic symbol of this element
        /// </summary>
        public string AtomicSymbol { get; private set; }

        /// <summary>
        /// The average mass of all this element's isotopes weighted by their
        /// relative natural abundance (in unified atomic mass units)
        /// </summary>
        public double AverageMass { get; private set; }

        /// <summary>
        /// The most abundant (principal) isotope of this element
        /// </summary>
        public Isotope PrincipalIsotope { get; private set; }

        /// <summary>
        /// Returns the atomic symbol
        /// </summary>
        /// <returns>The atomic symbol</returns>
        public override string ToString()
        {
            return AtomicSymbol;
        }

        /// <summary>
        /// Add an isotope to this element
        /// </summary>
        /// <param name="massNumber">The mass number of the isotope</param>
        /// <param name="atomicMass">The atomic mass of the isotope </param>
        /// <param name="abundance">The natural relative abundance of the isotope</param>
        /// <returns>The created isotopes that is added to this element</returns>
        public void AddIsotope(int massNumber, double atomicMass, double abundance)
        {
            if (Isotopes.ContainsKey(massNumber))
                throw new ArgumentException("Isotope with mass number " + massNumber + " already exists");
            var isotope = new Isotope(this, massNumber, atomicMass, abundance);
            Isotopes.Add(massNumber, isotope);
            if (PrincipalIsotope == null || (abundance > PrincipalIsotope.RelativeAbundance))
            {
                if (PrincipalIsotope != null)
                    PrincipalIsotope.IsPrincipalIsotope = false;
                PrincipalIsotope = isotope;
                PrincipalIsotope.IsPrincipalIsotope = true;
            }
        }
    }
}