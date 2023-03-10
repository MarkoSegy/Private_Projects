using FootballManager.Models;
using FootballManager.Models.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FootballManager.Controllers
{
    public class TeamController : Controller
    {
        public static List<Team> teamListe = new List<Team>();
        public static List<TeamMondayVM> teamMondayListe = new List<TeamMondayVM>();

        /// <summary>
        /// Die Methode gibt uns nur die "IsActive"(Aktive Teams) aus der temListe
        /// </summary>
        /// <returns></returns>
        public static List<Team> activeTeamListe()
        {
            List<Team> activeTeams = new List<Team>();
            foreach (var team in teamListe)
            {
                if (team.IsActive == true)
                {
                    activeTeams.Add(team);
                }
            }
            return activeTeams;
        }
        // GET: TeamController
        public ActionResult Index()
        {
            return View(teamListe.Where(x => x.IsActive == true));
        }
        
        public IActionResult IndexMondayVM()
        {
            teamMondayListe.Clear();
            foreach (var team in teamListe)
            {
                TeamMondayVM vm = new TeamMondayVM();
                vm.team = team;

                if (DateTime.Now.DayOfWeek.ToString() == "Tuesday")
                {
                    vm.isMonday = true;
                }

                teamMondayListe.Add(vm);
            }
            return View(teamMondayListe);
        }

        // GET: TeamController/Details/5
        public ActionResult Details(int id)
        {

            var elemForDetails = teamListe.FirstOrDefault(x => x.Id == id);
            //bool isMonday = false;

            //if (DateTime.Now.DayOfWeek.ToString() == "Monday")
            //{
            //    isMonday = true;
            //}
            //ViewBag.isMonday = isMonday;
            return View(elemForDetails);
        }
        // GET: TeamController/Details/5
        public ActionResult DetailsTeamMonday(int id)
        {

            var elemForDetails = teamMondayListe.FirstOrDefault(x => x.team.Id == id);

            return View(elemForDetails);
        }

        // GET: TeamController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: TeamController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Team team)
        {
            try
            {
                teamListe.Add(new Team()
                {
                    Id = FootballManager.Controllers.TeamController.teamListe.Count + 1,
                    Name = team.Name,
                    ImagePath = team.ImagePath
                });
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: TeamController/Edit/5
        public ActionResult Edit(int id)
        {
            var elemToEdit = teamListe.FirstOrDefault(x => x.Id == id);
            return View(elemToEdit);
        }

        // POST: TeamController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Team team)
        {
            try
            {
                var elemToEdit = teamListe.FirstOrDefault(x => x.Id == id);
                elemToEdit.Name = team.Name;
                elemToEdit.ImagePath = team.ImagePath;

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: TeamController/Delete/5
        public ActionResult Delete(int id)
        {
            var elemtodelete = teamListe.FirstOrDefault(x => x.Id == id);
            return View(elemtodelete);
        }

        // POST: TeamController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, Team team)
        {
            try
            {
                var elemToDelete = teamListe.FirstOrDefault(x => x.Id == id);
                elemToDelete.IsActive = false;
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
