using LeanKit.Analytics.Controllers;
using LeanKit.Analytics.Models.ViewModels;
using NUnit.Framework;

namespace LeanKit.Analytics.Tests.Controllers
{
    [TestFixture]
    public class HomeControllerTests : IHomeViewModelFactory
    {
        private HomeViewModel _expectedModel;

        [Test]
        public void IndexSetsViewToIndex()
        {
            Assert.That(new HomeController(this).Index().ViewName, Is.EqualTo("Index"));
        }

        [Test]
        public void IndexSetsViewModel()
        {
            _expectedModel = new HomeViewModel();

            Assert.That(new HomeController(this).Index().Model, Is.EqualTo(_expectedModel));
        }

        public HomeViewModel Build()
        {
            return _expectedModel;
        }
    }
}
