using DriversJournal.Models;
using DriversJournal.Services;
using DriversJournal.ViewModel;
using System;
using System.Linq;
using System.Web.Mvc;
using System.Web.Security;

namespace DriversJournal.Controllers
{
    /// <summary>
    /// Controller for the views in Home folder
    /// </summary>
    public class HomeController : Controller
    {
        /// <summary>
        /// 
        /// </summary>
        private DriversJournalContext db = new DriversJournalContext();
        private JournalUser user = new JournalUser();

        
        /// <summary>
        /// index method for returning index view
        /// </summary>
        /// <returns>index-view</returns>
        [HttpGet]
        public ActionResult Index()
        {
            if (getSessionState() == true)
            {
                return RedirectToAction("LoggedIn", "Home");
            }
            else
            {
                return View();
            }
        }

        /// <summary>
        /// index method for login and redirect
        /// </summary>
        /// <param name="journalUser">User object</param>
        /// <returns>LoggedIn-view if valid, else index-view</returns>
        [HttpPost]
        public ActionResult Index(JournalUser journalUser)
        {
            //check model from view is valid or not
            if (ModelState.IsValid)
            {
                //if true
                if (isValid(journalUser.Email, journalUser.Password))
                {
                    //creates authorized session cookie for JournalUser
                    FormsAuthentication.SetAuthCookie(this.user.Email, true);

                    //return redirected view
                    //using this method just to make sure the auth-key was created upon login.
                    if (getSessionState() == true)
                    {
                        Session["SessionUser"] = this.user;
                        return RedirectToAction("LoggedIn", "Home");
                    }
                }
            }

            return View(journalUser);
        }

        /// <summary>
        /// method to erase authorized session cookie and return JournalUser to index-page
        /// </summary>
        /// <returns>index-view</returns>
        public ActionResult Logout()
        {
            //sign out of cookie authorization
            FormsAuthentication.SignOut();
            //abandon current session
            Session.Abandon();
            //clear system of any session cookies
            Session.Clear();
            return RedirectToAction("Index", "Home");
        }

