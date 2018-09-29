using IEPProject.Data_Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Web;

namespace IEPProject.Utilities
{
    public class MailSender
    {
        public static void SendMail(string email, string subject, string content)
        {
            using (var smtpClient = new SmtpClient("smtp.gmail.com", 587))
            {
                smtpClient.UseDefaultCredentials = true;
                smtpClient.Credentials = new System.Net.NetworkCredential("dm150489d@gmail.com", "Iep_Projekat1.");
                smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtpClient.EnableSsl = true;
                using (var mail = new MailMessage())
                {

                    mail.From = new MailAddress("dm150489d@gmail.com", "Iep Projekat");
                    mail.To.Add(new MailAddress(email));
                    mail.IsBodyHtml = false;
                    mail.Subject = subject;
                    mail.Body = content;

                    smtpClient.Send(mail);
                }
            }
        }

        public static string OrderToString(Order order, string status)
        {
            var content = "Transaction submitted at " + order.SubmittionTime + "(" + order.NumTokens + " tokens) ";
            if(status == "success")
            {
                content += "succeeded!";
            }
            else
            {
                content += "failed!";
            }

            return content;
        }
    }
}