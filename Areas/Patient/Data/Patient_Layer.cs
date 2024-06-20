using Hospital_Management.Areas.Doctor.Models;
using Hospital_Management.Areas.Patient.Models;
using Hospital_Management.Areas.User.Models;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;
using System.Data;

namespace Hospital_Management.Areas.Patient.Data
{
    public class Patient_Layer
    {
        string connection = "Data Source=_aimerstar_\\HARSHILMSSQL;Initial Catalog=HospitalDB;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=True;Application Intent=ReadWrite;Multi Subnet Failover=False";

        public IEnumerable<AppoinmnetModel> GetAppointments(string userId, HttpContext httpContext)
        {
            List<AppoinmnetModel> list = new List<AppoinmnetModel>();

            using (SqlConnection conn = new SqlConnection(connection))
            {
                SqlCommand cmd = new SqlCommand("GetAppoinmentById", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@User_Id", userId); // Corrected parameter name
                conn.Open();
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
        public IEnumerable<AppoinmnetModel> GetAllAppointments()
        {
            List<AppoinmnetModel> list = new List<AppoinmnetModel>();

            using (SqlConnection conn = new SqlConnection(connection))
            {
                SqlCommand cmd = new SqlCommand("GetAppoinment", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                conn.Open();
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
        public IEnumerable<AppoinmnetModel> GetAppointmentsbypatient(String id, string name)
        {
            List<AppoinmnetModel> list = new List<AppoinmnetModel>();

            using (SqlConnection conn = new SqlConnection(connection))
            {
                SqlCommand cmd = new SqlCommand("GetAppoinmentByPatientName", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Appoinment_Id", id);
                cmd.Parameters.AddWithValue("@Patient_Name", name);
                
                conn.Open();
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
                    appointment.IsDone = (bool)reader["IsDone"];
                    list.Add(appointment);
                }
            }
            return list;
        }
        public IEnumerable<AppoinmnetModel> GetAppointmentsbypatientname(string name)
        {
            List<AppoinmnetModel> list = new List<AppoinmnetModel>();
           
            using (SqlConnection conn = new SqlConnection(connection))
            {
                SqlCommand cmd = new SqlCommand("GetAppoinmentByPatientNames", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Doctor_Name", name);
                
                conn.Open();
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
                    appointment.IsDone = (bool)reader["IsDone"];
                    list.Add(appointment);
                }
            }
            return list;
        }
        public void UpdateAppointmentDoneStatus(string appointmentId)
        {
           using (SqlConnection conn = new SqlConnection(connection))
            {
                SqlCommand cmd = new SqlCommand("UpdateAppointmentisDoneStatus", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@AppointmentId", appointmentId);
                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }
        public void UpdateAppointmentStatus(string Appoinment_Id, bool isDone)
        {
            using (SqlConnection conn = new SqlConnection(connection))
            {
                SqlCommand cmd = new SqlCommand("UpdateAppointmentDoneStatus", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@AppointmentId", Appoinment_Id);
                cmd.Parameters.AddWithValue("@IsDone", isDone);
                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }
        public bool GetIsDoneStatus(string appointmentId)
        {
            using (SqlConnection conn = new SqlConnection(connection))
            {
                SqlCommand cmd = new SqlCommand("GetIsDoneStatus", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@AppointmentId", appointmentId);
                conn.Open();
                // Execute the command and get the result
                bool isDone = (bool)cmd.ExecuteScalar();

                return isDone;
            }
        }
        public IEnumerable<AppoinmnetModel> GetAppoinmnetModel(string userId)
        {
            List<AppoinmnetModel> list = new List<AppoinmnetModel>();

            using (SqlConnection conn = new SqlConnection(connection))
            {
                SqlCommand cmd = new SqlCommand("GetUserById", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@User_Id", userId); // Corrected parameter name
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    AppoinmnetModel appointment = new AppoinmnetModel();
                    
                    appointment.User_Id = reader["User_ID"].ToString();
                    appointment.Patient_Name = reader["Patient_Name"].ToString();
                    appointment.Doctor_Name = reader["Doctor_Name"].ToString();
                    appointment.Date =reader["Date"].ToString();
                    appointment.STime = (TimeSpan)reader["Time"];
                    list.Add(appointment);
                }
            }

            return list;
        }
        public IEnumerable<DoctorModel> GetDoctorProfile()
        {
            List<DoctorModel> list = new List<DoctorModel>();

            using (SqlConnection conn = new SqlConnection(connection))
            {
                SqlCommand cmd = new SqlCommand("GetDoctorByProfile", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                
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
        public IEnumerable<DoctorModel> GetAllDoctorProfile()
        {
            List<DoctorModel> list = new List<DoctorModel>();

            using (SqlConnection conn = new SqlConnection(connection))
            {
                SqlCommand cmd = new SqlCommand("Select * from User_tbl where Role_ID = 1", conn);
                cmd.CommandType = CommandType.Text;

                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    DoctorModel user = new DoctorModel();
                    user.User_Id = Convert.ToInt32(reader["User_ID"]);
                    user.Name = reader["Name"].ToString();
                    user.Designation = reader["Designation"].ToString();
                    user.Phone = reader["Phone"].ToString();
                    user.Gender = reader["Gender"].ToString();
                    list.Add(user);
                }
                conn.Close();
            }
            return list;
        }
        public available GetDoctorAvailableProfileById(string id)
        {
            available user = new available();

            using (SqlConnection conn = new SqlConnection(connection))
            {
                SqlCommand cmd = new SqlCommand("GetAvailableById", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("User_Id", id);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    user.Name = reader["Name"].ToString();
                    user.Date = Convert.ToDateTime(reader["Date"].ToString());

                }
                conn.Close();
            }
            return user;
        }
        public available GetAvailableDatesForDoctor(string Name)
        {
            available user = new available();

            using (SqlConnection conn = new SqlConnection(connection))
            {
                SqlCommand cmd = new SqlCommand("GetAvailableByName", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("Name", Name);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    user.Date = Convert.ToDateTime(reader["Date"].ToString());
                }
                conn.Close();
            }
            return user;
        }
        public IEnumerable<usermodel> GetAllPatientProfileById(string id)
        {
            List<usermodel> user = new List<usermodel>();

            using (SqlConnection conn = new SqlConnection(connection))
            {
                SqlCommand cmd = new SqlCommand("GetUserById", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("User_ID", id);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    usermodel use = new usermodel();
                    use.UserName = reader["Name"].ToString();
                    use.Email = reader["Email"].ToString();
                    user.Add(use);
                }
                conn.Close();
            }
            return user;
        }
        public usermodel GetPatientProfileById(string id)
        {
            usermodel user = new usermodel();

            using (SqlConnection conn = new SqlConnection(connection))
            {
                SqlCommand cmd = new SqlCommand("GetUserById", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("User_ID", id);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    user.UserName = reader["Name"].ToString();

                }
                conn.Close();
            }
            return user;
        }
        public void AddAppoinment(AppoinmnetModel appoinmnet)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(connection))
                {
                    using (SqlCommand cmd = new SqlCommand("Add_Appoinment", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@User_ID", appoinmnet.User_Id);
                        cmd.Parameters.AddWithValue("@Patient_Name", appoinmnet.Patient_Name);
                        cmd.Parameters.AddWithValue("@Doctor_Name", appoinmnet.Doctor_Name);
                        cmd.Parameters.AddWithValue("@Date", appoinmnet.Date);
                        cmd.Parameters.AddWithValue("@Timeperiod", appoinmnet.Timeperiod);
                        cmd.Parameters.AddWithValue("@STime", appoinmnet.STime);
                        cmd.Parameters.AddWithValue("@ETime", appoinmnet.ETime);
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
        public bool DeleteAppointment(string name)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connection))
                {
                    string query = "DELETE FROM Appoinment_tbl WHERE Patient_Name = @id";

                    using (SqlCommand command = new SqlCommand(query, conn))
                    {
                        command.Parameters.AddWithValue("@id", name);

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
