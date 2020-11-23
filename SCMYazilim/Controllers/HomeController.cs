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
     //   [Route("aktivasyon")]
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

        //public ActionResult Authorization()
        //{
        //    //  List<CustomerInfo>customers=
        //    return View();
        //}
        //[HttpPost]
        public ActionResult Authorization()
        {
           List<CustomerInfo>infos = customerManager.GetCustomers();

            
            return View(infos);    
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