using _19033684_Kumar_Pulami.Models.ViewModel.Teacher.Manage;
using _19033684_Kumar_Pulami.Services;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.SqlClient;

namespace _19033684_Kumar_Pulami.Controllers.Teacher
{
    public class ManageTeacherController : Controller
    {
        public IActionResult Index()
        {
            ViewBag.TitleName = "Manage Teacher";
            DataTable queryData;
            List<TeacherIndexListViewModel> teacherList = new List<TeacherIndexListViewModel>();
            TeacherIndexListViewModel teacher;

            using (SqlConnection connection = new SqlConnection(DatabaseAccess.GetConnection()))
            {
                if (connection.State == System.Data.ConnectionState.Closed)
                {
                    connection.Open();
                }
                using (SqlCommand command = new SqlCommand("SELECT Teacher.ID, Person.Name, CONCAT(Address.Province, ', ', Address.District, ', ', Address.City, '-', Address.Ward) AS Address, Teacher.Contact_Number, Teacher.Hire_Date FROM Teacher JOIN Person ON Teacher.ID = Person.ID JOIN Person_Address ON Person.ID = Person_Address.Person_ID JOIN Address ON Address.ID = Person_Address.Address_ID WHERE Person_Address.Address_Type = 'Temporary' ORDER BY Person.Name;", connection))
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
                    teacher = new TeacherIndexListViewModel();
                    teacher.TeacherID = row[0].ToString();
                    teacher.TeacherName = row[1].ToString();
                    teacher.Address = row[2].ToString();
                    teacher.Contact = row[3].ToString();
                    teacher.HireDate = Convert.ToDateTime(row[4].ToString()).Date;
                    teacherList.Add(teacher);
                }
            }

            TeacherIndexViewModel model = new TeacherIndexViewModel();
            model.SearchTeacherName = null;
            model.TeacherTable = teacherList;

            return View(model);
        }

        [HttpPost]
        public IActionResult Index(TeacherIndexViewModel value)
        {
            ViewBag.TitleName = "Manage Teacher";            
            DataTable queryData;
            TeacherIndexListViewModel teacherData;
            List<TeacherIndexListViewModel> teacherList = new List<TeacherIndexListViewModel>();
            String teacherName = value.SearchTeacherName;


            using (SqlConnection connection = new SqlConnection(DatabaseAccess.GetConnection()))
            {
                if (connection.State == System.Data.ConnectionState.Closed)
                {
                    connection.Open();
                }
                using (SqlCommand command = new SqlCommand("SELECT Teacher.ID, Person.Name, Teacher.Contact_Number, Teacher.Hire_Date, CONCAT(Address.Province, ', ', Address.District, ', ', Address.City, '-', Address.Ward) AS Address FROM Teacher JOIN Person ON Teacher.ID = Person.ID JOIN Person_Address ON Person.ID = Person_Address.Person_ID JOIN Address ON Address.ID = Person_Address.Address_ID WHERE Person_Address.Address_Type = 'Temporary' AND Person.Name = @name ORDER BY Person.Name;", connection))
                {
                    command.Parameters.AddWithValue("@name", teacherName);                    
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
                    teacherData = new TeacherIndexListViewModel();
                    teacherData.TeacherID = row[0].ToString();
                    teacherData.TeacherName = row[1].ToString();
                    teacherData.Contact = row[2].ToString();
                    teacherData.HireDate = Convert.ToDateTime(row[3].ToString()).Date;
                    teacherData.Address = row[4].ToString();
                    teacherList.Add(teacherData);
                }
            }

            TeacherIndexViewModel model = new TeacherIndexViewModel();
            model.SearchTeacherName = teacherName;
            model.TeacherTable = teacherList;
            return View(model);
        }


