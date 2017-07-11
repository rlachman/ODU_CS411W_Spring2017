using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CPP2;
using Microsoft.AspNet.Identity;


namespace CPP2.Controllers
{
	public class ViolationsController : Controller
	{
		private CPPdatabaseEntities db = new CPPdatabaseEntities();


		// GET: Violations
		public ActionResult Index()
		{
			var violations = db.Violations.Include(v => v.CppUser).Include(v => v.CppUser1);
			return View(violations.ToList());
		}

		// GET: Violations/Details/5
		public ActionResult Details(int? id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			Violation violation = db.Violations.Find(id);
			if (violation == null)
			{
				return HttpNotFound();
			}
			return View(violation);
		}

		// GET: Violations/Create
		public ActionResult Create()
		{
			ViewBag.ReportedId = new SelectList(db.CppUsers, "Id", "Id");
			var EmailIdStringDictionary = new Dictionary<int, string>();
			ViewBag.EmailStringDict = new Dictionary<int, string>();

			var cppUsers = db.CppUsers.Select(u => u.Id);

			foreach (var id in cppUsers)
			{
				//get email
				EmailIdStringDictionary.Add(id, Services.CppUserService.GetCppUserEmail2((int)id, db));
			}
			var violationDictFromDb = new Dictionary<int, string>();
			var violationTypesFromDb = db.ViolationTypes.ToList();
			foreach (var type in violationTypesFromDb)
			{
				violationDictFromDb.Add(type.Id, type.Description);
			}
			//foreach record, create new entry in dict and pass this dict in instead of hard coded dict


			ViewBag.ViolationDict = violationDictFromDb;//CPP2.Services.ViolationService.GetViolationDictionary();

			ViewBag.EmailStringDict = EmailIdStringDictionary;
			return View();
		}

		// POST: Violations/Create
		// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
		// more details see https://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Create([Bind(Include = "Id,ReporterId,ReportedId,ViolationId")] Violation violation)
		{
			if (ModelState.IsValid)
			{
				violation.ReporterId = CPP2.Services.CppUserService.GetCppUserId(User.Identity.GetUserId(), db);
				CPP2.Services.MessageService.SendViolationEmail(violation.ViolationId, violation.ReportedId, CPP2.Services.CppUserService.GetCppUserEmail2(violation.ReporterId, db));
				db.Violations.Add(violation);
				db.SaveChanges();
				return RedirectToAction("Contact", "Home");
			}

			ViewBag.ReportedId = new SelectList(db.CppUsers, "Id", "Id", violation.ReportedId);
			return View(violation);
		}

		// GET: Violations/Edit/5
		public ActionResult Edit(int? id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			Violation violation = db.Violations.Find(id);
			if (violation == null)
			{
				return HttpNotFound();
			}
			ViewBag.ReportedId = new SelectList(db.CppUsers, "Id", "AspNetUserId", violation.ReportedId);
			ViewBag.ReportedId = new SelectList(db.CppUsers, "Id", "AspNetUserId", violation.ReportedId);
			return View(violation);
		}

		// POST: Violations/Edit/5
		// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
		// more details see https://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Edit([Bind(Include = "Id,ReporterId,ReportedId,ViolationId")] Violation violation)
		{
			if (ModelState.IsValid)
			{
				db.Entry(violation).State = EntityState.Modified;
				db.SaveChanges();
				return RedirectToAction("Index");
			}
			ViewBag.ReportedId = new SelectList(db.CppUsers, "Id", "AspNetUserId", violation.ReportedId);
			ViewBag.ReportedId = new SelectList(db.CppUsers, "Id", "AspNetUserId", violation.ReportedId);
			return View(violation);
		}

		// GET: Violations/Delete/5
		public ActionResult Delete(int? id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			Violation violation = db.Violations.Find(id);
			if (violation == null)
			{
				return HttpNotFound();
			}
			return View(violation);
		}

		// POST: Violations/Delete/5
		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public ActionResult DeleteConfirmed(int id)
		{
			Violation violation = db.Violations.Find(id);
			db.Violations.Remove(violation);
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
