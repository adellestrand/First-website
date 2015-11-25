using System;
using System.ComponentModel;
namespace DriversJournal.Models
{
    /// <summary>Class that represent table Journal in DB</summary>
    public class Journal
    {
        /// <summary> Primary key </summary>
        public int JournalId { get; set; }

        [DisplayName("Traveler companion(s):")]
        public string Travelers { get; set; }

         [DisplayName("Project no:")]
        public string ProjectNumber { get; set; }

        [DisplayName("Odometer at start:")]
        public int OdometerStart { get; set; }

        [DisplayName("Odometer at stop:")]
        public int OdometerEnd { get; set; }

        [DisplayName("Date at start:")]
        public DateTime StartDate { get; set; }

        [DisplayName("Date at stop:")]
        public DateTime EndDate{ get; set; }

        [DisplayName("From:")]
        public string FromDestination { get; set; }

        [DisplayName("To:")]
        public string ToDestination { get; set; }

        [DisplayName("Debit:")]
        public int Debit { get; set; }

        [DisplayName("Km:")]
        public int KmNo { get; set; }

        [DisplayName("Purpose:")]
        public string Purpose { get; set; }

        public int SavedNotSent { get; set; }

        /// <summary> Foreign key references JournalUser(UserId) </summary>
        public int UserId { get; set; }

        public virtual JournalUser JournalUser { get; set; }

        /// <summary> Foreign key references Car(Regno) </summary>
        public string Regno { get; set; }

        public virtual Car Car { get; set; }
    }
}