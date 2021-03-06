﻿using Common.Helpers;
using Common.Messages;
using Dal;
using Dal.Repository;
using Entities;
using Entities.ViewModels;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity.Validation;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Bl
{
    public class CustomerManager<T> where T : class
    {
        private BL_Result<Customer> result = new BL_Result<Customer>();
        private BL_Result<CustomerInfo> result1 = new BL_Result<CustomerInfo>();
        private Repository<Customer> repo = new Repository<Customer>();
        private CustomerRepository<CustomerInfo> repo_customer = new CustomerRepository<CustomerInfo>();
        private static CreateDbContext createContext;
        public BL_Result<Customer> Register(RegisterViewModel registerViewModel)
        {
            var searchCompanyId = registerViewModel.CompanyId;
            var registeredUserEmail = repo.Find(x => x.Email == registerViewModel.Email);
            if (registeredUserEmail == null)
            {
                Customer admin = repo.Find(x => x.CompanyId == searchCompanyId);    //kayit olmaya calisan kullanici icin myDataBase icerisinde kayit olacagi sirketin admini araniyor.
                if(admin!=null)
                {
                    var adminCompanyName = admin.CompanyName;
                    //CompanyId alani bos gelmemis ise kullanici myDataBase icerisine ekleniyor.
                    if (searchCompanyId != null)
                    {
                        if (registerViewModel.Password == registerViewModel.Repass)
                        {
                            int db_result = repo.Insert(new Customer()
                            {
                                Name = registerViewModel.Name,
                                Email = registerViewModel.Email,
                                Password = EncodePassword(registerViewModel.Password),
                                Repass = EncodePassword(registerViewModel.Repass),
                                IsActive = true,
                                IsAdmin = false,
                                Guid = "merve",
                                CompanyId = registerViewModel.CompanyId,
                                CreateDate = DateTime.Now,
                                ModifiedDate = DateTime.Now,
                                ModifiedUser = "System",
                                CompanyName = adminCompanyName,
                                Birthday = DateTime.Now
                            });
                        }
                        else
                        {
                            result.addError(ErrorMessages.PasswordsDoNotMatch, "Şifre eşleşmiyor.Tekrar deneyiniz.");
                        }
                    }
               

                      
                    
                        //Kayit olmaya calisan kullanicinin belirttigi companyId ye ait admin(sirket) bos degilse kullanici CustomerInfoes icerisine ekleniyor.
                        string deneme = admin.CompanyName + admin.CompanyId;
                        string databasename = Connection.DatabaseConnection(deneme);
                        if (databasename != "")
                        {
                            string baseConnectionString = ConfigurationManager.ConnectionStrings["BaseConnectionString"].ConnectionString;
                            createContext = new CreateDbContext(string.Format(baseConnectionString, databasename));
                            CustomerInfo user = createContext.CustomerInfos.FirstOrDefault(x => x.Email == registerViewModel.Email);

                            if (user != null)
                            {
                                result.addError(ErrorMessages.RegisteredUser, "Kayıtlı kullanıcı");
                            }
                            else
                            {
                                createContext.CustomerInfos.Add(new CustomerInfo()
                                {
                                    Name = registerViewModel.Name,
                                    Email = registerViewModel.Email,
                                    CompanyName = adminCompanyName,
                                    Password = EncodePassword(registerViewModel.Password),
                                    Repass = EncodePassword(registerViewModel.Repass),
                                    IsAdmin = false,
                                    CompanyId = registerViewModel.CompanyId,
                                    CreateDate = DateTime.Now,
                                    ModifiedDate = DateTime.Now,
                                    ModifiedUser = "System",
                                    Birthday = DateTime.Now
                                });
                                createContext.SaveChanges();
                            }
                        }

                    
                    else
                    {
                        result.addError(ErrorMessages.CompanyNotFound, "Böyle bir şirket bulunamadı.");
                    }
                }
                else
                {
                    result.addError(ErrorMessages.CompanyNotFound, "Böyle bir şirket bulunamadı.");
                }
                    Customer customer = repo.Find(x => x.Email == registerViewModel.Email);
                    //  Adminin daha once kayitli olma durumu kontrolu
                    if (customer != null)
                    {
                        //result.Messages.Add("Kayıtlı kullanıcı");
                        result.addError(ErrorMessages.RegisteredUser, "Kayıtlı kullanıcı");
                    }
                    else
                    {
                        if (registerViewModel.Password == registerViewModel.Repass && registerViewModel.CompanyName!=null)
                        {
                            //admin kaydi yapiliyor.
                            int db_result = repo.Insert(new Customer()
                            {
                                Name = registerViewModel.Name,
                                Email = registerViewModel.Email,
                                CompanyName = String.Join("", registerViewModel.CompanyName.Normalize(NormalizationForm.FormD).Where(c => char.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark)),
                                Password = EncodePassword(registerViewModel.Password),
                                Repass = EncodePassword(registerViewModel.Repass),
                                IsActive = false,
                                IsAdmin = true,
                                Guid = Guid.NewGuid().ToString(),
                                CompanyId = Guid.NewGuid().ToString().Substring(0, 6),
                                CreateDate = DateTime.Now,
                                ModifiedDate = DateTime.Now,
                                ModifiedUser = "System",
                                Birthday = DateTime.Now
                            });

                            if (db_result > 0)
                            {  //admin icin aktivasyon maili
                                result.Result = repo.Find(x => x.Email == registerViewModel.Email);
                                //Aktivasyon Maili Gonderme
                                string body = "Hello " + result.Result.Name + ",";
                                body += "<br /><br />Please click the following link to activate your account <br /> Your CompanyId: " + result.Result.CompanyId;
                                body += "<br /><a href = '" + string.Format("{0}://{1}/Home/Activation/{2}", "https", "localhost:44313", result.Result.Guid) + "'>Click here to activate your account.</a>";
                                body += "<br /><br />Thanks";

                                MailHelper mailHelper = new MailHelper();
                                mailHelper.SendMail(result.Result.Email, body);
                            }
                        
                        else
                        {
                            result.addError(ErrorMessages.PasswordsDoNotMatch, "Şifre eşleşmiyor. Tekrar deneyiniz.");
                        }

                    }
                }
            }
            else
            {
                result.addError(ErrorMessages.RegisteredUser, "Kayıtlı kullanıcı");
            }
            return result;
        }
        public void Activation(string guid)
        {
            Customer customer = repo.Find(x => x.Guid == guid);

            if (customer == null)
            {
                //Var ise hata mesajı...
            }
            else
            {
                customer.IsActive = true;

                string baseConnectionString = ConfigurationManager.ConnectionStrings["BaseConnectionString"].ConnectionString;
                createContext = new CreateDbContext(string.Format(baseConnectionString, customer.CompanyName + customer.CompanyId));
                createContext.CustomerInfos.Add(new CustomerInfo()
                {
                    Name = customer.Name,
                    Email = customer.Email,
                    CompanyName = String.Join("", customer.CompanyName.Normalize(NormalizationForm.FormD).Where(c => char.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark)),
                    Password = EncodePassword(customer.Password),
                    Repass = EncodePassword(customer.Repass),
                    CompanyId = customer.CompanyId,
                    IsAdmin = true,
                    CreateDate = DateTime.Now,
                    ModifiedDate = DateTime.Now,
                    ModifiedUser = "System",
                    Birthday = DateTime.Now,
                    ProfileImage = "",
                }) ; 
                createContext.SaveChanges();

                repo.Update(customer);
            }
        }
        public BL_Result<CustomerInfo> LogIn(UserViewModel userViewModel)
        {
            Customer customer = repo.Find( x => x.Email == userViewModel.Email);
            string CompanyName = repo.Find(x => x.CompanyId == customer.CompanyId && x.IsAdmin == true).CompanyName;
            string companyId = repo.Find(x => x.CompanyId == customer.CompanyId && x.IsAdmin == true).CompanyId;
            if (customer != null)
            {
                string companyDatabase = CompanyName + companyId;
                string databaseName = Connection.DatabaseConnection(companyDatabase);

                if (databaseName != "")
                {
                    string baseConnectionString = ConfigurationManager.ConnectionStrings["BaseConnectionString"].ConnectionString;
                     createContext = new CreateDbContext(string.Format(baseConnectionString, databaseName));
                    CustomerInfo _customerInfo = repo_customer.Find(x => x.Email == userViewModel.Email, string.Format(baseConnectionString, databaseName));
                   
                    result1.Result = _customerInfo;
                   
                 //   cnn = string.Format(baseConnectionString, databaseName);
                    if (_customerInfo == null)
                    {
                        result1.addError(ErrorMessages.UserNotFound, "Kullanıcı bulunamadı.");
                     
                       
                    }
                  
                }
                else
                {
                    result1.addError(ErrorMessages.CompanyNotFound, "Şirket bulunamadı.");
                }
            }
            else
            {
                result1.addError(ErrorMessages.UserNotFound, "Kullanıcı bulunamadı.");
            }
            return result1;
        }


        public string EncodePassword(string originalPassword)
        {

            Byte[] originalBytes;
            Byte[] encodedBytes;
            MD5 md5;
            md5 = new MD5CryptoServiceProvider();
            originalBytes = ASCIIEncoding.Default.GetBytes(originalPassword);
            encodedBytes = md5.ComputeHash(originalBytes);
            return BitConverter.ToString(encodedBytes);
        }

        public List <CustomerInfo> GetCustomers()
        {
           return createContext.CustomerInfos.ToList();
        }

        /*public string UpdateUserImage(string imageName, int id) {
            createContext.Database.ExecuteSqlCommand("Update CustomerInfoes set ProfileImage='~/Content/Images/" +imageName+"' where Id='"+id+"' ");
            return "";
        } */
        public void UpdateUserImage(int id, string contentUrl)
        {
            var data=createContext.CustomerInfos.FirstOrDefault(x => x.Id == id);
            data.ProfileImage = contentUrl;
            createContext.SaveChanges();
        }


    }

}




