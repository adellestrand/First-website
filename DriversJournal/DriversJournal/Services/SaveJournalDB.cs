using DriversJournal.Models;
using System;
using System.Linq;

namespace DriversJournal.Services
{
    /// <summary>
    /// Class that contains methods thats saves data to DB and assembles viewmodels
    /// </summary>
    public class SaveJournalDB
    {
        /// <summary> Object used to insert, update, delete and read from database</summary>
        private DriversJournalContext db = new DriversJournalContext();

        /// <summary>
        /// Saves a journal to DB, this journal is just saved and not completed.
        /// SavedNotSent == 1
        /// </summary>
        /// <param name="vm">create journal based on vm values</param>
        /// <param name="project">project to be stored in journal</param>
        /// <param name="userId">userId to be stored in journal</param>
        /// <param name="debit">debit to be stored in journal</param>
        /// <param name="regno">regno to be stored in journal</param>
        public void SaveJournal(ViewModel.JournalVM vm, string project, int userId, string debit, string regno)
        {
            
            if (vm.EndDate == null)
            {
                vm.EndDate = DateTime.Now.ToString("yyyy-MM-dd");
            }
           

            //converts string to int
            int odometerEnd;
            Int32.TryParse(vm.OdometerEnd, out odometerEnd);

            Journal journal = new Journal
            {
                UserId = userId,
                Travelers = vm.Travelers,
                ProjectNumber = project,
                OdometerStart = vm.OdometerStart,
                OdometerEnd = odometerEnd,
                StartDate = Convert.ToDateTime(vm.StartDate),
                EndDate = Convert.ToDateTime(vm.EndDate),
                FromDestination = vm.From,
                ToDestination = vm.To,
                Debit = Convert.ToInt16(debit),
                Purpose = vm.Purpose,
                Regno = regno,
                SavedNotSent = 1 // when saved == 1
            };

            //if a journal alredi exist, we want to update it
            // retrive the journal with column SavedNotSent == 1 depening on user
            var listJournals = from u in db.Users
                               join j in db.Journals
                                   on u.UserId equals j.UserId
                               where (u.UserId == userId) && (j.SavedNotSent == 1)
                               select j;

            if (listJournals.Any())
            {
                var originalJournal = listJournals.First();
                journal.JournalId = originalJournal.JournalId;

                db.Entry(originalJournal).CurrentValues.SetValues(journal);
                db.SaveChanges();
            }
            else
            {
                //saves journal to db
                db.Journals.Add(journal);
                db.SaveChanges();
            }
        }

        /// <summary>
        /// Saves a completed journal to DB, SavedNotSent == 0
        /// </summary>
        /// <param name="vm">create journal based on vm values</param>
        /// <param name="project">project to be stored in journal</param>
        /// <param name="userId">userId to be stored in journal</param>
        /// <param name="debit">debit to be stored in journal</param>
        /// <param name="regno">regno to be stored in journal</param>
        public void SendJournal(ViewModel.JournalVM vm, string project, int userId, string debit, string regno)
        {
            //converts string to int
            int odometerEnd;
            Int32.TryParse(vm.OdometerEnd, out odometerEnd);

            if (vm.EndDate == null)
            {
                vm.EndDate = DateTime.Now.ToString("yyyy-MM-dd");
            }

            Journal journal = new Journal
            {
                UserId = userId,
                Travelers = vm.Travelers,
                ProjectNumber = project,
                OdometerStart = vm.OdometerStart,
                OdometerEnd = odometerEnd,
                StartDate = Convert.ToDateTime(vm.StartDate),
                EndDate = Convert.ToDateTime(vm.EndDate),
                FromDestination = vm.From,
                ToDestination = vm.To,
                Debit = Convert.ToInt16(debit),
                KmNo = odometerEnd - vm.OdometerStart,
                Purpose = vm.Purpose,
                Regno = regno,
                SavedNotSent = 0 // when send == 1
            };

            //if a journal alredi exist, we want to update it
            // retrive the journal with column SavedNotSent == 1 depening on user
            var listJournals = from u in db.Users
                               join j in db.Journals
                                   on u.UserId equals j.UserId
                               where (u.UserId == userId) && (j.SavedNotSent == 1)//hardcoded user
                               select j;

            if (listJournals.Any())
            {
                var originalJournal = listJournals.First();
                journal.JournalId = originalJournal.JournalId;

                db.Entry(originalJournal).CurrentValues.SetValues(journal);
                db.SaveChanges();/////TEST
            }
            else
            {
                //saves journal to db
                db.Journals.Add(journal);
            }

            //Get car from db
            var car = db.Cars.Single(c => c.Regno == regno);

            car.Odometer = odometerEnd;// update the Car Odometer value
            db.SaveChanges();
        }
    }
}