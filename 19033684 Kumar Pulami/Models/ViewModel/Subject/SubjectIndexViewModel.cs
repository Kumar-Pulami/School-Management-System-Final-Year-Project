using System.ComponentModel.DataAnnotations;

namespace _19033684_Kumar_Pulami.Models.ViewModel.Subject
{
    public class SubjectIndexViewModel
    {
        [Required( ErrorMessage = "Please, Enter Subject Name.")]
        [Display(Name = "Subject Name")]
        public String? SearchValue { get; set; }

        public List<SubjectViewModel>? SubjectList { get; set; }
    }
}
