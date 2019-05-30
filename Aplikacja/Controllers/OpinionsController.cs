using Aplikacja.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Aplikacja.Controllers
{
    public class OpinionsController : Controller
    {
        private ApplicationDbContext _dbContext;//zmienna bazy

        public OpinionsController()
        {
            _dbContext = new ApplicationDbContext();
        }

        // GET: Opinions
        public ActionResult Show()//pokazanie opinii
        {
            var opinions = _dbContext.Opinions.OrderByDescending(v => v.EnrollmentDate).ToList();//przypisanie do zmiennej wszystkich opinii od najnowszej

            return View(opinions) ;//zwrócenie widoku z opiniami
        }

        [Authorize]//autoryzacja
        public ActionResult New()//zwrocenie widoku new opinii
        {
            return View();
        }

        [Authorize]
        [HttpPost]//sprawdzenie żądania http
        public ActionResult Add(Opinion opinion)//dodawanie opinii
        {
            opinion.EnrollmentDate = DateTime.Now;//przypisanie daty utworzenia opinii
            _dbContext.Opinions.Add(opinion);//przypisanie przesłanej opinii
            _dbContext.SaveChanges();//zapisanie zmian
            var opinionInDb = _dbContext.Opinions.SingleOrDefault(v => v.Id == opinion.Id);//wyszukanie opinii o danym id
            opinionInDb.AuthId = User.Identity.GetUserId();//przypisanie autora
            opinionInDb.AuthName = User.Identity.Name;//przypisanie nazwy autora
            _dbContext.SaveChanges();//zapisanie zmian

            return RedirectToAction("Show");//zwrocenie widoku z opiniami
        }

        [Authorize]
        public ActionResult Delete(int id)//usuwanie opinii
        {
            var opinion = _dbContext.Opinions.SingleOrDefault(v => v.Id == id);//wyszukanie danej opinii
            if (opinion == null)//sprawdzenie czy opinia została znaleziona
                return HttpNotFound();

            if (opinion.AuthId != User.Identity.GetUserId())//sprawdzenie czy zalogowany uzytkownik jest autorem
            {
                return HttpNotFound();
            }
            else
            {
                return View(opinion);//zwrócenie widoku usuń
            }
        }

        [Authorize]
        [HttpPost]
        public ActionResult OpinionDelete(int id)//usuwanie opinii
        {
            var opinion = _dbContext.Opinions.SingleOrDefault(v => v.Id == id);//wyszukanie opinii
            if (opinion == null)
                return HttpNotFound();

            if (opinion.AuthId != User.Identity.GetUserId())
            {
                return HttpNotFound();
            }
            else
            {
                _dbContext.Opinions.Remove(opinion);//usunięcie opinii
                _dbContext.SaveChanges();//zapisanie zmian
                return RedirectToAction("Show");
            }
        }
    }
}