using FootballManager.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace FootballManager.Controllers
{
    public class GameController : Controller
    {
        public static List<Game> gameListe = new List<Game>();
        /// <summary>
        /// Die Methode gibt uns nur die "IsActive"(Aktive Games) aus der gameiste
        /// </summary>
        /// <returns></returns>
        public static List<Game> activeGameListe()
        {
            List<Game> activeGames = new List<Game>();
            foreach (var game in gameListe)
            {
                if (game.IsActive == true)
                {
                    activeGames.Add(game);
                }
            }
            return activeGames;
        }
        // GET: GameController
        public ActionResult Index()
        {
            return View(gameListe.Where(x => x.IsActive == true));
        }

        // GET: GameController/Details/5
        public ActionResult Details(int id)
        {
            var elemToDesplay = gameListe.FirstOrDefault(x => x.Id == id);
            return View(elemToDesplay);
        }

        // GET: GameController/Create
        public ActionResult Create()
        {
            //ViewBag.teamList = TeamController.teamListe;
            return View();
        }

        // POST: GameController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Game game)
        {
            try
            {
                Game temp = new Game();
                temp.Id = gameListe.Count + 1;

                // Hier unten kommt nur game.Team1/2.Name vom Parameter rein in die Liste und nimmt den ganzen Objekt aus der liste raus und speichert im temp.Team1/2...
                temp.Team1 = FootballManager.Controllers.TeamController.teamListe.Where(x => x.Name == game.Team1.Name).FirstOrDefault();
                temp.Team2 = FootballManager.Controllers.TeamController.teamListe.Where(x => x.Name == game.Team2.Name).FirstOrDefault();

                temp.GameDate = game.GameDate;
                temp.GoalsTeam1 = game.GoalsTeam1;
                temp.GoalsTeam2 = game.GoalsTeam2;

                var team1 = FootballManager.Controllers.TeamController.
                    teamListe.FirstOrDefault(x => x.Name == game.Team1.Name);
                game.Team1.ImagePath = team1.ImagePath;

                var team2 = FootballManager.Controllers.TeamController.
                    teamListe.FirstOrDefault(x => x.Name == game.Team2.Name);
                game.Team2.ImagePath = team2.ImagePath;

                if (game.Team1.Name != game.Team2.Name)
                {
                    gameListe.Add(temp);
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    TempData["errorImpossible"] = "<font color=red> It is impossible for the same team to play against itself! PLEASE TRY AGAIN!!!</font>";
                    return RedirectToAction("Create", "Game", new { area = "CreateAgain" });
                }

            }
            catch
            {
                return View(gameListe);
            }
        }// GET: GameController/CreateRandom
        public ActionResult CreateRandom()
        {
            Game game = RandomTeams();
            //ViewBag.Team1Foto = game.Team1.ImagePath.ToString();
            ViewBag.GameForView = game;
            return View();
        }

        // POST: GameController/CreateRandom
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult CreateRandomPost(Game game)
        //{
        //    try
        //    {
        //        if (game.Team1 == null && game.Team2 == null)
        //        {
        //        game = RandomTeams();
        //            //ViewData["Team1"] = game;
        //            ViewBag.Team1Foto = game.Team1.ImagePath.ToString();
        //            //ViewData["Team2Name"] = game.Team2.Name;
        //            //ViewData["Team2Foto"] = game.Team2.ImagePath;
        //            gameListe.Add(game);
        //            //  ViewData["GameData"] = game;
        //            return RedirectToAction(nameof(CreateRandom));
        //        }
        //        else 
        //            return RedirectToAction(nameof(Index));
        //    }
        //    catch
        //    {
        //        return View(gameListe);
        //    }
        //}
        public Game RandomTeams()
        {
            Game randomGame = new Game();
            Team team1;
            Team team2;
                
            do
            {
                Random rnd1 = new Random();
                var randomNumber1 = rnd1.Next(0, FootballManager.Controllers.TeamController.
                    teamListe.Count());

                team1 = FootballManager.Controllers.TeamController.teamListe[randomNumber1];

                Random rnd2 = new Random();
                var randomNumber2 = rnd2.Next(0, FootballManager.Controllers.TeamController.
                    teamListe.Count());
                team2 = FootballManager.Controllers.TeamController.teamListe[randomNumber2];

            } while (team1 == team2);
            randomGame.Team1 = team1;
            randomGame.Team2 = team2;
            return randomGame;
        }
        // POST: GameController/Create
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Create(Game game)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            if (!gameListe.Any(x => x.Id == game.Id))
        //            {
        //                game.Id = gameListe.Count + 1;

        //                game.Team1 = TeamController.teamListe.Where(x => x.Id == game.Team1.Id).FirstOrDefault();
        //                game.Team2 = TeamController.teamListe.Where(x => x.Id == game.Team2.Id).FirstOrDefault();

        //                gameListe.Add(game);
        //            }
        //            return RedirectToAction(nameof(Index));
        //        }
        //        catch
        //        {
        //            return View();
        //        }
        //    }
        //    else
        //    {
        //        return View();
        //    }
        //}

        // GET: GameController/Edit/5
        public ActionResult Edit(int id)
        {
            var elemToEdit = gameListe.FirstOrDefault(x => x.Id == id);

            return View(elemToEdit);
        }

        // POST: GameController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Game game)
        {
            try
            {
                var gameZumAendern = gameListe.FirstOrDefault(x => x.Id == game.Id);
                gameZumAendern.GameDate = game.GameDate;
                gameZumAendern.Team1 = game.Team1;
                gameZumAendern.Team2 = game.Team2;
                gameZumAendern.GoalsTeam1 = game.GoalsTeam1;
                gameZumAendern.GoalsTeam2 = game.GoalsTeam2;

                var team1 = FootballManager.Controllers.TeamController.
                    teamListe.FirstOrDefault(x => x.Name == game.Team1.Name);
                gameZumAendern.Team1.ImagePath = team1.ImagePath;

                var team2 = FootballManager.Controllers.TeamController.
                    teamListe.FirstOrDefault(x => x.Name == game.Team2.Name);
                gameZumAendern.Team2.ImagePath = team2.ImagePath;

                if (gameZumAendern.Team1.Name != gameZumAendern.Team2.Name)
                {
                    return RedirectToAction(nameof(Index));

                }
                else
                {
                    TempData["errorImpossible"] = "<font color=red> It is impossible for the same team to play against itself! PLEASE TRY AGAIN!!!</font>";
                    return RedirectToAction("Edit", "Game", new { area = "EditAgain" });
                }

            }
            catch
            {
                return View();
            }
        }

        // GET: GameController/Delete/5
        public ActionResult Delete(int id)
        {
            var elemToDelete = gameListe.FirstOrDefault(x => x.Id == id);
            return View(elemToDelete);
        }

        // POST: GameController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                var elemToDelete = gameListe.FirstOrDefault(x => x.Id == id);
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
