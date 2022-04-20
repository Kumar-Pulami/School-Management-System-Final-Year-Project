using System.ComponentModel.DataAnnotations;

namespace _19033684_Kumar_Pulami.Models.ViewModel.Exam.Result
{
    public class ResultSubjectMarksViewModel
    {
        [Display(Name = "Subject Name")]
        public String? SubjectName { get; set; }

        [Display(Name = "Obtained Marks")]
        public String? ObtainedMarks { get; set; }
    }
}
