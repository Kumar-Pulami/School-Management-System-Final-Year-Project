using System.ComponentModel.DataAnnotations;

namespace _19033684_Kumar_Pulami.Models.ViewModel.Exam.Result
{
    public class ResultStudentMarksDetailIndexViewModel
    {
        [Required(ErrorMessage = "Please, Select Batch")]
        [Range(2000, 3000, ErrorMessage = ("Please, Select Batch"))]
        public int? Batch { get; set; }

        [Required(ErrorMessage = "Please, Select Grade")]
        [Range(1, 15, ErrorMessage = ("Please, Select Grade"))]
        public int? Grade { get; set; }

        [Required(ErrorMessage = "Please, Select Terminal")]
        [Range(1, 5, ErrorMessage = ("Please, Select Terminal"))]
        public int? Terminal { get; set; }
        public String? TerminalName { get; set; }
        public List<ResultSubjectName>? SubjectNames { get; set; }
        public List<ResultStudentMarksDetailViewModel>? StudentResultList { get; set; }
    }
}
