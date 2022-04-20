using _19033684_Kumar_Pulami.Models.ViewModel;
using _19033684_Kumar_Pulami.Services;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.SqlClient;

namespace _19033684_Kumar_Pulami.Controllers.Subject
{
    public class SubjectEntryController : Controller
    {
        [HttpGet]
        public IActionResult SubjectEntry()
        {
            ViewBag.TitleName = "Subject Entry";
            return View();
        }

        [HttpPost]
        public IActionResult SubjectEntry(SubjectViewModel value)
        {
            if (IsDuplicateEntry(value.SubjectID))
            {
                TempData["Error"] = "Duplicate Entry...!";
                return View(value);
            }
            else
            {
                using (SqlConnection connection = new SqlConnection(DatabaseAccess.GetConnection()))
                {
                    if (connection.State == ConnectionState.Closed)
                    {
                        connection.Open();
                    }
                    using (SqlCommand command = new SqlCommand("INSERT INTO Subject VALUES(@id, @name);", connection))
                    {
                        command.Parameters.AddWithValue("@name", value.SubjectName);
                        command.Parameters.AddWithValue("@id", value.SubjectID);
                        command.ExecuteNonQuery();
                    }
                }
                TempData["SuccessMessage"] = "Subject Registered Successfully...!";
                return RedirectToAction("SubjectEntry");
            }
        }

        public bool IsDuplicateEntry(int subjectID)
        {
            DataTable queryData;
            SubjectViewModel subjectDetail = new SubjectViewModel();
            using (SqlConnection connection = new SqlConnection(DatabaseAccess.GetConnection()))
            {
                if (connection.State == ConnectionState.Closed)
                {
                    connection.Open();
                }
                using (SqlCommand command = new SqlCommand("SELECT * FROM Subject WHERE Subject.ID = @subject;", connection))
                {
                    command.Parameters.AddWithValue("@subject", subjectID);
                    using (SqlDataAdapter dataAdapter = new SqlDataAdapter(command))
                    {
                        queryData = new DataTable();
                        dataAdapter.Fill(queryData);
                    }
                }
                if (queryData.Rows.Count != 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
    }
}
