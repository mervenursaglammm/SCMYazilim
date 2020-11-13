using Common.Messages;
using Dal;
using Entities;
using Entities.ViewModels;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Bl
{
    public class CustomerManager<T> where T : class
    {
        private BL_Result<Customer> result = new BL_Result<Customer>();
        private Repository<Customer> repo = new Repository<Customer>();
        public BL_Result<Customer> Register(RegisterViewModel registerViewModel)
        {
            Customer customer = repo.Find(x => x.Email == registerViewModel.Email);

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
                    Password = registerViewModel.Password,
                    Repass = registerViewModel.Repass,
                    IsActive = false,
                    IsAdmin = true,
                    Guid = Guid.NewGuid(),
                    CreateDate = DateTime.Now,
                    ModifiedDate = DateTime.Now,
                    ModifiedUser = "System",
                    Birthday = DateTime.Now
                });

                if (db_result > 0)
                {
                    result.Result = repo.Find(x => x.Email == registerViewModel.Email);

                    string baseConnectionString = ConfigurationManager.ConnectionStrings["BaseConnectionString"].ConnectionString;
                    CreateDbContext createContext = new CreateDbContext(string.Format(baseConnectionString, "customer" + result.Result.Id));
                    var deneme = createContext.DenemeEntities.ToList();

                    // Doğrulama kodu gönder

                    Guid activationCode = result.Result.Guid;


                   
                    using (MailMessage mm = new MailMessage("cnurztrk@gmail.com", result.Result.Email))
                    {
                        mm.Subject = "Account Activation";
                        var Request = HttpContext.Current.Request;
                        string body = "Hello " + result.Result.Name + ",";
                        body += "<br /><br />Please click the following link to activate your account";
                        body += "<br /><a href = '" + string.Format("{0}://{1}/Home/Activation/{2}","https", "localhost:44313", activationCode) + "'>Click here to activate your account.</a>";
                        body += "<br /><br />Thanks";
                        mm.Body = body;
                        mm.IsBodyHtml = true;
                        SmtpClient smtp = new SmtpClient();
                        smtp.Host = "smtp.gmail.com";
                        smtp.EnableSsl = true;
                        NetworkCredential NetworkCred = new NetworkCredential("cnurztrk@gmail.com", "ceylan1234");
                        smtp.UseDefaultCredentials = true;
                        smtp.Credentials = NetworkCred;
                        smtp.Port = 587;
                        smtp.Send(mm);

                    }

                }

            }
            return result;
        }
    }
}
