using Hospital_Management.Areas.Patient.Models;
using Hospital_Management.Areas.User.Models;
using Microsoft.Data.SqlClient;
using System.Data;

namespace Hospital_Management.Areas.Staff.Data
{
    public class Staff_layer
    {
        string cn = "Data Source=_aimerstar_\\HARSHILMSSQL;Initial Catalog=HospitalDB;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=True;Application Intent=ReadWrite;Multi Subnet Failover=False";

        public IEnumerable<AppoinmnetModel> appoinmnets()
        {
            List<AppoinmnetModel> list = new List<AppoinmnetModel>();

            using (SqlConnection conn = new SqlConnection(cn))
            {
                SqlCommand cmd = new SqlCommand("Select * from Appoinment_tbl", conn);
                cmd.CommandType = CommandType.Text;
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
        public IEnumerable<AppoinmnetModel> GetAppointmentById(string id)
        {
            List<AppoinmnetModel> list = new List<AppoinmnetModel>();

            using (SqlConnection conn = new SqlConnection(cn))
            {
                SqlCommand cmd = new SqlCommand("Select * from Appoinment_tbl where Appoinment_Id=@id", conn);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@id", id);
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
        public void EditAppointment(AppoinmnetModel appointment)
        {
            using (SqlConnection conn = new SqlConnection(cn))
            {
                SqlCommand cmd = new SqlCommand("UPDATE Appoinment_tbl SET User_ID = @UserId, Patient_Name = @PatientName, Doctor_Name = @DoctorName, Date = @Date, Timeperiod = @Timeperiod, STime = @STime, ETime = @ETime WHERE Appoinment_Id = @AppointmentId", conn);

                cmd.Parameters.AddWithValue("@UserId", appointment.User_Id);
                cmd.Parameters.AddWithValue("@PatientName", appointment.Patient_Name);
                cmd.Parameters.AddWithValue("@DoctorName", appointment.Doctor_Name);
                cmd.Parameters.AddWithValue("@Date", appointment.Date);
                cmd.Parameters.AddWithValue("@Timeperiod", appointment.Timeperiod);
                cmd.Parameters.AddWithValue("@STime", appointment.STime);
                cmd.Parameters.AddWithValue("@ETime", appointment.ETime);
                cmd.Parameters.AddWithValue("@AppointmentId", appointment.Appoinment_Id);

                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }
        public void DeleteAppointment(int appointmentId)
        {
            using (SqlConnection conn = new SqlConnection(cn))
            {
                SqlCommand cmd = new SqlCommand("DELETE FROM Appoinment_tbl WHERE Appoinment_Id = @AppointmentId", conn);
                cmd.Parameters.AddWithValue("@AppointmentId", appointmentId);

                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }
        public AppoinmnetModel GetAppointmentById(int id)
        {
            AppoinmnetModel appointment = null;

            using (SqlConnection conn = new SqlConnection(cn))
            {
                SqlCommand cmd = new SqlCommand("SELECT * FROM Appoinment_tbl WHERE Appoinment_Id = @Id", conn);
                cmd.Parameters.AddWithValue("@Id", id);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    appointment = new AppoinmnetModel
                    {
                        Appoinment_Id = Convert.ToInt32(reader["Appoinment_Id"]),
                        User_Id = reader["User_ID"].ToString(),
                        Patient_Name = reader["Patient_Name"].ToString(),
                        Doctor_Name = reader["Doctor_Name"].ToString(),
                        Date = reader["Date"].ToString(),
                        Timeperiod = reader["Timeperiod"].ToString(),
                        STime = (TimeSpan)reader["STime"],
                        ETime = (TimeSpan)reader["ETime"]
                    };
                }
            }

            return appointment;
        }
        public IEnumerable<usermodel> GetAllPatients()
        {
            List<usermodel> users = new List<usermodel>();
            using (SqlConnection conn = new SqlConnection(cn))
            {
                SqlCommand cmd = new SqlCommand("select * from User_tbl where Role_ID = 2", conn);
                cmd.CommandType = CommandType.Text;

                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    usermodel user = new usermodel();
                    user.UserID = Convert.ToInt32(reader["User_ID"]);
                    user.UserName = reader["Name"].ToString();
                    user.Email = reader["Email"].ToString();
                    //user.Password = reader["Password"].ToString();
                    //user.RoleId = Convert.ToInt32(reader["Role_ID"]);
                    users.Add(user);
                }
                conn.Close();
            }
            return users;
        }
        public void AddTestReports(TestReports test)
        {
            using (SqlConnection conn = new SqlConnection(cn))
            {
                SqlCommand cmd = new SqlCommand("Insert into TestReports_tbl (UserId,DoctorName,TestCategory,TestDate,TestResult) values(@UserId,@DoctorName,@TestCategory,@TestDate,@TestResult)", conn);
                cmd.CommandType= CommandType.Text;
                cmd.Parameters.AddWithValue("@UserId", test.UserId);
                cmd.Parameters.AddWithValue("@DoctorName", test.DoctorName);
                cmd.Parameters.AddWithValue("@TestCategory", test.TestCategory);
                cmd.Parameters.AddWithValue("@TestDate", test.TestDate);
                cmd.Parameters.AddWithValue("@TestResult", test.TestResult);

                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }
        public IEnumerable<TestReports> ViewTestReports()
        {
            List<TestReports> testReports = new List<TestReports>();
            using (SqlConnection conn = new SqlConnection(cn))
            {
                SqlCommand cmd = new SqlCommand("select * from TestReports_tbl", conn);
                cmd.CommandType = CommandType.Text;

                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    TestReports test = new TestReports();
                    test.UserId = Convert.ToInt32(reader["UserId"]);
                    test.DoctorName = reader["DoctorName"].ToString();
                    test.TestCategory = reader["TestCategory"].ToString();
                    test.TestDate = reader["TestDate"].ToString();
                    test.TestResult = reader["TestResult"].ToString();
                    testReports.Add(test);
                }
                conn.Close();
            }
            return testReports;
        }

    }
}
