using Hospital_Management.Areas.Doctor.Models;
using Hospital_Management.Areas.Patient.Models;
using Hospital_Management.Areas.User.Models;
using Microsoft.Data.SqlClient;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using Role_Entity;
using System.Data;
using System.Numerics;

namespace Hospital_Management.Areas.Doctor.Data
{
    public class layer
    {
        string connection = "Data Source=_aimerstar_\\HARSHILMSSQL;Initial Catalog=HospitalDB;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=True;Application Intent=ReadWrite;Multi Subnet Failover=False";
        public IEnumerable<DoctorModel> GetDoctorProfile(int i)
        {
            List<DoctorModel> list = new List<DoctorModel>();

            using (SqlConnection conn = new SqlConnection(connection))
            {
                SqlCommand cmd = new SqlCommand("GetDoctorProfile", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@User_ID", i);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    DoctorModel user = new DoctorModel();
                    user.User_Id = Convert.ToInt32(reader["User_ID"]);
                    user.Name = reader["Name"].ToString();
                    user.Designation = reader["Designation"].ToString();
                    user.AvailableStartTime = (TimeSpan)reader["Start_Time"];
                    user.AvailableEndTime = (TimeSpan)reader["End_Time"];
                    user.Phone = reader["Phone"].ToString();
                    user.Gender = reader["Gender"].ToString();
                    list.Add(user);
                }
                conn.Close();
            }
            return list;
        }
        public DoctorModel GetDoctorProfileById(int id)
        {
            DoctorModel user = new DoctorModel();

            using (SqlConnection conn = new SqlConnection(connection))
            {
                SqlCommand cmd = new SqlCommand("sp_GetDoctorById", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("User_ID", id);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {

                    user.User_Id = Convert.ToInt32(reader["User_ID"]);
                    user.Name = reader["Name"].ToString();
                    user.Designation = reader["Designation"].ToString();
                    user.Phone = reader["Phone"].ToString();
                    user.Gender = reader["Gender"].ToString();

                }
                conn.Close();
            }
            return user;
        }
        public available GetDoctorAvailableProfileById(string id)
        {
            available user = new available();

            using (SqlConnection conn = new SqlConnection(connection))
            {
                SqlCommand cmd = new SqlCommand("sp_GetDoctorById", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("User_ID", id);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {

                    user.User_Id = Convert.ToInt32(reader["User_ID"]);
                    user.Name = reader["Name"].ToString();


                }
                conn.Close();
            }
            return user;
        }
        public IEnumerable<AppoinmnetModel> GetAppointments(string name)
        {
            List<AppoinmnetModel> list = new List<AppoinmnetModel>();

            using (SqlConnection conn = new SqlConnection(connection))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("GetAppoinmentByName", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Doctor_Name", name);

                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    AppoinmnetModel appointment = new AppoinmnetModel();
                    appointment.Appoinment_Id = Convert.ToInt32(reader["Appoinment_Id"]);
                    appointment.User_Id = reader["User_ID"].ToString();
                    appointment.Patient_Name = reader["Patient_Name"].ToString();
                    appointment.Doctor_Name = reader["Doctor_Name"].ToString();
                    appointment.Date = reader["Date"].ToString();
                    appointment.Timeperiod = reader["Timeperiod"].ToString();
                    appointment.STime = (TimeSpan)reader["STime"];
                    appointment.ETime = (TimeSpan)reader["ETime"];

                    list.Add(appointment);
                }
            }

            return list;
        }
        public void DoctorProfile(DoctorModel doctor, HttpContext httpContext)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(connection))
                {
                    using (SqlCommand cmd = new SqlCommand("Add_Doctor", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@User_ID", doctor.User_Id);
                        cmd.Parameters.AddWithValue("@Name", doctor.Name);
                        cmd.Parameters.AddWithValue("@Designation", doctor.Designation);
                        cmd.Parameters.AddWithValue("@Phone", doctor.Phone);
                        cmd.Parameters.AddWithValue("@Gender", doctor.Gender);
                        con.Open();
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle the exception here, you might want to log it or throw it further.
                Console.WriteLine("Error: " + ex.Message);
            }
        }
        public void DoctorAvailableProfile(available doctor, HttpContext httpContext)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(connection))
                {
                    using (SqlCommand cmd = new SqlCommand("Add_Available", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@User_ID", doctor.User_Id);
                        cmd.Parameters.AddWithValue("@Name", doctor.Name);
                        cmd.Parameters.AddWithValue("@Date", doctor.Date);
                        cmd.Parameters.AddWithValue("@Stime", doctor.STime);
                        cmd.Parameters.AddWithValue("@Etime", doctor.ETime);
                        con.Open();
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle the exception here, you might want to log it or throw it further.
                Console.WriteLine("Error: " + ex.Message);
            }
        }
        public IEnumerable<available> GetDoctorAvailableProfile(string id)
        {
            List<available> list = new List<available>();

            using (SqlConnection conn = new SqlConnection(connection))
            {
                SqlCommand cmd = new SqlCommand("GetAvailableById", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@User_Id", id);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    available user = new available();
                    user.User_Id = Convert.ToInt32(reader["User_ID"]);
                    user.Name = reader["Name"].ToString();
                    user.Date = Convert.ToDateTime(reader["Date"].ToString());
                    user.STime = (TimeSpan)reader["Start_Time"];
                    user.ETime = (TimeSpan)reader["End_Time"];
                    list.Add(user);
                }
                conn.Close();
            }
            return list;
        }
        public IEnumerable<available> GetDoctorAvailable()
        {
            List<available> list = new List<available>();

            using (SqlConnection conn = new SqlConnection(connection))
            {
                SqlCommand cmd = new SqlCommand("GetAvailable", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    available user = new available();
                    user.User_Id = Convert.ToInt32(reader["User_ID"]);
                    user.Name = reader["Name"].ToString();
                    user.Date = Convert.ToDateTime(reader["Date"].ToString());
                    user.STime = (TimeSpan)reader["Start_Time"];
                    user.ETime = (TimeSpan)reader["End_Time"];
                    list.Add(user);
                }
                conn.Close();
            }
            return list;
        }
        public bool IsTimeSlotAvailable(string doctorName, DateTime date, TimeSpan startTime, TimeSpan endTime)
        {
            using (SqlConnection conn = new SqlConnection(connection))
            {
                string query = @"
            SELECT COUNT(*) 
            FROM Available_tbl 
            WHERE Name = @Name 
            AND Date = @Date 
            AND (@Start_Time >= Start_Time AND @Start_Time < End_Time OR @End_Time > Start_Time AND @End_Time <= End_Time)";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Name", doctorName);
                cmd.Parameters.AddWithValue("@Date", date);
                cmd.Parameters.AddWithValue("@Start_Time", startTime);
                cmd.Parameters.AddWithValue("@End_Time", endTime);

                conn.Open();
                int count = (int)cmd.ExecuteScalar();

                return count == 0; // Returns true if no overlapping time slots found
            }
        }
        public void resetPassword(resetpasswordviewmodel model)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(connection))
                {
                    con.Open();

                    SqlCommand checkCmd = new SqlCommand("reset", con);
                    checkCmd.CommandType = CommandType.StoredProcedure;
                    checkCmd.Parameters.AddWithValue("@User_Id", model.UserId);
                    checkCmd.Parameters.AddWithValue("@password", model.OldPassword);
                    int count = (int)checkCmd.ExecuteScalar();

                    if (count == 1)
                    {
                        SqlCommand resetCmd = new SqlCommand("ResetPassword", con);
                        resetCmd.CommandType = CommandType.StoredProcedure;
                        resetCmd.Parameters.AddWithValue("@UserId", model.UserId);
                        resetCmd.Parameters.AddWithValue("@NewPassword", model.NewPassword);
                        resetCmd.ExecuteNonQuery();

                        Console.WriteLine("Password reset successfully.");
                    }
                    else
                    {
                        Console.WriteLine("Incorrect old password.");
                    }
                }
            }
            catch (Exception ex)
            {

                Console.WriteLine("Error: " + ex.Message);
            }
        }
        public bool DeleteAvailability(TimeSpan STime)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connection))
                {
                    string query = "  delete from Available_tbl where Start_Time=@Start_Time";

                    using (SqlCommand command = new SqlCommand(query, conn))
                    {
                        command.Parameters.AddWithValue("@Start_Time", STime);

                        conn.Open();
                        int rowsAffected = command.ExecuteNonQuery();

                        return rowsAffected > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                // Log the exception or display an error message
                Console.WriteLine("Error deleting availability entry: " + ex.Message);
                return false;
            }
        }
    }
}
