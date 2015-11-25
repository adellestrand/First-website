using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace DriversJournal.Models
{
    /// <summary>
    /// Context class that are used to query from a database and group together changes that will then be written back to the store as a unit.
    /// </summary>
    public class DriversJournalContext : DbContext
    {
        /// <summary> Object used to query or write to Project-table in DB </summary>
        public DbSet<Project> Projects { get; set; }

        /// <summary> Object used to query or write to JournalUser-table in DB </summary>
        public DbSet<JournalUser> Users { get; set; }

        /// <summary> Object used to query or write to Journal-table in DB </summary>
        public DbSet<Journal> Journals { get; set; }

        /// <summary> Object used to query or write to Role-table in DB </summary>
        public DbSet<Role> Roles { get; set; }

        /// <summary> Object used to query or write to Car-table in DB </summary>
        public DbSet<Car> Cars { get; set; }

        /// <summary> Object used to query or write to Salt-table in DB </summary>
        public DbSet<Salt> Salts{ get; set; }

        /// <summary>
        /// remove the plural in the tablenames
        /// </summary>
        /// <param name="modelBuilder"></param>
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }
    }
}