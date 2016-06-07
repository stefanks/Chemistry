﻿// Copyright 2012, 2013, 2014 Derek J. Bailey
// Modified work copyright 2016 Stefan Solntsev
// 
// This file (MassExtensions.cs) is part of Chemistry Library.
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
namespace Chemistry
{
    public static class MassExtensions
    {

        public static double ToMz(this IHasMass mass, int charge)
        {
            if (charge == 0)
                throw new DivideByZeroException("Charge cannot be zero");
            return mass.MonoisotopicMass / Math.Abs(charge) + Math.Sign(charge) * Constants.Proton;
        }

        public static double ToMass(this double mz, int charge)
        {
            if (charge == 0)
                throw new DivideByZeroException("Charge cannot be zero");
            return Math.Abs(charge) * mz - charge * Constants.Proton;
        }

        public static bool MassEquals(this double mass1, IHasMass mass2, double epsilon = Constants.MassEqualityEpsilon)
        {
            return Math.Abs(mass1 - mass2.MonoisotopicMass) < epsilon;
        }

        public static bool MassEquals(this double mass1, double mass2, double epsilon = Constants.MassEqualityEpsilon)
        {
            return Math.Abs(mass1 - mass2) < epsilon;
        }

        public static bool MassEquals(this IHasMass mass1, double mass2, double epsilon = Constants.MassEqualityEpsilon)
        {
            return Math.Abs(mass1.MonoisotopicMass - mass2) < epsilon;
        }

        public static bool MassEquals(this IHasMass mass1, IHasMass mass2, double epsilon = Constants.MassEqualityEpsilon)
        {
            return Math.Abs(mass1.MonoisotopicMass - mass2.MonoisotopicMass) < epsilon;
        }

        public static int CompareMass(this IHasMass mass1, IHasMass mass2, double epsilon = Constants.MassEqualityEpsilon)
        {
            double difference = mass1.MonoisotopicMass - mass2.MonoisotopicMass;
            if (difference < -epsilon)
                return -1;
            return difference > epsilon ? 1 : 0;
        }
    }
}
