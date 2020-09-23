using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using BO;
using samsam.Data;
using samsam.Models;

namespace samsam.Controllers
{
    public class SamouraisController : Controller
    {
        private samsamContext db = new samsamContext();

        // GET: Samourais
        public ActionResult Index()
        {
            return View(db.Samourais.ToList());
        }

        // GET: Samourais/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Samourai samourai = db.Samourais.Find(id);
            if (samourai == null)
            {
                return HttpNotFound();
            }
            return View(samourai);
        }

        // GET: Samourais/Create
        public ActionResult Create()
        {
            
            SamouraiVM samouraiVM = new SamouraiVM
            {
                Samourai = new Samourai(),
                Armes = db.Armes.Where(x => !db.Samourais.Select(y => y.Arme.Id).ToList().Contains(x.Id)).ToList(),
            };


            
            return View(samouraiVM);
        }

        // POST: Samourais/Create
        // Afin de déjouer les attaques par sur-validation, activez les propriétés spécifiques que vous voulez lier. Pour 
        // plus de détails, voir  https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(SamouraiVM samouraiVM)
        {

            samouraiVM.Samourai.Arme = db.Armes.FirstOrDefault(x => x.Id == samouraiVM.IdArme);


            if (ModelState.IsValid)
            {
                db.Samourais.Add(samouraiVM.Samourai);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            samouraiVM.Armes = db.Armes.Select(x => x).ToList();
            return View(samouraiVM);
        }

        // GET: Samourais/Edit/5
        public ActionResult Edit(int? id)
        {
            

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Samourai samourai = db.Samourais.Find(id);
            if (samourai == null)
            {
                return HttpNotFound();
            }

            SamouraiVM samouraiVM = new SamouraiVM()
            {
                Samourai = samourai,
                Armes = db.Armes.Select(x => x).ToList(),
            };
            if(samourai.Arme != null)
            {
                samouraiVM.IdArme = samourai.Arme.Id;
            }
            return View(samouraiVM);
        }

        // POST: Samourais/Edit/5
        // Afin de déjouer les attaques par sur-validation, activez les propriétés spécifiques que vous voulez lier. Pour 
        // plus de détails, voir  https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit( SamouraiVM samouraiVM)
        {

            if (ModelState.IsValid)
            {
                Samourai samourai = db.Samourais.Find(samouraiVM.Samourai.Id);
                Arme arme = samourai.Arme;
                arme = null;
                samourai.Arme = arme;
                samourai.Arme = db.Armes.FirstOrDefault(x => x.Id == samouraiVM.IdArme);
                db.Entry(samourai).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            samouraiVM.Armes = db.Armes.Select(x => x).ToList();
            return View(samouraiVM);
        }

        // GET: Samourais/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Samourai samourai = db.Samourais.Find(id);
            if (samourai == null)
            {
                return HttpNotFound();
            }
            return View(samourai);
        }

        // POST: Samourais/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Samourai samourai = db.Samourais.Find(id);
            db.Samourais.Remove(samourai);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
