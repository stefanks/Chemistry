// Copyright 2012, 2013, 2014 Derek J. Bailey
// Modified work copyright 2016 Stefan Solntsev
// 
// This file (IChemicalFormula.cs) is part of Chemistry Library.
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
    /// An object that has a chemical formula
    /// </summary>
    public interface IChemicalFormula : IMass
    {
        /// <summary>
        /// The chemical formula of this object
        /// </summary>
        ChemicalFormula ChemicalFormula { get; }
    }

    public static class ChemicalFormulaExtensions
    {
        [Flags]
        public enum FilterTypes
        {
            None = 0,
            Valence = 1,
            HydrogenCarbonRatio = 2,
            All = 3,
        }

        public static IEnumerable<IChemicalFormula> Validate(this IEnumerable<IChemicalFormula> formulas, FilterTypes filters = FilterTypes.All)
        {
            bool useHydrogenCarbonRatio = filters.HasFlag(FilterTypes.HydrogenCarbonRatio);

            foreach (IChemicalFormula formula in formulas)
            {
                if (useHydrogenCarbonRatio)
                {
                    double ratio = formula.ChemicalFormula.GetCarbonHydrogenRatio();

                    if (ratio < 0.5 || ratio > 2.0)
                        continue;
                }

                yield return formula;
            }
        }
    }
}