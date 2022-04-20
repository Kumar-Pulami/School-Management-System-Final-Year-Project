using System.ComponentModel.DataAnnotations;
namespace _19033684_Kumar_Pulami.Models.ViewModel.Teacher.Manage
{
    public class ManageTeacherEditViewModel
    {
        [Display(Name = "Teacher ID")]
        public String? TeacherID { get; set; }

        [Display(Name = "Teacher Name")]
        [Required(ErrorMessage = "Please, enter the teacher name.")]
        public string? Name { get; set; }


        [Display(Name = "Date of Birth")]
        [Required(ErrorMessage = "Please, select the Date of Birth")]
        [DataType(DataType.Date)]
        public DateTime DOB { get; set; }


        [Display(Name = "Father Name")]
        [Required(ErrorMessage = "Please, provide father name.")]
        public string? FatherName { get; set; }


        [Display(Name = "Mother Name")]
        [Required(ErrorMessage = "Please, provide mother name.")]
        public string? MotherName { get; set; }


        [Display(Name = "Contact Number")]
        [Required(ErrorMessage = "Please, provide contact number")]
        public string? Contact { get; set; }

        [Display(Name = "Hire Date")]
        [Required(ErrorMessage = "Please, select the hired date.")]
        [DataType(DataType.Date)]
        public DateTime? HireDate { get; set; }


        [Required(ErrorMessage = "Please, select the salary.")]
        public float? Salary { get; set; }


        [Required(ErrorMessage = "Please, select the Gender")]
        public string? Gender { get; set; }


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
