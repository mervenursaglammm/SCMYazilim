using Common.Helpers;
using Common.Messages;
using Dal;
using Dal.Repository;
using Entities;
using Entities.ViewModels;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity.Validation;
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
        private Repository<Customer> repo = new Repository<Customer>();
        private CustomerRepository<CustomerInfo> repo_customer = new CustomerRepository<CustomerInfo>();
        public BL_Result<Customer> Register(RegisterViewModel registerViewModel)
        {
            var searchCompanyId = registerViewModel.CompanyId;
            if (searchCompanyId != null)
            {
                Customer customer = repo.Find(x => x.CompanyId == searchCompanyId);
                if (customer != null)
                {
                    string deneme = customer.CompanyName + customer.Id;
                    string databasename = Connection.DatabaseConnection(deneme);
                    if (databasename != "")
                    {
                        string baseConnectionString = ConfigurationManager.ConnectionStrings["BaseConnectionString"].ConnectionString;
                        CreateDbContext createContext = new CreateDbContext(string.Format(baseConnectionString, databasename));
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
                                CompanyName = registerViewModel.CompanyName,
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

                }
                else
                {
                    result.addError(ErrorMessages.CompanyNotFound, "Böyle bir şirket bulunamadı.");
                }
            }
            else
            {
                Customer customer = repo.Find(x => x.Email == registerViewModel.Email);
                //  Kullanicinin kayitli olma durumu kontrolu
                if (customer != null)
                {
                    //result.Messages.Add("Kayıtlı kullanıcı");
                    result.addError(ErrorMessages.RegisteredUser, "Kayıtlı kullanıcı");
                }
                else
                {

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
                    {
                        result.Result = repo.Find(x => x.Email == registerViewModel.Email);
                        //Aktivasyon Maili Gonderme
                        string body = "Hello " + result.Result.Name + ",";
                        body += "<br /><br />Please click the following link to activate your account <br /> Your CompanyId: " + result.Result.CompanyId;
                        body += "<br /><a href = '" + string.Format("{0}://{1}/Home/Activation/{2}", "https", "localhost:44313", result.Result.Guid) + "'>Click here to activate your account.</a>";
                        body += "<br /><br />Thanks";

                        MailHelper mailHelper = new MailHelper();
                        mailHelper.SendMail(result.Result.Email, body);
                    }


                }
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
                CreateDbContext createContext = new CreateDbContext(string.Format(baseConnectionString, customer.CompanyName + customer.Id));
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
                    ProfileImage = "~/Content/Images/defaultUserImage.jpg"
                }); ;
                createContext.SaveChanges();

                repo.Update(customer);
            }
        }

        public BL_Result<Customer> LogIn(UserViewModel userViewModel)
        {
            Customer customer = repo.Find(x => x.CompanyId == userViewModel.CompanyId);
            if (customer != null)
            {
                string companyDatabase = customer.CompanyName + customer.Id;
                string databaseName = Connection.DatabaseConnection(companyDatabase);

                if (databaseName != "")
                {
                    string baseConnectionString = ConfigurationManager.ConnectionStrings["BaseConnectionString"].ConnectionString;
                    CreateDbContext createContext = new CreateDbContext(string.Format(baseConnectionString, databaseName));
                    CustomerInfo _customerInfo = repo_customer.Find(x => x.Email == userViewModel.Email, string.Format(baseConnectionString, databaseName));
                    if (_customerInfo == null)
                    {
                        result.addError(ErrorMessages.UserNotFound, "Kullanıcı bulunamadı.");
                    }
                }
                else
                {
                    result.addError(ErrorMessages.CompanyNotFound, "Şirket bulunamadı.");
                }
            }
            else
            {
                result.addError(ErrorMessages.UserNotFound, "Kullanıcı bulunamadı.");
            }
            return result;
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



    }

}




