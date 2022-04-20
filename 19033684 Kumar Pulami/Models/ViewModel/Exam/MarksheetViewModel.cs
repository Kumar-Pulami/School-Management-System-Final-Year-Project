using _19033684_Kumar_Pulami.Models.DatabaseModel.Exam;
using System.ComponentModel.DataAnnotations;

namespace _19033684_Kumar_Pulami.Models.ViewModel.Exam
{

    public class MarksheetSubjectViewModel
    {
        [Display(Name = "Subject ID")]
        public int SubjectID { get; set; }

        [Display(Name = "Subject Name")]
        public string? SubjectName { get; set; }

        [Display(Name = "Obtained Mark")]
        public String? ObtainedMark { get; set; }

        [Display(Name = "Total Mark")]
        public float? TotalMark { get; set; }

        [Display(Name = "Pass Mark")]
        public float? PassMark { get; set; }

        [Display(Name = "Grade")]
        public string? Grade { get; set; }

        [Display(Name = "Grade Point")]
        public float? GradePoint { get; set; }

        public String? Remarks { get; set; }


    }

    public class MarksheetViewModel
    {
        public String? StudentID { get; set; }

        public String? StudentName { get; set; }

        [DataType(DataType.Date)]
        public DateTime? DOB { get; set; }

        public String? FatherName { get; set; }

        public String? Grade { get; set; }

        public String? Batch { get; set; }

        public String? Section { get; set; }

        public float? GPA { get; set; }

        public float? Precentage { get; set; }

        public String? Terminal { get; set; }  
        
        public List<TerminalDataDatabaseModel>? TerminalList { get; set; }

        public List<MarksheetSubjectViewModel>? SubjectMarks { get; set; }

        public float? TotalMarks { get; set; }
        public float? TotalObtainedMarks { get; set; }
    }
}
