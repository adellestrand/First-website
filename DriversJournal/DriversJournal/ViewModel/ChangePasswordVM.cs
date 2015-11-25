using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DriversJournal.ViewModel
{
    /// <summary>
    /// represents data that is posted in the view ChangePassword.cshtml
    /// </summary>
    public class ChangePasswordVM
    {
        public int UserID{ get; set;}
        
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        [StringLength(255, MinimumLength = 8, ErrorMessage = "Your password need to be at least 8 characters long!")]
        [Required(ErrorMessage = "AbCdeF12")]
        public string Password { get; set; }

        [Compare("Password", ErrorMessage = "The passwords doesn't match!")]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Required(ErrorMessage = "Confirm your password")]
        public string ConfirmPassword { get; set; }
    }
}
