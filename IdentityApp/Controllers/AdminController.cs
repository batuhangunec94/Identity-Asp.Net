using IdentityApp.Context;
using IdentityApp.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IdentityApp.Controllers
{
    public class AdminController : Controller
    {
        private UserManager<AppUser> userManager;
        public AdminController()
        {
            userManager = new UserManager<AppUser>(new UserStore<AppUser>(new ProjectContext()));
        }

        // GET: Admin
        public ActionResult Index()
        {
            return View(userManager.Users);
        }
    }
}