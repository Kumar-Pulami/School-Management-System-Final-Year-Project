using System.ComponentModel.DataAnnotations;

namespace _19033684_Kumar_Pulami.Models.DatabaseModel.Exam
{
    public class TerminalDataDatabaseModel
    {
        [Display (Name = "Terminal ID")]
        public int TerminalID { get; set; }

        [Display(Name = "Terminal Name")]
        public String? TerminalName { get; set; }
    }
}
