using Bl;
using Dal;
using Entities;
using Entities.ViewModels;
using Microsoft.SqlServer.Management.Common;
using Microsoft.SqlServer.Management.Smo;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace SCMYazilim.Controllers
{
    public class HomeController : Controller
    {
        private CustomerManager<Customer> customerManager = new CustomerManager<Customer>();
        // GET: Home
        public ActionResult Index()
        {
            return View();
        }
        [Route("giris")]
        public ActionResult SignIn()
        {
            return View();
        }

        [Route("giris")]
        [HttpPost]
        public ActionResult SignIn(UserViewModel userViewModel)
        {

            if (ModelState.IsValid)
            {
                BL_Result<Customer> bl_result = customerManager.LogIn(userViewModel);
                if (bl_result.Messages.Count > 0)
                {
                    bl_result.Messages.ForEach(x => ModelState.AddModelError("", x));
                   
                    return View();
                }
                else
                {
                    Session["customer"] = userViewModel.Email;
                    return View("Dashboard");
                }
            }
            return View();
        }
        [Route("uyelik")]
        public ActionResult SignUp()
        {

            return View();
        }
        [Route("uyelik")]
        [HttpPost]
        public ActionResult SignUp(RegisterViewModel registerViewModel)
        {
            if (ModelState.IsValid)
            {
                BL_Result<Customer> bl_result = customerManager.Register(registerViewModel);

                // Türkçe karakter çevirme
                //var text = registerViewModel.Name;
                //var unaccentedText = String.Join("", text.Normalize(NormalizationForm.FormD)
                //           .Where(c => char.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark));
                //var t = unaccentedText;


                //var s = new Microsoft.SqlServer.Management.Smo.Server(@"DESKTOP-T7SEF7T\SQLEXPRESS");----
                //List<string> alldatabases = new List<string>();

                //foreach (Microsoft.SqlServer.Management.Smo.Database db in s.Databases)
                //{
                //    alldatabases.Add(db.Name);
                //}

                //List<string> alldatabasesSon = new List<string>();
                //alldatabasesSon = alldatabases;-----

                if (bl_result.Messages.Count > 0)
                {
                    bl_result.Messages.ForEach(x => ModelState.AddModelError("", x));
                    return View();
                }
                return View("SignIn");
            }
            return View();
        }


        //[Route("aktivasyon")]
        public ActionResult Activation(string id)
        {
            ViewBag.Message = "Invalid Activation code.";
            if (id != null)
            {
                customerManager.Activation(id);
                ViewBag.Message = "Activation successful.";
                return View();
            }
            return View();
        }

        public ActionResult Dashboard()
        {
            if(Session["customer"] == null)
            {
                return View("SignIn");
            }
            return View();    
        }

        public ActionResult Profile()
        {
            return View();
        }

        public ActionResult Logout()
        {
            Session.Clear();
            return View("SignIn");
        }
    }
}