using Common.Messages;
using Dal;
using Entities;
using Entities.ViewModels;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                    IsActive = true,
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
                }

            }
            return result;
        }
    }
}
