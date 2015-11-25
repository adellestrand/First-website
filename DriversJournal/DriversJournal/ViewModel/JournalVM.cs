using System.Collections.Generic;
using System.ComponentModel;
using System.Web.Mvc;

namespace DriversJournal.ViewModel
{
    /// <summary>
    /// represents the Journal-data that is displayed in the view Journal.cshtml
    /// </summary>
    public class JournalVM
    {
        [DisplayName("Odometer at start:")]
        public int OdometerStart { get; set; }

        [DisplayName("Odometer at stop:")]
        public string OdometerEnd { get; set; }

        [DisplayName("From:")]
        public string From { get; set; }

        [DisplayName("To:")]
        public string To { get; set; }

        [DisplayName("Purpose:")]
        public string Purpose { get; set; }

        public string RegNo { get; set; }

        public int JournalId { get; set; }

        [DisplayName("Date at start:")]
        public string StartDate { get; set; }

        [DisplayName("Date at stop:")]
        public string EndDate { get; set; }

        [DisplayName("Traveler companion(s):")]
        public string Travelers { get; set; }

        [DisplayName("Project no:")]
        public List<SelectListItem> Projects { get; set; }

        [DisplayName("Car:")]
        public List<SelectListItem> Cars { get; set; }

        public List<SelectListItem> Debits { get; set; }
    }
}