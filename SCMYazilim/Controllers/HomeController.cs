using Bl;
using Dal;
using Dal.Repository;
using Entities;
using Entities.ViewModels;
using Microsoft.SqlServer.Management.Common;
using Microsoft.SqlServer.Management.Smo;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace SCMYazilim.Controllers
{
    public class HomeController : Controller
    {
        private CustomerManager<Customer> customerManager = new CustomerManager<Customer>();
        private CustomerRepository<Customer> customerRepo = new CustomerRepository<Customer>();
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

        [Route("uyelik")]
        public ActionResult SignUp(UserViewModel userViewModel)
        {

            if (ModelState.IsValid)
            {
                BL_Result<CustomerInfo> bl_result = customerManager.LogIn(userViewModel);
                if (bl_result.Messages.Count > 0)
                {
                    bl_result.Messages.ForEach(x => ModelState.AddModelError("", x));
                   
                    return View();
                }
                else
                {
                    Session["customer"] = bl_result.Result;
                   
                    return View("Dashboard");
                }
            }
            return View();
        }


        [Route("uyelik")]
        [HttpPost]
        public ActionResult SignUp(RegisterViewModel registerViewModel)
        {
            if (ModelState.IsValid)
            {
                BL_Result<Customer> bl_result = customerManager.Register(registerViewModel);

            

                if (bl_result.Messages.Count > 0)
                {
                    bl_result.Messages.ForEach(x => ModelState.AddModelError("", x));
                    return View();
                }
                return View("SignIn");
            }
            return View(registerViewModel);
        }


        [Route("giris")]
        [HttpPost]
        public ActionResult SignIn(UserViewModel userViewModel)
        {
            if (ModelState.IsValid)
            {
                BL_Result<CustomerInfo> bl_result = customerManager.LogIn(userViewModel);
                if (bl_result.Messages.Count > 0)
                {
                    bl_result.Messages.ForEach(x => ModelState.AddModelError("", x));
                    return View();
                }
                else
                {
                    Session["customer"] = bl_result.Result;
                    return View("Dashboard");
                }
            }
            return View();
        }

        public ActionResult Authorization(CustomerInfo customerInfo)
        {
            return View();    
        }

        //[HttpPost]
        public ActionResult Profile()
        {
            //if (Request.Files.Count != 0)
            //{

            //    for (int i = 0; i < Request.Files.Count; i++)
            //    {
            //        var file = Request.Files[i];

            //        var fileName = Path.GetFileName(file.FileName);

            //        var path = Path.Combine(Server.MapPath("~/Content/Images/"), fileName);
            //        file.SaveAs(path);
            //    }

            //}
            return View();
        }

        public ActionResult Logout()
        {
            Session.Clear();
            return View("SignIn");
        }
    }
}