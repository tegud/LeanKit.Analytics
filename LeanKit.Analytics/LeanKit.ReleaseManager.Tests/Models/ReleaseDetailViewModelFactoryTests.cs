using System;
using System.Collections.Generic;
using LeanKit.Data;
using LeanKit.Data.SQL;
using LeanKit.ReleaseManager.Models.Releases;
using NUnit.Framework;

namespace LeanKit.ReleaseManager.Tests.Models
{
    [TestFixture]
    public class ReleaseDetailViewModelFactoryTests : IGetReleasesFromTheDatabase
    {
        private ReleaseRecord _dbRecord;

        [Test]
        public void SetsId()
        {
            _dbRecord = new ReleaseRecord { Id = 12345 };

            var releaseRepository = this;
            Assert.That(new ReleaseDetailViewModelFactory(releaseRepository).Build(0).Id, Is.EqualTo(12345));
        }

        [Test]
        public void SetsSvnRevision()
        {
            _dbRecord = new ReleaseRecord { SvnRevision = "812343" };

            var releaseRepository = this;
            Assert.That(new ReleaseDetailViewModelFactory(releaseRepository).Build(0).SvnRevision, Is.EqualTo("812343"));
        }

        public IEnumerable<ReleaseRecord> GetUpcomingReleases()
        {
            throw new NotImplementedException();
        }

        public int Create(ReleaseRecord newRelease)
        {
            throw new NotImplementedException();
        }

        public void Update(ReleaseRecord releaseRecord)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<ReleaseRecord> GetAllReleases(CycleTimeQuery query)
        {
            throw new NotImplementedException();
        }

        public ReleaseRecord GetRelease(int id)
        {
            return _dbRecord;
        }

        public void SetStartedDate(int id, DateTime started)
        {
            throw new NotImplementedException();
        }

        public void SetCompletedDate(int id, DateTime completed)
        {
            throw new NotImplementedException();
        }

        public int GetReleaseIdForSvnRevision(string svnRevision)
        {
            throw new NotImplementedException();
        }
    }
}
