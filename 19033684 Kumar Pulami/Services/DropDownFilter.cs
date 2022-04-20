using _19033684_Kumar_Pulami.Models.ViewModel.Exam.Result;
using System.Data;
using System.Data.SqlClient;

namespace _19033684_Kumar_Pulami.Services
{
    public static class DropDownFilter
    {
        public static List<String>? GetBatchList()
        {
            List<String>? batchList = new List<string>();
            DataTable queryData;
            using (SqlConnection connection = new SqlConnection(DatabaseAccess.GetConnection()))
            {
                if (connection.State == ConnectionState.Closed)
                {
                    connection.Open();
                }
                using (SqlCommand command = new SqlCommand("SELECT DISTINCT(Batch) FROM Student ORDER BY Batch DESC;", connection))
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
                        batchList.Add(row[0].ToString());
                    }
                }
                else
                {
                    batchList = null;
                }
            }
            return batchList;
        }



        public static List<int> GetGradeList(int? batch)
        {
            List<int> gradeList = new List<int>();

            if (String.IsNullOrEmpty(batch.ToString()))
            {
                gradeList = null;
            }
            else
            {
                DataTable queryData;
                using (SqlConnection connection = new SqlConnection(DatabaseAccess.GetConnection()))
                {
                    if (connection.State == ConnectionState.Closed)
                    {
                        connection.Open();
                    }
                    using (SqlCommand command = new SqlCommand("SELECT DISTINCT(Grade) FROM Student WHERE Batch = @batch ORDER BY Grade ASC;", connection))
                    {
                        command.Parameters.AddWithValue("@batch", batch);
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
                            gradeList.Add(int.Parse(row[0].ToString()));
                        }
                    }
                }
            }
            return gradeList;
        }
        public static List<ResultTerminalViewModel> GetTerminalList(int? batch, int? grade)
        {
            List<ResultTerminalViewModel> terminalList = new List<ResultTerminalViewModel>();

            if (String.IsNullOrEmpty(batch.ToString()))
            {
                terminalList = null;
            }
            else
            {
                DataTable queryData;
                using (SqlConnection connection = new SqlConnection(DatabaseAccess.GetConnection()))
                {
                    if (connection.State == ConnectionState.Closed)
                    {
                        connection.Open();
                    }
                    using (SqlCommand command = new SqlCommand("SELECT DISTINCT(Marks.TerminalID), Terminal.TerminalName FROM Marks JOIN Terminal ON Marks.TerminalID = Terminal.ID JOIN Student ON Marks.StudentID = Student.ID WHERE Student.Batch = @batch AND Student.Grade = @grade ORDER BY Terminal.TerminalName ASC;", connection))
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
                        ResultTerminalViewModel terminal;
                        foreach (DataRow row in queryData.Rows)
                        {
                            terminal = new ResultTerminalViewModel();
                            terminal.TerminalID = int.Parse(row[0].ToString());
                            terminal.TerminalName = row[1].ToString();
                            terminalList.Add(terminal);
                        }
                    }
                }
            }
            return terminalList;
        }
        public static List<String> GetSectionList(int? batch, int? grade)
        {
            List<String> sectionList = new List<String>();

            if (String.IsNullOrEmpty(batch.ToString()))
            {
                sectionList = null;
            }
            else
            {
                DataTable queryData;
                using (SqlConnection connection = new SqlConnection(DatabaseAccess.GetConnection()))
                {
                    if (connection.State == ConnectionState.Closed)
                    {
                        connection.Open();
                    }
                    using (SqlCommand command = new SqlCommand("SELECT DISTINCT(Student.Section) FROM Student WHERE Student.Batch = @batch AND Student.Grade = @grade ORDER BY Student.Section;", connection))
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
                            sectionList.Add(row[0].ToString());
                        }
                    }
                }
            }
            return sectionList;
        }


    }
}
