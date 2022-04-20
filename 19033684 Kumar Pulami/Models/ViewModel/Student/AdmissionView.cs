using System.ComponentModel.DataAnnotations;

namespace _19033684_Kumar_Pulami.Models.ViewModel.Student
{
    public class AdmissionView
    {
        [Display(Name = "Student Name")]
        [Required(ErrorMessage = "Please, enter the student name.")]
        public string? Name { get; set; }


        [Display(Name = "Date of Birth")]
        [Required(ErrorMessage = "Please, select the Date of Birth")]
        [DataType(DataType.Date)]
        public DateTime BOD { get; set; }

        [Required(ErrorMessage = "Please, select the gender.")]
        public String? Gender { get; set; }


        [Required(ErrorMessage = "Please, enter the Batch.")]
        //[Range(2000, 3000, ErrorMessageResourceName = "Invalid Batch Number")]
        public int? Batch { get; set; }

        [Required(ErrorMessage = "Please, enter the garde.")]
        //[Range(1, 12, ErrorMessageResourceName = "Invalid Batch Number")]
        public int? Grade { get; set; }

        [Required(ErrorMessage = "Please, enter the section")]
        //[StringLength(2, MinimumLength = 1, ErrorMessageResourceName = "Invalid Character Number.")]
        public string? Section { get; set; }


        [Display(Name = "Father Name")]
        [Required(ErrorMessage = "Please, provide father name.")]
        public string? FatherName { get; set; }


        [Display(Name = "Mother Name")]
        [Required(ErrorMessage = "Please, provide mother name.")]
        public string? MotherName { get; set; }


        [Display(Name = "Guardian Contact")]
        [Required(ErrorMessage = "Please, provide guardian contact number.")]
        public string? GuardianContact { get; set; }


        [Display(Name = "Province")]
        [Required(ErrorMessage = "Please, enter the Province.")]
        public string? PermamentProvince { get; set; }


        [Display(Name = "District")]
        [Required(ErrorMessage = "Please, enter the District.")]
        public string? PermamentDistrict { get; set; }

        [Display(Name = "City")]
        [Required(ErrorMessage = "Please, enter the City.")]
        public string? PermamentCity { get; set; }

        [Display(Name = "Ward No")]
        [Required(ErrorMessage = "Please, enter the Ward No.")]
        public string? PermamentWard { get; set; }

        [Display(Name = "Province")]
        [Required(ErrorMessage = "Please, enter the Province.")]
        public string? TemporaryProvince { get; set; }

        [Display(Name = "District")]
        [Required(ErrorMessage = "Please, enter the City.")]
        public string? TemporaryDistrict { get; set; }

        [Display(Name = "City")]
        [Required(ErrorMessage = "Please, enter the City.")]
        public string? TemporaryCity { get; set; }

        [Display(Name = "Ward No")]
        [Required(ErrorMessage = "Please, enter the Ward No.")]
        public string? TemporaryWard { get; set; }

        [Display(Name = "Previous School Name")]
        [Required(ErrorMessage = "Please, enter the previous school grade.")]
        public String? PreviousSchool { get; set; }

        [Required(ErrorMessage = "Please, enter the previous school grade.")]
        [Display(Name = "Previous School Grade")]
        public String? PreviousSchoolGrade { get; set; }
    }
}
