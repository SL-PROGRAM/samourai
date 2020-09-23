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
            List<Samourai> samourais = db.Samourais.ToList();
            foreach(Samourai samourai in samourais)
            {
                samourai.Potentiel = (samourai.Force + samourai.Arme.Degats) * (samourai.ArtMartials.Count() + 1);
            }
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
                Armes = GetArmesDisponible(),
                ArtMartials = GetArtMartialsDisponible(),

        };



            return View(samouraiVM);
        }

        private List<Arme> GetArmesDisponible()
        {
            return db.Armes.Where(x => !db.Samourais.Select(y => y.Arme.Id).ToList().Contains(x.Id)).ToList();
        }

        // POST: Samourais/Create
        // Afin de déjouer les attaques par sur-validation, activez les propriétés spécifiques que vous voulez lier. Pour 
        // plus de détails, voir  https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(SamouraiVM samouraiVM)
        {
            samouraiVM = UpdateSamouraiVM(samouraiVM);

            if (ModelState.IsValid)
            {
                db.Samourais.Add(samouraiVM.Samourai);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            samouraiVM = GetPropositionElementDisponible(samouraiVM);

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
                Armes = GetArmesDisponible(),
                ArtMartials = GetArtMartialsDisponible()

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
                samouraiVM.Samourai = samourai;

                samouraiVM = UpdateSamouraiVM(samouraiVM);
                samourai = samouraiVM.Samourai;
                
                db.Entry(samourai).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            samouraiVM = GetPropositionElementDisponible(samouraiVM);
            return View(samouraiVM);
        }

        private List<ArtMartial> GetArtMartialsDisponible()
        {
            return db.ArtMartials.ToList();
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

        private SamouraiVM GetPropositionElementDisponible(SamouraiVM samouraiVM)
        {
            samouraiVM.Armes = GetArmesDisponible();
            samouraiVM.ArtMartials = GetArtMartialsDisponible();
            return samouraiVM;
        }

        private SamouraiVM UpdateSamouraiVM(SamouraiVM samouraiVM)
        {
            samouraiVM.Samourai.Arme = GetArmeSamourai(samouraiVM);
            samouraiVM.Samourai.ArtMartials = GetListArtMartials(samouraiVM);

            return samouraiVM;
        }

        private List<ArtMartial> GetListArtMartials(SamouraiVM samouraiVM)
        {
            return db.ArtMartials.Where(x => samouraiVM.IdArtMartials.Contains(x.Id)).ToList();
        }

        private Arme GetArmeSamourai(SamouraiVM samouraiVM)
        {
            return db.Armes.FirstOrDefault(x => x.Id == samouraiVM.IdArme);
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
