using FootballManager.Classes;
using FootballManager.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FootballManager.Controllers
{
	public class PersonController : Controller
	{
		public static List<Person> personenListe = new List<Person>();
		/// <summary>
		/// Die Methode gibt uns nur die "IsActive"(Aktive Personen) aus der personeniste
		/// </summary>
		/// <returns></returns>
		public static List<Person> activePersonenListe()
		{
			List<Person> activePersone = new List<Person>();
			foreach (var person in personenListe)
			{
				if (person.IsActive == true)
				{
					activePersone.Add(person);
				}
			}
			return activePersone;
		}
		/// <summary>
		/// Bereinigt die Liste von Personnen die in keinem Team spielen, da der View keine objekte mit Null's annehmen kann...
		/// </summary>
		/// <returns>Liste der Personnen, aber nur der wo Team Field nicht Null ist</returns>
		public static List<Person> PlayerListWithoutNulls()
		{
			List<Person> listeOhne = new List<Person>();
			foreach (var person in personenListe)
			{
				if (person.Team != null)
				{
					listeOhne.Add(person);
				}
			}
			return listeOhne;
		}
		// GET: PersonController
		public ActionResult Index()
		{
			return View(personenListe.Where(x => x.IsActive == true));
		}

		// GET: PersonController/Details/5
		public ActionResult Details(int id)
		{

			var elemForDetails = personenListe.FirstOrDefault(x => x.Id == id);

			return View(elemForDetails);
		}

		// GET: PersonController/Create
		public ActionResult Create()
		{
			return View();
		}

		// POST: PersonController/Create
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Create(Person person) //IFormCollection collection
		{
			if (ModelState.IsValid)
			{
				try
				{
					if (!personenListe.Any(x => x.Email == person.Email))
					{
						person.Id = personenListe.Count + 1;
						//hash
						CryptoHelper.SaltAndEncrypt(person);
						personenListe.Add(person);
					}
					return RedirectToAction(nameof(Index));
				}
				catch
				{
					return View();
				}
			}
			else
			{
				return View();
			}
		}

		// GET: PersonController/Edit/5
		public ActionResult Edit(int id)
		{
			var elemToEdit = personenListe.FirstOrDefault(x => x.Id == id);
			var admin = HttpContext.Request.Cookies["AdminPermission"];
			if (admin == "True")
			{
				return View(elemToEdit);
			}
			else
			{
				return StatusCode((int)System.Net.HttpStatusCode.Forbidden);//403
																			//return StatusCode ((int)System.Net.HttpStatusCode.NotFound);//404
			}
		}

		// POST: PersonController/Edit/5
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Edit(int id, Person person)
		{
			try
			{
				var elemToEdit = personenListe.FirstOrDefault(x => x.Id == id);

				elemToEdit.Firstname = person.Firstname;
				elemToEdit.Lastname = person.Lastname;
				elemToEdit.Birthday = person.Birthday;
				elemToEdit.Email = person.Email;
				elemToEdit.PersonRole = person.PersonRole;
				elemToEdit.Password = person.Password;
				elemToEdit.AdminPermission = person.AdminPermission;

				return RedirectToAction(nameof(Index));
			}
			catch
			{
				return View();
			}
		}
		//// GET: PersonController/EditTeam/
		public ActionResult EditTeam()
		{
			return View();
		}

		// POST: PersonController/EditTeam/
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult EditTeam(Person person)
		{
			try
			{
				Person personZumAendern = new Person();
				personZumAendern = personenListe.FirstOrDefault(x => x.Id == person.Id);

				personZumAendern.Team = FootballManager.Controllers.TeamController.teamListe.
					FirstOrDefault(x => x.Name == person.Team.Name);

				return RedirectToAction(nameof(Index));
			}
			catch
			{
				return View(personenListe);
			}
		}
		/// <summary>
		/// Die Methode zeigt uns wieviele Personen spielen überhaupt / Generell in irgendeinem Team
		/// Das wird im IndexStatisticVM angezeigt unter PersonsPlayInTeams
		/// </summary>
		/// <returns>int Anzahl von Personen</returns>
		public static int PersonsPlayInTeamsMethod()
		{
			int count = 0;
			List<Person> personen = personenListe.Where(x => x.Team != null && x.IsActive == true).ToList();
			foreach (var person in personen)
			{
				count++;
			}
			return count;
		}
		// GET: PersonController/Delete/5
		public ActionResult Delete(int id)
		{
			var elemToDelete = personenListe.FirstOrDefault(x => x.Id == id);
			return View(elemToDelete);
		}

		// POST: PersonController/Delete/5
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Delete(int id, IFormCollection collection)
		{
			try
			{
				var elemToDelete = personenListe.FirstOrDefault(x => x.Id == id);
				elemToDelete.IsActive = false;
				return RedirectToAction(nameof(Index));
			}
			catch
			{
				return View();
			}
		}
		public ActionResult Login()
		{
			return View("_Layout");
		}

		[HttpPost]
		public ActionResult LoginPost(string email, string password)
		{
			var person = personenListe.Where(x => x.Email == email).FirstOrDefault();
			if (person is null)
			{
				Person p = new Person();
				person = p;
			}
			CryptoHelper.SaltAndEncrypt(person);

			if (person != null && person.SaltedPassword != null)
			{
				//1. aus der plaintext passwort eingabe ein array machen
				byte[] plainTextPwdByteArray = Encoding.UTF8.GetBytes(password);

				//2. salt aus db mit dem plainTextPwdByteArray zusammenhaengen

				//byte[] saltedArrayFromDB = Encoding.UTF8.GetBytes(person.SaltForPassword);

				byte[] saltedArrayFromDB = Convert.FromBase64String(person.SaltedPassword);

				byte[] combinedArray = plainTextPwdByteArray.Concat(saltedArrayFromDB).ToArray();

				//3. hashen und vergleichen mit eintrag aus db

				SHA512 shaM = new SHA512Managed();
				byte[] encryptedResultArray = shaM.ComputeHash(combinedArray);

				string encryptedPasswordString = Convert.ToBase64String(encryptedResultArray);

				//if (person.Password == password)
				if (person.PasswordEncrypted == encryptedPasswordString)
				{
					//alles ok
					//TempData["welcomeMsg"] = "<font color=green> hello " + person.Email + "</font>";
					TempData["LogedIn"] = "isLogedIn";
					TempData["Picture"] = person.ImagePath.ToString();
					HttpContext.Response.Cookies.Append("email", person.Email);
					HttpContext.Response.Cookies.Append("AdminPermission", person.AdminPermission.ToString());
					person.isLogedIn = true;

				}
				else
				{
					TempData["errorMsg"] = "<font color=red> Password don't match! &nbsp; Please try again...</font>";
					TempData["AgainModal"] = "clicker";
				}
			}
			else
			{
				TempData["errorMsg"] = "<font color=red> Unkown email adress: " + email + "</font>";
				TempData["AgainModal"] = "clicker";
			}

			return Redirect("http://localhost:37094/Home/Index");
		}
		[HttpPost]
		public ActionResult LogOut(string email, string password)
		{
			var person = new Person();
			person = personenListe.FirstOrDefault(x => x.Email == email);
			if (person is null)
			{
				Person p = new Person();
				person = p;
			}
			var logedPerson = personenListe.FirstOrDefault(x => x.isLogedIn == true);

			if (logedPerson.Email == person.Email && logedPerson.Password == password)
			{
				HttpContext.Response.Cookies.Delete("email");
				HttpContext.Response.Cookies.Delete("AdminPermission");
			}
			else
			{
				
				//if (person.Email != personLogedIn.Email && person.Password != personLogedIn.Password)
				//{
				//	TempData["errorMsgLogOut"] = $"<font color=red> Email: {email} and Password {password} do not match </font>";
				//	return Redirect("http://localhost:37094/Home/Index");
				//}
				if (logedPerson.Password != password && logedPerson.Email != email)
				{
					TempData["LogedIn"] = "isLogedIn";
					TempData["AgainLogOutModal"] = "LogOutAgain";
					TempData["Picture"] = logedPerson.ImagePath.ToString();
					TempData["errorMsgLogOut1"] = "<font color=red> Unkown Email Adress: " + email + "</font>";
					TempData["errorMsgLogOut2"] = "<font color=red> Unkown Password: " + password + "</font>";

					return Redirect("http://localhost:37094/Home/Index");

				}
				if (logedPerson.Password != password)
				{
					TempData["LogedIn"] = "isLogedIn";
					TempData["AgainLogOutModal"] = "LogOutAgain";
					TempData["Picture"] = logedPerson.ImagePath.ToString();
					TempData["errorMsgLogOut2"] = "<font color=red> Unkown Password: " + password + "</font>";
					return Redirect("http://localhost:37094/Home/Index");

				}
				if (logedPerson.Email != email)
				{
					TempData["LogedIn"] = "isLogedIn";
					TempData["AgainLogOutModal"] = "LogOutAgain";
					TempData["Picture"] = logedPerson.ImagePath.ToString();
					TempData["errorMsgLogOut1"] = "<font color=red> Unkown Email Adress: " + email + "</font>";
					return Redirect("http://localhost:37094/Home/Index");

				}
			}
			return Redirect("http://localhost:37094/Home/Index");

		}
	}
}
