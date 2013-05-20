using System.Collections.Generic;
using LeanKit.Analytics.Controllers;
using LeanKit.Analytics.Models.ViewModels;

namespace LeanKit.Analytics.Models.Factories
{
    public class HomeViewModelFactory : IHomeViewModelFactory
    {
        public HomeViewModel Build()
        {
            return new HomeViewModel
                {
                    MainWasteGraph = new WasteGraph(new List<WasteGraphActivity>
                        {
                            new WasteGraphActivity { Activity = "Developing", Percent = 30 },
                            new WasteGraphActivity { Activity = "Testing", Percent = 15 },
                            new WasteGraphActivity { Activity = "Waiting to Test", Percent = 5, IsWaste = true },
                            new WasteGraphActivity { Activity = "Waiting to Release", Percent = 15, IsWaste = true },
                            new WasteGraphActivity { Activity = "Blocked", Percent = 35, IsWaste = true }
                        })
                };
        }
    }
}