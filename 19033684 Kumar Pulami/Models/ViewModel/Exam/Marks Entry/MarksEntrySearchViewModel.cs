using System.ComponentModel.DataAnnotations;

namespace _19033684_Kumar_Pulami.Models.ViewModel.Exam.Marks_Entry
{
    public class MarksEntrySearchViewModel
    {   
        [Required(ErrorMessage = "Please, Select Batch.")]
        public int? Batch { get; set; }

        [Required(ErrorMessage = "Please, Select Grade.")]
        public int? Grade { get; set; } = null;

        [Required(ErrorMessage = "Please, Select Section.")]
        [StringLength(maximumLength: 2, MinimumLength = 1, ErrorMessage =("Please, Select Section."))]
        public String Section { get; set; }

        [Required(ErrorMessage = "Please, Select Terminal.")]
        [Range(maximum:10, minimum:1, ErrorMessage = "Please, Select Terminal.")]
        public int? Terminal { get; set; }
    }
}
