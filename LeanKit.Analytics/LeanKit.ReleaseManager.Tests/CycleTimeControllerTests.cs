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
    public class CycleTimeControllerTests : IBuildCycleTimeViewModels, IMakeCycleTimeQueries
    {
        private CycleTimeViewModel _expectedViewModel;
        private CycleTimeQuery _queryPassedToViewModelFactory;
        private CycleTimeQuery _query;

        [Test]
        public void SetsViewModelFromFactory()
        {
            _expectedViewModel = new CycleTimeViewModel();

            IBuildCycleTimeViewModels cycleTimeViewModelFactory = this;
            IMakeCycleTimeQueries queryFactory = this;
            Assert.That(new CycleTimeController(cycleTimeViewModelFactory, queryFactory).Index("").ViewData.Model, Is.EqualTo(_expectedViewModel));
        }

        [Test]
        public void UsesCycleTimeQueryBuiltByFactoryToPassToViewModelBuild()
        {
            var expectedQuery = new CycleTimeQuery();

            _query = expectedQuery;

            IBuildCycleTimeViewModels cycleTimeViewModelFactory = this;
            IMakeCycleTimeQueries queryFactory = this;
            new CycleTimeController(cycleTimeViewModelFactory, queryFactory).Index("");

            Assert.That(_queryPassedToViewModelFactory, Is.EqualTo(expectedQuery));
        }

        public CycleTimeViewModel Build(CycleTimeQuery query)
        {
            _queryPassedToViewModelFactory = query;
            return _expectedViewModel;
        }

        public CycleTimeQuery Build(string timePeriod)
        {
            return _query;
        }
    }
}
