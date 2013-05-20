using System.Web.Mvc;
using LeanKit.Analytics.Models.Factories;
using LeanKit.Analytics.Models.ViewModels;

namespace LeanKit.Analytics.Controllers
{
    public class HomeController : Controller
    {
        private readonly IHomeViewModelFactory _homeViewModelFactory;

        public HomeController()
        {
            _homeViewModelFactory = new HomeViewModelFactory();
        }

        public HomeController(IHomeViewModelFactory homeViewModelFactory)
        {
            _homeViewModelFactory = homeViewModelFactory;
        }

        public ViewResult Index()
        {
            return View("Index", _homeViewModelFactory.Build());
        }
    }

    public interface IHomeViewModelFactory
    {
        HomeViewModel Build();
    }
}