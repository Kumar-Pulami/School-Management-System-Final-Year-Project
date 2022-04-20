using System.ComponentModel.DataAnnotations;

namespace _19033684_Kumar_Pulami.Models.ViewModel
{
    public class SubjectViewModel
    {
        [Required(ErrorMessage = "Please, Enter Subject ID.")]
        [Display(Name = "Subject ID")]
        public int SubjectID { get; set; }

        [Required(ErrorMessage = "Please, Enter Subject Name.")]
        [Display(Name = "Subject Name")]
        public String? SubjectName { get; set; }

    }
}
