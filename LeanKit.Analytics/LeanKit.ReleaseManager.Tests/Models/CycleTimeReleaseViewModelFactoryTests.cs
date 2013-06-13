using LeanKit.Data;
using LeanKit.ReleaseManager.Models;
using LeanKit.ReleaseManager.Models.CycleTime;
using NUnit.Framework;

namespace LeanKit.ReleaseManager.Tests.Models
{
    public class CycleTimeReleaseViewModelFactoryTests
    {
        [Test]
        public void SetsReleaseId()
        {
            var release = new TicketReleaseInfo { Id = 12345 };
            var viewModel = new CycleTimeReleaseViewModelFactory().Build(release);

            Assert.That(viewModel.Id, Is.EqualTo(12345));
        }

        [Test]
        public void SetsReleaseNameToSvnRevisionWhenPresent()
        {
            var ticketReleaseInfo = new TicketReleaseInfo
                {
                    Id = 12345, SvnRevision = "864353"
                };


            var viewModel = new CycleTimeReleaseViewModelFactory().Build(ticketReleaseInfo);

            Assert.That(viewModel.Name, Is.EqualTo("864353"));
        }

        [Test]
        public void SetsReleaseNameToServiceNowIdWhenNoSvnRevisionPresent()
        {
            var ticketReleaseInfo = new TicketReleaseInfo
                {
                    Id = 12345, 
                    ServiceNowId = "CHG0001234"
                };

            var viewModel = new CycleTimeReleaseViewModelFactory().Build(ticketReleaseInfo);

            Assert.That(viewModel.Name, Is.EqualTo("CHG0001234"));
        }

        [Test]
        public void SetsReleaseNameReleaseIdWhenNoOtherIdentifierPresent()
        {
            var ticketReleaseInfo = new TicketReleaseInfo
                {
                    Id = 12345
                };

            var viewModel = new CycleTimeReleaseViewModelFactory().Build(ticketReleaseInfo);

            Assert.That(viewModel.Name, Is.EqualTo("12345"));
        }
    }
}