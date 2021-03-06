﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.ViewModels
{
    public class RegisterViewModel
    {
        [Required]
        public string Name { get; set; }
        [Required, EmailAddress]
        public string Email { get; set; }
        [Required, MinLength(6)]
        public string Password { get; set; }
        [Compare("Password", ErrorMessage = "Şifre eşleşmiyor.Tekrar deneyiniz.")]
        public string Repass { get; set; }
        public string CompanyName { get; set; }
        public string CompanyId { get; set; }
    }
}
