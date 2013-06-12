using System.Linq;
using LeanKit.ReleaseManager.Models;
using NUnit.Framework;

namespace LeanKit.ReleaseManager.Tests.Models
{
    [TestFixture]
    public class TimePeriodViewModelFactoryTests
    {
        [TestCase(0, "This Week")]
        [TestCase(1, "Last Week")]
        [TestCase(2, "Last 30 Days")]
        [TestCase(3, "All Time")]
        public void SetsLabel(int index, string expectedLabel)
        {
            Assert.That(new CycleTimePeriodViewModelFactory().Build(null).ElementAt(index).Label, Is.EqualTo(expectedLabel));
        }

        [TestCase(0, "this-week")]
        [TestCase(1, "last-week")]
        [TestCase(2, "30")]
        [TestCase(3, "all-time")]
        public void SetsValue(int index, string expectedValue)
        {
            Assert.That(new CycleTimePeriodViewModelFactory().Build(null).ElementAt(index).Value, Is.EqualTo(expectedValue));
        }

        [Test]
        public void SetsSelectedTrueOnSelectedItem()
        {
            Assert.That(new CycleTimePeriodViewModelFactory().Build("this-week").First().Selected, Is.True);
        }
    }
}