        [HttpGet]
        public IActionResult Edit(String teacherID)
        {
            ViewBag.TitleName = "Edit Teacher Information";
            ManageTeacherEditViewModel teacherData = new ManageTeacherEditViewModel();

            DataTable queryData;
            using (SqlConnection connection = new SqlConnection(DatabaseAccess.GetConnection()))
            {
                if (connection.State == System.Data.ConnectionState.Closed)
                {
                    connection.Open();
                }
                using (SqlCommand command = new SqlCommand("SELECT Person.*, Teacher.*, Address.* FROM Teacher JOIN Person ON Teacher.ID = Person.ID JOIN Person_Address ON Person.ID = Person_Address.Person_ID JOIN Address ON Address.ID = Person_Address.Address_ID WHERE Person_Address.Address_Type = 'Temporary' AND Teacher.ID = @teacherID;", connection))
                {
                    command.Parameters.AddWithValue("@teacherID", teacherID);
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
                    teacherData.TeacherID = row[0].ToString();
                    teacherData.Name = row[1].ToString();
                    teacherData.DOB = Convert.ToDateTime(row[2].ToString()).Date;
                    teacherData.Gender = row[3].ToString();
                    teacherData.FatherName = row[4].ToString();
                    teacherData.MotherName = row[5].ToString();
                    teacherData.Contact = row[7].ToString();
                    teacherData.HireDate = Convert.ToDateTime(row[8].ToString()).Date;
                    teacherData.Salary = float.Parse(row[9].ToString());
                    teacherData.TemporaryProvince = row[11].ToString();
                    teacherData.TemporaryDistrict = row[12].ToString();
                    teacherData.TemporaryCity = row[13].ToString();
                    teacherData.TemporaryWard = row[14].ToString();
                }
            }


            using (SqlConnection connection = new SqlConnection(DatabaseAccess.GetConnection()))
            {
                if (connection.State == System.Data.ConnectionState.Closed)
                {
                    connection.Open();
                }
                using (SqlCommand command = new SqlCommand("SELECT Address.* FROM Teacher JOIN Person ON Teacher.ID = Person.ID JOIN Person_Address ON Person.ID = Person_Address.Person_ID JOIN Address ON Address.ID = Person_Address.Address_ID WHERE Person_Address.Address_Type = 'Permament' AND Teacher.ID = @teacherID;", connection))
                {
                    command.Parameters.AddWithValue("@teacherID", teacherID);
                    using (SqlDataAdapter dataAdapter = new SqlDataAdapter(command))
                    {
                        queryData = new DataTable();
                        dataAdapter.Fill(queryData);
                    }
                }

                foreach (DataRow row in queryData.Rows)
                {
                    teacherData.PermamentProvince = row[1].ToString();
                    teacherData.PermamentDistrict = row[2].ToString();
                    teacherData.PermamentCity = row[3].ToString();
                    teacherData.PermamentWard = row[4].ToString();
                }
            }
            return View(teacherData);
        }

