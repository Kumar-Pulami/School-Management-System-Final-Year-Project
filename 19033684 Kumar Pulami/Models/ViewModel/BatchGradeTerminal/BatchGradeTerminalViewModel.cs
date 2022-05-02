using _19033684_Kumar_Pulami.Models.DatabaseModel.Exam;
using System.ComponentModel.DataAnnotations;

namespace _19033684_Kumar_Pulami.Models.ViewModel.BatchGradeTerminal
{
    public class BatchGradeTerminalViewModel
    {
        [Required(ErrorMessage = "Please, Select Batch.")]
        public int Batch { get; set; }

        [Required(ErrorMessage = "Please, Select Grade.")]
        public int Grade { get; set; }
        public List<TerminalDataDatabaseModel>? TerminalList { get; set; }
    }
}