        /// <summary>
        /// the method that retrieves the view for the page that you're sent to after a successful login.
        /// </summary>
        /// <returns>LoggedIn-View</returns>
        public ActionResult LoggedIn()
        {
            if (getSessionState() == true)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        
        /// <summary>
        /// Retrieves the view for the Register-page
        /// </summary>
        /// <returns>Register-View</returns>
        public ActionResult Register()
        {
            ViewBag.Message = "Register new JournalUser page";

            return View();
        }


        /// <summary>
        /// Method for registration of a new user.
        /// </summary>
        /// <param name="vm">RegisterValidateVM object</param>
        /// <returns>Register-View</returns>
        [HttpPost]
        public ActionResult Register(RegisterValidateVM vm)
        {
            JournalUser journalUser = new JournalUser
            {
                RoleId = 2,//register a user
                FirstName = vm.Forename.ToLower(),
                LastName = vm.Surname.ToLower(),
                Email = vm.Email.ToLower()
            };
            var existing = from u in db.Users
                           where u.Email == vm.Email
                           select u;

            if (existing.Any())
            {
                ModelState.AddModelError("", "That email address is allready in use");
                TempData["failed"] = "failed";
                return View();
            }
            //adds a user to the bd and saves
            db.Users.Add(journalUser);
            db.SaveChanges();
            var user = db.Users.Single(c => c.Email == vm.Email.ToLower());
            // Hashes the password and set it in the user.
            user.Password = PasswordHasher.createHash(user.UserId, vm.Password);
            db.SaveChanges();
            //sending a mail
            MailFunction mail = new MailFunction();
            var callbackUrl = Url.Action("ConfirmAccount", "Home", new { userId = user.UserId }, protocol: Request.Url.Scheme);
            mail.sendEmail(user.Email, callbackUrl);
            ModelState.Clear();

            return View();
        }


        /// <summary>
        /// Method for changing the password for the logged in user.
        /// </summary>
        /// <param name="vm"></param>
        /// <returns>ChangePassword-View</returns>
        [HttpPost]
        public ActionResult ChangePassword(ChangePasswordVM vm)
        {
            var user = db.Users.Single(c => c.UserId == vm.UserID);
            if(user != null && ModelState.IsValid){
                var salt = db.Salts.Single(c => c.UserId == vm.UserID);
                db.Salts.Remove(salt);
                user.Password = PasswordHasher.createHash(user.UserId, vm.Password);
                db.SaveChanges();
                TempData["changed"] = "true";
            }
            else
            {
                ModelState.AddModelError("", "fel");
            }
            return View();
        }


        /// <summary>
        /// Simple method for retrieving the ChangePassword view
        /// </summary>
        /// <returns>ChangePassword-View</returns>
        public ActionResult ChangePassword()
        {
            if (getSessionState() == true)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }


        /// <summary>
        /// A method for confirming your newly created account.
        /// </summary>
        /// <param name="userId">String that's the users id</param>
        /// <returns>ConfirmAccount-View</returns>
        public ActionResult ConfirmAccount(String userId)
        {
            int id = Convert.ToInt32(userId);
            //kod för att hämta user baserat på id
            var user = db.Users.Find(id);
            if (user != null)
            {
                if (user.AccountConfirmed == 1)
                {
                    return RedirectToAction("AccountIsConfirmed", "Home");
                }
                else
                {
                    // ändrar denna användares AccountConfirmed till 1
                    user.AccountConfirmed = 1;
                    // sparar ändringen i databasen
                    db.SaveChanges();
                    return View();
                }
            }
            else
            {
                return View("Error");
            }
        }


        /// <summary>
        /// Simple method for retrieving AccountIsConfirmed-View
        /// </summary>
        /// <returns>AccountIsConfirmed-View</returns>
        public ActionResult AccountIsConfirmed()
        {
            return View();
        }


        /// <summary>
        /// Simple method for retrieving AccountRegistered-View
        /// </summary>
        /// <returns>AccountRegistered-View</returns>
        public ActionResult AccountRegistered()
        {
            return View();
        }


        /// <summary>
        /// Method to validate JournalUser username and password
        /// </summary>
        /// <param name="username">username to validate</param>
        /// <param name="password">password to validate</param>
        /// <returns>true if valid</returns>
        private bool isValid(string username, string password)
        {
            bool isValid = false;

            //get JournalUser if equals to db content
            var user = db.Users.FirstOrDefault(u => u.Email == username);
            //if the user dont exist return false
            if (user == null)
            {
                ModelState.AddModelError("", "Username and/or password is wrong!");
                return isValid;
            }
            //if the account is confirmed and password is correct
            if (user.AccountConfirmed == 1)
            {
                if (user != null && PasswordHasher.validatePassword(user.UserId, password, user.Password))
                {
                    //set global user to user without storing password
                    this.user = db.Users.FirstOrDefault(u => u.Email == username);
                    this.user.Password = null;
                    this.user.Salts = null;
                    //set state as valid
                    isValid = true;
                }
                else
                {
                    //Shows state that username/password is worng
                    ModelState.AddModelError("", "Username and/or password is wrong!");
                }
            }
            else
            {
                //sending a mail
                MailFunction mail = new MailFunction();
                var callbackUrl = Url.Action("ConfirmAccount", "Home", new { userId = user.UserId }, protocol: Request.Url.Scheme);
                mail.sendEmail(user.Email, callbackUrl);
                //shows state that account is not confirmed
                ModelState.AddModelError("", "Account is not confirmed - a new email has been sent");
            }

            return isValid;
        }


        /// <summary>
        /// /Method to check if user has a validation cookie
        /// </summary>
        /// <returns>false if user is not logged in</returns>
        private bool getSessionState()
        {
            bool isActive = false;
            var cookie = Request.Cookies[FormsAuthentication.FormsCookieName];

            //if cookie does not exist, return false
            if (cookie == null)
            {
                isActive = false;
            }
            else //if cookies does exist
            {
                FormsAuthenticationTicket ticket = null;

                //Decrypts cookie and retrieves value
                try
                {
                    ticket = FormsAuthentication.Decrypt(cookie.Value);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }

                //if ticket is not authorized, return false
                if (ticket == null)
                {
                    isActive = false;
                }
                else
                {
                    //if ticket exists and has a greater expirational
                    //date than now, return true
                    if (ticket.Expiration > DateTime.Now)
                    {
                        isActive = true;
                    }
                }
            }

            return isActive;
        }
    }
}