       [HttpPost]
        public IActionResult Edit(ManageTeacherEditViewModel teacherData)
        {
            if (ModelState.IsValid)
            {
                String permamentAddID = "PERM-" + teacherData.TeacherID.Trim(new Char[] { 'T', 'H' });
                String temporaryAddID = "TEMP-" + teacherData.TeacherID.Trim(new Char[] { 'T', 'H' });
                try
                {
                    using (SqlConnection connection = new SqlConnection(DatabaseAccess.GetConnection()))
                    {
                        if (connection.State == System.Data.ConnectionState.Closed)
                        {
                            connection.Open();
                        }

                        SqlCommand updatePersonCommand = new SqlCommand("UPDATE Person SET Name = @name, DOB = @dob, Gender = @gender, Father_Name = @father, Mother_Name = @mother WHERE ID = @id", connection);
                        updatePersonCommand.Parameters.AddWithValue("@name", teacherData.Name);
                        updatePersonCommand.Parameters.AddWithValue("@dob", teacherData.DOB);
                        updatePersonCommand.Parameters.AddWithValue("@gender", teacherData.Gender);
                        updatePersonCommand.Parameters.AddWithValue("@father", teacherData.FatherName);
                        updatePersonCommand.Parameters.AddWithValue("@mother", teacherData.MotherName);
                        updatePersonCommand.Parameters.AddWithValue("@id", teacherData.TeacherID);
                        updatePersonCommand.ExecuteNonQuery();



                        SqlCommand updateTeacherCommand = new SqlCommand("UPDATE Teacher SET Contact_Number = @contact, Hire_Date = @hire, Salary = @salary WHERE ID = @id;", connection);
                        updateTeacherCommand.Parameters.AddWithValue("@contact", teacherData.Contact);
                        updateTeacherCommand.Parameters.AddWithValue("@hire", teacherData.HireDate);
                        updateTeacherCommand.Parameters.AddWithValue("@salary", teacherData.Salary);
                        updateTeacherCommand.Parameters.AddWithValue("@id", teacherData.TeacherID);
                        updateTeacherCommand.ExecuteNonQuery();


                        SqlCommand updateTempAddressCommand = new SqlCommand("UPDATE Address SET Province = @province, District = @district, City = @city, Ward = @ward WHERE ID = @id;", connection);
                        updateTempAddressCommand.Parameters.AddWithValue("@province", teacherData.TemporaryProvince);
                        updateTempAddressCommand.Parameters.AddWithValue("@district", teacherData.TemporaryDistrict);
                        updateTempAddressCommand.Parameters.AddWithValue("@city", teacherData.TemporaryCity);
                        updateTempAddressCommand.Parameters.AddWithValue("@ward", teacherData.TemporaryWard);
                        updateTempAddressCommand.Parameters.AddWithValue("@id", temporaryAddID);
                        updateTempAddressCommand.ExecuteNonQuery();


                        SqlCommand updatePerAddressCommand = new SqlCommand("UPDATE Address SET Province = @province, District = @district, City = @city, Ward = @ward WHERE ID = @id;", connection);
                        updatePerAddressCommand.Parameters.AddWithValue("@province", teacherData.PermamentProvince);
                        updatePerAddressCommand.Parameters.AddWithValue("@district", teacherData.PermamentDistrict);
                        updatePerAddressCommand.Parameters.AddWithValue("@city", teacherData.PermamentCity);
                        updatePerAddressCommand.Parameters.AddWithValue("@ward", teacherData.PermamentWard);
                        updatePerAddressCommand.Parameters.AddWithValue("@id", permamentAddID);
                        updatePerAddressCommand.ExecuteNonQuery();
                        TempData["SuccessMessage"] = "Teacher Information Updated Successfully...!";
                        return RedirectToAction("Index");
                    }
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
        public IActionResult Details(String teacherID)
        {
            ViewBag.TitleName = "Teacher Detail";
            TeacherDetailsViewModel teacherDetails = new TeacherDetailsViewModel(); ;
            DataTable queryData;
            using (SqlConnection connection = new SqlConnection(DatabaseAccess.GetConnection()))
            {
                if (connection.State == System.Data.ConnectionState.Closed)
                {
                    connection.Open();
                }
                using (SqlCommand command = new SqlCommand("SELECT Person.Name, Person.DOB, Person.Gender, Person.Father_Name, Person.Mother_Name, Teacher.Hire_Date, Teacher.Contact_Number, Teacher.Salary, CONCAT(Address.Province, ', ', Address.District, ', ', Address.City, '-', Address.Ward) AS Address FROM Teacher JOIN Person ON Teacher.ID = Person.ID JOIN Person_Address ON Person.ID = Person_Address.Person_ID JOIN Address ON Address.ID = Person_Address.Address_ID WHERE Person_Address.Address_Type = 'Temporary' AND Teacher.ID = @teacherID;", connection))
                {
                    command.Parameters.AddWithValue("@teacherID", teacherID);
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
                    teacherDetails.TeacherID = teacherID;
                    teacherDetails.TeacherName = row[0].ToString();
                    teacherDetails.DOB = Convert.ToDateTime(row[1].ToString()).Date;
                    teacherDetails.Gender = row[2].ToString();
                    teacherDetails.FatherName = row[3].ToString();
                    teacherDetails.MotherName = row[4].ToString();
                    teacherDetails.HireDate = Convert.ToDateTime(row[5].ToString()).Date;
                    teacherDetails.Contact = row[6].ToString();
                    teacherDetails.Salary = row[7].ToString();
                    teacherDetails.TemporaryAddress = row[8].ToString();
                }
            }

            using (SqlConnection connection = new SqlConnection(DatabaseAccess.GetConnection()))
            {
                if (connection.State == System.Data.ConnectionState.Closed)
                {
                    connection.Open();
                }
                using (SqlCommand command = new SqlCommand("SELECT CONCAT(Address.Province, ', ', Address.District, ', ', Address.City, '-', Address.Ward) AS Address FROM Teacher JOIN Person ON Teacher.ID = Person.ID JOIN Person_Address ON Person.ID = Person_Address.Person_ID JOIN Address ON Address.ID = Person_Address.Address_ID WHERE Person_Address.Address_Type = 'Permament' AND Teacher.ID = @teacherID;", connection))
                {
                    command.Parameters.AddWithValue("@teacherID", teacherID);
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
                    teacherDetails.PermamentAddress = row[0].ToString();
                }
            }
            return View(teacherDetails);
        }
        
        [HttpGet]
        public IActionResult Delete(String teacherID)
        {
            ViewBag.TitleName = "Teacher Detail";
            TeacherDetailsViewModel teacherDetails = new TeacherDetailsViewModel(); ;
            DataTable queryData;
            using (SqlConnection connection = new SqlConnection(DatabaseAccess.GetConnection()))
            {
                if (connection.State == System.Data.ConnectionState.Closed)
                {
                    connection.Open();
                }
                using (SqlCommand command = new SqlCommand("SELECT Person.Name, Person.DOB, Person.Gender, Person.Father_Name, Person.Mother_Name, Teacher.Hire_Date, Teacher.Contact_Number, Teacher.Salary, CONCAT(Address.Province, ', ', Address.District, ', ', Address.City, '-', Address.Ward) AS Address FROM Teacher JOIN Person ON Teacher.ID = Person.ID JOIN Person_Address ON Person.ID = Person_Address.Person_ID JOIN Address ON Address.ID = Person_Address.Address_ID WHERE Person_Address.Address_Type = 'Temporary' AND Teacher.ID = @teacherID;", connection))
                {
                    command.Parameters.AddWithValue("@teacherID", teacherID);
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
                    teacherDetails.TeacherID = teacherID;
                    teacherDetails.TeacherName = row[0].ToString();
                    teacherDetails.DOB = Convert.ToDateTime(row[1].ToString()).Date;
                    teacherDetails.Gender = row[2].ToString();
                    teacherDetails.FatherName = row[3].ToString();
                    teacherDetails.MotherName = row[4].ToString();
                    teacherDetails.HireDate = Convert.ToDateTime(row[5].ToString()).Date;
                    teacherDetails.Contact = row[6].ToString();
                    teacherDetails.Salary = row[7].ToString();
                    teacherDetails.TemporaryAddress = row[8].ToString();
                }
            }

            using (SqlConnection connection = new SqlConnection(DatabaseAccess.GetConnection()))
            {
                if (connection.State == System.Data.ConnectionState.Closed)
                {
                    connection.Open();
                }
                using (SqlCommand command = new SqlCommand("SELECT CONCAT(Address.Province, ', ', Address.District, ', ', Address.City, '-', Address.Ward) AS Address FROM Teacher JOIN Person ON Teacher.ID = Person.ID JOIN Person_Address ON Person.ID = Person_Address.Person_ID JOIN Address ON Address.ID = Person_Address.Address_ID WHERE Person_Address.Address_Type = 'Permament' AND Teacher.ID = @teacherID;", connection))
                {
                    command.Parameters.AddWithValue("@teacherID", teacherID);
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
                    teacherDetails.PermamentAddress = row[0].ToString();
                }
            }
            return View(teacherDetails);
        }

        [HttpPost]
        public IActionResult DeleteConfirmed(String teacherID)
        {
            String tempID = "TEMP-" + teacherID.Trim(new Char[] { 'T', 'H' });
            String permID = "PERM-" + teacherID.Trim(new Char[] { 'T', 'H' });

            using (SqlConnection connection = new SqlConnection(DatabaseAccess.GetConnection()))
            {
                if (connection.State == System.Data.ConnectionState.Closed)
                {
                    connection.Open();
                }
                using (SqlCommand command = new SqlCommand("DELETE FROM Person_Address WHERE Person_ID = @teacherID;", connection))
                {
                    command.Parameters.AddWithValue("@teacherID", teacherID);
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
                using (SqlCommand command = new SqlCommand("DELETE FROM Teacher WHERE ID = @id;", connection))
                {
                    command.Parameters.AddWithValue("@id", teacherID);
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
                    command.Parameters.AddWithValue("@id", teacherID);
                    command.ExecuteNonQuery();
                }
            }
            TempData["SuccessMessage"] = "Teacher Information Deleted Successfully...!";
            return RedirectToAction("Index");
        }
    }
}
