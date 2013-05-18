using LeanKit.Analytics.Controllers;
using NUnit.Framework;

namespace LeanKit.Analytics.Tests
{
    [TestFixture]
    public class HomeControllerTests
    {
        [Test]
        public void IndexSetsViewToIndex()
        {
            Assert.That(new HomeController().Index().ViewName, Is.EqualTo("Index"));
        }
    }
}
