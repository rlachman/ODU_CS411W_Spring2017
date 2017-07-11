using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CPP2.Services;
using CPP2.Models;
using Microsoft.AspNet.Identity;

namespace CPP2.Controllers
{
    public class ViolationControllerTest : Controller
    {
        // GET: Violation
        public ActionResult Index()
        {
            var vm = new ViolationViewModel();
            var db = new CPPdatabaseEntities();
            var thisCppUserId = CppUserService.GetCppUserId(User.Identity.GetUserId(), db);
            var sessions =
            db.SessionLogs.Where(session => session.CallReceiverId == thisCppUserId)
            .Select(session => session.CallSenderId)
            .ToList();
            sessions.AddRange(db.SessionLogs.Where(session => session.CallSenderId == thisCppUserId).Select(session => session.CallReceiverId));

            foreach (var partnerId in sessions)
            {
                //vm.List.Add(db.CppUsers.Where(u => u.Id == partnerId).Select(u => u.AspNetUser).Single());
                vm.List.Add(db.CppUsers.Where(u => u.Id == partnerId).Select(x=> new AspNetUserDTO()
                {
                    Email=x.AspNetUser.Email,
                    Id=x.AspNetUser.Id,
                    UserName=x.AspNetUser.UserName
                }).Single());
            }

            return View(vm);
        }

        public void createViolation()
        {
            var ReportedId = Request["ReportedId"];
        }
    }
}