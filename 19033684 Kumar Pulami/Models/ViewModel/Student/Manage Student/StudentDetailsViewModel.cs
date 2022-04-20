using System.ComponentModel.DataAnnotations;

namespace _19033684_Kumar_Pulami.Models.ViewModel.Student.Manage_Student
{
    public class StudentDetailsViewModel
    {

        [Display(Name = "Student ID")]
        public String? StudentID { get; set; }


        [Display(Name = "Student Name")]
        public String? StudentName { get; set; }


        [Display(Name = "Date of Birth")]
        [DataType(DataType.Date)]
        public DateTime DOB { get; set; }


        [Display(Name = "Guardian Contact")]
        public String? GuardianContact { get; set; }


        public String? Gender { get; set; }

        [Display(Name = "Permament Address")]
        public String? PermamentAddress{ get; set; }


        [Display(Name = "Temporary Address")]
        public String? TemporaryAddress { get; set; }


        [Display(Name = "Father Name")]
        public String? FatherName { get; set; }


        [Display(Name = "Mother Name")]
        public String? MotherName { get; set; }


        public String? Batch { get; set; }


        public String? Grade { get; set; }


        public String? Section { get; set; }


        [Display(Name = "Previous School Name")]
        public String? PreviousSchoolName { get; set; }


        [Display(Name = "Previous School Grade")]
        public String? PrevioiusSchoolGrade { get; set; }
    }
}
