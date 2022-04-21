using System.ComponentModel.DataAnnotations;

namespace _19033684_Kumar_Pulami.Models.ViewModel.BatchGradeSubject
{
    public class ClassSubjectAssignViewModel
    {
        [Required(ErrorMessage = "Please, Select Batch.")]
        public int Batch { get; set; }

        [Required(ErrorMessage = "Please, Select Grade.")]
        public int Grade { get; set; }

        [Required(ErrorMessage = "Please, Select Subject.")]
        public int SubjectID { get; set; }
    }
}


