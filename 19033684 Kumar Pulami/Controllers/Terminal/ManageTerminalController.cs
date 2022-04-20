using _19033684_Kumar_Pulami.Models.ViewModel.Terminal;
using _19033684_Kumar_Pulami.Services;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.SqlClient;

namespace _19033684_Kumar_Pulami.Controllers.Terminal
{
    public class ManageTerminalController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            ViewBag.TitleName = "Manage Terminal";
            TerminalIndexViewModel model = new TerminalIndexViewModel();
            model.TerminalList = GetTerminalList();
            return View(model);
        }


        [HttpPost]
        public IActionResult Index(TerminalIndexViewModel value)
        {
            ViewBag.TitleName = "Manage Terminal";
            TerminalIndexViewModel model = new TerminalIndexViewModel();
            model.TerminalList = SearchSubjet(value.SearchValue);
            return View(model);
        }


        public List<TerminalViewModel> GetTerminalList()
        {
            DataTable queryData;
            List<TerminalViewModel> subjecList = new List<TerminalViewModel>();
            TerminalViewModel terminalDetail;
            using (SqlConnection connection = new SqlConnection(DatabaseAccess.GetConnection()))
            {
                if (connection.State == ConnectionState.Closed)
                {
                    connection.Open();
                }
                using (SqlCommand command = new SqlCommand("SELECT * FROM Terminal;", connection))
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
                        terminalDetail = new TerminalViewModel();
                        terminalDetail.TerminalID = int.Parse(row[0].ToString());
                        terminalDetail.TerminalName = row[1].ToString();
                        subjecList.Add(terminalDetail);
                    }
                }
            }
            return subjecList;
        }

        public List<TerminalViewModel> SearchSubjet(String terminalname)
        {
            DataTable queryData;
            List<TerminalViewModel> subjecList = new List<TerminalViewModel>();
            TerminalViewModel terminalDetail;
            using (SqlConnection connection = new SqlConnection(DatabaseAccess.GetConnection()))
            {
                if (connection.State == ConnectionState.Closed)
                {
                    connection.Open();
                }
                using (SqlCommand command = new SqlCommand("SELECT * FROM Terminal WHERE Terminal.TerminalName = @terminal;", connection))
                {
                    command.Parameters.AddWithValue("@terminal", terminalname);
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
                        terminalDetail = new TerminalViewModel();
                        terminalDetail.TerminalID = int.Parse(row[0].ToString());
                        terminalDetail.TerminalName = row[1].ToString();
                        subjecList.Add(terminalDetail);
                    }
                }
            }
            return subjecList;
        }

        public TerminalViewModel GetTerminalDetails(int terminalID)
        {
            DataTable queryData;
            TerminalViewModel terminalDetail = new TerminalViewModel();
            using (SqlConnection connection = new SqlConnection(DatabaseAccess.GetConnection()))
            {
                if (connection.State == ConnectionState.Closed)
                {
                    connection.Open();
                }
                using (SqlCommand command = new SqlCommand("SELECT * FROM Terminal WHERE Terminal.ID = @terminal;", connection))
                {
                    command.Parameters.AddWithValue("@terminal", terminalID);
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
                        terminalDetail.TerminalID = int.Parse(row[0].ToString());
                        terminalDetail.TerminalName = row[1].ToString();
                    }
                }
            }

            return terminalDetail;
        }

        [HttpGet]
        public IActionResult Edit(int terminalID)
        {
            ViewBag.TitleName = "Edit Terminal Details";
            TerminalViewModel model = GetTerminalDetails(terminalID);
            return View(model);
        }


        [HttpPost]
        public IActionResult Edit(TerminalViewModel value)
        {
            using (SqlConnection connection = new SqlConnection(DatabaseAccess.GetConnection()))
            {
                if (connection.State == ConnectionState.Closed)
                {
                    connection.Open();
                }
                using (SqlCommand command = new SqlCommand("UPDATE Terminal SET TerminalName = @name WHERE ID = @id;", connection))
                {
                    command.Parameters.AddWithValue("@name", value.TerminalName);
                    command.Parameters.AddWithValue("@id", value.TerminalID);
                    command.ExecuteNonQuery();
                }
            }
            TempData["SuccessMessage"] = "Terminal Infromation Updated Successfully...!";
            return RedirectToAction("Index");
        }



        [HttpGet]
        public IActionResult Details(int terminalID)
        {
            ViewBag.TitleName = "Terminal Details";
            TerminalViewModel model = GetTerminalDetails(terminalID);
            return View(model);
        }


        [HttpGet]
        public IActionResult Delete(int terminalID)
        {
            ViewBag.TitleName = "Delete Terminal";
            TerminalViewModel model = GetTerminalDetails(terminalID);
            return View(model);
        }


        [HttpPost]
        public IActionResult Deleteconformed(int terminalID)
        {
            using (SqlConnection connection = new SqlConnection(DatabaseAccess.GetConnection()))
            {
                if (connection.State == ConnectionState.Closed)
                {
                    connection.Open();
                }
                using (SqlCommand command = new SqlCommand("DELETE FROM Marks WHERE terminalID = @id", connection))
                {
                    command.Parameters.AddWithValue("@id", terminalID);
                    command.ExecuteNonQuery();
                }
            }

            using (SqlConnection connection = new SqlConnection(DatabaseAccess.GetConnection()))
            {
                if (connection.State == ConnectionState.Closed)
                {
                    connection.Open();
                }
                using (SqlCommand command = new SqlCommand("DELETE FROM Terminal WHERE ID = @id", connection))
                {
                    command.Parameters.AddWithValue("@id", terminalID);
                    command.ExecuteNonQuery();
                }
            }
            TempData["SuccessMessage"] = "Terminal Infromation Deleted Successfully...!";
            return RedirectToAction("Index");
        }
    }
}
