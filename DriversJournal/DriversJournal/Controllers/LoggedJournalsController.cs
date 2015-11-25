using DriversJournal.Models;
using DriversJournal.Services;
using DriversJournal.ViewModel;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using System.Web.Security;

namespace DriversJournal.Controllers
{
    /// <summary>
    /// Controller for LoggedJournals.cshtml </summary>
    public class LoggedJournalsController : Controller
    {
        /// <summary>Object used to call methods that retrives data from DB </summary>
        private GetDataFromDb service = new GetDataFromDb();

        /// <summary> Object used to insert, update, delete and read from database</summary>
        private DriversJournalContext db = new DriversJournalContext();

        private string selectedYear = DateTime.Now.Year.ToString();
        private string selectedMonth = DateTime.Now.Month.ToString("D2");

        // GET: LoggedJournals
        /// <summary>
        /// ActionResult for displaying journals.
        /// </summary>
        public ActionResult LoggedJournals()
        {
            if (getSessionState() != true)
            {
                return RedirectToAction("LoggedIn", "Home");
            }
            else
            {
                JournalUser user = (JournalUser)Session["SessionUser"];

                LoggedJournalsVM vm = new LoggedJournalsVM
                {
                    Years = new List<SelectListItem>(service.GetJournalYears()
                        .Select(x => new SelectListItem()
                        {
                            Value = x.ToString(),
                            Text = x.ToString()
                        })
                    ),
                    Months = new List<SelectListItem>(DateTimeFormatInfo.InvariantInfo.MonthNames
                        //.Where to make sure there is only 12 months rather then the 13 that is returned.
                        .Where(m => !String.IsNullOrEmpty(m))
                        .Select((monthName, index) => new SelectListItem()
                        {
                            Value = monthName,
                            Text = (index + 1).ToString("D2")
                        })
                    ),
                    SelectedYear = selectedYear,
                    SelectedMonth = selectedMonth,
                    Journals = service.GetJournals(user.UserId, user.RoleId, Int32.Parse(DateTime.Now.Year.ToString()), Int32.Parse(DateTime.Now.Month.ToString("D2")))
                };
                return View(vm);
            }
        }

        /// <summary>
        /// ActionResult for displaying journals.
        /// </summary>
        /// <param name="model">Viewmodel of LoggedJournalVm</param>
        [HttpPost]
        public ActionResult LoggedJournals(DriversJournal.ViewModel.LoggedJournalsVM model)
        {
            if (getSessionState() != true)
            {
                return RedirectToAction("LoggedIn", "Home");
            }
            else
            {
                JournalUser user = (JournalUser)Session["SessionUser"];

                selectedYear = model.SelectedYear;
                selectedMonth = model.SelectedMonth;

                LoggedJournalsVM vm = new LoggedJournalsVM
                {
                    Years = new List<SelectListItem>(service.GetJournalYears()
                        .Select(x => new SelectListItem()
                        {
                            Value = x.ToString(),
                            Text = x.ToString()
                        })
                    ),
                    Months = new List<SelectListItem>(DateTimeFormatInfo.InvariantInfo.MonthNames
                        //.Where to make sure there is only 12 months rather then the 13 that is returned.
                        .Where(m => !String.IsNullOrEmpty(m))
                        .Select((monthName, index) => new SelectListItem()
                        {
                            Value = monthName,
                            Text = (index + 1).ToString("D2")
                        })
                    ),
                    SelectedYear = selectedYear,
                    SelectedMonth = selectedMonth,
                    Journals = service.GetJournals(user.UserId, user.RoleId, Int32.Parse(model.SelectedYear), Int32.Parse(model.SelectedMonth))
                };
                return View(vm);
            }
        }

        /// <summary>
        /// ActionResult for calling method to create an excel arc of the journals.
        /// </summary>
        /// <param name="model">Viewmodel of LoggedJournalVm</param>
        [HttpPost]
        public ActionResult Excel(DriversJournal.ViewModel.LoggedJournalsVM model)
        {
            JournalUser user = (JournalUser)Session["SessionUser"];
            DriversJournal.Services.Excel.ListToExcel(service.GetJournals(user.UserId, user.RoleId, Int32.Parse(model.SelectedYear), Int32.Parse(model.SelectedMonth)),
                model.SelectedMonth, model.SelectedYear);
            return RedirectToAction("LoggedJournals");
        }

        // GET: Journal/Edit/5
        /// <summary>
        /// ActionResult for calling edit on a journal.
        /// </summary>
        /// <param name="id">Id of an journal</param>
        public ActionResult Edit(int? id)
        {

            if (getSessionState() != true)
            {
                return RedirectToAction("LoggedIn", "Home");
            }
            else
            {


                //first check if the id is sent or not
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Journal journal = service.FindSingleJournal(id);
                if (journal == null)
                {
                    return HttpNotFound();
                }
                else
                {
                    LoggedJournalEditVm vm = service.GetLoggedJournalVm(journal); //get viewmodel 

                    ViewBag.UserId = new SelectList(db.Users, "UserId", "Email", journal.UserId);
                    return View(vm);

                }
            }
        }

        /// <summary>
        /// ActionResult for editing a journal.
        /// </summary>
        /// <param name="vm">Viewmodel of LoggedJournalEditVm</param>
        /// <param name="car">Car object linked to this journal</param>
        /// <param name="debit">Value of this journals debit</param>
        [HttpPost]
        public ActionResult Edit(LoggedJournalEditVm vm, string car, string debit)
        {


            Journal j = new Journal();
            if(ModelState.IsValid)
            {
                // retrives the car from db
                var dbcar = GetCar();
                //building journal object, values is from the posted form
                j = new Journal
                {
                    
                    JournalId = vm.JournalId,
                    Travelers = vm.Travelers,
                    ProjectNumber = vm.ProjectNumber,
                    OdometerStart = vm.OdometerStart,
                    OdometerEnd = vm.OdometerEnd,
                    StartDate = Convert.ToDateTime(vm.StartDate),
                    EndDate = Convert.ToDateTime(vm.EndDate),
                    FromDestination = vm.FromDestination,
                    ToDestination = vm.ToDestination,
                    Debit = Convert.ToInt16(debit),
                    KmNo = vm.KmNo,
                    Purpose = vm.Purpose,
                    SavedNotSent = vm.SavedNotSent,
                    UserId = vm.UserId,
                    Regno = dbcar.Regno,
                };
                db.Entry(j).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("LoggedJournals");
            }
            //ViewBag.UserId = new SelectList(db.Users, "UserId", "Email", journal.UserId);
            LoggedJournalEditVm vm1 = service.GetLoggedJournalVm(j);
            return View(vm1);
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
            // hämtar ud det första elementet i listan
            Car car = carList.First();

            return car;
        }

    }



}