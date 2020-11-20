using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    public class Customer
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string Surname { get; set; }
        [Required, EmailAddress]
        public string Email { get; set; }
        [Required, MinLength(6)]
        public string Password { get; set; }
        [Required, MinLength(6), Compare("Password", ErrorMessage = "Şifreler eşleşmedi")]
    
        public string Repass { get; set; }
        public DateTime Birthday { get; set; }
        public string ProfileImage { get; set; }
        [Required]
        public bool IsActive { get; set; }
        [Required]
        public bool IsAdmin { get; set; }
        [Required]
        public string Guid { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime ModifiedDate { get; set; }
        [Required]
        public string ModifiedUser { get; set; }
        public string CompanyName { get; set; }
        public string CompanyId { get; set; }
    }
}
