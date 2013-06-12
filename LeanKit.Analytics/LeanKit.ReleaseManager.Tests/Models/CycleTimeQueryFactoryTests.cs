using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LeanKit.ReleaseManager.Controllers;
using LeanKit.ReleaseManager.Models;
using LeanKit.Utilities.DateAndTime;
using NUnit.Framework;

namespace LeanKit.ReleaseManager.Tests.Models
{
    [TestFixture]
    public class CycleTimeQueryFactoryTests : IKnowTheCurrentDateAndTime
    {
        private DateTime _currentDateTime;

        [Test]
        public void EmptyStringReturnsEmptyQuery()
        {
            Assert.That(new CycleTimeQueryFactory(null).Build(""), Is.EqualTo(CycleTimeQuery.Empty));
        }

        [Test]
        public void AllTimeReturnsEmptyQuery()
        {
            Assert.That(new CycleTimeQueryFactory(null).Build("all-time"), Is.EqualTo(CycleTimeQuery.Empty));
        }

        [TestCase(-3)]
        [TestCase(-2)]
        [TestCase(-1)]
        [TestCase(0)]
        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        public void ThisWeekSetsStartDateToThePreviousSunday(int daysToAdd)
        {
            _currentDateTime = new DateTime(2013, 6, 5, 13, 44, 12).AddDays(daysToAdd);

            var expectedStartDate = new DateTime(2013, 6, 2);

            IKnowTheCurrentDateAndTime dateTimeWrapper = this;

            Assert.That(new CycleTimeQueryFactory(dateTimeWrapper).Build("this-week").Start, Is.EqualTo(expectedStartDate));
        }

        [TestCase(-3)]
        [TestCase(-2)]
        [TestCase(-1)]
        [TestCase(0)]
        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        public void ThisWeekSetsStartDateToTheNextSaturday(int daysToAdd)
        {
            _currentDateTime = new DateTime(2013, 6, 5, 13, 44, 12).AddDays(daysToAdd);

            var expectedStartDate = new DateTime(2013, 6, 8);

            IKnowTheCurrentDateAndTime dateTimeWrapper = this;

            Assert.That(new CycleTimeQueryFactory(dateTimeWrapper).Build("this-week").End, Is.EqualTo(expectedStartDate));
        }

        public DateTime Now()
        {
            return _currentDateTime;
        }
    }
}
