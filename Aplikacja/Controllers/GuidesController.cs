using Aplikacja.Models;
using Microsoft.AspNet.Identity;
using Rotativa;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Aplikacja.Controllers
{
    public class GuidesController : Controller
    {
        // GET: Guides

        private ApplicationDbContext _dbContext;//zmienna bazy danych

        public GuidesController()
        {
            _dbContext = new ApplicationDbContext();
        }
        public ActionResult Index()//zwrocenie poradnikow do listy
        {
            var guides = _dbContext.Guides.OrderByDescending(v => v.EnrollmentDate).ToList();//przypisanie poradników do zmiennej

            return View(guides);//zwrocenie widoku z poradnikami
        }

        public ActionResult Show(int id)//zwrocenie pojedyńczego poradnika
        {
            var guide = _dbContext.Guides.SingleOrDefault(v => v.Id == id);//odnalezienie poradnika o wskazanym id

            if (guide == null)//sprawdzenie czy został znaleziony poradnik
                return HttpNotFound();
            
            return View(guide);//zwrocenie widoku z poradnikiem
        }

        public ActionResult Print(int PrintId)//drukowanie poradnika do pdf
        {
            return new ActionAsPdf("Show", new { id = PrintId }) { FileName = "Poradnik.pdf" };//zwrócenie pliku pdf na podstawie id poradnika
        }
        [Authorize]//autoryzacja 
        public ActionResult New()//zwrocenie widoku new poradnika
        {
            return View();
        }

        [Authorize]
        [HttpPost]
        public ActionResult Add(Guide guide, HttpPostedFileBase Filename)//dodawanie poradnika 
        {
            guide.EnrollmentDate = DateTime.Now;//przypisanie daty utworzenia
            _dbContext.Guides.Add(guide);//przyjęcie wszystkich przesłanych danych
            _dbContext.SaveChanges();//zapisanie przesłanych pól
            var guideInDb = _dbContext.Guides.SingleOrDefault(v => v.Id == guide.Id);//wyszukanie poradnika o danym id
            string _FileName = Path.GetFileName(Filename.FileName);// utworzenie zmiennej z nazwa pliku
            guideInDb.Filename = _FileName;//zapisanie w bazie nazwy pliku
            string _path = Path.Combine(Server.MapPath("~/UploadedFiles"), _FileName);//określenie ścieżki zapisu
            Filename.SaveAs(_path);//zapis pliku
            guideInDb.AuthId = User.Identity.GetUserId();//przypisanie id autora do poradnika
            guideInDb.AuthName = User.Identity.Name;//przypisanie nazwy autora do poradnika
            _dbContext.SaveChanges();//zapisanie zmian

            return RedirectToAction("Index");//powrót do strony głównej
        }

        [Authorize]
        public ActionResult Edit(int id)//edycja poradnika
        {
            var guide = _dbContext.Guides.SingleOrDefault(v => v.Id == id);//wyszukanie poradnika

            if (guide == null)//sprawdzenie czy poradnik został znaleziony
                return HttpNotFound();

            if (guide.AuthId != User.Identity.GetUserId())//sprawdzenie czy zalogowany użytkownik jest autorem danego poradnika
            {
                return HttpNotFound();//zwrócenie błędu http
            }
            else
            {
                return View(guide);//zwrócenie widoku poranika
            }
            
        }

        [Authorize]
        [HttpPost]
        public ActionResult Update(Guide guide, HttpPostedFileBase Filename)
        {
            var guideInDb = _dbContext.Guides.SingleOrDefault(v => v.Id == guide.Id);//poradnik z podanym identyfikatorem z bazy
            
            if (guideInDb == null)//sprawdzamy, czy poradnik zostało znalezione, czy nie
                return HttpNotFound();

                string oldFilename = guideInDb.Filename;//zapamietanie starego pliku obrazu
                guideInDb.Title = guide.Title;//przypisanie tytułu poradnika
                guideInDb.Body = guide.Body;//przypisanie treści poradnika
                guideInDb.Category = guide.Category;// przypisanie kategorii poradnika
                if(Filename != null)// sprawdzenie czy plik zosytał przesłany
                {
                    string _FileName = Path.GetFileName(Filename.FileName);// utworzenie zmiennej z nazwa pliku
                    guideInDb.Filename = _FileName;//zapisanie w bazie nazwy pliku
                    string _path = Path.Combine(Server.MapPath("~/UploadedFiles"), _FileName);//określenie ścieżki zapisu
                    string _path_ = Path.Combine(Server.MapPath("~/UploadedFiles"), oldFilename);//określenie ścieżki usuniecia starego pliku
                    Filename.SaveAs(_path);//zapis pliku
                    System.IO.File.Delete(_path_);//usunięcie starego pliku 
            }
                _dbContext.SaveChanges();//zapisanie zmian

                return RedirectToAction("Index");
            
        }

        [Authorize]
        public ActionResult Delete(int id)
        {
            var guide = _dbContext.Guides.SingleOrDefault(v => v.Id == id);
            if (guide == null)
                return HttpNotFound();

            if (guide.AuthId != User.Identity.GetUserId())
            {
                return HttpNotFound();
            }
            else
            {
                return View(guide);
            }
        }

        [Authorize]
        [HttpPost]
        public ActionResult GuideDelete(int id)//usuwanie poradników
        {
            var guide = _dbContext.Guides.SingleOrDefault(v => v.Id == id);
            if (guide == null)
                return HttpNotFound();

            if (guide.AuthId != User.Identity.GetUserId())
            {
                return HttpNotFound();
            }
            else
            {
                string oldFilename = guide.Filename;//zapamietanie starego pliku obrazu
                string _path_ = Path.Combine(Server.MapPath("~/UploadedFiles"), oldFilename);//określenie ścieżki usuniecia starego pliku
                _dbContext.Guides.Remove(guide);//usunięcie poradnika
                _dbContext.SaveChanges();//zapisanie zmian
                System.IO.File.Delete(_path_);//usunięcie pliku obrazu poradnika
                return RedirectToAction("Index");//powrót na strone główną 
            }
        }
    }
}