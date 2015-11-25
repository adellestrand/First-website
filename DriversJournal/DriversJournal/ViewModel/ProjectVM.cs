using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace DriversJournal.ViewModel
{
    /// <summary>
    /// represents the project-data that is displayed in the view Journal.cshtml
    /// </summary>
    public class ProjectVM
    {
        
        public ProjectVM()
        {
            IsActive = true;
        }

        public int ProjectId { get; set; }


        [DisplayName("Project no")]
        [Required(ErrorMessage = "The field Project no is required")]
        public int ProjectNo { get; set; }

        
        public string Name { get; set; }

        [DisplayName("Detail")]
        public string Detail { get; set; }

        [DisplayName("Is active")]
        public bool IsActive { get; set; }

        public int UserId { get; set; }
    }
}