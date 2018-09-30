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
        public static void SendMail(string email, string firstName, string lastName, bool success, int amount)
        {
            string subject;
            string content;

            if (success)
            {
                subject = "Yay! You bought " + amount + " tokens!";
                content = string.Concat(
                    "Dear ", firstName, " ", lastName, ",\n",
                    "\n",
                    "You successfully bought ", amount, " tokens\n",
                    "\n",
                    "Best,\n",
                    "dm150489d\n");
            }
            else
            {
                subject = "Ooops! Your transaction for " + amount + " tokens failed!";
                content = string.Concat(
                    "Dear ", firstName, " ", lastName, ",\n",
                    "\n",
                    "Unfortunantely, you transaction for ", amount, " tokens failed.\n",
                    "Don't be discouraged to try again!\n",
                    "\n",
                    "Best,\n",
                    "dm150489d\n");
            }

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