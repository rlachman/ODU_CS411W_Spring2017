using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CPP2;

namespace CPP2.Controllers
{
    public class ViolationTypesController : Controller
    {
        private CPPdatabaseEntities db = new CPPdatabaseEntities();

        // GET: ViolationTypes
        public ActionResult Index()
        {
            return View(db.ViolationTypes.ToList());
        }

        // GET: ViolationTypes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ViolationType violationType = db.ViolationTypes.Find(id);
            if (violationType == null)
            {
                return HttpNotFound();
            }
            return View(violationType);
        }

        // GET: ViolationTypes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ViolationTypes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Description")] ViolationType violationType)
        {
            if (ModelState.IsValid)
            {
                db.ViolationTypes.Add(violationType);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(violationType);
        }

        // GET: ViolationTypes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ViolationType violationType = db.ViolationTypes.Find(id);
            if (violationType == null)
            {
                return HttpNotFound();
            }
            return View(violationType);
        }

        // POST: ViolationTypes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Description")] ViolationType violationType)
        {
            if (ModelState.IsValid)
            {
                db.Entry(violationType).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(violationType);
        }

        // GET: ViolationTypes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ViolationType violationType = db.ViolationTypes.Find(id);
            if (violationType == null)
            {
                return HttpNotFound();
            }
            return View(violationType);
        }

        // POST: ViolationTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ViolationType violationType = db.ViolationTypes.Find(id);
            db.ViolationTypes.Remove(violationType);
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
