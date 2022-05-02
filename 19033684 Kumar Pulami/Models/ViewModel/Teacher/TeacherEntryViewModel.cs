using System.ComponentModel.DataAnnotations;

namespace _19033684_Kumar_Pulami.Models.ViewModel.Teacher
{
    public class TeacherEntryViewModel
    {
        [Display(Name = "Teacher Name")]
        [Required(ErrorMessage = "Please, enter the teacher name.")]
        public string? Name { get; set; }


        [Display(Name = "Date of Birth")]
        [Required(ErrorMessage = "Please, select the Date of Birth")]
        [DataType(DataType.Date)]
        public DateTime BOD { get; set; }

        [Required(ErrorMessage = "Please, select the gender.")]
        public String? Gender { get; set; }


        [Display(Name = "Father Name")]
        [Required(ErrorMessage = "Please, provide father name.")]
        public string? FatherName { get; set; }


        [Display(Name = "Mother Name")]
        [Required(ErrorMessage = "Please, provide mother name.")]
        public string? MotherName { get; set; }


        [Display(Name = "Contact")]
        [Required(ErrorMessage = "Please, provide contact number")]
        public string? Contact { get; set; }



        [Display(Name = "Hire Date")]
        [Required(ErrorMessage = "Please, provide hired date.")]
        [DataType(DataType.Date)]
        public string? Hire_Date { get; set; }


        [Display(Name = "Salary")]
        [Required(ErrorMessage = "Please, provide salary.")]
        public int? Salary { get; set; }


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
        [Required(ErrorMessage = "Please, enter the District.")]
        public string? TemporaryDistrict { get; set; }

        [Display(Name = "City")]
        [Required(ErrorMessage = "Please, enter the City.")]
        public string? TemporaryCity { get; set; }

        [Display(Name = "Ward No")]
        [Required(ErrorMessage = "Please, enter the Ward No.")]
        public string? TemporaryWard { get; set; }
    }
}
