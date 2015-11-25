using DriversJournal.Models;
using DriversJournal.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace DriversJournal.Services
{
    /// <summary>
    /// Class that contains methods thats retrieve data from DB and assembles viewmodels
    /// </summary>
    public class GetDataFromDb
    {
        /// <summary> Object used to insert, update, delete and read from database</summary>
        private DriversJournalContext db = new DriversJournalContext();

        /// <summary>
        /// Get car odometer based on registration number, if the car can't be found the method return 0
        /// </summary>
        /// <param name="regNo">Car to get odometervalue from</param>
        /// <returns>Car odometer</returns>
        public int CarOdometer(string regNo)
        {
            var car = db.Cars.Find(regNo);
            if (car != null)
            {
                return car.Odometer;
            }
            else
            {
                
                return 0;
            }
        }

        /// <summary>
        /// Method for bulk updating the journal
        /// </summary>
        /// <param name="journal">Journal to be updated</param>
        public void UpdateJournal(Journal journal)
        {
            db.Journals.Attach(journal);

            var entry = db.Entry(journal);
            entry.State = System.Data.Entity.EntityState.Modified;

            //entry.Property(e => e.JournalId).IsModified = false; //id will never be changed

            db.SaveChanges();
        }

        /// <summary>
        /// method to find a single journal. Used in editing. 
        /// </summary>
        /// <param name="id">Journal to be found</param>
        /// <returns>Journal</returns>
        public Journal FindSingleJournal(int? id)
        {
            Journal journal = new Journal();

            journal = db.Journals.Find(id);

            return journal;
        }

        /// <summary>
        /// Method that retrieve journals,
        /// </summary>
        /// <param name="userid">Get journal with userid</param>
        /// <param name="roleid">rolid 1 == admin, show all journals for all users</param>
        /// <param name="year">Get journal based on month</param>
        /// <param name="month">Get journal based on year</param>
        /// <returns></returns>
        public List<Journal> GetJournals(int userid, int roleid, int year, int month)
        {
            if (roleid == 1) //Admin
            {
                var journal = db.Journals.Where(r => r.StartDate.Year == year && r.StartDate.Month == month && r.SavedNotSent == 0).ToList();
                return journal;
            }
            else if (roleid == 2) //User
            {
                var journal = db.Journals.Where(r => r.UserId == userid && r.StartDate.Year == year && r.StartDate.Month == month && r.SavedNotSent == 0).ToList();
                return journal;
            }
            return null;
        }

        
        public List<int> GetJournalYears()
        {
            var years = db.Journals.Select(r => r.StartDate.Year).Distinct().ToList();
            return years;
        }

        /// <summary>
        /// Return debit items, with Item No as selected
        /// </summary>
        /// <returns>list contains "No", "Yes"</returns>
        public List<SelectListItem> GetDebit()
        {
            List<SelectListItem> debits = new List<SelectListItem>();

            debits.Add(new SelectListItem { Text = "No", Selected = true, Value = "0" });
            debits.Add(new SelectListItem { Text = "Yes", Value = "1" });

            return debits;
        }

        /// <summary>
        /// Return debit items and depending on debit param sets the preselected
        /// </summary>
        /// <param name="debit">set preselected, 0 = No</param>
        /// <returns>list contains "No", "Yes"</returns>
        public List<SelectListItem> GetDebit(int debit)
        {
            List<SelectListItem> debits = new List<SelectListItem>();

            // if debit == 0, set "No" selected
            if (debit == 0)
            {
                debits.Add(new SelectListItem { Text = "No", Selected = true, Value = "0" });
                debits.Add(new SelectListItem { Text = "Yes", Value = "1" });
            }
            else
            {
                debits.Add(new SelectListItem { Text = "Yes", Selected = true, Value = "1" });
                debits.Add(new SelectListItem { Text = "No", Value = "0" });
            }

            return debits;
        }

        /// <summary>
        /// Returns cars items, for selectlist
        /// </summary>
        /// <param name="regNo">preselected item</param>
        /// <returns>list of cars</returns>
        public List<SelectListItem> GetCars(String regNo)
        {
            List<SelectListItem> cars = new List<SelectListItem>();

            foreach (var car in db.Cars.ToList())
            {
                //sets the item to selected
                if (regNo.Equals(car.Regno))
                {
                    cars.Add(new SelectListItem
                    {
                        Text = car.Regno,
                        Selected = true,
                        Value = car.Regno
                    });
                }
                else
                {
                    cars.Add(new SelectListItem
                    {
                        Text = car.Regno,
                        Value = car.Regno
                    });
                }
            }
            return cars;
        }

        /// <summary>
        /// Return car items for selectlist
        /// </summary>
        /// <returns>Cars</returns>
        public List<SelectListItem> GetCars()
        {
            List<SelectListItem> cars = new List<SelectListItem>();

            foreach (var car in db.Cars.ToList())
            {
                cars.Add(new SelectListItem
                {
                    Text = car.Regno,
                    Selected = true,
                    Value = car.Regno
                });
            }

            return cars;
        }

        /// <summary>
        /// Returns projects items depending on logged JournalUser
        /// </summary>
        /// <param name="userId">get projects for userId</param>
        /// <returns>projects</returns>
        public List<SelectListItem> GetProjects(int userId)
        {
            // retrieve active projects depending on userId
            var userProjects = from u in db.Projects
                               where (u.UserId == userId) && (u.Active == 1)
                               select u;

            List<SelectListItem> projects = new List<SelectListItem>();

            //loop throug userprojects and add them to Projects<SelectItem>
            foreach (var userProject in userProjects)
            {
                string projectNo = userProject.ProjectNo.ToString();

                // adds text and value to the selectListItem
                projects.Add(new SelectListItem
                {
                    Text = projectNo + " - " + userProject.Name,
                    Value = projectNo
                });
            }

            return projects;
        }

        /// <summary>
        /// Returns project dependeing on userID, and set saved project to selected
        /// </summary>
        /// <param name="userId">Return projects depening on userId</param>
        /// <param name="savedProjectNo">set this project to selected</param>
        /// <returns>List of projects</returns>
        public List<SelectListItem> GetProjects(int userId, string savedProjectNo)
        {
            // retrieve active projects depending on userId
            var userProjects = from u in db.Projects
                               where (u.UserId == userId) && (u.Active == 1)
                               select u;

            List<SelectListItem> projects = new List<SelectListItem>();

            //loop throug userprojects and add them to Projects<SelectItem>
            foreach (var userProject in userProjects)
            {
                string projectNo = userProject.ProjectNo.ToString();

                // if the saved projectNo is equal to project.ProjectNo set this item to selected
                if (savedProjectNo.Equals(projectNo))
                {
                    // adds text and value to the selectListItem and set it to selected
                    projects.Add(new SelectListItem
                    {
                        Text = projectNo + " - " + userProject.Name,
                        Selected = true,
                        Value = projectNo
                    });
                }
                else
                {
                    // adds text and value to the selectListItem
                    projects.Add(new SelectListItem
                    {
                        Text = projectNo + " - " + userProject.Name,
                        Value = projectNo
                    });
                }
            }

            return projects;
        }

        /// <summary>
        /// assembles a JournalVM
        /// </summary>
        /// <param name="journal">Saved journal</param>
        /// <returns>Journal-viewmodel</returns>
        public JournalVM GetSavedJournal(Journal journal)
        {
            string odometerEnd;

            if (journal.OdometerEnd == 0)
            {
                odometerEnd = "";
            }
            else
            {
                odometerEnd = journal.OdometerEnd.ToString();
            }

            JournalVM vm = new JournalVM
            {
                Travelers = journal.Travelers,
                Projects = GetProjects(journal.UserId, journal.ProjectNumber),
                OdometerStart = journal.OdometerStart,
                OdometerEnd = odometerEnd,
                From = journal.FromDestination,
                To = journal.ToDestination,
                Purpose = journal.Purpose,
                Cars = GetCars(journal.Regno),
                Debits = GetDebit(journal.Debit),
                JournalId = journal.JournalId,
                StartDate = journal.StartDate.ToString("yyyy-MM-dd"),
                EndDate = journal.EndDate.ToString("yyyy-MM-dd")
            };
            return vm;
        }


        /// <summary>
        /// Building LoggedJournalEditVm object
        /// </summary>
        /// <param name="journal">values to be inserted in the GetLoggedJournalVm object  </param>
        /// <returns>LoggedJournal-viewmodel</returns>
        public ViewModel.LoggedJournalEditVm GetLoggedJournalVm(Models.Journal journal)
        {
            LoggedJournalEditVm vm = new LoggedJournalEditVm
            {
                JournalId = journal.JournalId,
                Travelers = journal.Travelers,
                ProjectNumber = journal.ProjectNumber,
                OdometerStart = journal.OdometerStart,
                OdometerEnd = journal.OdometerEnd,
                StartDate = journal.StartDate.ToString("yyyy-MM-dd"),
                EndDate = journal.EndDate.ToString("yyyy-MM-dd"),
                FromDestination = journal.FromDestination,
                ToDestination = journal.ToDestination,
                Debits = GetDebit(journal.Debit),
                KmNo = journal.KmNo,
                Purpose = journal.Purpose,
                
            };

            return vm;
        }


       
    }
}