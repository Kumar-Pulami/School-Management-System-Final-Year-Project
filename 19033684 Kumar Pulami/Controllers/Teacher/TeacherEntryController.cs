using _19033684_Kumar_Pulami.Models.ViewModel.Teacher;
using _19033684_Kumar_Pulami.Services;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.SqlClient;

namespace _19033684_Kumar_Pulami.Controllers.Teacher
{
    
    public class TeacherEntryController : Controller
    {
        [HttpGet]
        public IActionResult Entry()
        {
            return View();
        }



        [HttpPost]
        public IActionResult Entry(TeacherEntryViewModel teacherDetails)
        {
            String? errorMessage = null;
            if (ModelState.IsValid)
            {
                String currentDateAndTime = DateTime.Now.ToString("yyyyMMddHHmmss");
                String teacherID = "TH" + currentDateAndTime;
                String tempAddID = "TEMP-" + currentDateAndTime;
                String permAddID = "PERM-" + currentDateAndTime;
                try
                {                 
                    using (SqlConnection connection = new SqlConnection(DatabaseAccess.GetConnection()))
                    {
                        if (connection.State == System.Data.ConnectionState.Closed)
                        {
                            connection.Open();
                        }

                        using (SqlCommand command = new SqlCommand("INSERT INTO Person VALUES(@id, @name, @DOB, @gender, @father, @mother)", connection))
                        {
                            command.Parameters.AddWithValue("@id", teacherID);
                            command.Parameters.AddWithValue("@name", teacherDetails.Name);
                            command.Parameters.AddWithValue("@DOB", teacherDetails.BOD);
                            command.Parameters.AddWithValue("@gender", teacherDetails.Gender);
                            command.Parameters.AddWithValue("@father", teacherDetails.FatherName);
                            command.Parameters.AddWithValue("@mother", teacherDetails.MotherName);
                            command.ExecuteNonQuery();
                        }

                        using (SqlCommand command = new SqlCommand("INSERT INTO Teacher VALUES(@id, @contact_number, @hire_date, @salary)", connection))
                        {
                            command.Parameters.AddWithValue("@id", teacherID);
                            command.Parameters.AddWithValue("@contact_number", teacherDetails.Contact);
                            command.Parameters.AddWithValue("@hire_date", teacherDetails.Hire_Date);
                            command.Parameters.AddWithValue("@salary", teacherDetails.Salary);
                            command.ExecuteNonQuery();
                        }

                        using (SqlCommand permanametAddressCommand = new SqlCommand("INSERT INTO Address VALUES(@id, @province, @district, @city, @ward)", connection))
                        {
                            permanametAddressCommand.Parameters.AddWithValue("@id", permAddID);
                            permanametAddressCommand.Parameters.AddWithValue("@province", teacherDetails.PermamentProvince);
                            permanametAddressCommand.Parameters.AddWithValue("@district", teacherDetails.PermamentDistrict);
                            permanametAddressCommand.Parameters.AddWithValue("@city", teacherDetails.PermamentCity);
                            permanametAddressCommand.Parameters.AddWithValue("@ward", teacherDetails.PermamentWard);

                            permanametAddressCommand.ExecuteNonQuery();
                        }

                        using (SqlCommand temporaryAddressCommand = new SqlCommand("INSERT INTO Address VALUES(@id, @province, @district, @city, @ward)", connection))
                        {
                            temporaryAddressCommand.Parameters.AddWithValue("@id", tempAddID);
                            temporaryAddressCommand.Parameters.AddWithValue("@province", teacherDetails.TemporaryProvince);
                            temporaryAddressCommand.Parameters.AddWithValue("@district", teacherDetails.TemporaryDistrict);
                            temporaryAddressCommand.Parameters.AddWithValue("@city", teacherDetails.TemporaryCity);
                            temporaryAddressCommand.Parameters.AddWithValue("@ward", teacherDetails.TemporaryWard);
                            temporaryAddressCommand.ExecuteNonQuery();
                        }
                    }

                    using (SqlConnection connection = new SqlConnection(DatabaseAccess.GetConnection()))
                    {
                        if (connection.State == System.Data.ConnectionState.Closed)
                        {
                            connection.Open();
                        }

                        using (SqlCommand personTempAddressCommand = new SqlCommand("INSERT INTO Person_Address VALUES(@stdID, @addressID, 'Temporary')", connection))
                        {
                            personTempAddressCommand.Parameters.AddWithValue("@stdID", teacherID);
                            personTempAddressCommand.Parameters.AddWithValue("@addressID", tempAddID);
                            personTempAddressCommand.ExecuteNonQuery();

                        }

                        using (SqlCommand personPerAddressCommand = new SqlCommand("INSERT INTO Person_Address VALUES(@stdID, @addressID, 'Permament')", connection))
                        {
                            personPerAddressCommand.Parameters.AddWithValue("@stdID", teacherID);
                            personPerAddressCommand.Parameters.AddWithValue("@addressID", permAddID);
                            personPerAddressCommand.ExecuteNonQuery();
                        }
                    }
                    TempData["SuccessMessage"] = "Teacher Registered Successfully...!";
                    return RedirectToAction("Entry");

                }
                catch (SqlException)
                {
                    using (SqlConnection connection = new SqlConnection(DatabaseAccess.GetConnection()))
                    {
                        if (connection.State == ConnectionState.Closed)
                        {
                            connection.Open();
                        }

                        SqlCommand deletePersonAddress = new SqlCommand("DELETE FROM Person_Address WHERE Person_ID = @personID", connection);
                        deletePersonAddress.Parameters.AddWithValue("@personID", teacherID);
                        deletePersonAddress.ExecuteNonQuery();

                        SqlCommand deleteTempAddress = new SqlCommand("DELETE FROM Address WHERE ID = @addID", connection);
                        deleteTempAddress.Parameters.AddWithValue("@addID", tempAddID);
                        deleteTempAddress.ExecuteNonQuery();

                        SqlCommand deletePermAddress = new SqlCommand("DELETE FROM Address WHERE ID = @addID", connection);
                        deletePermAddress.Parameters.AddWithValue("@addID", permAddID);
                        deletePermAddress.ExecuteNonQuery();

                        SqlCommand deleteStudent = new SqlCommand("DELETE FROM Teacher WHERE ID = @stdID", connection);
                        deleteStudent.Parameters.AddWithValue("@stdID", teacherID);
                        deleteStudent.ExecuteNonQuery();

                        SqlCommand deletePerson = new SqlCommand("DELETE FROM Person WHERE ID = @personID", connection);
                        deletePerson.Parameters.AddWithValue("@personID", teacherID);
                        deletePerson.ExecuteNonQuery();
                    }
                    errorMessage = "Something Went Wrong, Try Again.";
                }
                catch (Exception)
                {
                    errorMessage = "Something Went Wrong, Try Again.";
                }
            }
            TempData["Error"] = errorMessage;
            return RedirectToAction("Entry");
        }
    }    
}