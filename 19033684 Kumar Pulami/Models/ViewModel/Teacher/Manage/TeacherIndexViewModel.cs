using System.ComponentModel.DataAnnotations;

namespace _19033684_Kumar_Pulami.Models.ViewModel.Teacher.Manage
{
    public class TeacherIndexListViewModel
    {

        public String? TeacherID { get; set; }


        [Display(Name = "Teacher Name")]
        public String? TeacherName { get; set; }


        public String? Address { get; set; }



        [Display(Name = "Contact Number")]
        public String? Contact { get; set; }



        [Display(Name = "Hired Date")]
        [DataType(DataType.Date)]
        public DateTime? HireDate { get; set; }
    }


    public class TeacherIndexViewModel
    {

        [Display(Name = "Teacher Name")]
        public String? SearchTeacherName { get; set; }

        public List<TeacherIndexListViewModel>? TeacherTable { get; set; }
    }
} 
