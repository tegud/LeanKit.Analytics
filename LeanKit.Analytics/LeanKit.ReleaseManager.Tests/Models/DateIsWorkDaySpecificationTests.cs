using System;
using LeanKit.ReleaseManager.Models;
using NUnit.Framework;

namespace LeanKit.ReleaseManager.Tests.Models
{
    [TestFixture]
    public class DateIsWorkDaySpecificationTests
    {
        [Test]
        public void SaturdayReturnsFalse()
        {
            Assert.That(new DateIsWorkDaySpecification().IsSatisfiedBy(new DateTime(2013, 06, 01)), Is.False);
        }

        [Test]
        public void SundayReturnsFalse()
        {
            Assert.That(new DateIsWorkDaySpecification().IsSatisfiedBy(new DateTime(2013, 06, 02)), Is.False);
        }

        [Test]
        public void MondayReturnsTrue()
        {
            Assert.That(new DateIsWorkDaySpecification().IsSatisfiedBy(new DateTime(2013, 06, 03)), Is.True);
        }
    }
}