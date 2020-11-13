﻿using Bl;
using Entities;
using Entities.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SCMYazilim.Controllers
{
    [RoutePrefix("Sefa")]
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
        [Route("uyelik")]
        public ActionResult SignUp()
        {
            return View();
        }
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
            return View();
        }
    }
}