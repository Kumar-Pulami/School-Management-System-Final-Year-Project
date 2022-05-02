using _19033684_Kumar_Pulami.Models.ViewModel.Dashboard;
using _19033684_Kumar_Pulami.Models.ViewModel.Student.Manage_Student;
using _19033684_Kumar_Pulami.Services;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.SqlClient;

namespace _19033684_Kumar_Pulami.Controllers
{
    public class DashboardController : Controller
    {
        public IActionResult Index()
        {
            ViewBag.TitleName = "Dashboard";
            ViewBag.TotalStudent = GetTotalStudent();
            ViewBag.TotalTeacher = GetTotalTeacher();
            ViewBag.TotalSubject = GetTotalSubject();
            ViewBag.TotalTerminal = GetTotalTerminal();
            ViewBag.TotalExam = GetTotalExam();
            DashboardViewModel model = new DashboardViewModel();
            model.StudentList = GetRecentStudentList();
            return View(model);
        }
        

        public int GetTotalStudent()
        {
            DataTable queryData;
            int totalStudent = 0;
            using (SqlConnection connection = new SqlConnection(DatabaseAccess.GetConnection()))
            {
                if (connection.State == System.Data.ConnectionState.Closed)
                {
                    connection.Open();
                }

                using (SqlCommand command = new SqlCommand("SELECT COUNT(Student.ID) FROM Student;", connection))
                {
                    using (SqlDataAdapter dataAdapter = new SqlDataAdapter(command))
                    {
                        queryData = new DataTable();
                        dataAdapter.Fill(queryData);
                    }
                    foreach (DataRow row in queryData.Rows)
                    {
                        totalStudent = int.Parse(row[0].ToString());
                    }
                }
            }
            return totalStudent;
        }

        public int GetTotalTeacher()
        {
            DataTable queryData;
            int total = 0;
            using (SqlConnection connection = new SqlConnection(DatabaseAccess.GetConnection()))
            {
                if (connection.State == System.Data.ConnectionState.Closed)
                {
                    connection.Open();
                }

                using (SqlCommand command = new SqlCommand("SELECT COUNT(ID) FROM Teacher;", connection))
                {
                    using (SqlDataAdapter dataAdapter = new SqlDataAdapter(command))
                    {
                        queryData = new DataTable();
                        dataAdapter.Fill(queryData);
                    }
                    foreach (DataRow row in queryData.Rows)
                    {
                        total = int.Parse(row[0].ToString());
                    }
                }
            }
            return total;
        }


        public int GetTotalSubject()
        {
            DataTable queryData;
            int total = 0;
            using (SqlConnection connection = new SqlConnection(DatabaseAccess.GetConnection()))
            {
                if (connection.State == System.Data.ConnectionState.Closed)
                {
                    connection.Open();
                }

                using (SqlCommand command = new SqlCommand("SELECT COUNT(ID) FROM Subject;", connection))
                {
                    using (SqlDataAdapter dataAdapter = new SqlDataAdapter(command))
                    {
                        queryData = new DataTable();
                        dataAdapter.Fill(queryData);
                    }
                    foreach (DataRow row in queryData.Rows)
                    {
                        total = int.Parse(row[0].ToString());
                    }
                }
            }
            return total;
        }

        public int GetTotalExam()
        {
            DataTable queryData;
            int total = 0;
            using (SqlConnection connection = new SqlConnection(DatabaseAccess.GetConnection()))
            {
                if (connection.State == System.Data.ConnectionState.Closed)
                {
                    connection.Open();
                }

                using (SqlCommand command = new SqlCommand("SELECT COUNT(*) FROM BatchGradeTerminal;", connection))
                {
                    using (SqlDataAdapter dataAdapter = new SqlDataAdapter(command))
                    {
                        queryData = new DataTable();
                        dataAdapter.Fill(queryData);
                    }
                    foreach (DataRow row in queryData.Rows)
                    {
                        total = int.Parse(row[0].ToString());
                    }
                }
            }
            return total;
        }        
        
        public int GetTotalTerminal()
        {
            DataTable queryData;
            int total = 0;
            using (SqlConnection connection = new SqlConnection(DatabaseAccess.GetConnection()))
            {
                if (connection.State == System.Data.ConnectionState.Closed)
                {
                    connection.Open();
                }

                using (SqlCommand command = new SqlCommand("SELECT COUNT(*) FROM Terminal;", connection))
                {
                    using (SqlDataAdapter dataAdapter = new SqlDataAdapter(command))
                    {
                        queryData = new DataTable();
                        dataAdapter.Fill(queryData);
                    }
                    foreach (DataRow row in queryData.Rows)
                    {
                        total = int.Parse(row[0].ToString());
                    }
                }
            }
            return total;
        }

        public List<ManageStudentListViewModel> GetRecentStudentList()
        {
            DataTable queryData;
            ManageStudentListViewModel studentData;
            List<ManageStudentListViewModel> studentList = new List<ManageStudentListViewModel>();


            using (SqlConnection connection = new SqlConnection(DatabaseAccess.GetConnection()))
            {
                if (connection.State == System.Data.ConnectionState.Closed)
                {
                    connection.Open();
                }
                using (SqlCommand command = new SqlCommand("SELECT Distinct(Student.ID), Person.Name, Student.Batch, Student.Grade, Student.Section, CONCAT(Address.Province, ', ', Address.District, ', ', Address.City, '-', Address.Ward) AS Address, Student.Guardian_Contact FROM Student JOIN Person ON Student.ID = Person.ID JOIN Person_Address ON Person.ID = Person_Address.Person_ID JOIN Address ON Address.ID = Person_Address.Address_ID WHERE Person_Address.Address_Type = 'Temporary' ORDER BY Student.ID DESC OFFSET 0 ROWS FETCH FIRST 7 ROWS ONLY;", connection))
                {
                    using (SqlDataAdapter dataAdapter = new SqlDataAdapter(command))
                    {
                        queryData = new DataTable();
                        dataAdapter.Fill(queryData);
                    }
                }
            }

            if (queryData.Rows.Count > 0)
            {
                foreach (DataRow row in queryData.Rows)
                {
                    studentData = new ManageStudentListViewModel();
                    studentData.StudentID = row[0].ToString();
                    studentData.StudentName = row[1].ToString();
                    studentData.Batch = row[2].ToString();
                    studentData.Grade = row[3].ToString();
                    studentData.Section = row[4].ToString();
                    studentData.Address = row[5].ToString();
                    studentData.GuardianContact = row[6].ToString();
                    studentList.Add(studentData);
                }
            }
            return studentList;
        }

    }
}
