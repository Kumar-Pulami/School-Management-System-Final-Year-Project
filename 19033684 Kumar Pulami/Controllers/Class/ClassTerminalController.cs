using _19033684_Kumar_Pulami.Models.DatabaseModel.Exam;
using _19033684_Kumar_Pulami.Models.ViewModel.BatchGradeTerminal;
using _19033684_Kumar_Pulami.Services;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.SqlClient;

namespace _19033684_Kumar_Pulami.Controllers.Class
{
    public class ClassTerminalController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            ViewBag.TitleName = "Assign Terminal";
            ViewBag.BatchList = DropDownFilter.GetBatchList();

            List<String> gradeList = new List<String>();
            gradeList.Add("Select Grade");
            ViewBag.GradeList = gradeList;
            BatchGradeTerminalViewModel model = new BatchGradeTerminalViewModel();
            model.TerminalList = null;
            return View(model);
        }


        [HttpPost]
        public IActionResult Index(BatchGradeTerminalViewModel value)
        {
            ViewBag.TitleName = "Assign Terminal";
            ViewBag.BatchList = DropDownFilter.GetBatchList();
            ViewBag.GradeList = DropDownFilter.GetGradeList(value.Batch);
            value.TerminalList = GetTerminalList(value.Batch, value.Grade);
            return View(value);
        }


        [HttpGet]
        public IActionResult AssignTerminal()
        {
            ViewBag.TitleName = "Assign Terminal";

            ViewBag.BatchList = DropDownFilter.GetBatchList();

            List<String> gradeList = new List<String>();
            gradeList.Add("Select Grade");
            ViewBag.GradeList = gradeList;


            TerminalDataDatabaseModel termianlDetails = new TerminalDataDatabaseModel();
            termianlDetails.TerminalID = 0;
            termianlDetails.TerminalName = "Select Terminal";
            List<TerminalDataDatabaseModel> terminalList = new List<TerminalDataDatabaseModel>();
            terminalList.Add(termianlDetails);

            ViewBag.TerminalList = terminalList;

            return View();
        }

        [HttpPost]
        public IActionResult AssignTerminal(BatchGradeTerminalAssignViewModel value)
        {
            using (SqlConnection connection = new SqlConnection(DatabaseAccess.GetConnection()))
            {
                if (connection.State == ConnectionState.Closed)
                {
                    connection.Open();
                }
                using (SqlCommand command = new SqlCommand("INSERT INTO BatchGradeTerminal VALUES(@batch, @grade, @terminal);", connection))
                {
                    command.Parameters.AddWithValue("@batch", value.Batch);
                    command.Parameters.AddWithValue("@grade", value.Grade);
                    command.Parameters.AddWithValue("@terminal", value.TerminalID);
                    command.ExecuteNonQuery();
                }
            }
            AssignTerminalToStudent(value.Batch, value.Grade, value.TerminalID);
            TempData["SuccessMessage"] = "Terminal Assigned Successfully...!";
            return RedirectToAction("Index");
        }


        [HttpGet]
        public IActionResult Delete(int batch, int grade, int TermianlID)
        {
            using (SqlConnection connection = new SqlConnection(DatabaseAccess.GetConnection()))
            {
                if (connection.State == ConnectionState.Closed)
                {
                    connection.Open();
                }
                using (SqlCommand command = new SqlCommand("DELETE FROM BatchGradeTerminal WHERE Batch = @batch AND Grade = @grade AND TerminalID = @terminal;", connection))
                {
                    command.Parameters.AddWithValue("@batch", batch);
                    command.Parameters.AddWithValue("@grade", grade);
                    command.Parameters.AddWithValue("@terminal", TermianlID);
                    command.ExecuteNonQuery();
                }
            }

            using (SqlConnection connection = new SqlConnection(DatabaseAccess.GetConnection()))
            {
                if (connection.State == ConnectionState.Closed)
                {
                    connection.Open();
                }
                using (SqlCommand command = new SqlCommand("DELETE Marks FROM Marks JOIN Student ON Marks.StudentID = Student.ID WHERE Student.Batch = @batch AND Student.Grade = @grade AND Marks.TerminalID = @terminal;", connection))
                {
                    command.Parameters.AddWithValue("@batch", batch);
                    command.Parameters.AddWithValue("@grade", grade);
                    command.Parameters.AddWithValue("@terminal", TermianlID);
                    command.ExecuteNonQuery();
                }
            }
            TempData["SuccessMessage"] = "Terminal Infromation Deleted Successfully...!";
            BatchGradeTerminalAssignViewModel model = new BatchGradeTerminalAssignViewModel();
            model.Batch = batch;
            model.Grade = grade;
            return RedirectToAction("Index", model);
        }



        public List<TerminalDataDatabaseModel> GetTerminalList(int batch, int grade)
        {
            DataTable queryData;
            List<TerminalDataDatabaseModel> terminalList = new List<TerminalDataDatabaseModel>();
            TerminalDataDatabaseModel termianlDetails;
            using (SqlConnection connection = new SqlConnection(DatabaseAccess.GetConnection()))
            {
                if (connection.State == ConnectionState.Closed)
                {
                    connection.Open();
                }
                using (SqlCommand command = new SqlCommand("SELECT Terminal.ID, Terminal.TerminalName FROM BatchGradeTerminal JOIN Terminal ON BatchGradeTerminal.TerminalID = Terminal.ID WHERE BatchGradeTerminal.Grade = @grade AND BatchGradeTerminal.Batch = @batch;", connection))
                {
                    command.Parameters.AddWithValue("@batch", batch);
                    command.Parameters.AddWithValue("@grade", grade);
                    using (SqlDataAdapter dataAdapter = new SqlDataAdapter(command))
                    {
                        queryData = new DataTable();
                        dataAdapter.Fill(queryData);
                    }
                }
                if (queryData.Rows.Count != 0)
                {
                    foreach (DataRow row in queryData.Rows)
                    {
                        termianlDetails = new TerminalDataDatabaseModel();
                        termianlDetails.TerminalID = int.Parse(row[0].ToString());
                        termianlDetails.TerminalName = row[1].ToString();
                        terminalList.Add(termianlDetails);
                    }
                }
            }
            return terminalList;
        }

        public List<TerminalDataDatabaseModel> GetNonAssignedTermianlList(int batch, int grade)
        {
            DataTable queryData;
            List<TerminalDataDatabaseModel> terminalList = new List<TerminalDataDatabaseModel>();
            TerminalDataDatabaseModel termianlDetails;
            using (SqlConnection connection = new SqlConnection(DatabaseAccess.GetConnection()))
            {
                if (connection.State == ConnectionState.Closed)
                {
                    connection.Open();
                }
                using (SqlCommand command = new SqlCommand("SELECT * FROM Terminal WHERE Terminal.ID NOT IN (SELECT TerminalID FROM BatchGradeTerminal WHERE Batch = @batch AND Grade = @grade);", connection))
                {
                    command.Parameters.AddWithValue("@batch", batch);
                    command.Parameters.AddWithValue("@grade", grade);
                    using (SqlDataAdapter dataAdapter = new SqlDataAdapter(command))
                    {
                        queryData = new DataTable();
                        dataAdapter.Fill(queryData);
                    }
                }
                if (queryData.Rows.Count != 0)
                {
                    foreach (DataRow row in queryData.Rows)
                    {
                        termianlDetails = new TerminalDataDatabaseModel();
                        termianlDetails.TerminalID = int.Parse(row[0].ToString());
                        termianlDetails.TerminalName = row[1].ToString();
                        terminalList.Add(termianlDetails);
                    }
                }
            }
            return terminalList;
        }

        public JsonResult GetNonAssignedTerminalJSON(int batch, int grade)
        {
            return Json(GetNonAssignedTermianlList(batch, grade));
        }

        public void AssignTerminalToStudent(int batch, int grade, int terminal)
        {
            DataTable queryData;
            List<String> studentlist = new List<string>();
            // Getting Students list acc to batch and grade;
            using (SqlConnection connection = new SqlConnection(DatabaseAccess.GetConnection()))
            {
                if (connection.State == ConnectionState.Closed)
                {
                    connection.Open();
                }
                using (SqlCommand command = new SqlCommand("SELECT Student.ID FROM Student WHERE Batch = @batch AND Grade = @grade;", connection))
                {
                    command.Parameters.AddWithValue("@batch", batch);
                    command.Parameters.AddWithValue("@grade", grade);
                    using (SqlDataAdapter dataAdapter = new SqlDataAdapter(command))
                    {
                        queryData = new DataTable();
                        dataAdapter.Fill(queryData);
                    }
                }
                if (queryData.Rows.Count != 0)
                {
                    String studentID;
                    foreach (DataRow row in queryData.Rows)
                    {
                        studentID = row[0].ToString();
                        studentlist.Add(studentID);
                    }
                }
            }

            // Getting Subject Assigned to batch and grade
            List<int> subjectList = new List<int>();
            using (SqlConnection connection = new SqlConnection(DatabaseAccess.GetConnection()))
            {
                if (connection.State == ConnectionState.Closed)
                {
                    connection.Open();
                }
                using (SqlCommand command = new SqlCommand("SELECT SubjectID FROM BatchGradeSubject WHERE Batch = @batch AND Grade = @grade;", connection))
                {
                    command.Parameters.AddWithValue("@batch", batch);
                    command.Parameters.AddWithValue("@grade", grade);
                    using (SqlDataAdapter dataAdapter = new SqlDataAdapter(command))
                    {
                        queryData = new DataTable();
                        dataAdapter.Fill(queryData);
                    }
                }
                if (queryData.Rows.Count != 0)
                {
                    int subjectID;
                    foreach (DataRow row in queryData.Rows)
                    {
                        subjectID = int.Parse(row[0].ToString());
                        subjectList.Add(subjectID);
                    }
                }
            }


            int marks = 0;

            //Assign student to terminal.
            foreach (String studentID in studentlist)
            {
                foreach (int subjectID in subjectList) {
                    using (SqlConnection connection = new SqlConnection(DatabaseAccess.GetConnection()))
                    {
                        if (connection.State == ConnectionState.Closed)
                        {
                            connection.Open();
                        }
                        using (SqlCommand command = new SqlCommand("INSERT INTO Marks VALUES(@studentID, @terminalID, @subjectID, @total, @pass, @obtained);", connection))
                        {
                            command.Parameters.AddWithValue("@terminalID", terminal);
                            command.Parameters.AddWithValue("@studentID", studentID);
                            command.Parameters.AddWithValue("@subjectID", subjectID);
                            command.Parameters.AddWithValue("@total", marks);
                            command.Parameters.AddWithValue("@pass", marks);
                            command.Parameters.AddWithValue("@obtained", marks);
                            command.ExecuteNonQuery();
                        }
                    }
                }
            }        
        }
    }
} 
