

using System.ComponentModel;

namespace DriversJournal.Models
{
    /// <summary>Class that represent table Project in DB</summary>
    public class Project
    {
        /// <summary> Primary key </summary>
        public int ProjectId { get; set; }

        [DisplayName("Project no")]
        public int ProjectNo { get; set; }

        public string Name { get; set; }

        public string Detail { get; set; }
        public int Active { get; set; }

        /// <summary> Foreign key references JournalUser(UserId) </summary>
        public int UserId { get; set; }
        
        public virtual JournalUser JournalUser { get; set; }

       
    }
}