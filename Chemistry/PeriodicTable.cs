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
// License along with Chemistry Library. If not, see <http://www.gnu.org/licenses/>.

using System.Collections.Generic;

namespace Chemistry
{
    public static class PeriodicTable
    {
        /// <summary>
        /// The internal dictionary housing all the elements, keyed by their unique atomic symbol
        /// </summary>
        private static readonly Dictionary<string, Element> _elements = new Dictionary<string, Element>();

        public static void Add(string atomicSymbol, Element element)
        {
            _elements.Add(atomicSymbol, element);
        }

        public static int Count()
        {
            return _elements.Count;
        }

        public static Element GetElement(string element)
        {
            return _elements[element];
        }

        public static Isotope GetIsotope(string element, int atomicNumber)
        {
            return _elements[element][atomicNumber];
        }

        // Returns true if the periodic table contains an element with the specified elementSymbol; otherwise, false
        public static bool TryGetElement(string elementSymbol, out Element element)
        {
            return _elements.TryGetValue(elementSymbol, out element);
        }
    }
}
