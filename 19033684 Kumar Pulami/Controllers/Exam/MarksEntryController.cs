using _19033684_Kumar_Pulami.Models.ViewModel.Exam.Marks_Entry;
using _19033684_Kumar_Pulami.Models.ViewModel.Exam.Result;
using _19033684_Kumar_Pulami.Services;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.SqlClient;

namespace _19033684_Kumar_Pulami.Controllers.Exam
{
    public class MarksEntryController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            ViewBag.TitleName = "Marks Entry";
            return View();
        }

        [HttpGet]
        public IActionResult MarksEntry(MarksEntrySearchViewModel searchContent)
        {
            ViewBag.TitleName = "Marks Entry";
            MarksEntryIndexViewModel model = new MarksEntryIndexViewModel();
            model = GetEntryList(searchContent.Batch, searchContent.Grade, searchContent.Section, searchContent.Terminal);
            return View(model);
        }

        [HttpPost]
        public IActionResult MarksEntry(MarksEntryIndexViewModel model)
        {
            ViewBag.TitleName = "Marks Entry";
            return View("Index");
        }

        [HttpPost]
        public void MarksUpdate(string marks)
        {

        }

        public MarksEntryIndexViewModel GetEntryList(int? batch, int? grade, String? section, int? terminal)
        {
            MarksEntryIndexViewModel model = new MarksEntryIndexViewModel();
            // Getting Subject Details 
            List<MarksEntrySubjectViewModel> subjectDetails = new List<MarksEntrySubjectViewModel>();
            DataTable queryData;
            using (SqlConnection connection = new SqlConnection(DatabaseAccess.GetConnection()))
            {
                if (connection.State == ConnectionState.Closed)
                {
                    connection.Open();
                }
                using (SqlCommand command = new SqlCommand("SELECT DISTINCT(Subject.ID), Subject.SubjectName FROM Marks JOIN Subject ON Marks.SubjectID = Subject.ID JOIN Student ON Marks.StudentID = Student.ID WHERE Student.Batch = @batch AND Student.Grade = @grade AND Marks.TerminalID = @id ORDER BY Subject.ID ASC;", connection))
                {
                    command.Parameters.AddWithValue("@batch", batch);
                    command.Parameters.AddWithValue("@grade", grade);
                    command.Parameters.AddWithValue("@id", terminal);
                    using (SqlDataAdapter dataAdapter = new SqlDataAdapter(command))
                    {
                        queryData = new DataTable();
                        dataAdapter.Fill(queryData);
                    }
                }
                if (queryData.Rows.Count != 0)
                {
                    MarksEntrySubjectViewModel subject;
                    foreach (DataRow row in queryData.Rows)
                    {
                        subject = new MarksEntrySubjectViewModel();
                        subject.SubjectID = int.Parse(row[0].ToString());
                        subject.SubjectName = row[1].ToString();
                        subjectDetails.Add(subject);
                    }
                }
            }

            //Gettings student obtained marks
            List<String> studentList = new List<string>();
            using (SqlConnection connection = new SqlConnection(DatabaseAccess.GetConnection()))
            {
                if (connection.State == ConnectionState.Closed)
                {
                    connection.Open();
                }
                using (SqlCommand command = new SqlCommand("SELECT DISTINCT(Marks.StudentID) FROM Marks JOIN STUDENT ON Marks.StudentID = Student.ID WHERE Student.Batch = @batch AND Student.Grade = @grade AND Student.Section = @sec AND Marks.TerminalID = @id ORDER BY Marks.StudentID ASC;", connection))
                {
                    command.Parameters.AddWithValue("@batch", batch);
                    command.Parameters.AddWithValue("@grade", grade);
                    command.Parameters.AddWithValue("@sec", section);
                    command.Parameters.AddWithValue("@id", terminal);
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
                        studentList.Add(row[0].ToString());
                    }
                }
            }

            // Getting marks of each student of each subjects
            List<MarksEntryStudentDetailsViewModel> studentMarksDetailsList = new List<MarksEntryStudentDetailsViewModel>();
            using (SqlConnection connection = new SqlConnection(DatabaseAccess.GetConnection()))
            {
                if (connection.State == ConnectionState.Closed)
                {
                    connection.Open();
                }

                MarksEntryStudentDetailsViewModel studentMarksDetailDBModel;
                foreach (String studentID in studentList)
                {
                    studentMarksDetailDBModel = new MarksEntryStudentDetailsViewModel();
                    MarksEntryMarksDetailViewModel marksDetailsDB;
                    studentMarksDetailDBModel.StudentID = studentID;
                    studentMarksDetailDBModel.StudentName = GetStudentName(studentID);
                    List<MarksEntryMarksDetailViewModel> marksDetailsDBModelList = new List<MarksEntryMarksDetailViewModel>();

                    foreach (MarksEntrySubjectViewModel subject in subjectDetails)
                    {
                        using (SqlCommand command = new SqlCommand("SELECT Marks.Obtained_Mark FROM Marks WHERE Marks.TerminalID = @terminalID AND Marks.StudentID = @stdID AND Marks.SubjectID =@subID;", connection))
                        {
                            command.Parameters.AddWithValue("@terminalID", terminal);
                            command.Parameters.AddWithValue("@stdID", studentID);
                            command.Parameters.AddWithValue("@subID", subject.SubjectID);
                            using (SqlDataAdapter dataAdapter = new SqlDataAdapter(command))
                            {
                                queryData = new DataTable();
                                dataAdapter.Fill(queryData);
                            }
                        }
                        if (queryData.Rows.Count != 0)
                        {
                            marksDetailsDB = new MarksEntryMarksDetailViewModel();
                            foreach (DataRow row in queryData.Rows)
                            {
                                marksDetailsDB.ObtainedMarks = row[0].ToString();
                                marksDetailsDBModelList.Add(marksDetailsDB);
                            }
                        }
                    }
                    studentMarksDetailDBModel.MarksDetails = marksDetailsDBModelList;
                    studentMarksDetailsList.Add(studentMarksDetailDBModel);
                }
            }

            //Getting subject Total Marks and pass marks
            List<TotalMarksViewModel> totalMarksList = new List<TotalMarksViewModel>();
            List<PassMarksViewModel> passMarksList = new List<PassMarksViewModel>();
            foreach (MarksEntrySubjectViewModel subject in subjectDetails)
            {
                using (SqlConnection connection = new SqlConnection(DatabaseAccess.GetConnection()))
                {
                    if (connection.State == ConnectionState.Closed)
                    {
                        connection.Open();
                    }
                    using (SqlCommand command = new SqlCommand("SELECT DISTINCT(Marks.SubjectID), Marks.Total_Mark, Marks.Pass_Mark FROM Marks JOIN STUDENT ON Marks.StudentID = Student.ID WHERE Student.Batch = @batch AND Student.Grade = @grade AND Student.Section = @sec AND Marks.TerminalID = @id AND Marks.SubjectID = @subID;", connection))
                    {
                        command.Parameters.AddWithValue("@batch", batch);
                        command.Parameters.AddWithValue("@grade", grade);
                        command.Parameters.AddWithValue("@sec", section);
                        command.Parameters.AddWithValue("@id", terminal);
                        command.Parameters.AddWithValue("@subID", subject.SubjectID);
                        using (SqlDataAdapter dataAdapter = new SqlDataAdapter(command))
                        {
                            queryData = new DataTable();
                            dataAdapter.Fill(queryData);
                        }
                    }
                    if (queryData.Rows.Count != 0)
                    {
                        TotalMarksViewModel totalMarks;
                        PassMarksViewModel passMarks;
                        foreach (DataRow row in queryData.Rows)
                        {
                            totalMarks = new TotalMarksViewModel();
                            passMarks = new PassMarksViewModel();
                            int subjectID = int.Parse(row[0].ToString());
                            totalMarks.StubjectID = subjectID;
                            totalMarks.TotalMarks = float.Parse(row[1].ToString());
                            passMarks.StubjectID = subjectID;
                            passMarks.PassMarks = float.Parse(row[2].ToString());
                            totalMarksList.Add(totalMarks);
                            passMarksList.Add(passMarks);
                        }
                    }
                }
            }
            model.Batch = batch;
            model.Grade = grade;
            model.Terminal = terminal;
            model.Section = section;
            model.StudentDetails = studentMarksDetailsList;
            model.SubjectDetails = subjectDetails;
            model.TotalMarks = totalMarksList;
            model.PassMarks = passMarksList;
            return model;
        }


        public String? GetStudentName(String studentID)
        {
            String? studentName = null;
            DataTable queryData;
            using (SqlConnection connection = new SqlConnection(DatabaseAccess.GetConnection()))
            {
                if (connection.State == ConnectionState.Closed)
                {
                    connection.Open();
                }
                using (SqlCommand command = new SqlCommand("SELECT Person.Name from Person WHERE Person.ID = @id", connection))
                {
                    command.Parameters.AddWithValue("@id", studentID);
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
                        studentName = row[0].ToString();
                    }
                }
            }
            return studentName;
        }
    }
}
