using DriversJournal.Models;
using DriversJournal.Services;
using DriversJournal.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DriversJournal.Controllers
{
    /// <summary>
    /// A class that makes it possible for the user to get a new password if he/she forgot it.
    /// </summary>
    public class ForgotPasswordController : Controller
    {

        private MailFunction mail = new MailFunction();
        private CodeGenerator codgen = new CodeGenerator();
        private JournalUser user = new JournalUser();
        private DriversJournalContext db = new DriversJournalContext();


        // GET: ForgotPassword
        /// <summary>
        /// Simple method for retrieving Index-View.
        /// </summary>
        /// <returns>Index-View</returns>
        public ActionResult Index()
        {
            return View();
        }


        /// <summary>
        /// If user exists the method contacts ForgotPassword-method and sends a new password, else it fails and View is returned.
        /// </summary>
        /// <param name="vm"></param>
        /// <returns>Index-View</returns>
        [HttpPost]
        public ActionResult Index(ForgotPassword vm)
        {

            var existing = from u in db.Users
                           where u.Email == vm.Email
                           select u;
            
            if(existing.Any()){
               ForgotPassword(vm.Email);
               return View();
            }
            else
            {
                TempData["failed"] = "failed";
                return View();
            }
            
        }


        /// <summary>
        /// A mehtod that gets the user from the database, then sets a new random secure password for the user.
        /// The user is then emailed the new password.
        /// </summary>
        /// <param name="email">String variable of user email</param>
        public void ForgotPassword(string email)
        {
            var newcode = codgen.codeGenerator(); 

            var user = db.Users.FirstOrDefault(u => u.Email == email);

            var salt = db.Salts.Single(c => c.UserId == user.UserId);
            db.Salts.Remove(salt);
                       
            user.Password = PasswordHasher.createHash(user.UserId, newcode);
            db.SaveChanges();
            mail.sendForgottenEmail(email,newcode);

        } 

    }
}