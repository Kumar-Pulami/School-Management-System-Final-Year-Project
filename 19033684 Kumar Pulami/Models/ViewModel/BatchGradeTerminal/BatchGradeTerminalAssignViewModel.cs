using System.ComponentModel.DataAnnotations;

namespace _19033684_Kumar_Pulami.Models.ViewModel.BatchGradeTerminal
{
    public class BatchGradeTerminalAssignViewModel
    {
        [Required(ErrorMessage = "Please, Select Batch.")]
        public int Batch { get; set; }

        [Required(ErrorMessage = "Please, Select Grade.")]
        public int Grade { get; set; }

        [Required(ErrorMessage = "Please, Select Subject.")]
        public int TerminalID { get; set; }
    }
}
