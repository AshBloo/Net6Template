using AdminLTE.Common;
using AdminLTE.Common.Attributes;
using AdminLTE.Common.Extensions;
using AdminLTE.Data;
using AdminLTE.Models;
using AdminLTE.Models.ManageViewModels;
using AdminLTE.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;

namespace AdminLTE.Controllers
{
  

    public class HomeController : BaseController
    {
        private readonly ApplicationDbContext _context;
        private readonly IEmailSender _emailSender;
        public HomeController(ApplicationDbContext context, IEmailSender emailSender)
        {
            _context = context;
            _emailSender = emailSender;
        }

        [HelpDefinition]
        public IActionResult Index()
        {
            // AddPageHeader("Dashboard", "");
            // AddBreadcrumb("Dashboard", "/Home/");
            // AddBreadcrumb("Contact", "/Account/Contact");

            // ViewData["Title"] = @"Applicant Tracking System";
            // ViewData["PageTitle"] = @"Dashboard";
            // ViewData["PageSubTitle"] = @"Current Recruitment stats";
            // ViewData["Icon"] = "bi bi-grid-fill";

            //AddPageAlerts(PageAlertType.Info, "you may view the summary <a href='#'>here</a>");

            //AddToastAlerts(PageAlertType.Error, "Test Title", "Test Message");
            //AddToastAlerts(PageAlertType.Info, "Test Title", "Test Message");

            return View();
        }

        [HttpPost]
        public IActionResult Index(object model)
        {
            AddPageAlerts(PageAlertType.Info, "you may view the summary <a href='#'>here</a>");
            AddBreadcrumb("Register", "/Account/Register");
            AddBreadcrumb("Contact", "/Account/Contact");
            return View("Index");
        }

        [HelpDefinition]
        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";
            AddBreadcrumb("About", "/Account/About");

            return View();
        }

        [HelpDefinition("helpdefault")]
        public IActionResult Contact()
        {
            AddBreadcrumb("Register", "/Account/Register");
            AddBreadcrumb("Contact", "/Account/Contact");
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult CreateEmail(int id, string subject, string body, IFormFileCollection attachments)
        //{
        //    var candidate = _context.Candidate.FindAsync(id);
        //    var currentapplication = _context.Application.Where(a => a.CandidateId == id).OrderByDescending(c => c.ApplicationId).Take(1);

        //    var emailadd = candidate.Result.Email;

        //    var att = new Services.Attachment[attachments.Count];

        //    string usrname = User.GetUserProperty(CustomClaimTypes.GivenName) + " " + User.GetUserProperty(CustomClaimTypes.Surname);

        //    _emailSender.SendIformEmailAsync("Human Resources", "Recruitment@localsolutions.org.uk", candidate.Result.Forename.ToString(),
        //     emailadd.ToString(), subject,
        //      $"Dear " + candidate.Result.Forename + "," + "<br />" + body + "<br /> Local Solutions Recruitment Team" + "<br />" + usrname, attachments);

        //    return RedirectToAction("Details", "Applications", new { id = currentapplication.SingleOrDefault().ApplicationId, msg = "Email Sent" });

        //}
    }
}
