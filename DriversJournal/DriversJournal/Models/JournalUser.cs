using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Security.AccessControl;

namespace DriversJournal.Models
{
    /// <summary>Class that represent table JournalUser in DB</summary>
    public class JournalUser
    {
        /// <summary> Primary key </summary>
        [Key]
        public int UserId { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        /// <summary> Foreign key references Role(RoleId) </summary>
        public int RoleId { get; set; }

        public virtual Role Role { get; set; }
        public short AccountConfirmed { get; set; }

        public virtual List<Project> Projects { get; set; } 
        
        public virtual List<Journal> Journals { get; set; }
        public virtual List<Salt> Salts { get; set; }
    }
}