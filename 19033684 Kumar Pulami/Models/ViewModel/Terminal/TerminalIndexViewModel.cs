using System.ComponentModel.DataAnnotations;

namespace _19033684_Kumar_Pulami.Models.ViewModel.Terminal
{
    public class TerminalIndexViewModel
    {
        [Required(ErrorMessage = "Please, Enter Terminal Name.")]
        [Display(Name = "Terminal Name")]
        public String? SearchValue { get; set; }

        public List<TerminalViewModel>? TerminalList { get; set; }
    }
}
