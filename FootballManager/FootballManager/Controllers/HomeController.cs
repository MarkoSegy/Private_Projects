using FootballManager.Models;
using FootballManager.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace FootballManager.Controllers
{
    public class HomeController : Controller
    {
        public static List<StatistikVM> statisticList = new List<StatistikVM>();

        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }
        public IActionResult IndexStatisticVM()
        {
            statisticList.Clear();

            StatistikVM vm = new StatistikVM();
            vm.TeamAnzahl = FootballManager.Controllers.TeamController.activeTeamListe().Count;
            vm.GamesAnzahl = FootballManager.Controllers.GameController.activeGameListe().Count;
            vm.PersonAnzahl = FootballManager.Controllers.PersonController.activePersonenListe().Count;
            vm.PersonsPlayInTeams = FootballManager.Controllers.PersonController.PersonsPlayInTeamsMethod();
            statisticList.Add(vm);
            return View(statisticList);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
