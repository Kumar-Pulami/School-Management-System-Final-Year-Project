using _19033684_Kumar_Pulami.Models.ViewModel.Login;
using _19033684_Kumar_Pulami.Services;
using System.Data;
using System.Data.SqlClient;

namespace _19033684_Kumar_Pulami.Models.DatabaseModel.Login
{
    public class LoginDB
    {
        public DataTable Login(LoginView login)
        {
            using (SqlConnection connection = new SqlConnection(DatabaseAccess.GetConnection()))
            {
                if (connection.State == System.Data.ConnectionState.Closed)
                {
                    connection.Open();
                }
                using (SqlCommand command = new SqlCommand("SELECT * FROM Login WHERE UserName =  @userName AND Password = @password", connection))
                {
                    command.Parameters.AddWithValue("@userName", login.UserName);
                    command.Parameters.AddWithValue("@password", login.Password);
                    using (SqlDataAdapter dataAdapter = new SqlDataAdapter(command))
                    {
                        DataTable dataTable = new DataTable();
                        dataAdapter.Fill(dataTable);
                        return dataTable;
                    }
                }
            }
        }
    }
}
