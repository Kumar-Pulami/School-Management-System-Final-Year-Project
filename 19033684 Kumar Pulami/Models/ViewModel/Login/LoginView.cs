using System.ComponentModel.DataAnnotations;

namespace _19033684_Kumar_Pulami.Models.ViewModel.Login
{
    public class LoginView
    {
        [Display(Name = "User Name")]
        [Required(ErrorMessage = "Please, provide the User Name.")]
        public String UserName { get; set; }


        [Required(ErrorMessage = "Please, provide the Password.")]
        public String Password { get; set; }
    }
}
