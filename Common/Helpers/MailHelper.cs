﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Common.Helpers
{
    public class MailHelper
    {
        public void SendMail(string mail, string mailBody)
        {
            using (MailMessage mm = new MailMessage("cnurztrk@gmail.com", mail))
            {
                mm.Subject = "Account Activation";
                string body = mailBody;
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
}
