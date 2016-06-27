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
using System.Globalization;

namespace Chemistry
{

    public static class PeriodicTable
    {
        // Two datastores storing same elements! Need both for efficient access by both symbol and atomic number

        /// <summary>
        /// The internal dictionary housing elements, keyed by their unique atomic symbol
        /// </summary>
        private static Dictionary<string, Element> _elements = new Dictionary<string, Element>();

        /// <summary>
        /// The internal dictionary housing elements, keyed by their unique atomic number
        /// </summary>
        private static Element[] _elementsArray = new Element[Constants.MaximumNumberOfElementsAllowed];

        public static void Add(Element element)
        {
            if (element == null)
                throw new ArgumentNullException("element", "Cannot add a null element to periodic table");
            if (_elements.ContainsKey(element.AtomicSymbol))
                throw new ArgumentException("Element with symbol " + element.AtomicSymbol + " already added!");
            if (_elementsArray[element.AtomicNumber] != null)
                throw new ArgumentException("Element with atomic number " + element.AtomicNumber + " already added!");
            _elements.Add(element.AtomicSymbol, element);
            _elementsArray[element.AtomicNumber] = element;
        }

        /// <summary>
        /// Returns the element corresponding to a given atomic symbol. Needs to be fast.
        /// </summary>
        /// <param name="atomicSymbol"></param>
        /// <returns></returns>
        public static Element GetElement(string atomicSymbol)

        {
            return _elements[atomicSymbol];
        }

        /// <summary>
        /// Fast method of getting element by atomic number. Needs to be fast.
        /// </summary>
        /// <param name="atomicNumber"></param>
        /// <returns></returns>
        public static Element GetElement(int atomicNumber)
        {
            return _elementsArray[atomicNumber];
        }

        /// <summary>
        /// Validates the periodic table with relative accuracy epsilon
        /// </summary>
        public static PeriodicTableValidationResult ValidateAbundances(double epsilon)
        {
            foreach (var e in _elements)
            {
                double totalAbundance = 0;
                double averageMass = 0;
                foreach (Isotope i in e.Value.Isotopes)
                {
                    totalAbundance += i.RelativeAbundance;
                    averageMass += i.RelativeAbundance * i.AtomicMass;
                }
                if (Math.Abs(totalAbundance - 1) > epsilon)
                    return new PeriodicTableValidationResult(ValidationResult.FailedAbundanceValidation, string.Format(CultureInfo.InvariantCulture, StringResources.AbundancesNotValidated, e.Value, totalAbundance));
            }
            return new PeriodicTableValidationResult(ValidationResult.PassedAbundanceValidation, StringResources.ValidationPassed);
        }

        public static PeriodicTableValidationResult ValidateAverageMasses(double epsilon)
        {
            foreach (var e in _elements)
            {
                double totalAbundance = 0;
                double averageMass = 0;
                foreach (Isotope i in e.Value.Isotopes)
                {
                    totalAbundance += i.RelativeAbundance;
                    averageMass += i.RelativeAbundance * i.AtomicMass;
                }
                if (Math.Abs(averageMass - e.Value.AverageMass) / e.Value.AverageMass > epsilon)
                    return new PeriodicTableValidationResult(ValidationResult.FailedAverageMassValidation, string.Format(CultureInfo.InvariantCulture, StringResources.MassesNotValidated, e.Value, averageMass, e.Value.AverageMass));
            }
            return new PeriodicTableValidationResult(ValidationResult.PassedAverageMassValidation, StringResources.ValidationPassed);
        }
    }
}
