using _19033684_Kumar_Pulami.Models.ViewModel.Terminal;
using _19033684_Kumar_Pulami.Services;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.SqlClient;

namespace _19033684_Kumar_Pulami.Controllers.Terminal
{
    public class TerminalEntryController : Controller
    {
        [HttpGet]
        public IActionResult TerminalEntry()
        {
            ViewBag.TitleName = "Terminal Entry";
            return View();
        }

        [HttpPost]
        public IActionResult TerminalEntry(TerminalViewModel value)
        {
            if (IsDuplicateEntry(value.TerminalID))
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
                    using (SqlCommand command = new SqlCommand("INSERT INTO Terminal VALUES(@id, @name);", connection))
                    {
                        command.Parameters.AddWithValue("@name", value.TerminalName);
                        command.Parameters.AddWithValue("@id", value.TerminalID);
                        command.ExecuteNonQuery();
                    }
                }
                TempData["SuccessMessage"] = "Terminal Registered Successfully...!";
                return RedirectToAction("TerminalEntry");
            }
        }

        public bool IsDuplicateEntry(int terminalID)
        {
            DataTable queryData;
            TerminalViewModel subjectDetail = new TerminalViewModel();
            using (SqlConnection connection = new SqlConnection(DatabaseAccess.GetConnection()))
            {
                if (connection.State == ConnectionState.Closed)
                {
                    connection.Open();
                }
                using (SqlCommand command = new SqlCommand("SELECT * FROM Terminal WHERE Terminal.ID = @id;", connection))
                {
                    command.Parameters.AddWithValue("@id", terminalID);
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
