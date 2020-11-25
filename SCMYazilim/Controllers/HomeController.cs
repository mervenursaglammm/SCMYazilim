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
using System.Web.Security;

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
                    FormsAuthentication.SetAuthCookie(bl_result.Result.Name, false);
                    Session["customer"] = bl_result.Result;
                    return View("Dashboard");
                }
            }
            return View();
        }

        [Authorize]
        public ActionResult Authorization()
        {
            List<CustomerInfo> infos = customerManager.GetCustomers();


            return View(infos);
        }

        [Authorize]
        public ActionResult Profile()
        {
            return View();
        }
        [HttpPost]
        [Authorize]
        public ActionResult Profile(HttpPostedFile file)
        {


            return View("");
        }


        public JsonResult uploadFile()
        {
            // check if the user selected a file to upload
            if (Request.Files.Count > 0)
            {
                try
                {
                    CustomerInfo customerInfos = Session["customer"] as CustomerInfo;
                    HttpFileCollectionBase files = Request.Files;

                    HttpPostedFileBase file = files[0];

                    if (file != null && (file.ContentType == "image/jpeg" || file.ContentType == "image/jpg" || file.ContentType == "image/png"))
                    {
                        string filename = $"{customerInfos.Email}.{file.ContentType.Split('/')[1]}";
                        Directory.CreateDirectory(Server.MapPath("~/Content/Images/"));
                        string path = Path.Combine(Server.MapPath("~/Content/Images/"), filename);
                        string extension = Path.GetExtension(path);
                        string savedPath = "~/Content/Images/" + filename;
                        // save the file
                        file.SaveAs(path);


                        customerManager.UpdateUserImage(customerInfos.Id, savedPath);
                        file.SaveAs(Server.MapPath($"~/images/{filename}"));
                     
                    }
                    else
                    {

                    }
                  
                    
                
                
                    return Json("File uploaded successfully");
                }

                catch (Exception e)
                {
                    return Json("error" + e.Message);
                }
            }

            return Json("no files were selected !");
        }


        public ActionResult Logout()
        {
            Session.Clear();
            FormsAuthentication.SignOut();
            return View("SignIn");
        }
    }
}