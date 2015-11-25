using System.Collections.Generic;

namespace DriversJournal.Models
{
    /// <summary>Class that represent table Role in DB</summary>
    public class Role
    {
        /// <summary> Primary key </summary>
        public int RoleId { get; set; }

        public string Name { get; set; }

        public virtual List<JournalUser> Users { get; set; }
    }
}