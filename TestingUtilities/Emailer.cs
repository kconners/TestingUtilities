
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Mail;

namespace TestingUtilities
{
    public class Emailer
    {
        private string fromPassword;
        private string fromEmailAddress;
        public Emailer(string _FromEmailAddress = "", string _AccountCred = "")
        {
            SASS s = new SASS();
            fromPassword = s.EncryptToString(_AccountCred);
            fromEmailAddress = _FromEmailAddress;
        }

        public Emailer()
        {
            fromPassword = "091096194145110004094240253165077203221099204044";
            fromEmailAddress = "testertarts@gmail.com";
        }

        private void GMaiLTheResults(string _Body, string[] Addressees, string _Subject = "Results from testing")
        {
            var fromAddress = new MailAddress(fromEmailAddress, "Automated Testing");
            var toAddress = new MailAddress("kdcatbp@gmail.com", "EmailerAuthor");

            SASS s = new SASS();
            fromPassword = s.DecryptString(fromPassword);
            
            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromEmailAddress, fromPassword)
            };
            var message = new MailMessage(fromAddress, toAddress);
            message.Subject = _Subject;
            message.Body = _Body;

            if (Addressees.Count() >= 1)
            {
                foreach (var i in Addressees)
                {
                    message.To.Add(i);
                }
            }
            smtp.Send(message);

        }
    }
}
