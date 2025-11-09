using System.ComponentModel.DataAnnotations;

namespace siguria.Models
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Email-i është i detyruar")]
        [EmailAddress(ErrorMessage = "Ju lutemi vendosni një adresë email të vlefshme")]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Fjalëkalimi është i detyruar")]
        [Display(Name = "Fjalëkalim")]
        public string Password { get; set; }
    }
}