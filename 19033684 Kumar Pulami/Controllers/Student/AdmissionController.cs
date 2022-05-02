using _19033684_Kumar_Pulami.Models.ViewModel.Student;
using _19033684_Kumar_Pulami.Services;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.SqlClient;

namespace _19033684_Kumar_Pulami.Controllers.Student
{
    public class AdmissionController : Controller
    {
        [HttpGet]
        public IActionResult AdmissionForm()
        {
            ViewBag.TitleName = "Admission";
            return View();
        }


        [HttpPost]
        public IActionResult AdmissionForm(AdmissionView studentData)
        {
            String? errorMessage = null;
            if (ModelState.IsValid)
            {
                String currentDateAndTime = DateTime.Now.ToString("yyyyMMddHHmmss");
                String StudentID = "ST" + currentDateAndTime;
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
                            command.Parameters.AddWithValue("@id", StudentID);
                            command.Parameters.AddWithValue("@name", studentData.Name);
                            command.Parameters.AddWithValue("@DOB", studentData.BOD);
                            command.Parameters.AddWithValue("@gender", studentData.Gender);
                            command.Parameters.AddWithValue("@father", studentData.FatherName);
                            command.Parameters.AddWithValue("@mother", studentData.MotherName);
                            command.ExecuteNonQuery();
                        }

                        using (SqlCommand command = new SqlCommand("INSERT INTO Student VALUES(@id, @batch, @grade, @section, @guardian_contact, @school_name, @school_garde)", connection))
                        {
                            command.Parameters.AddWithValue("@id", StudentID);
                            command.Parameters.AddWithValue("@batch", studentData.Batch);
                            command.Parameters.AddWithValue("@grade", studentData.Grade);
                            command.Parameters.AddWithValue("@section", studentData.Section);
                            command.Parameters.AddWithValue("@guardian_contact", studentData.GuardianContact);
                            command.Parameters.AddWithValue("@school_name", studentData.PreviousSchool);
                            command.Parameters.AddWithValue("@school_garde", studentData.PreviousSchoolGrade);
                            command.ExecuteNonQuery();
                        }

                        using (SqlCommand permanametAddressCommand = new SqlCommand("INSERT INTO Address VALUES(@id, @province, @district, @city, @ward)", connection))
                        {
                            permanametAddressCommand.Parameters.AddWithValue("@id", permAddID);
                            permanametAddressCommand.Parameters.AddWithValue("@province", studentData.PermamentProvince);
                            permanametAddressCommand.Parameters.AddWithValue("@district", studentData.PermamentDistrict);
                            permanametAddressCommand.Parameters.AddWithValue("@city", studentData.PermamentCity);
                            permanametAddressCommand.Parameters.AddWithValue("@ward", studentData.PermamentWard);
                            permanametAddressCommand.ExecuteNonQuery();
                        }

                        using (SqlCommand temporaryAddressCommand = new SqlCommand("INSERT INTO Address VALUES(@id, @province, @district, @city, @ward)", connection))
                        {
                            temporaryAddressCommand.Parameters.AddWithValue("@id", tempAddID);
                            temporaryAddressCommand.Parameters.AddWithValue("@province", studentData.TemporaryProvince);
                            temporaryAddressCommand.Parameters.AddWithValue("@district", studentData.TemporaryDistrict);
                            temporaryAddressCommand.Parameters.AddWithValue("@city", studentData.TemporaryCity);
                            temporaryAddressCommand.Parameters.AddWithValue("@ward", studentData.TemporaryWard);
                            temporaryAddressCommand.ExecuteNonQuery();
                        }
                    }

                    using (SqlConnection connection = new SqlConnection(DatabaseAccess.GetConnection()))
                    {
                        if (connection.State == System.Data.ConnectionState.Closed)
                        {
                            connection.Open();
                        }

                        using(SqlCommand personTempAddressCommand = new SqlCommand("INSERT INTO Person_Address VALUES(@stdID, @addressID, 'Temporary')", connection))
                        {
                            personTempAddressCommand.Parameters.AddWithValue("@stdID", StudentID);
                            personTempAddressCommand.Parameters.AddWithValue("@addressID", tempAddID);
                            personTempAddressCommand.ExecuteNonQuery();

                        }

                        using (SqlCommand personPerAddressCommand = new SqlCommand("INSERT INTO Person_Address VALUES(@stdID, @addressID, 'Permament')", connection))
                        {
                            personPerAddressCommand.Parameters.AddWithValue("@stdID", StudentID);
                            personPerAddressCommand.Parameters.AddWithValue("@addressID", permAddID);
                            personPerAddressCommand.ExecuteNonQuery();
                        }
                    }
                    TempData["SuccessMessage"] = "Student Admitted Successfully...!";
                    return RedirectToAction("AdmissionForm");

                }
                catch(SqlException)
                {
                    using (SqlConnection connection = new SqlConnection(DatabaseAccess.GetConnection()))
                    {
                        if (connection.State == ConnectionState.Closed)
                        {
                            connection.Open();
                        }

                        SqlCommand deletePersonAddress = new SqlCommand("DELETE FROM Person_Address WHERE Person_ID = @personID", connection);
                        deletePersonAddress.Parameters.AddWithValue("@personID", StudentID);
                        deletePersonAddress.ExecuteNonQuery();

                        SqlCommand deleteTempAddress = new SqlCommand("DELETE FROM Address WHERE ID = @addID", connection);
                        deleteTempAddress.Parameters.AddWithValue("@addID", tempAddID);
                        deleteTempAddress.ExecuteNonQuery();

                        SqlCommand deletePermAddress = new SqlCommand("DELETE FROM Address WHERE ID = @addID", connection);
                        deletePermAddress.Parameters.AddWithValue("@addID", permAddID);
                        deletePermAddress.ExecuteNonQuery();

                        SqlCommand deleteStudent = new SqlCommand("DELETE FROM Student WHERE ID = @stdID", connection);
                        deleteStudent.Parameters.AddWithValue("@stdID", StudentID);
                        deleteStudent.ExecuteNonQuery();

                        SqlCommand deletePerson = new SqlCommand("DELETE FROM Person WHERE ID = @personID", connection);
                        deletePerson.Parameters.AddWithValue("@personID", StudentID);
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
            return RedirectToAction("AdmissionForm");
        }
    }
}
