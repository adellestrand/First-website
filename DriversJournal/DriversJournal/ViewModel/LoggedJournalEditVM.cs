using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using DriversJournal.Models;
using System.Web.Mvc;

namespace DriversJournal.ViewModel
{
    /// <summary>
    /// represents the Journal-data that is displayed in the Journal edit-view Edit.cshtml
    /// </summary>
    public class LoggedJournalEditVm
    {
        public int JournalId { get; set; }

        [DisplayName("Traveler companion(s):")]
        public string Travelers { get; set; }


        [DisplayName("Project no:")]
        [StringLength(15, ErrorMessage = "Please enter no more than 15 digits")]
        [RegularExpression(@"[0-9]*\.?[0-9]+", ErrorMessage = "Project no must be a number.")]
        public string ProjectNumber { get; set; }


        [DisplayName("Odometer at start:")]
        [RegularExpression(@"^\d{0,9}.", ErrorMessage = "Please enter no more than 10 digits")]
        [Required(ErrorMessage = "The field Odometer at stop is required")]
        public int OdometerStart { get; set; }

        
        [DisplayName("Odometer at stop:")]
        [RegularExpression(@"^\d{0,9}.", ErrorMessage = "Please enter no more than 10 digits")]
        [Required(ErrorMessage = "The field Odometer at start is required")]
        public int OdometerEnd { get; set; }
         

        [DisplayName("Date at start:")]
        [Required(ErrorMessage = "Please enter a date in the format yyyy-mm-dd")]
        [RegularExpression(@"^(19|20)\d\d-(0\d|1[012])-(0\d|1\d|2\d|3[01])$", ErrorMessage = "Please enter a date in the format yyyy-mm-dd")]
        public string StartDate { get; set; }


        [DisplayName("Date at stop:")]
        [Required(ErrorMessage = "Please enter a date in the format yyyy-mm-dd")]
        [RegularExpression(@"^(19|20)\d\d-(0\d|1[012])-(0\d|1\d|2\d|3[01])$", ErrorMessage = "Please enter a date in the format yyyy-mm-dd")]
        public string EndDate { get; set; }


        [DisplayName("From:")]
        public string FromDestination { get; set; }

        [DisplayName("To:")]
        public string ToDestination { get; set; }

        [DisplayName("Debit:")]
        public List<SelectListItem> Debits { get; set; }


        [DisplayName("Km:")]
        [RegularExpression(@"^\d{0,9}.", ErrorMessage = "Please enter no more than 10 digits")]
        [Required(ErrorMessage = "The field Km is required")]
        public int KmNo { get; set; }

        [DisplayName("Purpose:")]
        public string Purpose { get; set; }

        



        public int SavedNotSent { get; set; }

        public int UserId { get; set; }

        

        public string Regno { get; set; }

        

    }
}