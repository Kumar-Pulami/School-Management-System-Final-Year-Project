namespace _19033684_Kumar_Pulami.Models.ViewModel.Exam.Marks_Entry.PostBack
{
    public class PostBackMarksEntryViewModel
    {
        public int? Batch { get; set; }
        public int? Grade { get; set; }
        public String Section { get; set; }
        public int? Terminal { get; set; }
        public List<PostBackMarksEntryStudentDetailsViewModel>? StudentList { get; set; }
    }
}
