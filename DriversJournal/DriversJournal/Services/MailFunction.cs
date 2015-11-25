using System.Net;
using System.Net.Mail;

namespace DriversJournal.ViewModel
{
    /// <summary>
    /// class for sending emial
    /// </summary>
    public class MailFunction
    {
        /// <summary>
        /// Sending an email with a link to activate an account
        /// </summary>
        /// <param name="mailadress"></param>
        /// <param name="url"></param>
        public void sendEmail(string mailadress, string url)
        {
            MailAddress from = new MailAddress("confirmmail@support.Com");
            MailAddress reciever = new MailAddress(mailadress);

            MailMessage mail = new MailMessage(from, reciever);

            mail.Subject = "Confirm E-mail for Drivers Journal";
            mail.Body = "You have registerd an account on Sogeti's Drivers Journal </br>"
                + "Confirm you E-mailadress on the link below: </br>" +
                "<a herf='" + url + "'>" + url + "</a>";
            SmtpClient smtp = new SmtpClient();
            smtp.Host = "smtp.gmail.com";
            smtp.Port = 587;

            smtp.Credentials = new NetworkCredential(
                "driversjournalinfo@gmail.com", "HejHej123");

            smtp.EnableSsl = true;
            mail.IsBodyHtml = true;
            smtp.Send(mail);
        }
        /// <summary>
        /// sends an email with a new password to a user
        /// </summary>
        /// <param name="mailadress"></param>
        /// <param name="newcode"></param>
        public void sendForgottenEmail(string mailadress,string newcode)
        {
            MailAddress from = new MailAddress("confirmmail@support.Com");
            MailAddress reciever = new MailAddress(mailadress);

            MailMessage mail = new MailMessage(from, reciever);

            mail.Subject = "Forgotten password";
            mail.Body = "This is your new password: </br>"+
                "<h3>"+ newcode +"</h3></br>"+
                " You can change it in change password";
            SmtpClient smtp = new SmtpClient();
            smtp.Host = "smtp.gmail.com";
            smtp.Port = 587;

            smtp.Credentials = new NetworkCredential(
                "driversjournalinfo@gmail.com", "HejHej123");

            smtp.EnableSsl = true;
            mail.IsBodyHtml = true;
            smtp.Send(mail);
        }



    }
}