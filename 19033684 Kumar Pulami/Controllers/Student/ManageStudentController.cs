using _19033684_Kumar_Pulami.Models.ViewModel.Student;
using _19033684_Kumar_Pulami.Models.ViewModel.Student.Manage_Student;
using _19033684_Kumar_Pulami.Models.ViewModel.Student.Manage_Student.ManageStudentEditViewModel;
using _19033684_Kumar_Pulami.Services;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;

namespace _19033684_Kumar_Pulami.Controllers.Student
{
    public class ManageStudentController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            ViewBag.TitleName = "Manage Student";
            ViewBag.BatchList = DropDownFilter.GetBatchList();

            List<String> gradeList = new List<String>();
            gradeList.Add("Select Grade");
            ViewBag.GradeList = gradeList;

            List<String> sectionList = new List<String>();
            sectionList.Add("Select Section");
            ViewBag.SectionList = sectionList;
   
            ManageStudentIndexModel model = new ManageStudentIndexModel();
            model.ListModel = StudentDataFetcher.GetAllStudentList();

            return View(model);
        }


        [HttpPost]
        public IActionResult Index(ManageStudentIndexModel value)
        {
            ViewBag.TitleName = "Manage Student";
            if (ModelState.IsValid)
            {
                ViewBag.BatchList = DropDownFilter.GetBatchList();
                ViewBag.GradeList = DropDownFilter.GetGradeList(value.Batch);
                ViewBag.SectionList = DropDownFilter.GetSectionList(value.Batch, value.Grade);
                ManageStudentIndexModel indexModel = new ManageStudentIndexModel();
                DataTable queryData;
                ManageStudentListViewModel studentData;
                List<ManageStudentListViewModel> studentList = new List<ManageStudentListViewModel>();


                using (SqlConnection connection = new SqlConnection(DatabaseAccess.GetConnection()))
                {
                    if (connection.State == System.Data.ConnectionState.Closed)
                    {
                        connection.Open();
                    }
                    using (SqlCommand command = new SqlCommand("SELECT Student.ID AS StudentID, Person.Name, Student.Batch, Student.Grade, Student.Section, CONCAT(Address.Province, ', ', Address.District, ', ', Address.City, '-', Address.Ward) AS Address, Student.Guardian_Contact FROM Student JOIN Person ON Student.ID = Person.ID JOIN Person_Address ON Person.ID = Person_Address.Person_ID JOIN Address ON Address.ID = Person_Address.Address_ID WHERE Person_Address.Address_Type = 'Temporary' AND Student.Batch = @batch AND Student.Grade = @grade AND Student.Section = @section ORDER BY Person.Name;", connection))
                    {
                        command.Parameters.AddWithValue("@Batch", value.Batch);
                        command.Parameters.AddWithValue("@Grade", value.Grade);
                        command.Parameters.AddWithValue("@Section", value.Section);
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

                ManageStudentIndexModel model = new ManageStudentIndexModel();
                model.ListModel = studentList;
                model.Batch = value.Batch;
                model.Grade = value.Grade;
                model.Section = value.Section;
                return View(model);
            }
            else
            {
                return View();
            }
        }

        [HttpGet]
        public IActionResult Details(String studentID)
        {
            ViewBag.TitleName = "Student Detail";
            StudentDetailsViewModel studentDetails = new StudentDetailsViewModel(); ;
            DataTable queryData;
            using (SqlConnection connection = new SqlConnection(DatabaseAccess.GetConnection()))
            {
                if (connection.State == System.Data.ConnectionState.Closed)
                {
                    connection.Open();
                }
                using (SqlCommand command = new SqlCommand("SELECT Person.Name, Person.DOB, Person.Gender, Person.Father_Name, Person.Mother_Name, Student.Batch, Student.Grade, Student.Section, CONCAT(Address.Province, ', ', Address.District, ', ', Address.City, '-', Address.Ward) AS Address, Student.Guardian_Contact, Student.Previous_School_Name, Student.Previous_School_Grade FROM Student JOIN Person ON Student.ID = Person.ID JOIN Person_Address ON Person.ID = Person_Address.Person_ID JOIN Address ON Address.ID = Person_Address.Address_ID WHERE Person_Address.Address_Type = 'Temporary' AND Student.ID = @stdID;", connection))
                {
                    command.Parameters.AddWithValue("@stdID", studentID);
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
                    studentDetails.StudentID = studentID;
                    studentDetails.StudentName = row[0].ToString();
                    studentDetails.DOB = Convert.ToDateTime(row[1].ToString()).Date;
                    studentDetails.Gender = row[2].ToString();
                    studentDetails.FatherName = row[3].ToString();
                    studentDetails.MotherName = row[4].ToString();
                    studentDetails.Batch = row[5].ToString();
                    studentDetails.Grade = row[6].ToString();
                    studentDetails.Section = row[7].ToString();
                    studentDetails.TemporaryAddress = row[8].ToString();
                    studentDetails.GuardianContact = row[9].ToString();
                    studentDetails.PreviousSchoolName = row[10].ToString();
                    studentDetails.PrevioiusSchoolGrade = row[11].ToString();
                }
            }

            using (SqlConnection connection = new SqlConnection(DatabaseAccess.GetConnection()))
            {
                if (connection.State == System.Data.ConnectionState.Closed)
                {
                    connection.Open();
                }
                using (SqlCommand command = new SqlCommand("SELECT CONCAT(Address.Province, ', ', Address.District, ', ', Address.City, '-', Address.Ward) AS Address FROM Student JOIN Person ON Student.ID = Person.ID JOIN Person_Address ON Person.ID = Person_Address.Person_ID JOIN Address ON Address.ID = Person_Address.Address_ID WHERE Person_Address.Address_Type = 'Permament' AND Student.ID = @stdID;", connection))
                {
                    command.Parameters.AddWithValue("@stdID", studentID);
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
                    studentDetails.PermamentAddress = row[0].ToString();
                }
            }
            return View(studentDetails);
        }




        [HttpGet]
        public IActionResult Edit(string studentID)
        {
            ManageStudentEditViewModel studentDetails = new ManageStudentEditViewModel();
            ViewBag.TitleName = "Update Student Information";
            DataTable queryData;
            using (SqlConnection connection = new SqlConnection(DatabaseAccess.GetConnection()))
            {
                if (connection.State == System.Data.ConnectionState.Closed)
                {
                    connection.Open();
                }
                using (SqlCommand command = new SqlCommand("SELECT Person.*, Student.*, Address.* FROM Student JOIN Person ON Student.ID = Person.ID JOIN Person_Address ON Person.ID = Person_Address.Person_ID JOIN Address ON Address.ID = Person_Address.Address_ID WHERE Person_Address.Address_Type = 'Temporary' AND Student.ID = @stdID;", connection))
                {
                    command.Parameters.AddWithValue("@stdID", studentID);
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
                    studentDetails.StudentID = row[0].ToString();
                    studentDetails.Name = row[1].ToString();
                    studentDetails.DOB = Convert.ToDateTime(row[2].ToString()).Date;
                    studentDetails.Gender = row[3].ToString();
                    studentDetails.FatherName = row[4].ToString();
                    studentDetails.MotherName = row[5].ToString();
                    studentDetails.Batch = row[7].ToString();
                    studentDetails.Grade = row[8].ToString();
                    studentDetails.Section = row[9].ToString();
                    studentDetails.GuardianContact = row[10].ToString();
                    studentDetails.PreviousSchool = row[11].ToString();
                    studentDetails.PreviousSchoolGrade = row[12].ToString();
                    studentDetails.TemporaryProvince = row[14].ToString();
                    studentDetails.TemporaryDistrict = row[15].ToString();
                    studentDetails.TemporaryCity= row[16].ToString();
                    studentDetails.TemporaryWard = row[17].ToString();
                }
            }


            using (SqlConnection connection = new SqlConnection(DatabaseAccess.GetConnection()))
            {
                if (connection.State == System.Data.ConnectionState.Closed)
                {
                    connection.Open();
                }
                using (SqlCommand command = new SqlCommand("SELECT Address.* FROM Student JOIN Person ON Student.ID = Person.ID JOIN Person_Address ON Person.ID = Person_Address.Person_ID JOIN Address ON Address.ID = Person_Address.Address_ID WHERE Person_Address.Address_Type = 'Permament' AND Student.ID = @stdID;", connection))
                {
                    command.Parameters.AddWithValue("@stdID", studentID);
                    using (SqlDataAdapter dataAdapter = new SqlDataAdapter(command))
                    {
                        queryData = new DataTable();
                        dataAdapter.Fill(queryData);
                    }
                }

                foreach (DataRow row in queryData.Rows)
                {                
                    studentDetails.PermamentProvince = row[1].ToString();
                    studentDetails.PermamentDistrict = row[2].ToString();
                    studentDetails.PermamentCity = row[3].ToString();
                    studentDetails.PermamentWard = row[4].ToString();
                }
            }
            return View(studentDetails);
        }


        [HttpPost]
        public IActionResult Edit(ManageStudentEditViewModel studentDetails)
        {
            if (ModelState.IsValid)
            {
                String studentID = studentDetails.StudentID;
                String tempID = "TEMP-" + studentID.Trim(new Char[] { 'S', 'T' });
                String permID = "PERM-" + studentID.Trim(new Char[] { 'S', 'T' });
                try
                {
                    using (SqlConnection connection = new SqlConnection(DatabaseAccess.GetConnection()))
                    {
                        if (connection.State == System.Data.ConnectionState.Closed)
                        {
                            connection.Open();
                        }

                        SqlCommand updatePersonCommand = new SqlCommand("UPDATE Person SET Name = @name, DOB = @dob, Gender = @gender, Father_Name = @father, Mother_Name = @mother WHERE ID = @id", connection);
                        updatePersonCommand.Parameters.AddWithValue("@name", studentDetails.Name);
                        updatePersonCommand.Parameters.AddWithValue("@dob", studentDetails.DOB);
                        updatePersonCommand.Parameters.AddWithValue("@gender", studentDetails.Gender);
                        updatePersonCommand.Parameters.AddWithValue("@father", studentDetails.FatherName);
                        updatePersonCommand.Parameters.AddWithValue("@mother", studentDetails.MotherName);
                        updatePersonCommand.Parameters.AddWithValue("@id", studentDetails.StudentID);
                        updatePersonCommand.ExecuteNonQuery();



                        SqlCommand updateStudentCommand = new SqlCommand("UPDATE Student SET Batch = @batch, Grade = @grade, Section = @section, Guardian_Contact = @contact, Previous_School_Name = @schoolName, Previous_School_Grade = @schoolGrade WHERE ID = @id;", connection);
                        updateStudentCommand.Parameters.AddWithValue("@batch", studentDetails.Batch);
                        updateStudentCommand.Parameters.AddWithValue("@section", studentDetails.Section);
                        updateStudentCommand.Parameters.AddWithValue("@grade", studentDetails.Grade);
                        updateStudentCommand.Parameters.AddWithValue("@contact", studentDetails.GuardianContact);
                        updateStudentCommand.Parameters.AddWithValue("@schoolName", studentDetails.PreviousSchool);
                        updateStudentCommand.Parameters.AddWithValue("@schoolGrade", studentDetails.PreviousSchoolGrade);
                        updateStudentCommand.Parameters.AddWithValue("@id", studentDetails.StudentID);
                        updateStudentCommand.ExecuteNonQuery();

                        SqlCommand updateTempAddressCommand = new SqlCommand("UPDATE Address SET Province = @province, District = @district, City = @city, Ward = @ward WHERE ID = @id;", connection);
                        updateTempAddressCommand.Parameters.AddWithValue("@province", studentDetails.TemporaryProvince);
                        updateTempAddressCommand.Parameters.AddWithValue("@district", studentDetails.TemporaryDistrict);
                        updateTempAddressCommand.Parameters.AddWithValue("@city", studentDetails.TemporaryCity);
                        updateTempAddressCommand.Parameters.AddWithValue("@ward", studentDetails.TemporaryWard);
                        updateTempAddressCommand.Parameters.AddWithValue("@id", tempID);
                        updateTempAddressCommand.ExecuteNonQuery();

                        SqlCommand updatePerAddressCommand = new SqlCommand("UPDATE Address SET Province = @province, District = @district, City = @city, Ward = @ward WHERE ID = @id;", connection);
                        updatePerAddressCommand.Parameters.AddWithValue("@province", studentDetails.PermamentProvince);
                        updatePerAddressCommand.Parameters.AddWithValue("@district", studentDetails.PermamentDistrict);
                        updatePerAddressCommand.Parameters.AddWithValue("@city", studentDetails.PermamentCity);
                        updatePerAddressCommand.Parameters.AddWithValue("@ward", studentDetails.PermamentWard);
                        updatePerAddressCommand.Parameters.AddWithValue("@id", permID);
                        updatePerAddressCommand.ExecuteNonQuery();                        
                    }
                    TempData["SuccessMessage"] = "Student Information Updated Successfully...!";
                }
                catch (Exception)
                {

                }
            }
            else
            {
                return View();
            }
            return RedirectToAction("Index");
        }


        [HttpGet]
        public IActionResult Delete(String studentID)
        {
            ViewBag.TitleName = "Delete Student Information";
            StudentDetailsViewModel studentDetails = new StudentDetailsViewModel(); ;
            DataTable queryData;
            using (SqlConnection connection = new SqlConnection(DatabaseAccess.GetConnection()))
            {
                if (connection.State == System.Data.ConnectionState.Closed)
                {
                    connection.Open();
                }
                using (SqlCommand command = new SqlCommand("SELECT Person.Name, Person.DOB, Person.Gender, Person.Father_Name, Person.Mother_Name, Student.Batch, Student.Grade, Student.Section, CONCAT(Address.Province, ', ', Address.District, ', ', Address.City, '-', Address.Ward) AS Address, Student.Guardian_Contact, Student.Previous_School_Name, Student.Previous_School_Grade FROM Student JOIN Person ON Student.ID = Person.ID JOIN Person_Address ON Person.ID = Person_Address.Person_ID JOIN Address ON Address.ID = Person_Address.Address_ID WHERE Person_Address.Address_Type = 'Temporary' AND Student.ID = @stdID;", connection))
                {
                    command.Parameters.AddWithValue("@stdID", studentID);
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
                    studentDetails.StudentID = studentID;
                    studentDetails.StudentName = row[0].ToString();
                    studentDetails.DOB = Convert.ToDateTime(row[1].ToString()).Date;
                    studentDetails.Gender = row[2].ToString();
                    studentDetails.FatherName = row[3].ToString();
                    studentDetails.MotherName = row[4].ToString();
                    studentDetails.Batch = row[5].ToString();
                    studentDetails.Grade = row[6].ToString();
                    studentDetails.Section = row[7].ToString();
                    studentDetails.TemporaryAddress = row[8].ToString();
                    studentDetails.GuardianContact = row[9].ToString();
                    studentDetails.PreviousSchoolName = row[10].ToString();
                    studentDetails.PrevioiusSchoolGrade = row[11].ToString();
                }
            }

            using (SqlConnection connection = new SqlConnection(DatabaseAccess.GetConnection()))
            {
                if (connection.State == System.Data.ConnectionState.Closed)
                {
                    connection.Open();
                }
                using (SqlCommand command = new SqlCommand("SELECT CONCAT(Address.Province, ', ', Address.District, ', ', Address.City, '-', Address.Ward) AS Address FROM Student JOIN Person ON Student.ID = Person.ID JOIN Person_Address ON Person.ID = Person_Address.Person_ID JOIN Address ON Address.ID = Person_Address.Address_ID WHERE Person_Address.Address_Type = 'Permament' AND Student.ID = @stdID;", connection))
                {
                    command.Parameters.AddWithValue("@stdID", studentID);
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
                    studentDetails.PermamentAddress = row[0].ToString();
                }
            }
            return View(studentDetails);
        }

        [HttpPost]
        public IActionResult DeleteConfirmed(String studentID)
        {
            String tempID = "TEMP-" + studentID.Trim(new Char[] { 'S', 'T' });
            String permID = "PERM-" + studentID.Trim(new Char[] { 'S', 'T' });
            using (SqlConnection connection = new SqlConnection(DatabaseAccess.GetConnection()))
            {
                if (connection.State == System.Data.ConnectionState.Closed)
                {
                    connection.Open();
                }
                using (SqlCommand command = new SqlCommand("DELETE FROM Marks WHERE StudentID = @stdID;", connection))
                { 
                    command.Parameters.AddWithValue("@stdID", studentID);
                    command.ExecuteNonQuery();
                }
                
            }

            using (SqlConnection connection = new SqlConnection(DatabaseAccess.GetConnection()))
            {
                if (connection.State == System.Data.ConnectionState.Closed)
                {
                    connection.Open();
                }
                using (SqlCommand command = new SqlCommand("DELETE FROM Person_Address WHERE Person_ID = @stdID;", connection))
                {
                    command.Parameters.AddWithValue("@stdID", studentID);
                    command.ExecuteNonQuery();
                }
            }

            using (SqlConnection connection = new SqlConnection(DatabaseAccess.GetConnection()))
            {
                if (connection.State == System.Data.ConnectionState.Closed)
                {
                    connection.Open();
                }
                using (SqlCommand command = new SqlCommand("DELETE FROM Address WHERE ID = @id;", connection))
                {
                    command.Parameters.AddWithValue("@id", tempID);
                    command.ExecuteNonQuery();
                }
            }

            using (SqlConnection connection = new SqlConnection(DatabaseAccess.GetConnection()))
            {
                if (connection.State == System.Data.ConnectionState.Closed)
                {
                    connection.Open();
                }
                using (SqlCommand command = new SqlCommand("DELETE FROM Address WHERE ID = @id;", connection))
                {
                    command.Parameters.AddWithValue("@id", permID);
                    command.ExecuteNonQuery();
                }
            }


            using (SqlConnection connection = new SqlConnection(DatabaseAccess.GetConnection()))
            {
                if (connection.State == System.Data.ConnectionState.Closed)
                {
                    connection.Open();
                }
                using (SqlCommand command = new SqlCommand("DELETE FROM Student WHERE ID = @id;", connection))
                {
                    command.Parameters.AddWithValue("@id", studentID);
                    command.ExecuteNonQuery();
                }
            }
            
            using (SqlConnection connection = new SqlConnection(DatabaseAccess.GetConnection()))
            {
                if (connection.State == System.Data.ConnectionState.Closed)
                {
                    connection.Open();
                }
                using (SqlCommand command = new SqlCommand("DELETE FROM Person WHERE ID = @id;", connection))
                {
                    command.Parameters.AddWithValue("@id", studentID);
                    command.ExecuteNonQuery();
                }
            }
            TempData["SuccessMessage"] = "Student Information Deleted Successfully...!";
            return RedirectToAction("Index");
        }
    }
}
