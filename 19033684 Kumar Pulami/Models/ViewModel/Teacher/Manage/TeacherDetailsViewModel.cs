using System.ComponentModel.DataAnnotations;

namespace _19033684_Kumar_Pulami.Models.ViewModel.Teacher.Manage
{
    public class TeacherDetailsViewModel
    {
        [Display(Name = "Teacher ID")]
        public String? TeacherID { get; set; }


        [Display(Name = "Teacher Name")]
        public String? TeacherName { get; set; }


        [Display(Name = "Date of Birth")]
        [DataType(DataType.Date)]
        public DateTime DOB { get; set; }


        [Display(Name = "Contact Number")]
        public String? Contact { get; set; }


        public String? Gender { get; set; }

        [Display(Name = "Permament Address")]
        public String? PermamentAddress { get; set; }


        [Display(Name = "Temporary Address")]
        public String? TemporaryAddress { get; set; }


        [Display(Name = "Father Name")]
        public String? FatherName { get; set; }


        [Display(Name = "Mother Name")]
        public String? MotherName { get; set; }

        [Display(Name = "Hire Date")]
        [DataType(DataType.Date)]
        public DateTime? HireDate { get; set; }

        public String? Salary { get; set; }

    }
}
