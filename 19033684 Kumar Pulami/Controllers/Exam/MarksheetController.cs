using _19033684_Kumar_Pulami.Models.DatabaseModel.Exam;
using _19033684_Kumar_Pulami.Models.ViewModel.Exam;
using _19033684_Kumar_Pulami.Models.ViewModel.Student.Manage_Student;
using _19033684_Kumar_Pulami.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Data;
using System.Data.SqlClient;

namespace _19033684_Kumar_Pulami.Controllers.Exam
{
    public class MarksheetController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            ViewBag.TitleName = "Marksheet Index";

            ViewBag.BatchList = DropDownFilter.GetBatchList();

            List<String> gradeList = new List<String>();
            gradeList.Add("Select Grade");
            ViewBag.GradeList = gradeList;

            List<String> sectionList = new List<String>();
            sectionList.Add("Select Section");
            ViewBag.SectionList = sectionList;

            ManageStudentIndexModel model = new ManageStudentIndexModel();
            model.ListModel = StudentDataFetcher.GetAllStudentList();
            return View(model);
        }


        [HttpPost]
        public IActionResult Index(ManageStudentIndexModel value)
        {
            ViewBag.TitleName = "Marksheet Index";
            if (ModelState.IsValid)
            {
                ViewBag.BatchList = DropDownFilter.GetBatchList();
                ViewBag.GradeList = DropDownFilter.GetGradeList(value.Batch);
                ViewBag.SectionList = DropDownFilter.GetSectionList(value.Batch, value.Grade);
                ManageStudentIndexModel indexModel = new ManageStudentIndexModel();
                DataTable queryData;
                ManageStudentListViewModel studentData;
                List<ManageStudentListViewModel> studentList = new List<ManageStudentListViewModel>();


                using (SqlConnection connection = new SqlConnection(DatabaseAccess.GetConnection()))
                {
                    if (connection.State == System.Data.ConnectionState.Closed)
                    {
                        connection.Open();
                    }
                    using (SqlCommand command = new SqlCommand("SELECT Student.ID AS StudentID, Person.Name, Student.Batch, Student.Grade, Student.Section, CONCAT(Address.Province, ', ', Address.District, ', ', Address.City, '-', Address.Ward) AS Address, Student.Guardian_Contact FROM Student JOIN Person ON Student.ID = Person.ID JOIN Person_Address ON Person.ID = Person_Address.Person_ID JOIN Address ON Address.ID = Person_Address.Address_ID WHERE Person_Address.Address_Type = 'Temporary' AND Student.Batch = @batch AND Student.Grade = @grade AND Student.Section = @section ORDER BY Person.Name;", connection))
                    {
                        command.Parameters.AddWithValue("@Batch", value.Batch);
                        command.Parameters.AddWithValue("@Grade", value.Grade);
                        command.Parameters.AddWithValue("@Section", value.Section);
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
                        studentData = new ManageStudentListViewModel();
                        studentData.StudentID = row[0].ToString();
                        studentData.StudentName = row[1].ToString();
                        studentData.Batch = row[2].ToString();
                        studentData.Grade = row[3].ToString();
                        studentData.Section = row[4].ToString();
                        studentData.Address = row[5].ToString();
                        studentData.GuardianContact = row[6].ToString();
                        studentList.Add(studentData);
                    }
                }

                ManageStudentIndexModel model = new ManageStudentIndexModel();
                model.ListModel = studentList;
                model.Batch = value.Batch;
                model.Grade = value.Grade;
                model.Section = value.Section;
                return View(model);
            }
            else
            {
                ViewBag.BatchList = DropDownFilter.GetBatchList();
                List<String> gradeList = new List<String>();
                gradeList.Add("Select Grade");
                ViewBag.GradeList = gradeList;

                List<String> sectionList = new List<String>();
                sectionList.Add("Select Section");
                ViewBag.SectionList = sectionList;

                value.ListModel = StudentDataFetcher.GetAllStudentList();

                return View(value);
            }
        }


        public JsonResult GetSectionListJson(int batch, int grade)
        {
            return Json(DropDownFilter.GetSectionList(batch, grade));
        }

        public JsonResult GetGradeListJson(int batch)
        {
            return Json(DropDownFilter.GetGradeList(batch));
        }

            [HttpGet]
        public IActionResult Marksheet(String StudentID)
        {
            ViewBag.TitleName = "Student Marksheet";
            MarksheetViewModel marksheetModel = new MarksheetViewModel();
            
            marksheetModel.StudentID = StudentID;
            marksheetModel.TerminalList = GetTerminalList(StudentID);
            marksheetModel.SubjectMarks = null;
            return View(marksheetModel);
        }


        [HttpPost]
        public IActionResult Marksheet(MarksheetViewModel inputDetails)
        {
            ViewBag.TitleName = "Student Marksheet";
            DataTable queryData;
            MarksheetViewModel marksheetDetails = new MarksheetViewModel();
            MarksheetSubjectViewModel subjectDetail;
            List<MarksheetSubjectViewModel> subjectList = new List<MarksheetSubjectViewModel>();

            using (SqlConnection connection = new SqlConnection(DatabaseAccess.GetConnection()))
            {
                if (connection.State == ConnectionState.Closed)
                {
                    connection.Open();
                }
                using (SqlCommand command = new SqlCommand("SELECT Distinct(Student.ID), Person.Name, Person.DOB, Student.Batch, Student.Section, Person.Father_Name, Terminal.TerminalName, Student.Grade FROM Student JOIN Person ON Student.ID = person.ID JOIN Marks ON student.ID = Marks.StudentID JOIN Terminal ON marks.TerminalID = Terminal.ID WHERE Student.ID = @id AND Marks.TerminalID = @tid;", connection))
                {
                    command.Parameters.AddWithValue("@id", inputDetails.StudentID);
                    command.Parameters.AddWithValue("@tid", inputDetails.Terminal);
                    using (SqlDataAdapter dataAdapter = new SqlDataAdapter(command))
                    {
                        queryData = new DataTable();
                        dataAdapter.Fill(queryData);
                    }
                }
            }

            if (queryData.Rows.Count != 0)
            {
                DataRow rowData = queryData.Rows[0];
                marksheetDetails.StudentID = rowData[0].ToString();
                marksheetDetails.StudentName = rowData[1].ToString();
                marksheetDetails.DOB = Convert.ToDateTime(rowData[2].ToString()).Date;
                marksheetDetails.Batch = rowData[3].ToString();
                marksheetDetails.Section = rowData[4].ToString();
                marksheetDetails.FatherName = rowData[5].ToString();
                marksheetDetails.Terminal = rowData[6].ToString();
                TempData["TerminalName"] = rowData[6].ToString();
                marksheetDetails.Grade = rowData[7].ToString();
            }

            using (SqlConnection connection = new SqlConnection(DatabaseAccess.GetConnection()))
            {
                if (connection.State == ConnectionState.Closed)
                {
                    connection.Open();
                }
                using (SqlCommand command = new SqlCommand("SELECT Marks.subjectID, Subject.SubjectName, marks.TerminalID, Terminal.TerminalName, Marks.Total_mark, Marks.Pass_mark, Marks.Obtained_mark FROM Student JOIN Marks ON Student.ID = Marks.StudentID JOIN Subject ON Marks.SubjectID = Subject.ID JOIN Terminal ON Marks.TerminalID = Terminal.ID WHERE Marks.StudentID = @stdID AND Marks.TerminalID = @terminalID", connection))
                {
                    command.Parameters.AddWithValue("@stdID", inputDetails.StudentID);
                    command.Parameters.AddWithValue("@terminalID", inputDetails.Terminal);
                    using (SqlDataAdapter dataAdapter = new SqlDataAdapter(command))
                    {
                        queryData = new DataTable();
                        dataAdapter.Fill(queryData);
                    }
                }
            }

            if (queryData.Rows.Count != 0)
            {
                SubjectGradeAndGradePointAndRemarksModel gradePoints;
                foreach (DataRow row in queryData.Rows)
                {
                    gradePoints = new SubjectGradeAndGradePointAndRemarksModel();
                    subjectDetail = new MarksheetSubjectViewModel();
                    subjectDetail.SubjectID = int.Parse(row[0].ToString());
                    subjectDetail.SubjectName = row[1].ToString();
                    subjectDetail.TotalMark = float.Parse(row[4].ToString());
                    subjectDetail.PassMark = float.Parse(row[5].ToString());
                    subjectDetail.ObtainedMark = row[6].ToString();
                    gradePoints = CalculateSubjectGradeAndGradePointAndRemarks((float)subjectDetail.TotalMark, subjectDetail.ObtainedMark);
                    subjectDetail.Grade = gradePoints.Grade;
                    subjectDetail.GradePoint = gradePoints.GradePoint;
                    subjectDetail.Remarks = gradePoints.Remarks;
                    subjectList.Add(subjectDetail);
                }
            }

            float? GPA = 0F;
            float? totalMarks = 0F;
            float totalObtainedMark = 0F;
            foreach (MarksheetSubjectViewModel marks in subjectList)
            {
                float obtainedMark = 0F;
                Boolean isInt = float.TryParse(marks.ObtainedMark, out obtainedMark);
                GPA = GPA + marks.GradePoint;
                totalMarks = totalMarks + marks.TotalMark;
                totalObtainedMark = totalObtainedMark + obtainedMark;
            }
            marksheetDetails.TotalMarks = totalMarks;
            marksheetDetails.TotalObtainedMarks = totalObtainedMark;
            marksheetDetails.GPA = GPA / subjectList.Count();
            marksheetDetails.Precentage = (totalObtainedMark * 100) / totalMarks;
            marksheetDetails.SubjectMarks = subjectList;
            marksheetDetails.TerminalList = GetTerminalList(inputDetails.StudentID);
            return View(marksheetDetails);
        }


        public static SubjectGradeAndGradePointAndRemarksModel CalculateSubjectGradeAndGradePointAndRemarks(float totalMarks, String ObtainedMarks)
        {
            SubjectGradeAndGradePointAndRemarksModel subjectGradeAndGradePointAndRemarksModel = new SubjectGradeAndGradePointAndRemarksModel();
            float obtainedMark;
            Boolean isInt = float.TryParse(ObtainedMarks, out obtainedMark);
            float percentage;

            if (isInt)
            {
                percentage = (obtainedMark * 100) / totalMarks;

                if (percentage >= 90)
                {
                    subjectGradeAndGradePointAndRemarksModel.Grade = "A+";
                    subjectGradeAndGradePointAndRemarksModel.GradePoint = 4.0F;
                }
                else if (percentage >= 80 && percentage < 90)
                {
                    subjectGradeAndGradePointAndRemarksModel.Grade = "A";
                    subjectGradeAndGradePointAndRemarksModel.GradePoint = 3.6F;
                }
                else if (percentage >= 70 && percentage < 80)
                {
                    subjectGradeAndGradePointAndRemarksModel.Grade = "B+";
                    subjectGradeAndGradePointAndRemarksModel.GradePoint = 3.2F;
                }
                else if (percentage >= 60 && percentage < 70)
                {
                    subjectGradeAndGradePointAndRemarksModel.Grade = "B";
                    subjectGradeAndGradePointAndRemarksModel.GradePoint = 2.8F;
                }
                else if (percentage >= 50 && percentage < 60)
                {
                    subjectGradeAndGradePointAndRemarksModel.Grade = "C+";
                    subjectGradeAndGradePointAndRemarksModel.GradePoint = 2.4F;
                }
                else if (percentage >= 40 && percentage < 50)
                {
                    subjectGradeAndGradePointAndRemarksModel.Grade = "C";
                    subjectGradeAndGradePointAndRemarksModel.GradePoint = 2.0F;
                }
                else if (percentage >= 30 && percentage < 40)
                {
                    subjectGradeAndGradePointAndRemarksModel.Grade = "D+";
                    subjectGradeAndGradePointAndRemarksModel.GradePoint = 1.6F;
                }
                else if (percentage >= 20 && percentage < 30)
                {
                    subjectGradeAndGradePointAndRemarksModel.Grade = "D";
                    subjectGradeAndGradePointAndRemarksModel.GradePoint = 1.2F;
                }
                else if (percentage < 20)
                {
                    subjectGradeAndGradePointAndRemarksModel.Grade = "E";
                    subjectGradeAndGradePointAndRemarksModel.GradePoint = 0.8F;
                }
            }
            else
            {
                subjectGradeAndGradePointAndRemarksModel.Grade = "E";
                subjectGradeAndGradePointAndRemarksModel.GradePoint = 0.8F;
                subjectGradeAndGradePointAndRemarksModel.Remarks = "@";
            }
            return subjectGradeAndGradePointAndRemarksModel;
        }

        public List<TerminalDataDatabaseModel> GetTerminalList(String StudentID)
        {
            DataTable queryData;
            List<TerminalDataDatabaseModel> terminalList = new List<TerminalDataDatabaseModel>();
            TerminalDataDatabaseModel terminalData;
            using (SqlConnection connection = new SqlConnection(DatabaseAccess.GetConnection()))
            {
                if (connection.State == System.Data.ConnectionState.Closed)
                {
                    connection.Open();
                }
                using (SqlCommand command = new SqlCommand("SELECT DISTINCT(Terminal.ID), Terminal.TerminalName FROM Marks JOIN Terminal ON Marks.TerminalID = Terminal.ID WHERE Marks.StudentID = @ID", connection))
                {
                    command.Parameters.AddWithValue("@ID", StudentID);
                    using (SqlDataAdapter dataAdapter = new SqlDataAdapter(command))
                    {
                        queryData = new DataTable();
                        dataAdapter.Fill(queryData);
                    }
                }

                if (queryData != null)
                {
                    foreach (DataRow rowValue in queryData.Rows)
                    {
                        terminalData = new TerminalDataDatabaseModel();
                        terminalData.TerminalID = int.Parse(rowValue[0].ToString());
                        terminalData.TerminalName = rowValue[1].ToString();
                        terminalList.Add(terminalData);
                    }
                }
            }
            return terminalList;
        }
    }
}
