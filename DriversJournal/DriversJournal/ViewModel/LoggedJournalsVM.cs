using DriversJournal.Models;
using System.Collections.Generic;
using System.Web.Mvc;

namespace DriversJournal.ViewModel
{
    /// <summary>
    /// represents the Journal-data that is displayed in the view LoggedJournals.cshtml
    /// </summary>
    public class LoggedJournalsVM
    {
        public List<SelectListItem> Years { get; set; }

        public List<SelectListItem> Months { get; set; }

        public string SelectedYear { get; set; }

        public string SelectedMonth { get; set; }

        public List<Journal> Journals { get; set; }
    }
}