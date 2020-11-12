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
        // GET: Home
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult SignIn()
        {
            return View();
        }
        public ActionResult SignUp()
        {


            return View();
        }
        [HttpPost]
        public ActionResult SignUp(RegisterViewModel registerViewModel)
        {
            //if(ModelState.IsValid)
            //{

            //    if(registerViewModel.Email==null)
            //    {

            //    }
            //}

            return View();
        }
    }
}