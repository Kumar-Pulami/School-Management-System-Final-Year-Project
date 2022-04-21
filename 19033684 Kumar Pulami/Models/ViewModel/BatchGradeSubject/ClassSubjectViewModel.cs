using System.ComponentModel.DataAnnotations;

namespace _19033684_Kumar_Pulami.Models.ViewModel
{
    public class ClassSubjectViewModel
    {
        [Required(ErrorMessage = "Please, Select Batch.")]
        public int Batch { get; set; }

        [Required(ErrorMessage = "Please, Select Grade.")]
        public int Grade { get; set; }
        public List<SubjectViewModel>? SubjectList { get; set; }
    }
}
