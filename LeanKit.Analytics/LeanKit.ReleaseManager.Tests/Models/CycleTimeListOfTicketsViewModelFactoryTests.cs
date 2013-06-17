using System;
using System.Linq;
using LeanKit.Data;
using LeanKit.Data.SQL;
using LeanKit.ReleaseManager.Models;
using LeanKit.ReleaseManager.Models.CycleTime;
using NUnit.Framework;

namespace LeanKit.ReleaseManager.Tests.Models
{
    [TestFixture]
    public class CycleTimeListOfTicketsViewModelFactoryTests : IMakeCycleTimeReleaseViewModels
    {
        private CycleTimeReleaseViewModel _cycleTimeTicketReleaseInfo;

        [Test]
        public void SetsTicketId()
        {
            const int expectedId = 12345;

            IMakeCycleTimeReleaseViewModels cycleReleaseInfoFactory = this;

            var cycleTimeViewModelFactory = new CycleTimeListOfTicketsViewModelFactory(cycleReleaseInfoFactory);
            var cycleTimeViewModel = cycleTimeViewModelFactory.Build(new[]
                {
                    new Ticket { Id = expectedId, CycleTime = new WorkDuration() }
                });

            Assert.That(cycleTimeViewModel.First().Id, Is.EqualTo(expectedId));
        }

        [Test]
        public void SetsTicketExternalId()
        {
            const string expectedExternalId = "R-1234";

            IMakeCycleTimeReleaseViewModels cycleReleaseInfoFactory = this;

            var cycleTimeViewModelFactory = new CycleTimeListOfTicketsViewModelFactory(cycleReleaseInfoFactory);
            var cycleTimeViewModel = cycleTimeViewModelFactory.Build(new[]
                {
                    new Ticket { ExternalId = expectedExternalId, CycleTime = new WorkDuration() }
                });

            Assert.That(cycleTimeViewModel.First().ExternalId, Is.EqualTo(expectedExternalId));
        }

        [Test]
        public void SetsTicketTitle()
        {
            const string expectedTitle = "Example Ticket";

            IMakeCycleTimeReleaseViewModels cycleReleaseInfoFactory = this;

            var cycleTimeViewModelFactory = new CycleTimeListOfTicketsViewModelFactory(cycleReleaseInfoFactory);
            var cycleTimeViewModel = cycleTimeViewModelFactory.Build(new []
                {
                    new Ticket { Title = expectedTitle, CycleTime = new WorkDuration() }
                });

            Assert.That(cycleTimeViewModel.First().Title, Is.EqualTo(expectedTitle));
        }

        [Test]
        public void SetsTicketStartedFriendlyDate()
        {
            const string expectedStarted = "Today 10:00";

            IMakeCycleTimeReleaseViewModels cycleReleaseInfoFactory = this;

            var cycleTimeViewModelFactory = new CycleTimeListOfTicketsViewModelFactory(cycleReleaseInfoFactory);
            var cycleTimeViewModel = cycleTimeViewModelFactory.Build(new []
                {
                    new Ticket { Started = DateTime.Now.Date.AddHours(10), CycleTime = new WorkDuration() }
                });

            Assert.That(cycleTimeViewModel.First().StartedFriendlyText, Is.EqualTo(expectedStarted));
        }

        [Test]
        public void SetsTicketFinishedFriendlyDate()
        {
            const string expectedFinished = "Today 10:00";

            IMakeCycleTimeReleaseViewModels cycleReleaseInfoFactory = this;

            var cycleTimeViewModelFactory = new CycleTimeListOfTicketsViewModelFactory(cycleReleaseInfoFactory);
            var cycleTimeViewModel = cycleTimeViewModelFactory.Build(new []
                {
                    new Ticket { Finished = DateTime.Now.Date.AddHours(10), CycleTime = new WorkDuration() }
                });

            Assert.That(cycleTimeViewModel.First().FinishedFriendlyText, Is.EqualTo(expectedFinished));
        }

        [Test]
        public void SetsTicketDurationToOneDay()
        {
            const string expectedDuration = "1 Day";

            IMakeCycleTimeReleaseViewModels cycleReleaseInfoFactory = this;

            var cycleTimeViewModelFactory = new CycleTimeListOfTicketsViewModelFactory(cycleReleaseInfoFactory);
            var cycleTimeViewModel = cycleTimeViewModelFactory.Build(new[]
                {
                    new Ticket {CycleTime = new WorkDuration {Days = 1}}
                });

            Assert.That(cycleTimeViewModel.First().Duration, Is.EqualTo(expectedDuration));
        }

        [Test]
        public void SetsTicketDurationToTwoDays()
        {
            const string expectedDuration = "2 Days";

            IMakeCycleTimeReleaseViewModels cycleReleaseInfoFactory = this;

            var cycleTimeViewModelFactory = new CycleTimeListOfTicketsViewModelFactory(cycleReleaseInfoFactory);
            var cycleTimeViewModel = cycleTimeViewModelFactory.Build(new[]
                {
                    new Ticket {CycleTime = new WorkDuration {Days = 2}}
                });

            Assert.That(cycleTimeViewModel.First().Duration, Is.EqualTo(expectedDuration));
        }

        [Test]
        public void SetsTicketSize()
        {
            IMakeCycleTimeReleaseViewModels cycleReleaseInfoFactory = this;

            var cycleTimeViewModelFactory = new CycleTimeListOfTicketsViewModelFactory(cycleReleaseInfoFactory);
            var cycleTimeViewModel = cycleTimeViewModelFactory.Build(new []
                {
                    new Ticket{ Size = 2, CycleTime = new WorkDuration() }
                });

            Assert.That(cycleTimeViewModel.First().Size, Is.EqualTo("2"));
        }

        [Test]
        public void SetsTicketUnknownSizeWhenSizeIsZero()
        {
            IMakeCycleTimeReleaseViewModels cycleReleaseInfoFactory = this;

            var cycleTimeViewModelFactory = new CycleTimeListOfTicketsViewModelFactory(cycleReleaseInfoFactory);
            var cycleTimeViewModel = cycleTimeViewModelFactory.Build(new []
                {
                    new Ticket { CycleTime = new WorkDuration() }
                });

            Assert.That(cycleTimeViewModel.First().Size, Is.EqualTo("?"));
        }

        [Test]
        public void SetsReleaseInfo()
        {
            _cycleTimeTicketReleaseInfo = new CycleTimeReleaseViewModel();

            IMakeCycleTimeReleaseViewModels cycleReleaseInfoFactory = this;

            var cycleTimeViewModelFactory = new CycleTimeListOfTicketsViewModelFactory(cycleReleaseInfoFactory);
            var cycleTimeViewModel = cycleTimeViewModelFactory.Build(new[]
                {
                    new Ticket { CycleTime = new WorkDuration() }
                });

            Assert.That(cycleTimeViewModel.First().Release, Is.EqualTo(_cycleTimeTicketReleaseInfo));
        }

        public CycleTimeReleaseViewModel Build(TicketReleaseInfo ticketReleaseInfo)
        {
            return _cycleTimeTicketReleaseInfo;
        }
    }
}