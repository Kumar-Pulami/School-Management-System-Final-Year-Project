using System.ComponentModel.DataAnnotations;

namespace _19033684_Kumar_Pulami.Models.ViewModel.Exam.Marks_Entry
{
    public class MarksEntryMarksDetailViewModel
    {
        public int? SubjectID { get; set; }

        [Required(ErrorMessage = "Please, Enter Obtained Mark.")]
        public String? ObtainedMarks { get; set; }
    }
}
