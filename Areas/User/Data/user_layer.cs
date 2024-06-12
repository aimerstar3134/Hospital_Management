using Hospital_Management.Areas.User.Models;
using Microsoft.Data.SqlClient;
using System.Data;

namespace Hospital_Management.Areas.User.Data
{
    public class user_layer
    {
        string connection = "Data Source=_aimerstar_\\HARSHILMSSQL;Initial Catalog=HospitalDB;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=True;Application Intent=ReadWrite;Multi Subnet Failover=False";

        public IEnumerable<usermodel> GetAllUser()
        {
            List<usermodel> users = new List<usermodel>();
            using (SqlConnection conn = new SqlConnection(connection))
            {
                SqlCommand cmd = new SqlCommand("sp_GetAllUser", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    usermodel user = new usermodel();
                    user.UserID =Convert.ToInt32( reader["User_ID"]);
                    user.UserName = reader["Name"].ToString();
                    user.Email = reader["Email"].ToString();
                    user.Password = reader["Password"].ToString();
                    user.RoleId = Convert.ToInt32(reader["Role_ID"]);
                    users.Add(user);
                }
                conn.Close();
            }
            return users;   
        }

        public int GetUserCount()
        {
            using (SqlConnection conn = new SqlConnection(connection))
            {
                conn.Open(); // Open the connection

                SqlCommand cmd = new SqlCommand("SELECT COUNT(*) FROM User_tbl", conn);
                cmd.CommandType = CommandType.Text;

                // ExecuteScalar returns the first column of the first row in the result set
                int count = (int)cmd.ExecuteScalar();

                conn.Close(); // Close the connection

                return count;
            }
        }
        public int GetDoctorCount()
        {
            using (SqlConnection conn = new SqlConnection(connection))
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand("SELECT COUNT(*) FROM User_tbl where Role_ID = 1", conn);
                cmd.CommandType = CommandType.Text;

                int count = (int)cmd.ExecuteScalar();

                conn.Close();

                return count;
            }
        }

        public int GetPatientCount()
        {
            using (SqlConnection conn = new SqlConnection(connection))
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand("SELECT COUNT(*) FROM User_tbl where Role_ID = 2", conn);
                cmd.CommandType = CommandType.Text;

                int count = (int)cmd.ExecuteScalar();

                conn.Close();

                return count;
            }
        }

        public int GetStaffCount()
        {
            using (SqlConnection conn = new SqlConnection(connection))
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand("SELECT COUNT(*) FROM User_tbl where Role_ID = 3", conn);
                cmd.CommandType = CommandType.Text;

                int count = (int)cmd.ExecuteScalar();

                conn.Close();

                return count;
            }
        }

        public void AddUser(usermodel user, HttpContext httpContext)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(connection))
                {
                    using (SqlCommand cmd = new SqlCommand("SP_InsertUser", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@Name", user.UserName);
                        cmd.Parameters.AddWithValue("@Email", user.Email);
                        cmd.Parameters.AddWithValue("@Password", user.Password);
                        cmd.Parameters.AddWithValue("@Role_ID", user.RoleId);

                        con.Open();
                        cmd.ExecuteNonQuery();
                        httpContext.Session.SetString("Name", user.UserName);
                        httpContext.Session.SetString("Email",user.Email);
                        httpContext.Session.SetString("Password",user.Password);
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle the exception here, you might want to log it or throw it further.
                Console.WriteLine("Error: " + ex.Message);
            }
        }
    }
}
