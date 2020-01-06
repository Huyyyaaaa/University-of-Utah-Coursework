using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace Assignment2.Models
{
    public class EmailSender : IEmailSender
    {
        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            var task = new Task(() =>
            {
                SmtpClient smtpClient= new SmtpClient("smtp.Utah.edu", 25);
                MailMessage mail = new MailMessage();
                mail.From = new MailAddress("admin@cs4540.com", "LOT");
                mail.To.Add(new MailAddress("germain@cs.utah.edu"));
                mail.Subject = subject;
                mail.Body = htmlMessage;
                mail.IsBodyHtml = true;

                smtpClient.Send(mail);

            });
            task.Start();
            return task;
        }
    }
}
