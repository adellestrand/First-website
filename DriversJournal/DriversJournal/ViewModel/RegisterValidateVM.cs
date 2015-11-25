using System.ComponentModel.DataAnnotations;

namespace DriversJournal.ViewModel
{
    /// <summary>
    /// represents data that is posted in the view Register.cshtml
    /// </summary>
    public class RegisterValidateVM
    {
        [Required(ErrorMessage = "Enter your firstname")]
        [Display(Name = "Firstname")]
        public string Forename { get; set; }

        [Required(ErrorMessage = "Enter your lastame")]
        [Display(Name = "Lastname")]
        public string Surname { get; set; }

        [Required(ErrorMessage = "You must provide your email address!")]
        [Display(Name = "E-Mail address")]
        //[RegularExpression(@"[a-zA-Z0-9._%+-]+@sogeti.+\b(com|no|se)", ErrorMessage="The E-mail address has to be a Sogeti.se/no/com e-mail.")]
        public string Email { get; set; }

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