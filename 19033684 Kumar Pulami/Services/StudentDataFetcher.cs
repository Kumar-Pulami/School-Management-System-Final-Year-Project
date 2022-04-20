using _19033684_Kumar_Pulami.Models.ViewModel.Student.Manage_Student;
using System.Data;
using System.Data.SqlClient;

namespace _19033684_Kumar_Pulami.Services
{
    public static class StudentDataFetcher
    {
        public static List<ManageStudentListViewModel> GetAllStudentList()
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
                using (SqlCommand command = new SqlCommand("SELECT Student.ID AS StudentID, Person.Name, Student.Batch, Student.Grade, Student.Section, CONCAT(Address.Province, ', ', Address.District, ', ', Address.City, '-', Address.Ward) AS Address, Student.Guardian_Contact FROM Student JOIN Person ON Student.ID = Person.ID JOIN Person_Address ON Person.ID = Person_Address.Person_ID JOIN Address ON Address.ID = Person_Address.Address_ID WHERE Person_Address.Address_Type = 'Temporary' ORDER BY Person.Name;", connection))
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
