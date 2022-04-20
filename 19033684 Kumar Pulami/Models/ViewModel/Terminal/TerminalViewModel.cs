using System.ComponentModel.DataAnnotations;

namespace _19033684_Kumar_Pulami.Models.ViewModel.Terminal
{
    public class TerminalViewModel
    {
        [Required(ErrorMessage = "Please, Enter Terminal ID.")]
        [Display(Name = "Terminal ID")]
        public int TerminalID { get; set; }

        [Required(ErrorMessage = "Please, Enter Terminal Name.")]
        [Display(Name = "Terminal Name")]
        public String? TerminalName { get; set; }
    }
}
