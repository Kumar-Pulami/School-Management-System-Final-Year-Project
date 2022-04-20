namespace _19033684_Kumar_Pulami.Models.ViewModel.Exam.Result
{
    public class MarksDetailsDBModel
    {
        public int? SubjectID { get; set; }
        public float? TotalMark { get; set; }
        public float? PassMark { get; set; }
        public String? ObtainedMark { get; set; }
    }

    public class StudentMarksDetailDBModel
    {
        public String? StudentID { get; set; }
        public List<MarksDetailsDBModel>? MarksDetails { get; set; }

    }
}
