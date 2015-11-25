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
    /// Controller for the Journal.cshtml
    /// </summary>
    public class JournalsController : Controller
    {
        /// <summary> Object used to insert, update, delete and read from database</summary>
        private DriversJournalContext db = new DriversJournalContext();

        /// <summary>Object used to call methods that retrives data from DB </summary>
        private GetDataFromDb serviceGet = new GetDataFromDb();

        /// <summary>Object used to call methods that saved data to DB </summary>
        private SaveJournalDB serviceSave = new SaveJournalDB();


        /// <summary>
        /// Shows view Journal and fill the form depending on whether a saved journal exist. 
        /// </summary>
        /// <returns>Jornal-view </returns>
        public ActionResult Journal()
        {
           
            if (getSessionState() != true)
            {
                return RedirectToAction("LoggedIn", "Home");
            }
            else
            {
                JournalUser user = (JournalUser)Session["SessionUser"];
                int userId = user.UserId;

                JournalVM vm;

                // retrive the journal with column SavedNotSent == 1 depening on user
                var listJournals = from u in db.Users
                                   join j in db.Journals
                                       on u.UserId equals j.UserId
                                   where (u.UserId == userId) && (j.SavedNotSent == 1)
                                   select j;

                // retrives the car from db
                var dbcar = GetCar();

                // if their is a saved journal, create vm depending on these values.
                if (listJournals.Any())
                {
                    var journal = listJournals.First();
                    vm = serviceGet.GetSavedJournal(journal);
                }
                else
                {
                    vm = new JournalVM
                    {
                        OdometerStart = serviceGet.CarOdometer(dbcar.Regno),//hard coded regno
                        Cars = serviceGet.GetCars(),
                        Projects = serviceGet.GetProjects(userId),
                        Debits = serviceGet.GetDebit(),
                        StartDate = DateTime.Now.ToString("yyyy-MM-dd"),
                        EndDate = DateTime.Now.ToString("yyyy-MM-dd")
                    };
                }

                return View(vm);
            }
        }

        /// <summary>
        /// Saves jornalform to DB
        /// Shows view Journal and fill the form depending on whether a journal was saved or sent 
        /// </summary>
        /// <param name="vm">values to be saved to the DB </param>
        /// <param name="project">Saves projectnumber to DB</param>
        /// <param name="debit">Save debit to DB</param>
        /// <param name="car">Save car to DB</param>
        /// <returns>Jornal-view</returns>
        [HttpPost]
        public ActionResult Journal(JournalVM vm, string project, string debit, string car)
        {
            JournalUser user = (JournalUser)Session["SessionUser"];
            int userId = user.UserId;
                //if submit button save is pressed, this code execute
                if (Request.Form["save"] == "save")
                {
                    //saves the journal to db
                    serviceSave.SaveJournal(vm, project, userId, debit, car);
                    //get the saved drive
                    var listJournals = from u in db.Users
                                       join j in db.Journals
                                           on u.UserId equals j.UserId
                                       where (u.UserId == userId) && (j.SavedNotSent == 1)
                                       select j;
                    var journal = listJournals.First();
                    vm = serviceGet.GetSavedJournal(journal);
                }
                else
                {
                    serviceSave.SendJournal(vm, project, userId, debit, car);
                    ModelState.Clear();

                    // retrives the car from db
                    var dbcar = GetCar();

                    vm = new JournalVM
                    {
                        OdometerStart = serviceGet.CarOdometer(dbcar.Regno),//hard coded regno
                        Cars = serviceGet.GetCars(),
                        Projects = serviceGet.GetProjects(userId),
                        Debits = serviceGet.GetDebit(),
                        StartDate = DateTime.Now.ToString("yyyy-MM-dd"),
                        EndDate = DateTime.Now.ToString("yyyy-MM-dd")
                    };
                }
            
            return View(vm);
        }

        /// <summary>
        /// /Method to check if user is validated on session
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
        /// <summary>
        /// Retrieves the car from DB
        /// At this moment the method can only retrive one car
        ///  </summary>
        /// <returns>Car from db</returns>
        private Car GetCar()
        {
            // retrives car from db
            var carList = from c in db.Cars
                          select c;
            // retrieves the first element in the list
            Car car = carList.First();

            return car;
        }
    }
}