using Bl;
using Dal;
using Entities;
using Entities.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
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
                if (bl_result.Messages.Count > 0)
                {
                    bl_result.Messages.ForEach(x => ModelState.AddModelError("", x));
                    return View();
                }
                //return View("SignIn");
            }
            return View();
        }
        [Route("aktivasyon")]
        public ActionResult Activation(Guid id)
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
        [Route("dashboard")]
        public ActionResult Dashboard()
        {
            if(Session["customer"] == null)
            {
                return View("SignIn");
            }
            return View();    
        }

        public ActionResult Logout()
        {
            Session.Clear();
            return View("SignIn");
        }
    }
}