using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LeanKit.Analytics.Models.Factories;
using NUnit.Framework;

namespace LeanKit.Analytics.Tests.Models.Factories
{
    [TestFixture]
    [Ignore]
    public class HomeViewModelFactoryTests
    {
        [TestCase("Developing", 0)]
        [TestCase("Testing", 1)]
        [TestCase("Waiting to Test", 2)]
        [TestCase("Waiting to Release", 3)]
        [TestCase("Blocked", 4)]
        public void SetsActivities(string expectedActivityName, int index)
        {
            Assert.That(new HomeViewModelFactory().Build().MainWasteGraph.Activities.ElementAt(index).Activity, Is.EqualTo(expectedActivityName));
        }

        [TestCase("Developing", 30)]
        [TestCase("Testing", 15)]
        [TestCase("Waiting to Test", 5)]
        [TestCase("Waiting to Release", 15)]
        [TestCase("Blocked", 35)]
        public void SetActivityPercent(string activityName, int expectedPercent)
        {
            Assert.That(new HomeViewModelFactory().Build().MainWasteGraph.Activities.First(a => a.Activity == activityName).Percent, Is.EqualTo(expectedPercent));
        }
        
        [TestCase("Developing", false)]
        [TestCase("Testing", false)]
        [TestCase("Waiting to Test", true)]
        [TestCase("Waiting to Release", true)]
        [TestCase("Blocked", true)]
        public void SetActivityIsWaste(string activityName, bool expectedIsWaste)
        {
            Assert.That(new HomeViewModelFactory().Build().MainWasteGraph.Activities.First(a => a.Activity == activityName).IsWaste, Is.EqualTo(expectedIsWaste));
        }

        [Test]
        public void SetActivityClassName()
        {
            Assert.That(new HomeViewModelFactory().Build().MainWasteGraph.Activities.First(a => a.Activity == "Blocked").ClassName, Is.EqualTo("Blocked"));
        }

        [Test]
        public void SetActivityRemovesSpacesFromClassName()
        {
            Assert.That(new HomeViewModelFactory().Build().MainWasteGraph.Activities.First(a => a.Activity == "Waiting to Test").ClassName, Is.EqualTo("WaitingtoTest"));
        }
    }
}
