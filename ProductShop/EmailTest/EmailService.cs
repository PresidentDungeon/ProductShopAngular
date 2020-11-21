using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Mail;

namespace EmailTest
{
    public class EmailService
    {

        public void sendEmail()
        {
            var fromAddress = new MailAddress("anders.elis.madsen@gmail.com", "dd Madsen");
            var toAddress = new MailAddress("anders.elis.madsen@gmail.com", "Dear Customer");
            const string fromPassword = "wmittogyrdbhlcbj";
            const string subject = "Bestilling af ordre";
            const string body = "<p>Bestilling af ordre: 1</p> <p><br></p><p>Hej Anders:</p><p><br></p><p> Her er en bekræftelse af ordre nummer 1... Du har købt en... Is :D </p>";

            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromAddress.Address, fromPassword)
            };
            using (var message = new MailMessage(fromAddress, toAddress){
                Subject = subject,
                IsBodyHtml = true,
                Body = body
            })
            {
                smtp.Send(message);
            }
        }
    }
}
