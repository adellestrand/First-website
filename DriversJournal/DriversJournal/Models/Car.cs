using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DriversJournal.Models
{
    /// <summary>Class that represent table Car in DB</summary>
    public class Car
    {
        /// <summary> Primary key </summary>
        [Key]
        public string Regno { get; set; }

        public int Odometer { get; set; }

        public virtual List<Journal> Journals { get; set; }
    }
}