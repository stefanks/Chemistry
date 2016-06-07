﻿// Copyright 2012, 2013, 2014 Derek J. Bailey
//
// This file (MassTestFixture.cs) is part of CSMSL.Tests.
//
// CSMSL.Tests is free software: you can redistribute it and/or modify it
// under the terms of the GNU Lesser General Public License as published
// by the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// CSMSL.Tests is distributed in the hope that it will be useful, but WITHOUT
// ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or
// FITNESS FOR A PARTICULAR PURPOSE. See the GNU Lesser General Public
// License for more details.
//
// You should have received a copy of the GNU Lesser General Public
// License along with CSMSL.Tests. If not, see <http://www.gnu.org/licenses/>.

using Chemistry;
using NUnit.Framework;
using System;

namespace Test
{
    internal class ObjectWithMass1000 : IHasMass
    {
        public double MonoisotopicMass
        {
            get
            {
                return 1000;
            }
        }
    }


    [TestFixture]
    public class MassTestFixture
    {

        [Test]
        public void MassToMzPositiveCharge()
        {
            ObjectWithMass1000 a = new ObjectWithMass1000();
            double mz = a.ToMz(2);
            Assert.AreEqual(501.00727646687898, mz);
        }

        [Test]
        public void MassToMzNegativeCharge()
        {
            ObjectWithMass1000 a = new ObjectWithMass1000();
            double mz = a.ToMz(-2);
            Assert.AreEqual(498.99272353312102, mz);
        }

        [Test]
        public void MassToMzZeroCharge()
        {
            ObjectWithMass1000 a = new ObjectWithMass1000();
            var ex = Assert.Throws<DivideByZeroException>(() => a.ToMz(0));
            Assert.That(ex.Message, Is.EqualTo("Charge cannot be zero"));
        }

        [Test]
        public void MzToMassPostitiveCharge()
        {

            double a = 524.3;
            Assert.AreEqual(1046.5854470662418, a.ToMass(2));
        }

        [Test]
        public void MzToMassNegativeCharge()
        {
            double a = 524.3;
            Assert.AreEqual(1050.614552933758, a.ToMass(-2));
        }

        [Test]
        public void MzTomassZeroCharge()
        {
            double a = 524.3;
            var ex = Assert.Throws<DivideByZeroException>(() => a.ToMass(0));
            Assert.That(ex.Message, Is.EqualTo("Charge cannot be zero"));
        }

        [Test]
        public void MassEqualsTest1()
        {

            ObjectWithMass1000 a = new ObjectWithMass1000();
            ObjectWithMass1000 b = new ObjectWithMass1000();
            Assert.IsTrue(a.MassEquals(b));
            Assert.IsTrue(a.CompareMass(b) == 0);
        }


        [Test]
        public void MassEqualsTest2()
        {

            ObjectWithMass1000 a = new ObjectWithMass1000();
            double b = 1000 + 5e-11;
            double c = 1000 + 2e-10;
            Assert.IsTrue(a.MassEquals(b));
            Assert.IsFalse(c.MassEquals(a));
        }
    }
}