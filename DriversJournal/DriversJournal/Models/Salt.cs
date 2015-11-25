using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DriversJournal.Models
{
    /// <summary>Class that represent table Journal in DB</summary>
    public class Salt
    {
        /// <summary> Primary key </summary>
        public int SaltId { get; set; }
        public string SaltValue{ get; set; }

        // <summary> Foreign key references JournalUser(UserId) </summary>
        public int UserId{ get; set; } 

        public virtual JournalUser JournalUser { get; set; }

    }
}