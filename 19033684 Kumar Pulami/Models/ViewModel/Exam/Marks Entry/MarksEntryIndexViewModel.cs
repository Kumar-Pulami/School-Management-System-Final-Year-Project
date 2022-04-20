namespace _19033684_Kumar_Pulami.Models.ViewModel.Exam.Marks_Entry
{
    public class MarksEntryIndexViewModel
    {
        public int? Batch { get; set; }
        public int? Grade { get; set; }
        public String? Section { get; set; }
        public int? Terminal { get; set; }
        public List<MarksEntrySubjectViewModel>? SubjectDetails { get; set; }
        public List<MarksEntryStudentDetailsViewModel>? StudentDetails { get; set; }
        public List<TotalMarksViewModel>? TotalMarks { get; set; }
        public List<PassMarksViewModel>? PassMarks { get; set; }
    }
}
