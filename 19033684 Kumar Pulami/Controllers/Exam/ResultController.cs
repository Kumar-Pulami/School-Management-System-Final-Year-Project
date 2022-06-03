using _19033684_Kumar_Pulami.Models.ViewModel.Exam.Result;
using _19033684_Kumar_Pulami.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Data;
using System.Data.SqlClient;

namespace _19033684_Kumar_Pulami.Controllers.Exam
{
    public class ResultController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            ViewBag.TitleName = "Result";
            ResultStudentMarksDetailIndexViewModel model = new ResultStudentMarksDetailIndexViewModel();
            model.StudentResultList = null;
            ViewBag.BatchList = DropDownFilter.GetBatchList();

            List<String> gradeList = new List<String>();
            gradeList.Add("Select Grade");
            ViewBag.GradeList = gradeList;

            List<ResultTerminalViewModel> terminalList = new List<ResultTerminalViewModel>();
            ResultTerminalViewModel terminal = new ResultTerminalViewModel();
            terminal.TerminalID = 0;
            terminal.TerminalName = "Select Terminal";
            terminalList.Add(terminal);
            ViewBag.TerminalList = terminalList;
            return View(model);
        }

        [HttpPost]
        public IActionResult Index(ResultStudentMarksDetailIndexViewModel searchDetails)
        {
            if(ModelState.IsValid)
            {
                ViewBag.TitleName = "Result";
                ViewBag.BatchList = DropDownFilter.GetBatchList();
                ViewBag.GradeList = DropDownFilter.GetGradeList(searchDetails.Batch);
                ViewBag.TerminalList = DropDownFilter.GetTerminalList(searchDetails.Batch, searchDetails.Grade);
                ResultStudentMarksDetailIndexViewModel model = GenerateResult(searchDetails.Batch, searchDetails.Grade, searchDetails.Terminal);
                return View(model);
            }
            return View();
        }

        
        

        public JsonResult GetTerminalListJson(int batch, int grade)
        {
            return Json(DropDownFilter.GetTerminalList(batch, grade));
        }

        public JsonResult GetGradeListJson(int batch)
        {
            return Json(DropDownFilter.GetGradeList(batch));
        }


        public ResultStudentMarksDetailIndexViewModel GenerateResult(int? batch, int? grade, int? terminal)
        {
            ResultStudentMarksDetailIndexViewModel model = new ResultStudentMarksDetailIndexViewModel();
            model.Batch = batch;
            model.Grade = grade;
            model.Terminal = terminal;


            // Getting Subject Details 
            List<SubjectDetailsViewModel> subjectDetails = new List<SubjectDetailsViewModel>();
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
                    SubjectDetailsViewModel subject;
                    foreach (DataRow row in queryData.Rows)
                    {
                        subject = new SubjectDetailsViewModel();
                        subject.SubjectID = int.Parse(row[0].ToString());
                        subject.SubjectName = row[1].ToString();
                        subjectDetails.Add(subject);
                    }
                }
            }

            using (SqlConnection connection = new SqlConnection(DatabaseAccess.GetConnection()))
            {
                if (connection.State == ConnectionState.Closed)
                {
                    connection.Open();
                }
                using (SqlCommand command = new SqlCommand("SELECT Terminal.TerminalName From Terminal WHERE ID = @id;", connection))
                {
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
                        model.TerminalName = row[0].ToString();
                    }
                }
            }

            // Getting Students 
            List<String> studentList = new List<string>();
            using (SqlConnection connection = new SqlConnection(DatabaseAccess.GetConnection()))
            {
                if (connection.State == ConnectionState.Closed)
                {
                    connection.Open();
                }
                using (SqlCommand command = new SqlCommand("SELECT DISTINCT(Marks.StudentID) FROM Marks JOIN STUDENT ON Marks.StudentID = Student.ID WHERE Student.Batch = @batch AND Student.Grade = @grade AND Marks.TerminalID = @id ORDER BY Marks.StudentID ASC;", connection))
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
                    foreach (DataRow row in queryData.Rows)
                    {
                        studentList.Add(row[0].ToString());
                    }
                }
            }



            // Getting marks of each student of each subjects
            List<StudentMarksDetailDBModel> studentMarksDetailsList = new List<StudentMarksDetailDBModel>();
            using (SqlConnection connection = new SqlConnection(DatabaseAccess.GetConnection()))
            {
                if (connection.State == ConnectionState.Closed)
                {
                    connection.Open();
                }

                StudentMarksDetailDBModel studentMarksDetailDBModel;
                foreach (String studentID in studentList)
                {
                    studentMarksDetailDBModel = new StudentMarksDetailDBModel();
                    MarksDetailsDBModel marksDetailsDB;
                    studentMarksDetailDBModel.StudentID = studentID;
                    List<MarksDetailsDBModel> marksDetailsDBModelList = new List<MarksDetailsDBModel>();

                    foreach (SubjectDetailsViewModel subject in subjectDetails)
                    {
                        using (SqlCommand command = new SqlCommand("SELECT Marks.Total_Mark, Marks.Pass_Mark, Marks.Obtained_Mark FROM Marks WHERE Marks.TerminalID = @terminalID AND Marks.StudentID = @stdID AND Marks.SubjectID =@subID;", connection))
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
                            marksDetailsDB = new MarksDetailsDBModel();
                            foreach (DataRow row in queryData.Rows)
                            {
                                marksDetailsDB.SubjectID = subject.SubjectID;
                                marksDetailsDB.TotalMark = float.Parse(row[0].ToString());
                                marksDetailsDB.PassMark = float.Parse(row[1].ToString());
                                marksDetailsDB.ObtainedMark = row[2].ToString();
                                marksDetailsDBModelList.Add(marksDetailsDB);
                            }
                        }
                    }
                    studentMarksDetailDBModel.MarksDetails = marksDetailsDBModelList;
                    studentMarksDetailsList.Add(studentMarksDetailDBModel);
                }
            }


            //Generating Result

            //Seperating passed and failed student
            List<StudentMarksDetailDBModel> passedStudent = new List<StudentMarksDetailDBModel>();
            List<StudentMarksDetailDBModel> failedStudent = new List<StudentMarksDetailDBModel>();

            foreach (StudentMarksDetailDBModel studentData in studentMarksDetailsList)
            {
                int fail = 0;
                foreach (MarksDetailsDBModel subjectMarks in studentData.MarksDetails)
                {
                    float obtainedMark;
                    Boolean isInt = float.TryParse(subjectMarks.ObtainedMark, out obtainedMark);
                    if (isInt)
                    {
                        if (obtainedMark < subjectMarks.PassMark)
                        {
                            fail++;
                        }
                    }
                    else
                    {
                        fail++;
                    }
                }
                if (fail <= 0)
                {
                    passedStudent.Add(studentData);
                }
                else
                {
                    failedStudent.Add(studentData);
                }
            }

            List<ResultStudentMarksDetailViewModel> studentResultList = new List<ResultStudentMarksDetailViewModel>();
            List<ResultStudentMarksDetailViewModel> passed = CalucateMarks(passedStudent);
            List<ResultStudentMarksDetailViewModel> failed = CalucateMarks(failedStudent);
            foreach (ResultStudentMarksDetailViewModel std in passed)
            {
                std.Result = "Passed";
            }
            foreach (ResultStudentMarksDetailViewModel std in failed)
            {
                std.Result = "Failed";
            }

            var orderPassedStd = from std in passed
                          orderby std.Percentage descending
                          select std;

            var orderFailedStd = from std in failed
                                 orderby std.Percentage descending
                                 select std;

            int rank = 1;
            foreach (ResultStudentMarksDetailViewModel std in orderPassedStd)
            {
                std.Rank = rank.ToString();
                studentResultList.Add(std);
                rank++;

            }foreach (ResultStudentMarksDetailViewModel std in orderFailedStd)
            {
                std.Rank = rank.ToString();
                studentResultList.Add(std);
                rank++;
            }
            List<ResultSubjectName> subjectNamesList = new List<ResultSubjectName>();
            ResultSubjectName subjectName;
            foreach (SubjectDetailsViewModel subject in subjectDetails)
            {
                subjectName = new ResultSubjectName();
                subjectName.SubjectName = subject.SubjectName;
                subjectNamesList.Add(subjectName);
            }

            model.SubjectNames = subjectNamesList;
            model.StudentResultList = studentResultList;
            return model;
        }

        public List<ResultStudentMarksDetailViewModel> CalucateMarks(List<StudentMarksDetailDBModel> studentData)
        {
            ResultStudentMarksDetailViewModel model = null;
            List<ResultStudentMarksDetailViewModel> modelList = new List<ResultStudentMarksDetailViewModel>();
            List<ResultSubjectMark> resultSubjectMarks;
            foreach (StudentMarksDetailDBModel student in studentData)
            {
                model = new ResultStudentMarksDetailViewModel();
                resultSubjectMarks = new List<ResultSubjectMark>();
                ResultSubjectMark subjectMark;
                float? GPA = 0F;
                float? totalMarks = 0F;
                float totalObtainedMark = 0F;
                foreach (MarksDetailsDBModel marks in student.MarksDetails)
                {
                    subjectMark = new ResultSubjectMark();
                    subjectMark.SubjectMark = marks.ObtainedMark;
                    resultSubjectMarks.Add(subjectMark);
                    float obtainedMark = 0F;
                    Boolean isInt = float.TryParse(marks.ObtainedMark, out obtainedMark);
                    GPA = GPA + CalculateSubjectGradeAndGradePointAndRemarks((float) marks.TotalMark, marks.ObtainedMark);
                    totalMarks = totalMarks + marks.TotalMark;
                    totalObtainedMark = totalObtainedMark + obtainedMark;
                }
                model.StudentID = student.StudentID;
                model.StudentName = GetStudentName(student.StudentID);
                model.TotalMarks = totalMarks.ToString();
                model.TotalObtainedMarks = totalObtainedMark.ToString();
                model.GPA = Math.Round((decimal)(GPA / student.MarksDetails.Count()), 1).ToString();
                model.Percentage = ((totalObtainedMark * 100) / totalMarks).ToString();
                model.SubjectMarks = resultSubjectMarks;
                model.Result = "";
                model.Rank = "";
                modelList.Add(model);
            }
            return modelList;
        }

        public String? GetStudentName(String studentID)
        {
            String studentName = null;
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


        public static float CalculateSubjectGradeAndGradePointAndRemarks(float totalMarks, String ObtainedMarks)
        {
            float obtainedMark;
            Boolean isInt = float.TryParse(ObtainedMarks, out obtainedMark);
            float percentage;
            float gradePoint = 0F;

            if (isInt)
            {
                percentage = (obtainedMark * 100) / totalMarks;

                if (percentage >= 90)
                {                    
                    gradePoint = 4.0F;
                }
                else if (percentage >= 80 && percentage < 90)
                {                   
                    gradePoint = 3.6F;
                }
                else if (percentage >= 70 && percentage < 80)
                {
                    gradePoint = 3.2F;
                }
                else if (percentage >= 60 && percentage < 70)
                {
                    gradePoint = 2.8F;
                }
                else if (percentage >= 50 && percentage < 60)
                {
                    gradePoint = 2.4F;
                }
                else if (percentage >= 40 && percentage < 50)
                {
                    gradePoint = 2.0F;
                }
                else if (percentage >= 30 && percentage < 40)
                {
                    gradePoint = 1.6F;
                }
                else if (percentage >= 20 && percentage < 30)
                {
                    gradePoint = 1.2F;
                }
                else if (percentage < 20)
                {
                    gradePoint = 0.8F;
                }
            }
            else
            {
                gradePoint = 0.8F;
            }
            return gradePoint;
        }

    }
}