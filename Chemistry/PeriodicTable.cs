// Copyright 2012, 2013, 2014 Derek J. Bailey
// Modified work copyright 2016 Stefan Solntsev
// 
// This file (PeriodicTable.cs) is part of Chemistry Library.
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
    public static class PeriodicTable
    {
        /// <summary>
        /// The internal dictionary housing elements, keyed by their unique atomic symbol
        /// </summary>
        private static Dictionary<string, Element> _elements = new Dictionary<string, Element>();

        public static void Add(Element element)
        {
            _elements.Add(element.AtomicSymbol, element);
        }

        public static Element GetElement(string atomicSymbol)
        {
            Element element;
            if (_elements.TryGetValue(atomicSymbol, out element))
                return element;
            else
                throw new ArgumentException(string.Format("The atomic Symbol '{0}' does not exist in the Periodic Table", atomicSymbol));
        }
    }
}
