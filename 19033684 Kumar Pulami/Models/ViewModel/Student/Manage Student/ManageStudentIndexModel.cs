using System.ComponentModel.DataAnnotations;

namespace _19033684_Kumar_Pulami.Models.ViewModel.Student.Manage_Student
{
    public class ManageStudentListViewModel
    {
        public String? StudentID { get; set; }

        public String? StudentName { get; set; }

        public String? Batch { get; set; }

        public String? Grade { get; set; }

        public String? Section { get; set; }

        public String? Address { get; set; }

        public String? GuardianContact { get; set; }
    }

    public class ManageStudentIndexModel
    {
        [Required(ErrorMessage = "Please, Select Batch")]
        [Range(2000, 3000, ErrorMessage = ("Please, Select Batch"))]
        public int? Batch { get; set; }
        

        [Required(ErrorMessage = "Please, Select Grade")]
        [Range(1, 15, ErrorMessage = ("Please, Select Grade"))]
        public int? Grade { get; set; }

        [Required(ErrorMessage = "Please, Select Section")]
        [MaxLength(2, ErrorMessage = "Please, Select Section")]
        public String? Section { get; set; }
        public List<ManageStudentListViewModel>? ListModel { get; set; }
    }
}
