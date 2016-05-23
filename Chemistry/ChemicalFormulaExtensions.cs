using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chemistry
{
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

        public static IEnumerable<IHasChemicalFormula> Validate(this IEnumerable<IHasChemicalFormula> formulas, FilterTypes filters = FilterTypes.All)
        {
            bool useHydrogenCarbonRatio = filters.HasFlag(FilterTypes.HydrogenCarbonRatio);

            foreach (IHasChemicalFormula formula in formulas)
            {
                if (useHydrogenCarbonRatio)
                {
                    double ratio = formula.thisChemicalFormula.GetCarbonHydrogenRatio();

                    if (ratio < 0.5 || ratio > 2.0)
                        continue;
                }

                yield return formula;
            }
        }
    }
}
