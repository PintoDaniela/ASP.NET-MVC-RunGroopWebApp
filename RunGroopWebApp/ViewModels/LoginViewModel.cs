using System.ComponentModel.DataAnnotations;

namespace RunGroopWebApp.ViewModels
{
    public class LoginViewModel
    {
        [Required (ErrorMessage ="Email address is required.")]
        [Display (Name = "Email Address")]
        public string EmailAddress{ get; set; }
        [Required]
        [DataType (DataType.Password)]
        public string Password{ get; set; }
    }
}
