using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LeanKit.ReleaseManager.Controllers;
using LeanKit.ReleaseManager.Models;
using NUnit.Framework;

namespace LeanKit.ReleaseManager.Tests
{
    [TestFixture]
    class CycleTimeControllerTests : IBuildCycleTimeViewModels
    {
        private CycleTimeViewModel _expectedViewModel;

        [Test]
        public void SetsViewModelFromFactory()
        {
            _expectedViewModel = new CycleTimeViewModel();

            IBuildCycleTimeViewModels cycleTimeViewModelFactory = this;
            Assert.That(new CycleTimeController(cycleTimeViewModelFactory).Index().ViewData.Model, Is.EqualTo(_expectedViewModel));
        }

        public CycleTimeViewModel Build()
        {
            return _expectedViewModel;
        }
    }
}
