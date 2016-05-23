using System;
namespace Chemistry
{
    public static class MassExtensions
    {
        /// <summary>
        /// The mass difference tolerance for having identical masses
        /// </summary>
        public const double MassEqualityEpsilon = 1e-10;

        /// <summary>
        /// Converts the object that has a mass into a m/z value based on the charge state
        /// </summary>
        /// <param name="mass"></param>
        /// <param name="charge"></param>
        /// <param name="c13Isotope"></param>
        /// <returns></returns>
        public static double ToMz(this IHasMass mass, int charge)
        {
            return Mass.MzFromMass(mass.MonoisotopicMass, charge);
        }

        /// <summary>
        /// Converts the object that has a m/z into a mass value based on the charge state
        /// </summary>
        /// <param name="mz"></param>
        /// <param name="charge"></param>
        /// <returns></returns>
        public static double ToMass(this IHasMass mz, int charge)
        {
            return Mass.MassFromMz(mz.MonoisotopicMass, charge);
        }

        public static bool MassEquals(this double mass1, IHasMass mass2, double epsilon = MassEqualityEpsilon)
        {
            if (mass2 == null)
                return false;
            return Math.Abs(mass1 - mass2.MonoisotopicMass) < epsilon;
        }

        public static bool MassEquals(this double mass1, double mass2, double epsilon = MassEqualityEpsilon)
        {
            return Math.Abs(mass1 - mass2) < epsilon;
        }

        public static bool MassEquals(this IHasMass mass1, double mass2, double epsilon = MassEqualityEpsilon)
        {
            if (mass1 == null)
                return false;
            return Math.Abs(mass1.MonoisotopicMass - mass2) < epsilon;
        }

        public static bool MassEquals(this IHasMass mass1, IHasMass mass2, double epsilon = MassEqualityEpsilon)
        {
            if (mass1 == null || mass2 == null)
                return false;
            return Math.Abs(mass1.MonoisotopicMass - mass2.MonoisotopicMass) < epsilon;
        }

        public static int Compare(this IHasMass mass1, IHasMass mass2, double epsilon = MassEqualityEpsilon)
        {
            double difference = mass1.MonoisotopicMass - mass2.MonoisotopicMass;
            if (difference < -epsilon)
                return -1;
            return difference > epsilon ? 1 : 0;
        }
    }
}
