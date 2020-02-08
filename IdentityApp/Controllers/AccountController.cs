using IdentityApp.Context;
using IdentityApp.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IdentityApp.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private UserManager<AppUser> userManager;
        public AccountController()
        {
            userManager = new UserManager<AppUser>(new UserStore<AppUser>(new ProjectContext()));

            userManager.PasswordValidator = new PasswordValidator()
            {
                RequireDigit = true,
                RequiredLength = 7,
                RequireLowercase = true,
                RequireUppercase = true,
                RequireNonLetterOrDigit = true
            };
            userManager.UserValidator = new UserValidator<AppUser>(userManager)
            {
                RequireUniqueEmail = true,
                AllowOnlyAlphanumericUserNames = false
            };
        }
        // GET: Account
        
        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                return View("Error",new string[] { "ERİŞİM YETKİNİZ YOK"});
            }
            ViewBag.returnUrl = returnUrl;
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public ActionResult Login(LoginModel loginModel,string returnUrl )
        {
            if (ModelState.IsValid)
            {
            var user = userManager.Find(loginModel.UserName, loginModel.Password);
            if (user == null)
            {
                ModelState.AddModelError("","Yanlış kullanıcı adı yada parola");
            }
            else
            {
                var authManager = HttpContext.GetOwinContext().Authentication;
                var identity = userManager.CreateIdentity(user, "ApplicationCookie");
                var authProperties = new AuthenticationProperties()
                {
                    IsPersistent = true
                };
                authManager.SignOut();
                authManager.SignIn(authProperties,identity);
                return Redirect(string.IsNullOrEmpty(returnUrl) ? "/" : returnUrl);

            }
            }
            ViewBag.returnUrl = returnUrl;
            return View(loginModel);
        }
        [HttpGet]
        [AllowAnonymous]
        public ActionResult Register()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public ActionResult Register(Register model)
        {
            if (ModelState.IsValid)
            {
                var user = new AppUser();
                user.UserName = model.UserName;
                user.Email = model.Email;

                var result = userManager.Create(user,model.Password);
                if (result.Succeeded)
                {
                    userManager.AddToRole(user.Id,"User");
                    return RedirectToAction("Login");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("",error);
                    }
                }
            }
            return View(model);
        }
        public ActionResult Logout()
        {
           var authManager = HttpContext.GetOwinContext().Authentication;
            authManager.SignOut();
            return RedirectToAction("Login");
        }
    }
}