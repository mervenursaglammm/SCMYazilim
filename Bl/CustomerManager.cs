using Common.Helpers;
using Common.Messages;
using Dal;
using Dal.Repository;
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
        private CustomerRepository<DenemeEntity> repo_customer = new CustomerRepository<DenemeEntity>();
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
                    createContext.DenemeEntities.Add(new DenemeEntity()
                    {
                        str = "sefa"
                    });
                    createContext.SaveChanges();
                    createContext.DenemeEntities.ToList();

                    string body = "Hello " + result.Result.Name + ",";
                    body += "<br /><br />Please click the following link to activate your account";
                    body += "<br /><a href = '" + string.Format("{0}://{1}/Home/Activation/{2}", "https", "localhost:44313", result.Result.Guid) + "'>Click here to activate your account.</a>";
                    body += "<br /><br />Thanks";

                    MailHelper mailHelper = new MailHelper();
                    mailHelper.SendMail(result.Result.Email, body);
                }
            }
            return result;
        }
        public void Activation(Guid guid)
        {
            Customer customer = repo.Find(x => x.Guid == guid);

            if (customer == null)
            {
                // Var ise hata mesajı...
            }
            else
            {
                customer.IsActive = true;
                repo.Update(customer);
            }
        }
        public BL_Result<Customer> LogIn(UserViewModel userViewModel)
        {
            Customer customer = repo.Find(x => x.Email == userViewModel.Email && x.Password == userViewModel.Password);
          
            if(customer != null)
            {
                if(customer.IsActive == false)
                {
                    result.addError(ErrorMessages.UserNotActive, "Kullanıcı aktif değil");
                }

            }
            else
            {
                result.addError(ErrorMessages.UserNotFound, "Kullanıcı bulunamadı");
            }
            return result;
        }
    }
}
