using _19033684_Kumar_Pulami.Models.ViewModel;
using _19033684_Kumar_Pulami.Models.ViewModel.Subject;
using _19033684_Kumar_Pulami.Services;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.SqlClient;

namespace _19033684_Kumar_Pulami.Controllers
{
    public class ManageSubjectController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            ViewBag.TitleName = "Manage Subject";
            SubjectIndexViewModel model = new SubjectIndexViewModel();
            model.SubjectList = GetSubjectList();
            return View(model);
        }


        [HttpPost]
        public IActionResult Index(SubjectIndexViewModel value)
        {
            ViewBag.TitleName = "Manage Subject";
            SubjectIndexViewModel model = new SubjectIndexViewModel();
            model.SubjectList = SearchSubjet(value.SearchValue);
            return View(model);
        }


        public List<SubjectViewModel> GetSubjectList()
        {
            DataTable queryData;
            List<SubjectViewModel> subjecList = new List<SubjectViewModel>();
            SubjectViewModel subjectDetail;
            using (SqlConnection connection = new SqlConnection(DatabaseAccess.GetConnection()))
            {
                if (connection.State == ConnectionState.Closed)
                {
                    connection.Open();
                }
                using (SqlCommand command = new SqlCommand("SELECT * FROM Subject;", connection))
                {
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
                        subjectDetail = new SubjectViewModel();
                        subjectDetail.SubjectID = int.Parse(row[0].ToString());
                        subjectDetail.SubjectName = row[1].ToString();
                        subjecList.Add(subjectDetail);
                    }
                }
            }
            return subjecList;
        }

        public List<SubjectViewModel> SearchSubjet(String subjectname)
        {
            DataTable queryData;
            List<SubjectViewModel> subjecList = new List<SubjectViewModel>();
            SubjectViewModel subjectDetail;
            using (SqlConnection connection = new SqlConnection(DatabaseAccess.GetConnection()))
            {
                if (connection.State == ConnectionState.Closed)
                {
                    connection.Open();
                }
                using (SqlCommand command = new SqlCommand("SELECT * FROM Subject WHERE Subject.SubjectName = @subject;", connection))
                {
                    command.Parameters.AddWithValue("@subject", subjectname);
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
                        subjectDetail = new SubjectViewModel();
                        subjectDetail.SubjectID = int.Parse(row[0].ToString());
                        subjectDetail.SubjectName = row[1].ToString();
                        subjecList.Add(subjectDetail);
                    }
                }
            }
            return subjecList;
        }

        public SubjectViewModel GetSubjectDetails(int subjectID)
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
                    foreach (DataRow row in queryData.Rows)
                    {
                        subjectDetail.SubjectID = int.Parse(row[0].ToString());
                        subjectDetail.SubjectName = row[1].ToString();
                    }
                }
            }

            return subjectDetail;
        }

        [HttpGet]
        public IActionResult Edit(int subjectID)
        {
            ViewBag.TitleName = "Edit Subject Details";
            SubjectViewModel model = GetSubjectDetails(subjectID);
            return View(model);
        }


        [HttpPost]
        public IActionResult Edit(SubjectViewModel value)
        {
            using (SqlConnection connection = new SqlConnection(DatabaseAccess.GetConnection()))
            {
                if (connection.State == ConnectionState.Closed)
                {
                    connection.Open();
                }
                using (SqlCommand command = new SqlCommand("UPDATE Subject SET SubjectName = @name WHERE ID = @id;", connection))
                {
                    command.Parameters.AddWithValue("@name", value.SubjectName);
                    command.Parameters.AddWithValue("@id", value.SubjectID);
                    command.ExecuteNonQuery();
                }
            }
            TempData["SuccessMessage"] = "Subject Infromation Updated Successfully...!";
            return RedirectToAction("Index");
        }



        [HttpGet]
        public IActionResult Details(int subjectID)
        {
            ViewBag.TitleName = "Subject Details";
            SubjectViewModel model = GetSubjectDetails(subjectID);
            return View(model);
        }


        [HttpGet]
        public IActionResult Delete(int subjectID)
        {
            ViewBag.TitleName = "Delete Subject";
            SubjectViewModel model = GetSubjectDetails(subjectID);
            return View(model);
        }


        [HttpPost]
        public IActionResult Deleteconformed(int subjectID)
        {
            using (SqlConnection connection = new SqlConnection(DatabaseAccess.GetConnection()))
            {
                if (connection.State == ConnectionState.Closed)
                {
                    connection.Open();
                }
                using (SqlCommand command = new SqlCommand("DELETE FROM Marks WHERE subjectID = @id", connection))
                {                    
                    command.Parameters.AddWithValue("@id", subjectID);
                    command.ExecuteNonQuery();
                }
            }

            using (SqlConnection connection = new SqlConnection(DatabaseAccess.GetConnection()))
            {
                if (connection.State == ConnectionState.Closed)
                {
                    connection.Open();
                }
                using (SqlCommand command = new SqlCommand("DELETE FROM Subject WHERE ID = @id", connection))
                {
                    command.Parameters.AddWithValue("@id", subjectID);
                    command.ExecuteNonQuery();
                }
            }
            TempData["SuccessMessage"] = "Subject Infromation Deleted Successfully...!";
            return RedirectToAction("Index");
        }
    }
}
